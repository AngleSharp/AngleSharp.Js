namespace AngleSharp.Scripting.JavaScript.Generator.Templates
{
    public class ClassInstanceModel
    {
    }

    partial class ClassInstance
    {
        public ClassInstance(ClassInstanceModel model)
        {
            Model = model;
        }

        public ClassInstanceModel Model
        {
            get;
            private set;
        }
    }
}
