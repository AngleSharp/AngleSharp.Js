namespace AngleSharp.Scripting.JavaScript.Generator
{
    using AngleSharp.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    sealed class BindingClass
    {
        readonly Dictionary<String, BindingMember> _members;
        readonly Dictionary<Special, BindingMember> _specials;

        public BindingClass(String name, Boolean createNoInterfaceObject = false)
        {
            Name = name;
            _members = new Dictionary<String, BindingMember>();
            _specials = new Dictionary<Special, BindingMember>();
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
            get { return GetSpecials(Special.Constructor); }
        }

        public IEnumerable<BindingMember> Deleters
        {
            get { return GetSpecials(Special.Destructor); }
        }

        public IEnumerable<BindingMember> Getters
        {
            get { return GetSpecials(Special.Getter); }
        }

        public IEnumerable<BindingMember> Setters
        {
            get { return GetSpecials(Special.Setter); }
        }

        public String Name
        {
            get;
            private set;
        }

        public void BindConstructor(BindingMember value)
        {
            var key = Special.Constructor;
            _specials.Add(key, value);
        }

        public void Bind(String name, BindingMember value)
        {
            _members.Add(name, value);
        }

        public void Bind(Accessors accessor, BindingMember value)
        {
            var key = Translate[accessor];
            _specials.Add(key, value);
        }

        IEnumerable<BindingMember> GetSpecials(Special key)
        {
            return _specials.Where(m => m.Key == key).Select(m => m.Value);
        }

        static readonly Dictionary<Accessors, Special> Translate = new Dictionary<Accessors, Special>
        {
            { Accessors.None, Special.None },
            { Accessors.Getter, Special.Getter },
            { Accessors.Setter, Special.Setter },
            { Accessors.Deleter, Special.Destructor },
        };

        enum Special
        {
            None,
            Constructor,
            Destructor,
            Getter,
            Setter
        }
    }
}
