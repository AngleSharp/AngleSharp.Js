namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;

    sealed class BindingEnum : BindingType
    {
        readonly Dictionary<String, String> _fields;

        public BindingEnum(String name)
            : base(name)
        {
            _fields = new Dictionary<String, String>();
        }

        public IEnumerable<KeyValuePair<String, String>> Fields
        {
            get { return _fields; }
        }

        public void Bind(String name, String location)
        {
            _fields.Add(name, location);
        }
    }
}
