namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Files
    {
        public static IEnumerable<GeneratedFile> Generate()
        {
            var options = new Options();
            return Generate(options);
        }

        public static IEnumerable<GeneratedFile> Generate(Options options)
        {
            if (options == null)
                throw new ArgumentNullException("options");

            var assembly = typeof(BrowsingContext).Assembly;
            var candidates = assembly.GetCandidates();
            var bindings = candidates.GetBindings();
            return bindings.Select(m => new GeneratedFile(m, options.Extension));
        }
    }
}
