namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Xml;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class XMLDocumentPrototype : XMLDocumentInstance
    {
        public XMLDocumentPrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("append", Engine.AsValue(Append), true, true, true);
            FastAddProperty("prepend", Engine.AsValue(Prepend), true, true, true);
            FastAddProperty("querySelector", Engine.AsValue(QuerySelector), true, true, true);
            FastAddProperty("querySelectorAll", Engine.AsValue(QuerySelectorAll), true, true, true);
            FastAddProperty("enableStyleSheetsForSet", Engine.AsValue(EnableStyleSheetsForSet), true, true, true);
            FastAddProperty("getElementById", Engine.AsValue(GetElementById), true, true, true);
            FastSetProperty("children", Engine.AsProperty(GetChildren));
            FastSetProperty("firstElementChild", Engine.AsProperty(GetFirstElementChild));
            FastSetProperty("lastElementChild", Engine.AsProperty(GetLastElementChild));
            FastSetProperty("childElementCount", Engine.AsProperty(GetChildElementCount));
            FastSetProperty("styleSheets", Engine.AsProperty(GetStyleSheets));
            FastSetProperty("selectedStyleSheetSet", Engine.AsProperty(GetSelectedStyleSheetSet, SetSelectedStyleSheetSet));
            FastSetProperty("lastStyleSheetSet", Engine.AsProperty(GetLastStyleSheetSet));
            FastSetProperty("preferredStyleSheetSet", Engine.AsProperty(GetPreferredStyleSheetSet));
            FastSetProperty("styleSheetSets", Engine.AsProperty(GetStyleSheetSets));
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

        public static XMLDocumentPrototype CreatePrototypeObject(EngineInstance engine, XMLDocumentConstructor constructor)
        {
            var obj = new XMLDocumentPrototype(engine.Jint)
            {
                Prototype = engine.Constructors.Document.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue Append(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<XMLDocumentInstance>(Fail).RefXMLDocument;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Append(nodes);
            return JsValue.Undefined;
        }

        JsValue Prepend(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<XMLDocumentInstance>(Fail).RefXMLDocument;
            var nodes = DomTypeConverter.ToNodeArray(arguments.At(0));
            reference.Prepend(nodes);
            return JsValue.Undefined;
        }

        JsValue QuerySelector(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<XMLDocumentInstance>(Fail).RefXMLDocument;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelector(selectors));
        }

        JsValue QuerySelectorAll(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<XMLDocumentInstance>(Fail).RefXMLDocument;
            var selectors = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.QuerySelectorAll(selectors));
        }

        JsValue EnableStyleSheetsForSet(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<XMLDocumentInstance>(Fail).RefXMLDocument;
            var name = TypeConverter.ToString(arguments.At(0));
            reference.EnableStyleSheetsForSet(name);
            return JsValue.Undefined;
        }

        JsValue GetElementById(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<XMLDocumentInstance>(Fail).RefXMLDocument;
            var elementId = TypeConverter.ToString(arguments.At(0));
            return Engine.Select(reference.GetElementById(elementId));
        }

        JsValue GetChildren(JsValue thisObj)
        {
            var reference = thisObj.TryCast<XMLDocumentInstance>(Fail).RefXMLDocument;
            return Engine.Select(reference.Children);
        }


        JsValue GetFirstElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<XMLDocumentInstance>(Fail).RefXMLDocument;
            return Engine.Select(reference.FirstElementChild);
        }


        JsValue GetLastElementChild(JsValue thisObj)
        {
            var reference = thisObj.TryCast<XMLDocumentInstance>(Fail).RefXMLDocument;
            return Engine.Select(reference.LastElementChild);
        }


        JsValue GetChildElementCount(JsValue thisObj)
        {
            var reference = thisObj.TryCast<XMLDocumentInstance>(Fail).RefXMLDocument;
            return Engine.Select(reference.ChildElementCount);
        }


        JsValue GetStyleSheets(JsValue thisObj)
        {
            var reference = thisObj.TryCast<XMLDocumentInstance>(Fail).RefXMLDocument;
            return Engine.Select(reference.StyleSheets);
        }


        JsValue GetSelectedStyleSheetSet(JsValue thisObj)
        {
            var reference = thisObj.TryCast<XMLDocumentInstance>(Fail).RefXMLDocument;
            return Engine.Select(reference.SelectedStyleSheetSet);
        }

        void SetSelectedStyleSheetSet(JsValue thisObj, JsValue argument)
        {
            var reference = thisObj.TryCast<XMLDocumentInstance>(Fail).RefXMLDocument;
            var value = TypeConverter.ToString(argument);
            reference.SelectedStyleSheetSet = value;
        }

        JsValue GetLastStyleSheetSet(JsValue thisObj)
        {
            var reference = thisObj.TryCast<XMLDocumentInstance>(Fail).RefXMLDocument;
            return Engine.Select(reference.LastStyleSheetSet);
        }


        JsValue GetPreferredStyleSheetSet(JsValue thisObj)
        {
            var reference = thisObj.TryCast<XMLDocumentInstance>(Fail).RefXMLDocument;
            return Engine.Select(reference.PreferredStyleSheetSet);
        }


        JsValue GetStyleSheetSets(JsValue thisObj)
        {
            var reference = thisObj.TryCast<XMLDocumentInstance>(Fail).RefXMLDocument;
            return Engine.Select(reference.StyleSheetSets);
        }


        JsValue GetAbortedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onabort");
        }

        void SetAbortedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onblur");
        }

        void SetBlurredEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("oncancel");
        }

        void SetCancelledEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("oncanplay");
        }

        void SetCanPlayEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("oncanplaythrough");
        }

        void SetCanPlayThroughEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onchange");
        }

        void SetChangedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onclick");
        }

        void SetClickedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("oncuechange");
        }

        void SetCueChangedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("ondblclick");
        }

        void SetDoubleClickEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("ondrag");
        }

        void SetDragEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("ondragend");
        }

        void SetDragEndEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("ondragenter");
        }

        void SetDragEnterEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("ondragexit");
        }

        void SetDragExitEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("ondragleave");
        }

        void SetDragLeaveEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("ondragover");
        }

        void SetDragOverEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("ondragstart");
        }

        void SetDragStartEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("ondrop");
        }

        void SetDroppedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("ondurationchange");
        }

        void SetDurationChangedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onemptied");
        }

        void SetEmptiedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onended");
        }

        void SetEndedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onerror");
        }

        void SetErrorEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onfocus");
        }

        void SetFocusedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("oninput");
        }

        void SetInputEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("oninvalid");
        }

        void SetInvalidEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onkeydown");
        }

        void SetKeyDownEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onkeypress");
        }

        void SetKeyPressEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onkeyup");
        }

        void SetKeyUpEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onload");
        }

        void SetLoadedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onloadeddata");
        }

        void SetLoadedDataEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onloadedmetadata");
        }

        void SetLoadedMetadataEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onloadstart");
        }

        void SetLoadingEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onmousedown");
        }

        void SetMouseDownEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onmouseenter");
        }

        void SetMouseEnterEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onmouseleave");
        }

        void SetMouseLeaveEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onmousemove");
        }

        void SetMouseMoveEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onmouseout");
        }

        void SetMouseOutEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onmouseover");
        }

        void SetMouseOverEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onmouseup");
        }

        void SetMouseUpEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onmousewheel");
        }

        void SetMouseWheelEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onpause");
        }

        void SetPausedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onplay");
        }

        void SetPlayedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onplaying");
        }

        void SetPlayingEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onprogress");
        }

        void SetProgressEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onratechange");
        }

        void SetRateChangedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onreset");
        }

        void SetResettedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onresize");
        }

        void SetResizedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onscroll");
        }

        void SetScrolledEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onseeked");
        }

        void SetSeekedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onseeking");
        }

        void SetSeekingEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onselect");
        }

        void SetSelectedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onshow");
        }

        void SetShownEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onstalled");
        }

        void SetStalledEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onsubmit");
        }

        void SetSubmittedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onsuspend");
        }

        void SetSuspendedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("ontimeupdate");
        }

        void SetTimeUpdatedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("ontoggle");
        }

        void SetToggledEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onvolumechange");
        }

        void SetVolumeChangedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            return instance.Get("onwaiting");
        }

        void SetWaitingEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<XMLDocumentInstance>(Fail);
            var reference = instance.RefXMLDocument;
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
            return "[object XMLDocument]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}