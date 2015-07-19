namespace AngleSharp.Scripting.JavaScript.Generator
{
    using Microsoft.CodeAnalysis.CSharp;

    static class RoslynExtensions
    {
        public static void AddClass(this SyntaxWriter writer, BindingClass member)
        {
            var classDeclaration = SyntaxFactory.ClassDeclaration(member.Name);
            var classKeyword = SyntaxFactory.Token(SyntaxKind.ClassKeyword);
            var partialKeyword = SyntaxFactory.Token(SyntaxKind.PartialKeyword);
            var openBrace = SyntaxFactory.Token(SyntaxKind.OpenBraceToken);
            var closeBrace = SyntaxFactory.Token(SyntaxKind.CloseBraceToken);

            var cls = classDeclaration.WithKeyword(classKeyword)
                                      .WithKeyword(partialKeyword)
                                      .WithOpenBraceToken(openBrace)
                                      .WithCloseBraceToken(closeBrace);

            writer.Add(cls);
        }
    }
}
