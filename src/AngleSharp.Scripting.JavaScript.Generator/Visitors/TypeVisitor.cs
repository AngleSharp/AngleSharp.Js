namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;

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
            VisitParameters(constructor);
        }

        public void Visit(BindingClass @class)
        {
            foreach (var member in @class.GetMembers())
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

        public void Visit(BindingParameter parameter)
        {
            Include(parameter.ValueType);
        }

        public void Visit(BindingMethod method)
        {
            Include(method.ReturnType);
            VisitParameters(method);
        }

        public void Visit(BindingIndex index)
        {
            Include(index.ValueType);
            VisitParameters(index);
        }

        public void Visit(BindingProperty property)
        {
            Include(property.ValueType);
        }

        void VisitParameters(BindingFunction function)
        {
            foreach (var parameter in function.Parameters)
                parameter.Accept(this);
        }

        void Include(Type type)
        {
            if (_dependencies.Contains(type) == false)
                _dependencies.Add(type);
        }
    }
}
