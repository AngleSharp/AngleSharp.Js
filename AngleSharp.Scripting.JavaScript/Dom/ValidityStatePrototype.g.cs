namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Dom.Html;
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Descriptors;
    using Jint.Runtime.Interop;
    using System;

    sealed partial class ValidityStatePrototype : ValidityStateInstance
    {
        public ValidityStatePrototype(Engine engine)
            : base(engine)
        {
            FastAddProperty("toString", Engine.AsValue(ToString), true, true, true);
            FastSetProperty("valueMissing", Engine.AsProperty(GetValueMissing));
            FastSetProperty("typeMismatch", Engine.AsProperty(GetTypeMismatch));
            FastSetProperty("patternMismatch", Engine.AsProperty(GetPatternMismatch));
            FastSetProperty("tooLong", Engine.AsProperty(GetTooLong));
            FastSetProperty("tooShort", Engine.AsProperty(GetTooShort));
            FastSetProperty("badInput", Engine.AsProperty(GetBadInput));
            FastSetProperty("rangeUnderflow", Engine.AsProperty(GetRangeUnderflow));
            FastSetProperty("rangeOverflow", Engine.AsProperty(GetRangeOverflow));
            FastSetProperty("stepMismatch", Engine.AsProperty(GetStepMismatch));
            FastSetProperty("customError", Engine.AsProperty(GetCustomError));
            FastSetProperty("valid", Engine.AsProperty(GetValid));
        }

        public static ValidityStatePrototype CreatePrototypeObject(EngineInstance engine, ValidityStateConstructor constructor)
        {
            var obj = new ValidityStatePrototype(engine.Jint)
            {
                Prototype = engine.Constructors.Object.PrototypeObject,
                Extensible = true,
            };
            obj.FastAddProperty("constructor", constructor, true, false, true);
            return obj;
        }

        JsValue GetValueMissing(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ValidityStateInstance>(Fail).RefValidityState;
            return Engine.Select(reference.IsValueMissing);
        }


        JsValue GetTypeMismatch(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ValidityStateInstance>(Fail).RefValidityState;
            return Engine.Select(reference.IsTypeMismatch);
        }


        JsValue GetPatternMismatch(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ValidityStateInstance>(Fail).RefValidityState;
            return Engine.Select(reference.IsPatternMismatch);
        }


        JsValue GetTooLong(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ValidityStateInstance>(Fail).RefValidityState;
            return Engine.Select(reference.IsTooLong);
        }


        JsValue GetTooShort(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ValidityStateInstance>(Fail).RefValidityState;
            return Engine.Select(reference.IsTooShort);
        }


        JsValue GetBadInput(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ValidityStateInstance>(Fail).RefValidityState;
            return Engine.Select(reference.IsBadInput);
        }


        JsValue GetRangeUnderflow(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ValidityStateInstance>(Fail).RefValidityState;
            return Engine.Select(reference.IsRangeUnderflow);
        }


        JsValue GetRangeOverflow(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ValidityStateInstance>(Fail).RefValidityState;
            return Engine.Select(reference.IsRangeOverflow);
        }


        JsValue GetStepMismatch(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ValidityStateInstance>(Fail).RefValidityState;
            return Engine.Select(reference.IsStepMismatch);
        }


        JsValue GetCustomError(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ValidityStateInstance>(Fail).RefValidityState;
            return Engine.Select(reference.IsCustomError);
        }


        JsValue GetValid(JsValue thisObj)
        {
            var reference = thisObj.TryCast<ValidityStateInstance>(Fail).RefValidityState;
            return Engine.Select(reference.IsValid);
        }


        JsValue ToString(JsValue thisObj, JsValue[] arguments)
        {
            return "[object ValidityState]";
        }

        void Fail(JsValue obsolete)
        {
            throw new JavaScriptException(Engine.TypeError);
        }
    }
}