namespace AngleSharp.Scripting.JavaScript.Generator.Templates
{
    using System;

    public class PropertyModel
    {
        public String Name
        {
            get;
            set;
        }

        public MethodModel Getter
        {
            get;
            set;
        }

        public MethodModel Setter
        {
            get;
            set;
        }

        public String Call
        {
            get
            {
                if (Getter.IsVoid == false && Setter.IsVoid == false)
                    return Getter.RefName + ", " + Setter.RefName;
                else if (Getter.IsVoid == false)
                    return Getter.RefName;
                else if (Setter.IsVoid == false)
                    return Setter.RefName;

                return String.Empty;
            }
        }
    }
}
