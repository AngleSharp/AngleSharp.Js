namespace AngleSharp.Js
{
    using Jint.Native;
    using Jint.Native.Function;
    using Jint.Runtime;
    using Jint.Runtime.Interop;
    using System;
    using System.Reflection;

    static class JsValueExtensions
    {
        private static Object AsComplex(this JsValue value, Type targetType, EngineInstance engine)
        {
            var obj = value.FromJsValue();
            var sourceType = obj.GetType();

            if (sourceType == targetType || sourceType.GetTypeInfo().IsSubclassOf(targetType) || targetType.GetTypeInfo().IsAssignableFrom(sourceType.GetTypeInfo()))
            {
                return obj;
            }
            else if (targetType.GetTypeInfo().IsSubclassOf(typeof(Delegate)))
            {
                var f = obj as FunctionInstance;

                if (f == null && obj is String b)
                {
                    var e = engine.Jint;
                    var p = new[] { new JsValue(b) };
                    f = new ClrFunctionInstance(e, (_this, args) => e.Eval.Call(_this, p));
                }

                if (f != null)
                {
                    return targetType.ToDelegate(f, engine);
                }
            }

            var method = sourceType.PrepareConvert(targetType) ??
                throw new JavaScriptException("[Internal] Could not find corresponding cast target.");

            return method.Invoke(obj, null);
        }

        public static Object FromJsValue(this JsValue val)
        {
            switch (val.Type)
            {
                case Types.Boolean:
                    return val.AsBoolean();
                case Types.Number:
                    return val.AsNumber();
                case Types.String:
                    return val.AsString();
                case Types.Object:
                    var obj = val.AsObject();
                    var node = obj as DomNodeInstance;
                    return node != null ? node.Value : obj;
                case Types.Undefined:
                    return Undefined.Text;
                case Types.Null:
                    return null;
            }

            return val.ToObject();
        }

        public static Object As(this JsValue value, Type targetType, EngineInstance engine)
        {
            if (value != JsValue.Null)
            {
                if (targetType == typeof(Int32))
                {
                    return TypeConverter.ToInt32(value);
                }
                else if (targetType == typeof(Double))
                {
                    return TypeConverter.ToNumber(value);
                }
                else if (targetType == typeof(String))
                {
                    return value.IsPrimitive() ? TypeConverter.ToString(value) : value.ToString();
                }
                else if (targetType == typeof(Boolean))
                {
                    return TypeConverter.ToBoolean(value);
                }
                else if (targetType == typeof(UInt32))
                {
                    return TypeConverter.ToUint32(value);
                }
                else if (targetType == typeof(UInt16))
                {
                    return TypeConverter.ToUint16(value);
                }
                else
                {
                    return value.AsComplex(targetType, engine);
                }
            }

            return null;
        }
    }
}
