namespace AngleSharp.Scripting.JavaScript.Generator.Templates
{
    public class ClassConstructorModel
    {
    }

    partial class ClassConstructor
    {
        public ClassConstructor(ClassConstructorModel model)
        {
            Model = model;
        }

        public ClassConstructorModel Model
        {
            get;
            private set;
        }
    }
}
