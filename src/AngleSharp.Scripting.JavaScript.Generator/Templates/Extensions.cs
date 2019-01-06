namespace AngleSharp.Scripting.JavaScript.Generator.Templates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    static class Extensions
    {
        public static String Stringify(this IEnumerable<ParameterModel> parameters)
        {
            return String.Join(", ", parameters.Select(m => m.Name));
        }
    }
}
