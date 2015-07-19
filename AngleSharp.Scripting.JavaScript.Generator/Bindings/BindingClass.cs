namespace AngleSharp.Scripting.JavaScript.Generator
{
    using AngleSharp.Attributes;
    using System;
    using System.Collections.Generic;

    sealed class BindingClass
    {
        readonly Dictionary<String, BindingMember> _members;
        readonly Dictionary<Special, BindingMember> _specials;

        public BindingClass(String name)
        {
            Name = name;
            _members = new Dictionary<String, BindingMember>();
            _specials = new Dictionary<Special, BindingMember>();
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
