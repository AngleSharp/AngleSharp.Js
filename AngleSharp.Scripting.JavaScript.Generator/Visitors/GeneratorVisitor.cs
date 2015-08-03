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
        readonly DependencyTree<String> _names;
        readonly Options _options;

        #endregion

        #region ctor

        public GeneratorVisitor(Options options)
        {
            _files = new List<GeneratedFile>();
            _names = new DependencyTree<String>();
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

        void IVisitor.Visit(BindingParameter parameter)
        {
        }

        void IVisitor.Visit(BindingConstructor constructor)
        {
        }

        void IVisitor.Visit(BindingEvent @event)
        {
        }

        void IVisitor.Visit(BindingField field)
        {
        }

        void IVisitor.Visit(BindingMethod method)
        {
        }

        void IVisitor.Visit(BindingIndex index)
        {
        }

        void IVisitor.Visit(BindingProperty property)
        {
        }

        #endregion

        #region Visit

        void IVisitor.Visit(BindingClass @class)
        {
            Generate(new ClassInstanceModel
            {
                BaseName = @class.BaseName,
                OriginalName = @class.OriginalName,
                Name = @class.Name,
                OriginalNamespace = @class.OriginalNamespace,
                Namespace = _options.Namespace,
                GenericArguments = GenericRef(@class),
                Getters = @class.Getters.Select(CreateMethod).ToArray(),
                Setters = @class.Setters.Select(CreateMethod).ToArray(),
                Deleters = @class.Deleters.Select(CreateMethod).ToArray(),
                Fields = @class.GetAll<BindingField>().Select(m => CreateField(m.Key, m.Value)).ToArray()
            });

            Generate(new ClassPrototypeModel
            {
                OriginalName = @class.OriginalName,
                Name = @class.Name,
                OriginalNamespace = @class.OriginalNamespace,
                Namespace = _options.Namespace,
                Prototype = String.Concat("engine.Constructors.", @class.BaseName, ".PrototypeObject"),
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
                Constructors = @class.Constructors.Select(CreateMethod).ToArray(),
                Constants = @class.GetAll<BindingField>().Select(m => CreateField(m.Key, m.Value)).ToArray()
            });

            _names.Include(@class.Name, @class.BaseName);
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
            var refName = name.Capitalize();

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
                RefName = "Get" + refName,
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
                        Name = "value",
                        ParameterType = targetType
                    }
                },
                RefName = "Set" + refName,
                IsVoid = ignoreSet
            };

            return new PropertyModel
            {
                Name = name,
                Getter = getter,
                Setter = setter
            };
        }

        MethodModel CreateMethod(BindingFunction function)
        {
            return new MethodModel
            {
                Name = String.Empty,
                IsVoid = true,
                OriginalName = function.OriginalName,
                Converter = GetConverter(function.Type),
                RefName = function.OriginalName,
                IsLenient = true,
                Parameters = function.Parameters.Select(CreateParameter).ToArray()
            };
        }

        MethodModel CreateMethod(String name, BindingMethod method)
        {
            return new MethodModel
            {
                Name = name,
                IsVoid = method.ReturnType == typeof(void),
                Converter = GetConverter(method.Type),
                OriginalName = method.OriginalName,
                RefName = name.Capitalize(),
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
                Name = parameter.OriginalName,
                ParameterType = parameter.ValueType
            };
        }

        #endregion

        #region Generate Files

        public void GenerateAuxiliary()
        {
            Generate(new DomConstructorsModel
            {
                Namespace = _options.Namespace,
                Constructors = _names.Dependencies(typeof(Object).Name).ToArray()
            });
        }

        void Generate(DomConstructorsModel model)
        {
            var template = new DomConstructors(model);
            var content = template.TransformText();
            var fileName = String.Concat("DomConstructors", _options.Extension);
            _files.Add(new GeneratedFile(content, fileName));
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

        #endregion

        #region Helpers

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
            else if (type.IsGenericType)
                return GetGenericConverter(type, type.GenericTypeArguments);

            return String.Concat("UnresolvedConverter.To<", type.FullName, ">");
        }

        static String GetGenericConverter(Type type, Type[] arguments)
        {
            var name = type.FullName.Replace("`1", "");
            var args = String.Join(", ", arguments.Select(m => m.FullName));
            return String.Concat("UnresolvedConverter.To<", name, "<", args, ">>");
        }

        #endregion
    }
}
