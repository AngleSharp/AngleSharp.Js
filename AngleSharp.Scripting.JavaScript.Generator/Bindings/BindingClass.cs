namespace AngleSharp.Scripting.JavaScript.Generator
{
    using AngleSharp.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    sealed class BindingClass : BindingType
    {
        readonly Dictionary<String, BindingMember> _members;
        readonly Dictionary<Accessors, BindingMember> _specials;
        readonly List<BindingConstructor> _constructors;

        public BindingClass(String name, Boolean createNoInterfaceObject = false)
            : base(name)
        {
            _members = new Dictionary<String, BindingMember>();
            _specials = new Dictionary<Accessors, BindingMember>();
            _constructors = new List<BindingConstructor>();
            IsInterfaced = createNoInterfaceObject == false;
        }

        public Boolean IsInterfaced
        {
            get;
            private set;
        }

        public IEnumerable<KeyValuePair<String, BindingMember>> Members
        {
            get { return _members; }
        }

        public IEnumerable<BindingMember> Constructors
        {
            get { return _constructors; }
        }

        public IEnumerable<BindingMember> Deleters
        {
            get { return GetSpecials(Accessors.Deleter); }
        }

        public IEnumerable<BindingMember> Getters
        {
            get { return GetSpecials(Accessors.Getter); }
        }

        public IEnumerable<BindingMember> Setters
        {
            get { return GetSpecials(Accessors.Setter); }
        }

        public void BindConstructor(BindingConstructor value)
        {
            _constructors.Add(value);
        }

        public void Bind(String name, BindingMember value)
        {
            _members.Add(name, value);
        }

        public void Bind(Accessors accessor, BindingMember value)
        {
            _specials.Add(accessor, value);
        }

        IEnumerable<BindingMember> GetSpecials(Accessors key)
        {
            return _specials.Where(m => m.Key == key).Select(m => m.Value);
        }
        
        public override IEnumerable<GeneratedFile> ToFiles(String extension)
        {
            return Enumerable.Empty<GeneratedFile>();
        }

        public override IEnumerable<BindingMember> GetMembers()
        {
            return _members.Select(m => m.Value);
        }
    }
}
