var target = Argument("target", "Default");
var projectName = "AngleSharp.Js";
var solutionName = "AngleSharp.Js";
var frameworks = new Dictionary<String, String>
{
    { "net46", "net46" },
    { "netstandard2.0", "netstandard2.0" },
};

#load tools/anglesharp.cake

RunTarget(target);
