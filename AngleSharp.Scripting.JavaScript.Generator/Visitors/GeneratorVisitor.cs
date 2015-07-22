namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;

    sealed class GeneratorVisitor : IVisitor
    {
        readonly List<GeneratedFile> _files;

        public GeneratorVisitor()
        {
            _files = new List<GeneratedFile>();
        }

        public IEnumerable<GeneratedFile> Files
        {
            get { return _files; }
        }

        #region Not Implemented

        public void Visit(BindingConstructor constructor)
        {
        }

        public void Visit(BindingEvent @event)
        {
        }

        public void Visit(BindingField field)
        {
        }

        public void Visit(BindingMethod method)
        {
        }

        public void Visit(BindingIndex index)
        {
        }

        public void Visit(BindingProperty property)
        {
        }

        #endregion

        public void Visit(BindingClass @class)
        {
            throw new NotImplementedException();
        }

        public void Visit(BindingEnum @enum)
        {
            throw new NotImplementedException();
        }
    }
}
