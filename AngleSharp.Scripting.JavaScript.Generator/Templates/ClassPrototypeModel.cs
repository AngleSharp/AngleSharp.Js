namespace AngleSharp.Scripting.JavaScript.Generator.Templates
{
    public class ClassPrototypeModel
    {
    }

    partial class ClassPrototype
    {
        public ClassPrototype(ClassPrototypeModel model)
        {
            Model = model;
        }

        public ClassPrototypeModel Model
        {
            get;
            private set;
        }
    }
}
