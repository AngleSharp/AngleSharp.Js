namespace AngleSharp.Js
{
    using AngleSharp.Attributes;
    using Jint;
    using Jint.Native;
    using Jint.Native.Function;
    using Jint.Native.Object;
    using Jint.Runtime;
    using Jint.Runtime.Interop;
    using System.Linq;
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
                var f = obj as Function;

                if (f == null && obj is String b)
                {
                    var e = engine.Jint;
                    var p = new[] { JsValue.FromObjectWithType(e, b, typeof(String)) };
                    f = new ClrFunction(e, "AsComplex", (_this, args) => e.Intrinsics.Eval.Call(_this, p));
                }

                if (f != null)
                {
                    return targetType.ToDelegate(f, engine);
                }
            }

            if (obj is ObjectInstance dictionary && TryCreateDictionary(dictionary, targetType, engine, out var created))
            {
                return created;
            }

            var method = sourceType.PrepareConvert(targetType) ??
                throw new JavaScriptException("[Internal] Could not find corresponding cast target.");

            return method.Invoke(obj, null);
        }

        /// <summary>
        /// Converts a plain object into a WebIDL dictionary, i.e. a class whose members carry a
        /// [DomName] (e.g. { top: 100, behavior: 'smooth' } into ScrollToOptions). Members the
        /// object lacks keep their default.
        /// </summary>
        private static Boolean TryCreateDictionary(ObjectInstance source, Type targetType, EngineInstance engine, out Object result)
        {
            var typeInfo = targetType.GetTypeInfo();
            result = null;

            if (!typeInfo.IsClass || typeInfo.IsAbstract || targetType == typeof(String) || targetType.GetConstructor(Type.EmptyTypes) == null)
            {
                return false;
            }

            var members = targetType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.SetMethod != null && m.SetMethod.IsPublic && m.GetIndexParameters().Length == 0)
                .Select(m => new { Property = m, Name = m.GetCustomAttribute<DomNameAttribute>()?.OfficialName })
                .Where(m => m.Name != null)
                .ToArray();

            if (members.Length == 0)
            {
                return false;
            }

            result = Activator.CreateInstance(targetType);

            foreach (var member in members)
            {
                var value = source.Get(member.Name);

                if (!value.IsUndefined())
                {
                    member.Property.SetValue(result, value.As(member.Property.PropertyType, engine));
                }
            }

            return true;
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
                    var node = obj as IDomProxy;
                    return node != null ? node.Value : obj;
                case Types.Undefined:
                    return JsValue.Undefined.ToString();
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
                else if (targetType == typeof(Nullable<Int32>))
                {
                    if (value.IsUndefined())
                    {
                        return null;
                    }
                    else
                    {
                        return TypeConverter.ToInt32(value);
                    }
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
                else if (targetType.GetTypeInfo().IsEnum)
                {
                    if (value.IsString())
                    {
                        var literal = TypeConverter.ToString(value);
                        var member = targetType
                            .GetTypeInfo()
                            .DeclaredFields
                            .Where(m => m.IsLiteral)
                            .FirstOrDefault(m => m.GetCustomAttributes<DomNameAttribute>().FirstOrDefault()?.OfficialName == literal || m.Name == literal);

                        if (member != null)
                        {
                            return Enum.Parse(targetType, member.Name);
                        }
                    }

                    var underlyingType = Enum.GetUnderlyingType(targetType);
                    var raw = Convert.ChangeType(TypeConverter.ToNumber(value), underlyingType);
                    return Enum.ToObject(targetType, raw);
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
