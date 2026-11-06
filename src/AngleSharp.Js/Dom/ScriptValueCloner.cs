namespace AngleSharp.Js.Dom
{
    using AngleSharp.Dom;
    using Jint.Native;
    using System;

    /// <summary>
    /// Tells the DOM what the values of script are, see <see cref="StructuredClone"/>.
    /// </summary>
    sealed class ScriptValueCloner : IScriptValueCloner
    {
        public Object Undefined => JsValue.Undefined;

        public Object Clone(Object value) => StructuredClone.Clone(value);
    }
}
