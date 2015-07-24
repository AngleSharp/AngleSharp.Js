namespace AngleSharp.Scripting.JavaScript
{
    using AngleSharp.Attributes;
    using Jint;
    using Jint.Native.Object;
    using Jint.Runtime.Descriptors;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    sealed class DomNodeInstance : ObjectInstance
    {
        readonly Object _value;
        PropertyInfo _numericIndexer;
        PropertyInfo _stringIndexer;

        public DomNodeInstance(Engine engine, Object value)
            : base(engine)
        {
            _value = value;
            SetMembers(value.GetType());
        }

        public override PropertyDescriptor GetOwnProperty(String propertyName)
        {
            //  If we have a numeric indexer and the property is numeric
            int numericIndex;

            if (_numericIndexer != null && int.TryParse(propertyName, out numericIndex))
                return new PropertyDescriptor(_numericIndexer.GetMethod.Invoke(_value, new Object[] { numericIndex }).ToJsValue(Engine), false, false, false);

            //  Else a string property
            //  If we have a string indexer and no property exists for this name then use the string indexer
            //  Jint possibly has a limitation here - if an object has a string indexer.  How do we know whether to use the defined indexer or a property?
            //  Eg. object.callMethod1()  vs  object['callMethod1'] is not necessarily the same if the object has a string indexer?? (I'm not an ECMA expert!)
            //  node.attributes is one such object - has both a string and numeric indexer
            //  This GetOwnProperty override might need an additional parameter to let us know this was called via an indexer
            if (_stringIndexer != null && Properties.ContainsKey(propertyName) == false)
                return new PropertyDescriptor(_stringIndexer.GetMethod.Invoke(_value, new Object[] {propertyName}).ToJsValue(Engine), false, false, false);
            
            //  Else try to return a registered property
            return base.GetOwnProperty(propertyName);
        }

        void SetMembers(Type type)
        {
            if (type.GetCustomAttribute<DomNameAttribute>() == null)
            {
                foreach (var contract in type.GetInterfaces())
                    SetMembers(contract);
            }
            else
            {
                SetProperties(type.GetProperties());
                SetMethods(type.GetMethods());
                SetEvents(type.GetEvents());
            }
        }

        void SetEvents(EventInfo[] eventInfos)
        {
            foreach (var eventInfo in eventInfos)
            {
                var names = eventInfo.GetCustomAttributes<DomNameAttribute>();

                foreach (var name in names.Select(m => m.OfficialName))
                {
                    FastSetProperty(name, new PropertyDescriptor(
                        new DomFunctionInstance(this, eventInfo.RaiseMethod),
                        new DomFunctionInstance(this, eventInfo.AddMethod), false, false));
                }
            }
        }

        void SetProperties(IEnumerable<PropertyInfo> properties)
        {
            foreach (var property in properties)
            {
                var index = property.GetCustomAttribute<DomAccessorAttribute>();

                if (index != null)
                {
                    var indexParameters = property.GetIndexParameters();

                    if (indexParameters.Length == 1)
                    {
                        if (indexParameters[0].ParameterType == typeof(Int32))
                            _numericIndexer = property;
                        else if (indexParameters[0].ParameterType == typeof(String))
                            _stringIndexer = property;
                    }
                }

                var names = property.GetCustomAttributes<DomNameAttribute>();
                
                foreach (var name in names.Select(m => m.OfficialName))
                {
                    FastSetProperty(name, new PropertyDescriptor(
                        new DomFunctionInstance(this, property.GetMethod),
                        new DomFunctionInstance(this, property.SetMethod), false, false));
                }
            }
        }

        void SetMethods(IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                var names = method.GetCustomAttributes<DomNameAttribute>();

                foreach (var name in names.Select(m => m.OfficialName))
                {
                    //TODO Jint
                    // If it already has a property with the given name (usually another method),
                    // then convert that method to a two-layer method, which decides which one
                    // to pick depending on the number (and probably types) of arguments.
                    if (HasProperty(name))
                        continue;

                    FastAddProperty(name, new DomFunctionInstance(this, method), false, false, false);
                }
            }
        }

        public Object Value
        {
            get { return _value; }
        }

        public override String ToString()
        {
            return String.Format("[object {0}]", _value.GetType().Name);
        }
    }
}
