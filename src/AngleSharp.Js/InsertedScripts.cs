namespace AngleSharp.Js
{
    using AngleSharp.Dom;
    using AngleSharp.Html.Dom;
    using AngleSharp.Io;
    using Jint.Runtime;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Runs a script that script inserts (e.g. by <c>head.appendChild(script)</c> with its text)
    /// before the insertion returns, as browsers do. AngleSharp prepares such a script during the
    /// insertion, but evaluates it later, as a task of the event loop - after the script inserting
    /// it, which may look at what it did right away (MediaWiki evaluates the modules it cached in
    /// localStorage this way, and fetches the ones still missing right after).
    /// </summary>
    static class InsertedScripts
    {
        //  If AngleSharp prepared a script (its "already started" flag), which it does not expose.
        //  Without the field the scripts run when AngleSharp evaluates them, as before.
        private static readonly FieldInfo StartedField = typeof(IHtmlScriptElement).Assembly
            .GetType("AngleSharp.Html.Dom.HtmlScriptElement")?
            .GetField("_started", BindingFlags.Instance | BindingFlags.NonPublic) is { } field && field.FieldType == typeof(Boolean) ? field : null;

        private static readonly ConditionalWeakTable<IHtmlScriptElement, Object> Ran = new ConditionalWeakTable<IHtmlScriptElement, Object>();

        /// <summary>
        /// Finds the scripts a call of a DOM member may insert: its target, or its arguments, as
        /// far as AngleSharp did not prepare them yet.
        /// </summary>
        public static List<IHtmlScriptElement> FindUnstarted(Object target, Object[] arguments)
        {
            if (StartedField is null)
            {
                return null;
            }

            var found = default(List<IHtmlScriptElement>);
            Add(target, ref found);

            foreach (var argument in arguments)
            {
                if (argument is Array items)
                {
                    foreach (var item in items)
                    {
                        Add(item, ref found);
                    }
                }
                else
                {
                    Add(argument, ref found);
                }
            }

            return found;
        }

        /// <summary>
        /// Runs the scripts AngleSharp prepared during the call (see <see cref="FindUnstarted"/>),
        /// which are of the document of the engine and have their text. AngleSharp's evaluation of
        /// them later is skipped (see <see cref="TakeRan"/>).
        /// </summary>
        public static void RunPrepared(EngineInstance instance, IDocument document, List<IHtmlScriptElement> scripts)
        {
            foreach (var script in scripts)
            {
                if (IsStarted(script) && CanRunNow(script, document))
                {
                    Ran.Remove(script);
                    Ran.Add(script, null);

                    try
                    {
                        instance.RunScript(script.Text, script.Type, script.BaseUri, default);
                    }
                    catch (JavaScriptException error)
                    {
                        instance.ReportError(error);
                    }
                }
            }
        }

        /// <summary>
        /// Gets if the script already ran when it was inserted, which it then does not again.
        /// </summary>
        public static Boolean TakeRan(IElement element) =>
            element is IHtmlScriptElement script && Ran.Remove(script);

        private static void Add(Object candidate, ref List<IHtmlScriptElement> found)
        {
            if (candidate is IHtmlScriptElement script && !IsStarted(script))
            {
                (found ??= new List<IHtmlScriptElement>()).Add(script);
            }
            else if (candidate is IDocumentFragment fragment)
            {
                //  the children of a fragment are what is inserted
                foreach (var child in fragment.ChildNodes)
                {
                    Add(child, ref found);
                }
            }
        }

        private static Boolean IsStarted(IHtmlScriptElement script) =>
            script.GetType() == StartedField.DeclaringType && (Boolean)StartedField.GetValue(script);

        //  a classic script with its text (not one to download, a module or an import map), in
        //  the document the engine runs, as AngleSharp runs it
        private static Boolean CanRunNow(IHtmlScriptElement script, IDocument document)
        {
            if (!ReferenceEquals(script.Owner, document) || script.HasAttribute(AttributeNames.Src) || String.IsNullOrEmpty(script.Text))
            {
                return false;
            }

            if (script.HasAttribute(AttributeNames.Event) && script.HasAttribute(AttributeNames.For))
            {
                return false;
            }

            var type = script.Type;

            if (String.IsNullOrEmpty(type))
            {
                var language = script.GetAttribute(AttributeNames.Language);
                return String.IsNullOrEmpty(language) || MimeTypeNames.IsJavaScript("text/" + language);
            }

            return MimeTypeNames.IsJavaScript(type);
        }
    }
}
