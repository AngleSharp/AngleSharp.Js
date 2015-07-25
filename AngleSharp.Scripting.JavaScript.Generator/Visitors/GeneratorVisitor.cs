namespace AngleSharp.Scripting.JavaScript.Generator
{
    using AngleSharp.Scripting.JavaScript.Generator.Templates;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    sealed class GeneratorVisitor : IVisitor
    {
        readonly List<GeneratedFile> _files;
        readonly Options _options;

        public GeneratorVisitor(Options options)
        {
            _files = new List<GeneratedFile>();
            _options = options;
        }

        public IEnumerable<GeneratedFile> Files
        {
            get { return _files; }
        }

        #region Not Implemented

        public void Visit(BindingParameter parameter)
        {
        }

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
            var hasConstructor = @class.Constructors.Any();

            Generate(new ClassInstanceModel
            {
                BaseName = @class.BaseName,
                OriginalName = @class.OriginalName,
                Name = @class.Name,
                OriginalNamespace = @class.OriginalNamespace,
                Namespace = _options.Namespace,
                Fields = @class.GetAll<BindingField>().Select(m => new FieldModel 
                { 
                    Name = m.Key, 
                    Value = FieldRef(m.Value) 
                })
            });

            Generate(new ClassPrototypeModel
            {
                OriginalName = @class.OriginalName,
                Name = @class.Name,
                OriginalNamespace = @class.OriginalNamespace,
                Namespace = _options.Namespace,
                HasConstructor = hasConstructor,
                Prototype = "engine.Object.PrototypeObject",
                Methods = @class.GetAll<BindingMethod>().Select(m => new MethodModel 
                { 
                    IsVoid = m.Value.ReturnType == typeof(void), 
                    Name = m.Key,
                    OriginalName = m.Value.OriginalName,
                    RefName = "Wrap" + m.Value.OriginalName,
                    Parameters = m.Value.Parameters.Select(n => new ParameterModel
                    {
                        Converter = GetConverter(n.ValueType),
                        Index = n.Position,
                        IsOptional = n.IsOptional,
                        Name = n.OriginalName
                    })
                })
            });

            if (hasConstructor)
            {
                Generate(new ClassConstructorModel
                {
                    Name = @class.Name,
                    Namespace = _options.Namespace,
                    OriginalNamespace = @class.OriginalNamespace,
                    OriginalName = @class.OriginalName,
                    Parameters = Enumerable.Empty<ParameterModel>()
                });
            }
        }

        void Generate(ClassInstanceModel model)
        {
            var template = new ClassInstance(model);
            var content = template.TransformText();
            var fileName = String.Concat(model.Name, "Instance", _options.Extension);
            _files.Add(new GeneratedFile(content, fileName));
        }

        void Generate(ClassPrototypeModel model)
        {
            var template = new ClassPrototype(model);
            var content = template.TransformText();
            var fileName = String.Concat(model.Name, "Prototype", _options.Extension);
            _files.Add(new GeneratedFile(content, fileName));
        }

        void Generate(ClassConstructorModel model)
        {
            var template = new ClassConstructor(model);
            var content = template.TransformText();
            var fileName = String.Concat(model.Name, "Constructor", _options.Extension);
            _files.Add(new GeneratedFile(content, fileName));
        }

        static String FieldRef(BindingField field)
        {
            return String.Concat("(Int32)(", field.ValueType.FullName, ".", field.OriginalName, ")");
        }

        String GetConverter(Type type)
        {
            var converter = default(String);

            if (_options.TypeConverters.TryGetValue(type, out converter))
                return converter;

            return "UnresolvedConverter.To" + type.Name;
        }
    }
}
