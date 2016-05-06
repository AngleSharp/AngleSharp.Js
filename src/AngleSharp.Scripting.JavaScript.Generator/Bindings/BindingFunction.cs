namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;

    abstract class BindingFunction : BindingMember
    {
        readonly List<BindingParameter> _parameters;

        public BindingFunction(String originalName, Type type)
            : base(originalName, type)
        {
            _parameters = new List<BindingParameter>();
        }

        public IEnumerable<BindingParameter> Parameters
        {
            get { return _parameters; }
        }

        public void Bind(BindingParameter parameter)
        {
            _parameters.Add(parameter);
        }
    }
}
