namespace AngleSharp.Js.Proxies
{
    using Jint.Native;
    using Jint.Native.Object;
    using Jint.Runtime;
    using System.Reflection;

    sealed class DomConstructorFunctionInstance : Constructor
    {
        private readonly EngineInstance _instance;
        private readonly MethodInfo _constructorFunction;

        public DomConstructorFunctionInstance(EngineInstance instance, MethodInfo constructorFunction, string name) : base(instance.Jint, name)
        {
            _instance = instance;
            _constructorFunction = constructorFunction;
        }

        public override ObjectInstance Construct(JsValue[] arguments, JsValue newTarget)
        {
            try
            {
                return _instance.Call(_constructorFunction, _instance.Window, arguments) as ObjectInstance;
            }
            catch
            {
                throw new JavaScriptException(_instance.Jint.Intrinsics.Error);
            }
        }
    }
}
