namespace AngleSharp.Scripting.JavaScript.Generator
{
    using AngleSharp.Scripting.JavaScript.Generator.Templates;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    sealed class GeneratorVisitor : IVisitor
    {
        #region Fields

        readonly List<GeneratedFile> _files;
        readonly Options _options;

        #endregion

        #region ctor

        public GeneratorVisitor(Options options)
        {
            _files = new List<GeneratedFile>();
            _options = options;
        }

        #endregion

        #region Properties

        public IEnumerable<GeneratedFile> Files
        {
            get { return _files; }
        }

        #endregion

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
                GenericArguments = GenericRef(@class),
                Fields = @class.GetAll<BindingField>().Select(m => CreateField(m.Key, m.Value)).ToArray()
            });

            Generate(new ClassPrototypeModel
            {
                OriginalName = @class.OriginalName,
                Name = @class.Name,
                OriginalNamespace = @class.OriginalNamespace,
                Namespace = _options.Namespace,
                Prototype = @class.Prototype,
                Events = @class.GetAll<BindingEvent>().Select(m => CreateEvent(m.Key, m.Value)).ToArray(),
                Properties = @class.GetAll<BindingProperty>().Select(m => CreateProperty(m.Key, m.Value)).ToArray(),
                Methods = @class.GetAll<BindingMethod>().Select(m => CreateMethod(m.Key, m.Value)).ToArray()
            });

            Generate(new ClassConstructorModel
            {
                Name = @class.Name,
                Namespace = _options.Namespace,
                OriginalNamespace = @class.OriginalNamespace,
                OriginalName = @class.OriginalName,
                HasConstructor = hasConstructor,
                Parameters = Enumerable.Empty<ParameterModel>()
            });
        }

        EventModel CreateEvent(String name, BindingEvent @event)
        {
            return new EventModel
            {
                Name = name,
                Converter = GetConverter(@event.HandlerType),
                IsLenient = @event.IsLenient,
                OriginalName = @event.OriginalName
            };
        }

        FieldModel CreateField(String name, BindingField field)
        {
            return new FieldModel
            {
                Name = name,
                Value = FieldRef(field)
            };
        }

        PropertyModel CreateProperty(String name, BindingProperty property)
        {
            var originalName = property.OriginalName;
            var forwardedTo = property.ForwardedTo;
            var targetType = property.ValueType;
            var ignoreSet = property.AllowSet == false;

            if (forwardedTo != null)
            {
                originalName = String.Concat(originalName, ".", forwardedTo);
                ignoreSet = false;
                targetType = typeof(String);
            }

            var getter = new MethodModel
            {
                Name = name,
                OriginalName = property.OriginalName,
                IsLenient = property.IsLenient,
                Parameters = Enumerable.Empty<ParameterModel>(),
                RefName = "Get" + property.OriginalName,
                IsVoid = property.AllowGet == false
            };

            var setter = new MethodModel
            {
                Name = name,
                OriginalName = originalName,
                IsLenient = property.IsLenient,
                Parameters = new []
                {
                    new ParameterModel 
                    {
                        Converter = GetConverter(targetType),
                        Index = 0,
                        IsOptional = false,
                        Name = "value"
                    }
                },
                RefName = "Set" + property.OriginalName,
                IsVoid = ignoreSet
            };

            return new PropertyModel
            {
                Name = name,
                Getter = getter,
                Setter = setter
            };
        }

        MethodModel CreateMethod(String name, BindingMethod method)
        {
            var suffix = String.Empty;

            if (method.Parameters.Any())
                suffix = "_" + String.Concat(method.Parameters.Select(m => m.ValueType.Name.Substring(0, 1).ToLowerInvariant()));

            return new MethodModel
            {
                Name = name,
                IsVoid = method.ReturnType == typeof(void),
                OriginalName = method.OriginalName,
                RefName = method.OriginalName + suffix,
                IsLenient = method.IsLenient,
                Parameters = method.Parameters.Select(CreateParameter).ToArray()
            };
        }

        ParameterModel CreateParameter(BindingParameter parameter)
        {
            return new ParameterModel
            {
                Converter = GetConverter(parameter.ValueType),
                Index = parameter.Position,
                IsOptional = parameter.IsOptional,
                Name = parameter.OriginalName
            };
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

        static String GenericRef(BindingClass @class)
        {
            if (@class.GenericArguments.Count > 0)
            {
                var strs = @class.GenericArguments.
                                  Select(m => m.GetGenericParameterConstraints().FirstOrDefault() ?? typeof(Object)).
                                  Select(m => m.Name);
                var rep = String.Join(", ", strs);
                return String.Concat("<", rep, ">");
            }

            return String.Empty;
        }

        static String FieldRef(BindingField field)
        {
            return String.Concat("(UInt32)(", field.ValueType.FullName, ".", field.OriginalName, ")");
        }

        String GetConverter(Type type)
        {
            var converter = default(String);

            if (_options.TypeConverters.TryGetValue(type, out converter))
                return converter;
            else if (type.IsGenericType && _options.TypeConverters.TryGetValue(type.GetGenericTypeDefinition(), out converter))
                return converter;

            return String.Concat("UnresolvedConverter.To<", type.FullName, ">");
        }
    }
}
