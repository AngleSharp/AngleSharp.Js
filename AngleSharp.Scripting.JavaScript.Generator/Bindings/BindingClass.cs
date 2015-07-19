namespace AngleSharp.Scripting.JavaScript.Generator
{
    using AngleSharp.Attributes;
    using System;
    using System.Collections.Generic;

    sealed class BindingClass
    {
        readonly Dictionary<String, BindingMember> _members;
        readonly Dictionary<Special, BindingMember> _specials;
        readonly List<BindingMember> _wrappers;

        public BindingClass(String name)
        {
            Name = name;
            _members = new Dictionary<String, BindingMember>();
            _specials = new Dictionary<Special, BindingMember>();
            _wrappers = new List<BindingMember>();
        }

        public void BindConstructor(BindingMember value)
        {
            _specials.Add(Special.Constructor, value);
        }

        public void Bind(String name, BindingMember value)
        {
            _members.Add(name, value);
        }

        public void Bind(Accessors accessor, BindingMember value)
        {
            _specials.Add(Translate[accessor], value);
        }

        public void Wrap(BindingMember member)
        {
            _wrappers.Add(member);
        }

        public String Name
        {
            get;
            private set;
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
