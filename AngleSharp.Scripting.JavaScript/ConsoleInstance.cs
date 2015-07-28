namespace AngleSharp.Scripting.JavaScript
{
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime.Interop;
    using System;

    sealed class ConsoleInstance : ObjectInstance
    {
        public ConsoleInstance(Engine engine)
            : base(engine)
        {
            Action<Object> log = obj => Console.WriteLine(obj);
            FastAddProperty("log", new ClrFunctionInstance(engine, Log), false, false, false);
        }

        JsValue Log(JsValue ctx, JsValue[] args)
        {
            var strs = new String[args.Length];
            
            for (var i = 0; i < args.Length; i++)
                strs[i] = args[i].ToString();

            Console.WriteLine(String.Join(", ", strs));
            return JsValue.Undefined;
        }
    }
}
