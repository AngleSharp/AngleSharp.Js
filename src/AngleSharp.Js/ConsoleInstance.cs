namespace AngleSharp.Js
{
    using Jint;
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime.Interop;
    using System;

    sealed class ConsoleInstance : ObjectInstance
    {
        private readonly IConsoleLogger _logger;

        public ConsoleInstance(Engine engine, IConsoleLogger logger)
            : base(engine)
        {
            _logger = logger;
            FastAddProperty("log", new ClrFunctionInstance(engine, "log", Log), false, false, false);
        }

        private JsValue Log(JsValue ctx, JsValue[] args)
        {
            if (_logger != null)
            {
                var objs = new Object[args.Length];

                for (var i = 0; i < args.Length; i++)
                {
                    objs[i] = args[i].FromJsValue();
                }

                _logger.Log(objs);
            }

            return JsValue.Undefined;
        }
    }
}
