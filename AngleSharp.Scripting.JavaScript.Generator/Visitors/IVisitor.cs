namespace AngleSharp.Scripting.JavaScript.Generator
{
    interface IVisitor
    {
        void Visit(BindingConstructor constructor);

        void Visit(BindingClass @class);

        void Visit(BindingEnum @enum);

        void Visit(BindingEvent @event);

        void Visit(BindingField field);

        void Visit(BindingMethod method);

        void Visit(BindingIndex index);

        void Visit(BindingProperty property);

        void Visit(BindingParameter parameter);
    }
}
