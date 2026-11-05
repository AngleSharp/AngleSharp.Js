namespace AngleSharp.Js
{
    using AngleSharp.Attributes;
    using AngleSharp.Dom;
    using AngleSharp.Js.Cache;
    using Jint;
    using Jint.Native;
    using Jint.Native.Function;
    using Jint.Native.Object;
    using Jint.Native.Symbol;
    using Jint.Runtime;
    using Jint.Runtime.Interop;
    using System.Linq;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;

    static class JsValueExtensions
    {
        //  The one kind of buffer Jint has no public type for.
        private static readonly Type DataViewType = typeof(Engine).GetTypeInfo().Assembly.GetType("Jint.Native.JsDataView");

        /// <summary>
        /// Checks for a WebIDL BufferSource, i.e. an ArrayBuffer or a view on one.
        /// </summary>
        public static Boolean IsBufferSource(this JsValue value) =>
            value is JsArrayBuffer || value is JsTypedArray || (value is ObjectInstance && value.GetType() == DataViewType);

        /// <summary>
        /// Gets the element type if the type stands for a WebIDL sequence, otherwise null.
        /// </summary>
        public static Type GetSequenceElementType(this Type type)
        {
            if (type.IsArray)
            {
                return type.GetArrayRank() == 1 ? type.GetElementType() : null;
            }

            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsInterface && typeInfo.IsGenericType && typeInfo.GenericTypeArguments.Length == 1)
            {
                var elementType = typeInfo.GenericTypeArguments[0];
                return typeInfo.IsAssignableFrom(elementType.MakeArrayType().GetTypeInfo()) ? elementType : null;
            }

            return null;
        }

        //  Copied rather than shared: the view may only cover a part of its buffer, and the CLR
        //  side keeps what it was given while the script is free to change the buffer afterwards.
        internal static Byte[] GetBytes(this ObjectInstance source)
        {
            if (source is JsArrayBuffer buffer)
            {
                return (Byte[])((Byte[])buffer.ToObject()).Clone();
            }

            var whole = (Byte[])source.Get("buffer").ToObject();
            var offset = (Int32)TypeConverter.ToNumber(source.Get("byteOffset"));
            var length = (Int32)TypeConverter.ToNumber(source.Get("byteLength"));
            var bytes = new Byte[length];
            Buffer.BlockCopy(whole, offset, bytes, 0, length);
            return bytes;
        }

        private static Boolean TryCreateSequence(ObjectInstance source, Type targetType, EngineInstance engine, out Object result)
        {
            var elementType = targetType.GetSequenceElementType();
            result = null;

            if (elementType == null)
            {
                return false;
            }

            if (source is not JsArray items)
            {
                //  WebIDL takes any iterable (e.g. a Set), but not an array-like or plain object.
                if (!source.Get(GlobalSymbolRegistry.Iterator).IsCallable())
                {
                    return false;
                }

                var from = engine.Jint.Intrinsics.Array.Get("from");
                items = (JsArray)engine.Jint.Call(from, engine.Jint.Intrinsics.Array, new JsValue[] { source });
            }

            var length = (Int32)items.Length;
            var array = Array.CreateInstance(elementType, length);

            for (var i = 0; i < length; i++)
            {
                array.SetValue(items[i].As(elementType, engine), i);
            }

            result = array;
            return true;
        }

        //  WebIDL's conversion of a number to an integer type: the fraction is dropped, what is
        //  not a finite number becomes zero, and what is out of range wraps around.
        private static Object ToNumeric(Double number, TypeCode code)
        {
            var integer = Double.IsNaN(number) || Double.IsInfinity(number) ? 0.0 : Math.Truncate(number);

            switch (code)
            {
                case TypeCode.Single:
                    return (Single)number;
                case TypeCode.Decimal:
                    return number >= (Double)Decimal.MinValue && number < (Double)Decimal.MaxValue ? (Decimal)number : 0m;
                case TypeCode.Byte:
                    return unchecked((Byte)(Int64)Wrap(integer));
                case TypeCode.SByte:
                    return unchecked((SByte)(Int64)Wrap(integer));
                case TypeCode.Int16:
                    return unchecked((Int16)(Int64)Wrap(integer));
                case TypeCode.Int64:
                    return unchecked((Int64)Wrap(integer));
                default:
                    return integer < 0 ? unchecked((UInt64)(Int64)Wrap(integer)) : number >= UInt64.MaxValue ? 0UL : (UInt64)integer;
            }
        }

        private static Double Wrap(Double integer) =>
            Math.Abs(integer) < 9.2233720368547748E+18 ? integer : Math.IEEERemainder(integer, 18446744073709551616.0);

        private static Object AsComplex(this JsValue value, Type targetType, EngineInstance engine)
        {
            var obj = value.FromJsValue();
            var sourceType = obj.GetType();

            //  Ahead of the check below, as otherwise an Object parameter would receive the Jint
            //  object, about which a library can only learn "[object Uint8Array]" (e.g. a body).
            if (obj is ObjectInstance binary && value.IsBufferSource())
            {
                if (targetType == typeof(Byte[]) || targetType == typeof(Object))
                {
                    return GetBytes(binary);
                }

                if (targetType == typeof(ArraySegment<Byte>))
                {
                    return new ArraySegment<Byte>(GetBytes(binary));
                }
            }

            if (sourceType == targetType || sourceType.GetTypeInfo().IsSubclassOf(targetType) || targetType.GetTypeInfo().IsAssignableFrom(sourceType.GetTypeInfo()))
            {
                return obj;
            }
            else if (targetType.GetTypeInfo().IsSubclassOf(typeof(Delegate)))
            {
                var f = value.AsFunction(engine);

                if (f == null && obj is String b)
                {
                    var e = engine.Jint;
                    var p = new[] { JsValue.FromObjectWithType(e, b, typeof(String)) };
                    f = new ClrFunction(e, "AsComplex", (_this, args) => e.Intrinsics.Eval.Call(_this, p));
                }

                if (f == null && obj != null && value is ObjectInstance callbackObject)
                {
                    f = callbackObject.AsCallbackObject(targetType, engine);
                }

                if (f != null)
                {
                    return targetType.ToDelegate(f, engine);
                }
            }

            if (obj is ObjectInstance sequence && TryCreateSequence(sequence, targetType, engine, out var items))
            {
                return items;
            }

            if (obj is ObjectInstance record && TryCreateRecord(record, targetType, engine, out var entries))
            {
                return entries;
            }

            if (obj is ObjectInstance dictionary && TryCreateDictionary(dictionary, targetType, engine, out var created))
            {
                return created;
            }

            //  A class converting implicitly from several types is how a WebIDL union such as
            //  (DOMString or sequence<DOMString>) is written, as a member cannot be overloaded.
            var conversions = targetType.GetImplicitConversions();

            if (conversions.Length > 0)
            {
                var conversion = conversions.SelectOverload(new[] { value });
                return conversion.Invoke(null, new[] { value.As(conversion.GetParameters()[0].ParameterType, engine) });
            }

            var method = sourceType.PrepareConvert(targetType) ??
                throw new JavaScriptException($"[Internal] Could not find corresponding cast target from {sourceType.Name} to {targetType.Name}.");

            return method.IsStatic ? method.Invoke(null, new[] { obj }) : method.Invoke(obj, null);
        }

        /// <summary>
        /// Converts a plain object into a WebIDL dictionary, i.e. a class whose members carry a
        /// [DomName] (e.g. { top: 100, behavior: 'smooth' } into ScrollToOptions). Members the
        /// object lacks keep their default.
        /// </summary>
        /// <summary>
        /// A WebIDL record (e.g. the headers of fetch) is given as a plain object, whose own
        /// enumerable properties become the entries.
        /// </summary>
        private static Boolean TryCreateRecord(ObjectInstance source, Type targetType, EngineInstance engine, out Object result)
        {
            var valueType = GetRecordValueType(targetType);
            result = null;

            if (valueType == null)
            {
                return false;
            }

            var entries = (IDictionary)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(typeof(String), valueType));

            foreach (var property in source.GetOwnProperties())
            {
                if (property.Key.IsString() && property.Value.Enumerable)
                {
                    var value = source.Get(property.Key);
                    entries[property.Key.AsString()] = valueType == typeof(String) ? TypeConverter.ToString(value) : value.As(valueType, engine);
                }
            }

            result = entries;
            return true;
        }

        //  Dictionary<String, T> and the interfaces it implements with String keys.
        private static Type GetRecordValueType(Type targetType)
        {
            var typeInfo = targetType.GetTypeInfo();

            if (typeInfo.IsGenericType && typeInfo.GenericTypeArguments.Length == 2 && typeInfo.GenericTypeArguments[0] == typeof(String))
            {
                var dictionary = typeof(Dictionary<,>).MakeGenericType(typeInfo.GenericTypeArguments);

                if (targetType.GetTypeInfo().IsAssignableFrom(dictionary.GetTypeInfo()))
                {
                    return typeInfo.GenericTypeArguments[1];
                }
            }

            return null;
        }

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
                else if (Nullable.GetUnderlyingType(targetType) is Type nullableType)
                {
                    return value.IsUndefined() ? null : value.As(nullableType, engine);
                }
                else if (targetType == typeof(UInt64) || targetType == typeof(Int64) || targetType == typeof(Single) ||
                    targetType == typeof(Int16) || targetType == typeof(Byte) || targetType == typeof(SByte) || targetType == typeof(Decimal))
                {
                    return ToNumeric(TypeConverter.ToNumber(value), Type.GetTypeCode(targetType));
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
