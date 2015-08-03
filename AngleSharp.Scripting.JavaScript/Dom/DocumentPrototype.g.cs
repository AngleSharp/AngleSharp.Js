namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class DocumentPrototype : DocumentInstance
    {
        public DocumentPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("open", Engine.AsValue(Open), true, true, true);
            FastAddProperty("close", Engine.AsValue(Close), true, true, true);
            FastAddProperty("write", Engine.AsValue(Write), true, true, true);
            FastAddProperty("writeln", Engine.AsValue(Writeln), true, true, true);
            FastAddProperty("load", Engine.AsValue(Load), true, true, true);
            FastAddProperty("getElementsByName", Engine.AsValue(GetElementsByName), true, true, true);
            FastAddProperty("getElementsByClassName", Engine.AsValue(GetElementsByClassName), true, true, true);
            FastAddProperty("getElementsByTagName", Engine.AsValue(GetElementsByTagName), true, true, true);
            FastAddProperty("getElementsByTagNameNS", Engine.AsValue(GetElementsByTagNameNS), true, true, true);
            FastAddProperty("createEvent", Engine.AsValue(CreateEvent), true, true, true);
            FastAddProperty("createRange", Engine.AsValue(CreateRange), true, true, true);
            FastAddProperty("createComment", Engine.AsValue(CreateComment), true, true, true);
            FastAddProperty("createDocumentFragment", Engine.AsValue(CreateDocumentFragment), true, true, true);
            FastAddProperty("createElement", Engine.AsValue(CreateElement), true, true, true);
            FastAddProperty("createElementNS", Engine.AsValue(CreateElementNS), true, true, true);
            FastAddProperty("createAttribute", Engine.AsValue(CreateAttribute), true, true, true);
            FastAddProperty("createAttributeNS", Engine.AsValue(CreateAttributeNS), true, true, true);
            FastAddProperty("createProcessingInstruction", Engine.AsValue(CreateProcessingInstruction), true, true, true);
            FastAddProperty("createTextNode", Engine.AsValue(CreateTextNode), true, true, true);
            FastAddProperty("createNodeIterator", Engine.AsValue(CreateNodeIterator), true, true, true);
            FastAddProperty("createTreeWalker", Engine.AsValue(CreateTreeWalker), true, true, true);
            FastAddProperty("importNode", Engine.AsValue(ImportNode), true, true, true);
            FastAddProperty("adoptNode", Engine.AsValue(AdoptNode), true, true, true);
            FastAddProperty("hasFocus", Engine.AsValue(HasFocus), true, true, true);
            FastAddProperty("execCommand", Engine.AsValue(ExecCommand), true, true, true);
            FastAddProperty("queryCommandEnabled", Engine.AsValue(QueryCommandEnabled), true, true, true);
            FastAddProperty("queryCommandIndeterm", Engine.AsValue(QueryCommandIndeterm), true, true, true);
            FastAddProperty("queryCommandState", Engine.AsValue(QueryCommandState), true, true, true);
            FastAddProperty("queryCommandSupported", Engine.AsValue(QueryCommandSupported), true, true, true);
            FastAddProperty("queryCommandValue", Engine.AsValue(QueryCommandValue), true, true, true);
            FastAddProperty("append", Engine.AsValue(Append), true, true, true);
            FastAddProperty("prepend", Engine.AsValue(Prepend), true, true, true);
            FastAddProperty("querySelector", Engine.AsValue(QuerySelector), true, true, true);
            FastAddProperty("querySelectorAll", Engine.AsValue(QuerySelectorAll), true, true, true);
            FastAddProperty("enableStyleSheetsForSet", Engine.AsValue(EnableStyleSheetsForSet), true, true, true);
            FastAddProperty("getElementById", Engine.AsValue(GetElementById), true, true, true);
            FastSetProperty("all", Engine.AsProperty(GetAll));
            FastSetProperty("anchors", Engine.AsProperty(GetAnchors));
            FastSetProperty("implementation", Engine.AsProperty(GetImplementation));
            FastSetProperty("designMode", Engine.AsProperty(GetDesignMode, SetDesignMode));
            FastSetProperty("dir", Engine.AsProperty(GetDir, SetDir));
            FastSetProperty("documentURI", Engine.AsProperty(GetDocumentURI));
            FastSetProperty("characterSet", Engine.AsProperty(GetCharacterSet));
            FastSetProperty("compatMode", Engine.AsProperty(GetCompatMode));
            FastSetProperty("URL", Engine.AsProperty(GetURL));
            FastSetProperty("contentType", Engine.AsProperty(GetContentType));
            FastSetProperty("doctype", Engine.AsProperty(GetDoctype));
            FastSetProperty("documentElement", Engine.AsProperty(GetDocumentElement));
            FastSetProperty("lastModified", Engine.AsProperty(GetLastModified));
            FastSetProperty("readyState", Engine.AsProperty(GetReadyState));
            FastSetProperty("location", Engine.AsProperty(GetLocation));
            FastSetProperty("forms", Engine.AsProperty(GetForms));
            FastSetProperty("images", Engine.AsProperty(GetImages));
            FastSetProperty("scripts", Engine.AsProperty(GetScripts));
            FastSetProperty("embeds", Engine.AsProperty(GetEmbeds));
            FastSetProperty("plugins", Engine.AsProperty(GetPlugins));
            FastSetProperty("commands", Engine.AsProperty(GetCommands));
            FastSetProperty("links", Engine.AsProperty(GetLinks));
            FastSetProperty("title", Engine.AsProperty(GetTitle, SetTitle));
            FastSetProperty("head", Engine.AsProperty(GetHead));
            FastSetProperty("body", Engine.AsProperty(GetBody, SetBody));
            FastSetProperty("cookie", Engine.AsProperty(GetCookie, SetCookie));
            FastSetProperty("origin", Engine.AsProperty(GetOrigin));
            FastSetProperty("domain", Engine.AsProperty(GetDomain, SetDomain));
            FastSetProperty("referrer", Engine.AsProperty(GetReferrer));
            FastSetProperty("activeElement", Engine.AsProperty(GetActiveElement));
            FastSetProperty("currentScript", Engine.AsProperty(GetCurrentScript));
            FastSetProperty("defaultView", Engine.AsProperty(GetDefaultView));
            FastSetProperty("children", Engine.AsProperty(GetChildren));
            FastSetProperty("firstElementChild", Engine.AsProperty(GetFirstElementChild));
            FastSetProperty("lastElementChild", Engine.AsProperty(GetLastElementChild));
            FastSetProperty("childElementCount", Engine.AsProperty(GetChildElementCount));
            FastSetProperty("styleSheets", Engine.AsProperty(GetStyleSheets));
            FastSetProperty("selectedStyleSheetSet", Engine.AsProperty(GetSelectedStyleSheetSet, SetSelectedStyleSheetSet));
            FastSetProperty("lastStyleSheetSet", Engine.AsProperty(GetLastStyleSheetSet));
            FastSetProperty("preferredStyleSheetSet", Engine.AsProperty(GetPreferredStyleSheetSet));
            FastSetProperty("styleSheetSets", Engine.AsProperty(GetStyleSheetSets));
            FastSetProperty("onreadystatechange", Engine.AsProperty(GetReadyStateChangedEvent, SetReadyStateChangedEvent));
            FastSetProperty("onabort", Engine.AsProperty(GetAbortedEvent, SetAbortedEvent));
            FastSetProperty("onblur", Engine.AsProperty(GetBlurredEvent, SetBlurredEvent));
            FastSetProperty("oncancel", Engine.AsProperty(GetCancelledEvent, SetCancelledEvent));
            FastSetProperty("oncanplay", Engine.AsProperty(GetCanPlayEvent, SetCanPlayEvent));
            FastSetProperty("oncanplaythrough", Engine.AsProperty(GetCanPlayThroughEvent, SetCanPlayThroughEvent));
            FastSetProperty("onchange", Engine.AsProperty(GetChangedEvent, SetChangedEvent));
            FastSetProperty("onclick", Engine.AsProperty(GetClickedEvent, SetClickedEvent));
            FastSetProperty("oncuechange", Engine.AsProperty(GetCueChangedEvent, SetCueChangedEvent));
            FastSetProperty("ondblclick", Engine.AsProperty(GetDoubleClickEvent, SetDoubleClickEvent));
            FastSetProperty("ondrag", Engine.AsProperty(GetDragEvent, SetDragEvent));
            FastSetProperty("ondragend", Engine.AsProperty(GetDragEndEvent, SetDragEndEvent));
            FastSetProperty("ondragenter", Engine.AsProperty(GetDragEnterEvent, SetDragEnterEvent));
            FastSetProperty("ondragexit", Engine.AsProperty(GetDragExitEvent, SetDragExitEvent));
            FastSetProperty("ondragleave", Engine.AsProperty(GetDragLeaveEvent, SetDragLeaveEvent));
            FastSetProperty("ondragover", Engine.AsProperty(GetDragOverEvent, SetDragOverEvent));
            FastSetProperty("ondragstart", Engine.AsProperty(GetDragStartEvent, SetDragStartEvent));
            FastSetProperty("ondrop", Engine.AsProperty(GetDroppedEvent, SetDroppedEvent));
            FastSetProperty("ondurationchange", Engine.AsProperty(GetDurationChangedEvent, SetDurationChangedEvent));
            FastSetProperty("onemptied", Engine.AsProperty(GetEmptiedEvent, SetEmptiedEvent));
            FastSetProperty("onended", Engine.AsProperty(GetEndedEvent, SetEndedEvent));
            FastSetProperty("onerror", Engine.AsProperty(GetErrorEvent, SetErrorEvent));
            FastSetProperty("onfocus", Engine.AsProperty(GetFocusedEvent, SetFocusedEvent));
            FastSetProperty("oninput", Engine.AsProperty(GetInputEvent, SetInputEvent));
            FastSetProperty("oninvalid", Engine.AsProperty(GetInvalidEvent, SetInvalidEvent));
            FastSetProperty("onkeydown", Engine.AsProperty(GetKeyDownEvent, SetKeyDownEvent));
            FastSetProperty("onkeypress", Engine.AsProperty(GetKeyPressEvent, SetKeyPressEvent));
            FastSetProperty("onkeyup", Engine.AsProperty(GetKeyUpEvent, SetKeyUpEvent));
            FastSetProperty("onload", Engine.AsProperty(GetLoadedEvent, SetLoadedEvent));
            FastSetProperty("onloadeddata", Engine.AsProperty(GetLoadedDataEvent, SetLoadedDataEvent));
            FastSetProperty("onloadedmetadata", Engine.AsProperty(GetLoadedMetadataEvent, SetLoadedMetadataEvent));
            FastSetProperty("onloadstart", Engine.AsProperty(GetLoadingEvent, SetLoadingEvent));
            FastSetProperty("onmousedown", Engine.AsProperty(GetMouseDownEvent, SetMouseDownEvent));
            FastSetProperty("onmouseenter", Engine.AsProperty(GetMouseEnterEvent, SetMouseEnterEvent));
            FastSetProperty("onmouseleave", Engine.AsProperty(GetMouseLeaveEvent, SetMouseLeaveEvent));
            FastSetProperty("onmousemove", Engine.AsProperty(GetMouseMoveEvent, SetMouseMoveEvent));
            FastSetProperty("onmouseout", Engine.AsProperty(GetMouseOutEvent, SetMouseOutEvent));
            FastSetProperty("onmouseover", Engine.AsProperty(GetMouseOverEvent, SetMouseOverEvent));
            FastSetProperty("onmouseup", Engine.AsProperty(GetMouseUpEvent, SetMouseUpEvent));
            FastSetProperty("onmousewheel", Engine.AsProperty(GetMouseWheelEvent, SetMouseWheelEvent));
            FastSetProperty("onpause", Engine.AsProperty(GetPausedEvent, SetPausedEvent));
            FastSetProperty("onplay", Engine.AsProperty(GetPlayedEvent, SetPlayedEvent));
            FastSetProperty("onplaying", Engine.AsProperty(GetPlayingEvent, SetPlayingEvent));
            FastSetProperty("onprogress", Engine.AsProperty(GetProgressEvent, SetProgressEvent));
            FastSetProperty("onratechange", Engine.AsProperty(GetRateChangedEvent, SetRateChangedEvent));
            FastSetProperty("onreset", Engine.AsProperty(GetResettedEvent, SetResettedEvent));
            FastSetProperty("onresize", Engine.AsProperty(GetResizedEvent, SetResizedEvent));
            FastSetProperty("onscroll", Engine.AsProperty(GetScrolledEvent, SetScrolledEvent));
            FastSetProperty("onseeked", Engine.AsProperty(GetSeekedEvent, SetSeekedEvent));
            FastSetProperty("onseeking", Engine.AsProperty(GetSeekingEvent, SetSeekingEvent));
            FastSetProperty("onselect", Engine.AsProperty(GetSelectedEvent, SetSelectedEvent));
            FastSetProperty("onshow", Engine.AsProperty(GetShownEvent, SetShownEvent));
            FastSetProperty("onstalled", Engine.AsProperty(GetStalledEvent, SetStalledEvent));
            FastSetProperty("onsubmit", Engine.AsProperty(GetSubmittedEvent, SetSubmittedEvent));
            FastSetProperty("onsuspend", Engine.AsProperty(GetSuspendedEvent, SetSuspendedEvent));
            FastSetProperty("ontimeupdate", Engine.AsProperty(GetTimeUpdatedEvent, SetTimeUpdatedEvent));
            FastSetProperty("ontoggle", Engine.AsProperty(GetToggledEvent, SetToggledEvent));
            FastSetProperty("onvolumechange", Engine.AsProperty(GetVolumeChangedEvent, SetVolumeChangedEvent));
            FastSetProperty("onwaiting", Engine.AsProperty(GetWaitingEvent, SetWaitingEvent));
        }

        public static DocumentPrototype CreatePrototypeObject(EngineInstance engine, DocumentConstructor constructor)
        {
            var obj = new DocumentPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.Node.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Open(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var type = TypeConverter.ToString(arguments.At(0));
            var replace = TypeConverter.ToString(arguments.At(1));
            return Engine.Select(reference.Open(type, replace));
        }

        JsValue Close(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            reference.Close();
            return JsValue.Undefined;
        }

        JsValue Write(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var content = TypeConverter.ToString(arguments.At(0));
            reference.Write(content);
            return JsValue.Undefined;
        }

        JsValue Writeln(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var content = TypeConverter.ToString(arguments.At(0));
            reference.WriteLine(content);
            return JsValue.Undefined;
        }

        JsValue Load(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var url = TypeConverter.ToString(arguments.At(0));
            reference.Load(url);
            return JsValue.Undefined;
        }

        JsValue GetElementsByName(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var name = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.GetElementsByName(name));
        }

        JsValue GetElementsByClassName(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var classNames = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.GetElementsByClassName(classNames));
        }

        JsValue GetElementsByTagName(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var tagName = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.GetElementsByTagName(tagName));
        }

        JsValue GetElementsByTagNameNS(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var namespaceUri = TypeConverter.ToString(arguments.At(0));
            var tagName = TypeConverter.ToString(arguments.At(1));
            return Engine.Select(reference.GetElementsByTagName(namespaceUri, tagName));
        }

        JsValue CreateEvent(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var type = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.CreateEvent(type));
        }

        JsValue CreateRange(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.CreateRange());
        }

        JsValue CreateComment(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var data = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.CreateComment(data));
        }

        JsValue CreateDocumentFragment(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.CreateDocumentFragment());
        }

        JsValue CreateElement(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var name = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.CreateElement(name));
        }

        JsValue CreateElementNS(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var namespaceUri = TypeConverter.ToString(arguments.At(0));
            var name = TypeConverter.ToString(arguments.At(1));
            return Engine.Select(reference.CreateElement(namespaceUri, name));
        }

        JsValue CreateAttribute(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var name = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.CreateAttribute(name));
        }

        JsValue CreateAttributeNS(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var namespaceUri = TypeConverter.ToString(arguments.At(0));
            var name = TypeConverter.ToString(arguments.At(1));
            return Engine.Select(reference.CreateAttribute(namespaceUri, name));
        }

        JsValue CreateProcessingInstruction(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var target = TypeConverter.ToString(arguments.At(0));
            var data = TypeConverter.ToString(arguments.At(1));
            return Engine.Select(reference.CreateProcessingInstruction(target, data));
        }

        JsValue CreateTextNode(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var data = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.CreateTextNode(data));
        }

        JsValue CreateNodeIterator(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var root = DomTypeConverter.ToNode(arguments.At(0));
            var settings = DomTypeConverter.ToFilterSettings(arguments.At(1));
            var filter = DomTypeConverter.ToNodeFilter(arguments.At(2));
            return Engine.Select(reference.CreateNodeIterator(root, settings, filter));
        }

        JsValue CreateTreeWalker(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var root = DomTypeConverter.ToNode(arguments.At(0));
            var settings = DomTypeConverter.ToFilterSettings(arguments.At(1));
            var filter = DomTypeConverter.ToNodeFilter(arguments.At(2));
            return Engine.Select(reference.CreateTreeWalker(root, settings, filter));
        }

        JsValue ImportNode(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var externalNode = DomTypeConverter.ToNode(arguments.At(0));
            var deep = TypeConverter.ToBoolean(arguments.At(1));
            return Engine.Select(reference.Import(externalNode, deep));
        }

        JsValue AdoptNode(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var externalNode = DomTypeConverter.ToNode(arguments.At(0));
            return Engine.Select(reference.Adopt(externalNode));
        }

        JsValue HasFocus(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.HasFocus());
        }

        JsValue ExecCommand(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var commandId = TypeConverter.ToString(arguments.At(0));
            var showUserInterface = TypeConverter.ToBoolean(arguments.At(1));
            var value = TypeConverter.ToString(arguments.At(2));
            return Engine.Select(reference.ExecuteCommand(commandId, showUserInterface, value));
        }

        JsValue QueryCommandEnabled(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var commandId = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.IsCommandEnabled(commandId));
        }

        JsValue QueryCommandIndeterm(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var commandId = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.IsCommandIndeterminate(commandId));
        }

        JsValue QueryCommandState(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var commandId = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.IsCommandExecuted(commandId));
        }

        JsValue QueryCommandSupported(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var commandId = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.IsCommandSupported(commandId));
        }

        JsValue QueryCommandValue(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var commandId = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.GetCommandValue(commandId));
        }

        JsValue Append(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Append(nodes);
            return JsValue.Undefined;
        }

        JsValue Prepend(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Prepend(nodes);
            return JsValue.Undefined;
        }

        JsValue QuerySelector(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelector(selectors));
        }

        JsValue QuerySelectorAll(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelectorAll(selectors));
        }

        JsValue EnableStyleSheetsForSet(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var name = TypeConverter.ToString(arguments.At(0));
            reference.EnableStyleSheetsForSet(name);
            return JsValue.Undefined;
        }

        JsValue GetElementById(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var elementId = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.GetElementById(elementId));
        }

        JsValue GetAll(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.All);
        }


        JsValue GetAnchors(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.Anchors);
        }


        JsValue GetImplementation(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.Implementation);
        }


        JsValue GetDesignMode(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.DesignMode);
        }

        void SetDesignMode(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var value = TypeConverter.ToString(argument);
            reference.DesignMode = value;
        }

        JsValue GetDir(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.Direction);
        }

        void SetDir(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var value = TypeConverter.ToString(argument);
            reference.Direction = value;
        }

        JsValue GetDocumentURI(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.DocumentUri);
        }


        JsValue GetCharacterSet(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.CharacterSet);
        }


        JsValue GetCompatMode(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.CompatMode);
        }


        JsValue GetURL(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.Url);
        }


        JsValue GetContentType(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.ContentType);
        }


        JsValue GetDoctype(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.Doctype);
        }


        JsValue GetDocumentElement(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.DocumentElement);
        }


        JsValue GetLastModified(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.LastModified);
        }


        JsValue GetReadyState(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.ReadyState);
        }


        JsValue GetLocation(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.Location);
        }


        JsValue GetForms(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.Forms);
        }


        JsValue GetImages(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.Images);
        }


        JsValue GetScripts(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.Scripts);
        }


        JsValue GetEmbeds(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.Plugins);
        }


        JsValue GetPlugins(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.Plugins);
        }


        JsValue GetCommands(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.Commands);
        }


        JsValue GetLinks(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.Links);
        }


        JsValue GetTitle(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.Title);
        }

        void SetTitle(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var value = TypeConverter.ToString(argument);
            reference.Title = value;
        }

        JsValue GetHead(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.Head);
        }


        JsValue GetBody(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.Body);
        }

        void SetBody(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var value = DomTypeConverter.ToHtmlElement(argument);
            reference.Body = value;
        }

        JsValue GetCookie(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.Cookie);
        }

        void SetCookie(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var value = TypeConverter.ToString(argument);
            reference.Cookie = value;
        }

        JsValue GetOrigin(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.Origin);
        }


        JsValue GetDomain(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.Domain);
        }

        void SetDomain(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var value = TypeConverter.ToString(argument);
            reference.Domain = value;
        }

        JsValue GetReferrer(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.Referrer);
        }


        JsValue GetActiveElement(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.ActiveElement);
        }


        JsValue GetCurrentScript(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.CurrentScript);
        }


        JsValue GetDefaultView(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.DefaultView);
        }


        JsValue GetChildren(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.Children);
        }


        JsValue GetFirstElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.FirstElementChild);
        }


        JsValue GetLastElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.LastElementChild);
        }


        JsValue GetChildElementCount(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.ChildElementCount);
        }


        JsValue GetStyleSheets(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.StyleSheets);
        }


        JsValue GetSelectedStyleSheetSet(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.SelectedStyleSheetSet);
        }

        void SetSelectedStyleSheetSet(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            var value = TypeConverter.ToString(argument);
            reference.SelectedStyleSheetSet = value;
        }

        JsValue GetLastStyleSheetSet(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.LastStyleSheetSet);
        }


        JsValue GetPreferredStyleSheetSet(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.PreferredStyleSheetSet);
        }


        JsValue GetStyleSheetSets(JsValue thisObj)
        {
            var reference = thisObj.TryCast<DocumentInstance>(Fail).RefDocument;
            return Engine.Select(reference.StyleSheetSets);
        }


        JsValue GetReadyStateChangedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onreadystatechange");
        }

        void SetReadyStateChangedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetReadyStateChangedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.ReadyStateChanged -= method;
            }

            instance.Put("onreadystatechange", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.ReadyStateChanged += method;
            }
        }

        JsValue GetAbortedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onabort");
        }

        void SetAbortedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetAbortedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Aborted -= method;
            }

            instance.Put("onabort", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Aborted += method;
            }
        }

        JsValue GetBlurredEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onblur");
        }

        void SetBlurredEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetBlurredEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Blurred -= method;
            }

            instance.Put("onblur", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Blurred += method;
            }
        }

        JsValue GetCancelledEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("oncancel");
        }

        void SetCancelledEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetCancelledEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Cancelled -= method;
            }

            instance.Put("oncancel", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Cancelled += method;
            }
        }

        JsValue GetCanPlayEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("oncanplay");
        }

        void SetCanPlayEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetCanPlayEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.CanPlay -= method;
            }

            instance.Put("oncanplay", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.CanPlay += method;
            }
        }

        JsValue GetCanPlayThroughEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("oncanplaythrough");
        }

        void SetCanPlayThroughEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetCanPlayThroughEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.CanPlayThrough -= method;
            }

            instance.Put("oncanplaythrough", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.CanPlayThrough += method;
            }
        }

        JsValue GetChangedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onchange");
        }

        void SetChangedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetChangedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Changed -= method;
            }

            instance.Put("onchange", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Changed += method;
            }
        }

        JsValue GetClickedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onclick");
        }

        void SetClickedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetClickedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Clicked -= method;
            }

            instance.Put("onclick", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Clicked += method;
            }
        }

        JsValue GetCueChangedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("oncuechange");
        }

        void SetCueChangedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetCueChangedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.CueChanged -= method;
            }

            instance.Put("oncuechange", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.CueChanged += method;
            }
        }

        JsValue GetDoubleClickEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("ondblclick");
        }

        void SetDoubleClickEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetDoubleClickEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.DoubleClick -= method;
            }

            instance.Put("ondblclick", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.DoubleClick += method;
            }
        }

        JsValue GetDragEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("ondrag");
        }

        void SetDragEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetDragEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Drag -= method;
            }

            instance.Put("ondrag", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Drag += method;
            }
        }

        JsValue GetDragEndEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("ondragend");
        }

        void SetDragEndEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetDragEndEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.DragEnd -= method;
            }

            instance.Put("ondragend", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.DragEnd += method;
            }
        }

        JsValue GetDragEnterEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("ondragenter");
        }

        void SetDragEnterEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetDragEnterEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.DragEnter -= method;
            }

            instance.Put("ondragenter", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.DragEnter += method;
            }
        }

        JsValue GetDragExitEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("ondragexit");
        }

        void SetDragExitEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetDragExitEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.DragExit -= method;
            }

            instance.Put("ondragexit", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.DragExit += method;
            }
        }

        JsValue GetDragLeaveEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("ondragleave");
        }

        void SetDragLeaveEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetDragLeaveEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.DragLeave -= method;
            }

            instance.Put("ondragleave", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.DragLeave += method;
            }
        }

        JsValue GetDragOverEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("ondragover");
        }

        void SetDragOverEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetDragOverEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.DragOver -= method;
            }

            instance.Put("ondragover", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.DragOver += method;
            }
        }

        JsValue GetDragStartEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("ondragstart");
        }

        void SetDragStartEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetDragStartEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.DragStart -= method;
            }

            instance.Put("ondragstart", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.DragStart += method;
            }
        }

        JsValue GetDroppedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("ondrop");
        }

        void SetDroppedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetDroppedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Dropped -= method;
            }

            instance.Put("ondrop", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Dropped += method;
            }
        }

        JsValue GetDurationChangedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("ondurationchange");
        }

        void SetDurationChangedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetDurationChangedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.DurationChanged -= method;
            }

            instance.Put("ondurationchange", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.DurationChanged += method;
            }
        }

        JsValue GetEmptiedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onemptied");
        }

        void SetEmptiedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetEmptiedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Emptied -= method;
            }

            instance.Put("onemptied", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Emptied += method;
            }
        }

        JsValue GetEndedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onended");
        }

        void SetEndedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetEndedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Ended -= method;
            }

            instance.Put("onended", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Ended += method;
            }
        }

        JsValue GetErrorEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onerror");
        }

        void SetErrorEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetErrorEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Error -= method;
            }

            instance.Put("onerror", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Error += method;
            }
        }

        JsValue GetFocusedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onfocus");
        }

        void SetFocusedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetFocusedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Focused -= method;
            }

            instance.Put("onfocus", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Focused += method;
            }
        }

        JsValue GetInputEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("oninput");
        }

        void SetInputEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetInputEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Input -= method;
            }

            instance.Put("oninput", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Input += method;
            }
        }

        JsValue GetInvalidEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("oninvalid");
        }

        void SetInvalidEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetInvalidEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Invalid -= method;
            }

            instance.Put("oninvalid", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Invalid += method;
            }
        }

        JsValue GetKeyDownEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onkeydown");
        }

        void SetKeyDownEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetKeyDownEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.KeyDown -= method;
            }

            instance.Put("onkeydown", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.KeyDown += method;
            }
        }

        JsValue GetKeyPressEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onkeypress");
        }

        void SetKeyPressEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetKeyPressEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.KeyPress -= method;
            }

            instance.Put("onkeypress", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.KeyPress += method;
            }
        }

        JsValue GetKeyUpEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onkeyup");
        }

        void SetKeyUpEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetKeyUpEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.KeyUp -= method;
            }

            instance.Put("onkeyup", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.KeyUp += method;
            }
        }

        JsValue GetLoadedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onload");
        }

        void SetLoadedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetLoadedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Loaded -= method;
            }

            instance.Put("onload", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Loaded += method;
            }
        }

        JsValue GetLoadedDataEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onloadeddata");
        }

        void SetLoadedDataEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetLoadedDataEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.LoadedData -= method;
            }

            instance.Put("onloadeddata", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.LoadedData += method;
            }
        }

        JsValue GetLoadedMetadataEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onloadedmetadata");
        }

        void SetLoadedMetadataEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetLoadedMetadataEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.LoadedMetadata -= method;
            }

            instance.Put("onloadedmetadata", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.LoadedMetadata += method;
            }
        }

        JsValue GetLoadingEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onloadstart");
        }

        void SetLoadingEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetLoadingEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Loading -= method;
            }

            instance.Put("onloadstart", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Loading += method;
            }
        }

        JsValue GetMouseDownEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onmousedown");
        }

        void SetMouseDownEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetMouseDownEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.MouseDown -= method;
            }

            instance.Put("onmousedown", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.MouseDown += method;
            }
        }

        JsValue GetMouseEnterEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onmouseenter");
        }

        void SetMouseEnterEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetMouseEnterEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.MouseEnter -= method;
            }

            instance.Put("onmouseenter", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.MouseEnter += method;
            }
        }

        JsValue GetMouseLeaveEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onmouseleave");
        }

        void SetMouseLeaveEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetMouseLeaveEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.MouseLeave -= method;
            }

            instance.Put("onmouseleave", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.MouseLeave += method;
            }
        }

        JsValue GetMouseMoveEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onmousemove");
        }

        void SetMouseMoveEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetMouseMoveEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.MouseMove -= method;
            }

            instance.Put("onmousemove", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.MouseMove += method;
            }
        }

        JsValue GetMouseOutEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onmouseout");
        }

        void SetMouseOutEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetMouseOutEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.MouseOut -= method;
            }

            instance.Put("onmouseout", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.MouseOut += method;
            }
        }

        JsValue GetMouseOverEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onmouseover");
        }

        void SetMouseOverEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetMouseOverEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.MouseOver -= method;
            }

            instance.Put("onmouseover", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.MouseOver += method;
            }
        }

        JsValue GetMouseUpEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onmouseup");
        }

        void SetMouseUpEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetMouseUpEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.MouseUp -= method;
            }

            instance.Put("onmouseup", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.MouseUp += method;
            }
        }

        JsValue GetMouseWheelEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onmousewheel");
        }

        void SetMouseWheelEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetMouseWheelEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.MouseWheel -= method;
            }

            instance.Put("onmousewheel", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.MouseWheel += method;
            }
        }

        JsValue GetPausedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onpause");
        }

        void SetPausedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetPausedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Paused -= method;
            }

            instance.Put("onpause", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Paused += method;
            }
        }

        JsValue GetPlayedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onplay");
        }

        void SetPlayedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetPlayedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Played -= method;
            }

            instance.Put("onplay", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Played += method;
            }
        }

        JsValue GetPlayingEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onplaying");
        }

        void SetPlayingEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetPlayingEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Playing -= method;
            }

            instance.Put("onplaying", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Playing += method;
            }
        }

        JsValue GetProgressEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onprogress");
        }

        void SetProgressEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetProgressEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Progress -= method;
            }

            instance.Put("onprogress", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Progress += method;
            }
        }

        JsValue GetRateChangedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onratechange");
        }

        void SetRateChangedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetRateChangedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.RateChanged -= method;
            }

            instance.Put("onratechange", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.RateChanged += method;
            }
        }

        JsValue GetResettedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onreset");
        }

        void SetResettedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetResettedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Resetted -= method;
            }

            instance.Put("onreset", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Resetted += method;
            }
        }

        JsValue GetResizedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onresize");
        }

        void SetResizedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetResizedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Resized -= method;
            }

            instance.Put("onresize", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Resized += method;
            }
        }

        JsValue GetScrolledEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onscroll");
        }

        void SetScrolledEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetScrolledEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Scrolled -= method;
            }

            instance.Put("onscroll", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Scrolled += method;
            }
        }

        JsValue GetSeekedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onseeked");
        }

        void SetSeekedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetSeekedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Seeked -= method;
            }

            instance.Put("onseeked", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Seeked += method;
            }
        }

        JsValue GetSeekingEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onseeking");
        }

        void SetSeekingEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetSeekingEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Seeking -= method;
            }

            instance.Put("onseeking", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Seeking += method;
            }
        }

        JsValue GetSelectedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onselect");
        }

        void SetSelectedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetSelectedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Selected -= method;
            }

            instance.Put("onselect", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Selected += method;
            }
        }

        JsValue GetShownEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onshow");
        }

        void SetShownEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetShownEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Shown -= method;
            }

            instance.Put("onshow", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Shown += method;
            }
        }

        JsValue GetStalledEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onstalled");
        }

        void SetStalledEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetStalledEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Stalled -= method;
            }

            instance.Put("onstalled", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Stalled += method;
            }
        }

        JsValue GetSubmittedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onsubmit");
        }

        void SetSubmittedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetSubmittedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Submitted -= method;
            }

            instance.Put("onsubmit", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Submitted += method;
            }
        }

        JsValue GetSuspendedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onsuspend");
        }

        void SetSuspendedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetSuspendedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Suspended -= method;
            }

            instance.Put("onsuspend", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Suspended += method;
            }
        }

        JsValue GetTimeUpdatedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("ontimeupdate");
        }

        void SetTimeUpdatedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetTimeUpdatedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.TimeUpdated -= method;
            }

            instance.Put("ontimeupdate", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.TimeUpdated += method;
            }
        }

        JsValue GetToggledEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("ontoggle");
        }

        void SetToggledEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetToggledEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Toggled -= method;
            }

            instance.Put("ontoggle", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Toggled += method;
            }
        }

        JsValue GetVolumeChangedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onvolumechange");
        }

        void SetVolumeChangedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetVolumeChangedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.VolumeChanged -= method;
            }

            instance.Put("onvolumechange", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.VolumeChanged += method;
            }
        }

        JsValue GetWaitingEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            return instance.Get("onwaiting");
        }

        void SetWaitingEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<DocumentInstance>(Fail);
            var reference = instance.RefDocument;
            var existing = GetWaitingEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.Waiting -= method;
            }

            instance.Put("onwaiting", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.Waiting += method;
            }
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object Document]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}