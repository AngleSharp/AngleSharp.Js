namespace AngleSharp.Scripting.JavaScript.Generator.Templates
{
    using System;

    public class EventModel
    {
        public String Name 
        { 
            get; 
            set;
        }

        public String Converter 
        { 
            get; 
            set;
        }

        public Boolean IsLenient 
        { 
            get; 
            set; 
        }

        public String OriginalName 
        { 
            get; 
            set; 
        }

        public String RefName
        {
            get { return OriginalName + "Event"; }
        }
    }
}
