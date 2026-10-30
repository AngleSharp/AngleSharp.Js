namespace AngleSharp.Js
{
    using Jint;
    using Jint.Native;
    using Jint.Native.Function;
    using System;
    using System.Reflection;

    /// <summary>
    /// Picks one of several overloads of a DOM operation (e.g. scrollTo(x, y) and
    /// scrollTo(options)) for the arguments of a call, in the spirit of WebIDL's overload
    /// resolution: the number of arguments decides first, their types second.
    /// </summary>
    static class OverloadResolution
    {
        public static MethodInfo SelectOverload(this MethodInfo[] overloads, JsValue[] arguments)
        {
            var count = arguments.Length;

            //  Trailing undefined arguments count as not passed.
            while (count > 0 && arguments[count - 1].IsUndefined())
            {
                count--;
            }

            var best = overloads[0];
            var bestScore = Int32.MinValue;

            foreach (var overload in overloads)
            {
                var score = Score(overload, arguments, count);

                if (score > bestScore)
                {
                    best = overload;
                    bestScore = score;
                }
            }

            return best;
        }

        private static Int32 Score(MethodInfo method, JsValue[] arguments, Int32 count)
        {
            var parameters = method.GetParameters();
            //  The receiver of an extension method is not an argument.
            var offset = method.IsStatic && parameters.Length > 0 ? 1 : 0;
            var available = parameters.Length - offset;
            var required = 0;

            for (var i = offset; i < parameters.Length; i++)
            {
                if (!parameters[i].IsOptional && !parameters[i].IsDefined(typeof(ParamArrayAttribute), false))
                {
                    required++;
                }
            }

            if (count < required)
            {
                return -1000 + count - required;
            }

            var score = count > available ? available - count : 0;

            for (var i = 0; i < Math.Min(count, available); i++)
            {
                score += Match(arguments[i], parameters[i + offset].ParameterType);
            }

            return score;
        }

        private static Int32 Match(JsValue value, Type type)
        {
            var target = Nullable.GetUnderlyingType(type) ?? type;
            var info = target.GetTypeInfo();

            if (value.IsNumber())
            {
                return IsNumeric(target) ? 2 : target == typeof(Object) ? 1 : 0;
            }

            if (value.IsBoolean())
            {
                return target == typeof(Boolean) ? 2 : target == typeof(Object) ? 1 : 0;
            }

            if (value.IsString())
            {
                return target == typeof(String) ? 2 : info.IsEnum || target == typeof(Object) ? 1 : 0;
            }

            if (value.IsNull() || value.IsUndefined())
            {
                return !info.IsValueType || type != target ? 1 : 0;
            }

            if (value is Function)
            {
                return info.IsSubclassOf(typeof(Delegate)) ? 2 : target == typeof(Object) ? 1 : 0;
            }

            if (value is IDomProxy proxy)
            {
                return info.IsAssignableFrom(proxy.Value.GetType().GetTypeInfo()) ? 3 : 0;
            }

            //  A plain object fits a dictionary (e.g. ScrollToOptions), not a primitive.
            return info.IsClass && target != typeof(String) ? 2 : 0;
        }

        private static Boolean IsNumeric(Type type) =>
            type == typeof(Double) || type == typeof(Single) || type == typeof(Int32) || type == typeof(Int64) ||
            type == typeof(UInt32) || type == typeof(UInt16) || type == typeof(Int16) || type == typeof(Byte);
    }
}
