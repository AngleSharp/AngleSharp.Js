namespace AngleSharp.Scripting.JavaScript.Generator
{
    using AngleSharp.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    sealed class BindingClass : BindingType
    {
        readonly Dictionary<String, BindingMember> _members;
        readonly List<KeyValuePair<Accessors, BindingFunction>> _specials;
        readonly List<BindingConstructor> _constructors;
        readonly List<Type> _genericArguments;

        public BindingClass(String name, String originalName, String originalNamespace, String baseName)
            : base(name, originalName, originalNamespace)
        {
            _members = new Dictionary<String, BindingMember>();
            _specials = new List<KeyValuePair<Accessors, BindingFunction>>();
            _constructors = new List<BindingConstructor>();
            _genericArguments = new List<Type>();
            BaseName = baseName ?? typeof(Object).Name;
        }

        public Boolean IsGeneric
        {
            get { return _genericArguments.Count > 0; }
        }

        public String BaseName 
        { 
            get; 
            private set; 
        }

        public List<Type> GenericArguments
        {
            get { return _genericArguments; }
        }

        public IEnumerable<KeyValuePair<String, BindingMember>> Members
        {
            get { return _members; }
        }

        public IEnumerable<KeyValuePair<String, T>> GetAll<T>()
            where T : BindingMember
        {
            return _members.Select(m => new KeyValuePair<String, T>(m.Key, m.Value as T)).Where(m => m.Value != null);
        }

        public IEnumerable<BindingConstructor> Constructors
        {
            get { return _constructors; }
        }

        public IEnumerable<BindingFunction> Deleters
        {
            get { return GetSpecials(Accessors.Deleter); }
        }

        public IEnumerable<BindingFunction> Getters
        {
            get { return GetSpecials(Accessors.Getter); }
        }

        public IEnumerable<BindingFunction> Setters
        {
            get { return GetSpecials(Accessors.Setter); }
        }

        public void BindConstructor(BindingConstructor value)
        {
            _constructors.Add(value);
        }

        public void Bind(String name, BindingMember value)
        {
            _members[name] = value;
        }

        public void Bind(Accessors accessor, BindingFunction value)
        {
            _specials.Add(new KeyValuePair<Accessors, BindingFunction>(accessor, value));
        }

        IEnumerable<BindingFunction> GetSpecials(Accessors key)
        {
            return _specials.Where(m => m.Key == key).Select(m => m.Value);
        }

        public override IEnumerable<BindingMember> GetMembers()
        {
            return _members.Select(m => m.Value);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
