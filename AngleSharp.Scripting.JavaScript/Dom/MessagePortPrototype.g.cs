namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Events;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class MessagePortPrototype : MessagePortInstance
    {
        readonly EngineInstance _engine;

        public MessagePortPrototype(EngineInstance engine)
            : base(engine)
        {
            _engine = engine;
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastAddProperty("postMessage", Engine.AsValue(PostMessage), true, true, true);
            FastAddProperty("start", Engine.AsValue(Start), true, true, true);
            FastAddProperty("close", Engine.AsValue(Close), true, true, true);
            FastSetProperty("onmessage", Engine.AsProperty(GetMessageReceivedEvent, SetMessageReceivedEvent));
        }

        public static MessagePortPrototype CreatePrototypeObject(EngineInstance engine, MessagePortConstructor constructor)
        {
            var obj = new MessagePortPrototype(engine)
            {
                Prototype = engine.Constructors.EventTarget.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue PostMessage(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<MessagePortInstance>(Fail).RefMessagePort;
            var message = SystemTypeConverter.ToObject(arguments.At(0));
            reference.Send(message);
            return JsValue.Undefined;
        }

        JsValue Start(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<MessagePortInstance>(Fail).RefMessagePort;
            reference.Open();
            return JsValue.Undefined;
        }

        JsValue Close(JsValue thisObj, JsValue[] arguments)
        {
            var reference = thisObj.TryCast<MessagePortInstance>(Fail).RefMessagePort;
            reference.Close();
            return JsValue.Undefined;
        }

        JsValue GetMessageReceivedEvent(JsValue thisObj)
        {
            var instance = thisObj.TryCast<MessagePortInstance>(Fail);
            return instance.Get("onmessage");
        }

        void SetMessageReceivedEvent(JsValue thisObj, JsValue argument)
        {
            var instance = thisObj.TryCast<MessagePortInstance>(Fail);
            var reference = instance.RefMessagePort;
            var existing = GetMessageReceivedEvent(thisObj);

            if (existing != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(existing);
                reference.MessageReceived -= method;
            }

            instance.Put("onmessage", argument, false);

            if (argument != JsValue.Undefined)
            {
                var method = DomTypeConverter.ToEventHandler(argument);
                reference.MessageReceived += method;
            }
        }

        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object MessagePort]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}