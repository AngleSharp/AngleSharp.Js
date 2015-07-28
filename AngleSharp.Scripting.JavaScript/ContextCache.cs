using System.Collections.Generic;
using Jint;
using Jint.Runtime.Environments;

namespace AngleSharp.Scripting.JavaScript
{
    internal class ContextCache
    {
        readonly Dictionary<object, DomNodeInstance> _objects = new Dictionary<object, DomNodeInstance>();
        
        internal LexicalEnvironment LexicalEnvironment { get; set; }
        internal LexicalEnvironment VariableEnvironment { get; set; }

        internal DomNodeInstance GetDomNodeInstance(Engine engine, object obj)
        {
            DomNodeInstance domNodeInstance;

            if (_objects.TryGetValue(obj, out domNodeInstance))
                return domNodeInstance;
            
            domNodeInstance = new DomNodeInstance(engine, obj);

            _objects.Add(obj, domNodeInstance);

            return domNodeInstance;
        }
    }
}
