namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    sealed class BindingEnum : BindingType
    {
        readonly Dictionary<String, BindingField> _fields;

        public BindingEnum(String name, String originalName, String originalNamespace)
            : base(name, originalName, originalNamespace)
        {
            _fields = new Dictionary<String, BindingField>();
        }

        public IEnumerable<KeyValuePair<String, BindingField>> Fields
        {
            get { return _fields; }
        }

        public void Bind(String name, BindingField location)
        {
            _fields.Add(name, location);
        }

        public override IEnumerable<BindingMember> GetMembers()
        {
            return _fields.Select(m => m.Value);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
