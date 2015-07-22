namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    sealed class TypeVisitor : IVisitor
    {
        readonly List<Type> _dependencies;

        public TypeVisitor()
        {
            _dependencies = new List<Type>();
        }

        public IEnumerable<Type> Dependencies
        {
            get { return _dependencies; }
        }

        public void Visit(BindingConstructor constructor)
        {
            Include(constructor.Parameters.Select(m => m.Value));
        }

        public void Visit(BindingClass @class)
        {
            foreach (var member in @class.GetMembers())
                member.Accept(this);
        }

        public void Visit(BindingEnum @enum)
        {
            foreach (var member in @enum.GetMembers())
                member.Accept(this);
        }

        public void Visit(BindingEvent @event)
        {
            Include(@event.HandlerType);
        }

        public void Visit(BindingField field)
        {
            Include(field.ValueType);
        }

        public void Visit(BindingMethod method)
        {
            Include(method.ReturnType);
            Include(method.Parameters.Select(m => m.Value));
        }

        public void Visit(BindingIndex index)
        {
            Include(index.ValueType);
            Include(index.Parameters.Select(m => m.Value));
        }

        public void Visit(BindingProperty property)
        {
            Include(property.ValueType);
        }

        void Include(IEnumerable<Type> types)
        {
            foreach (var type in types)
                Include(type);
        }

        void Include(Type type)
        {
            if (_dependencies.Contains(type) == false)
                _dependencies.Add(type);
        }
    }
}
