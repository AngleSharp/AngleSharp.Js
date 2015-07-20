namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;

    sealed class BindingEnum : BindingType
    {
        readonly Dictionary<String, Object> _fields;

        public BindingEnum(String name)
            : base(name)
        {
            _fields = new Dictionary<String, Object>();
        }

        public IEnumerable<KeyValuePair<String, Object>> Fields
        {
            get { return _fields; }
        }

        public void Bind(String name, Object value)
        {
            _fields.Add(name, value);
        }

        public override void Serialize(SyntaxWriter writer)
        {
            writer.AddEnum(this);
        }
    }
}
