namespace AngleSharp.Scripting.JavaScript.Generator
{
    using Microsoft.CodeAnalysis.CSharp;

    static class RoslynExtensions
    {
        public static void AddEnum(this SyntaxWriter writer, BindingEnum type)
        {
            //TODO
        }

        public static void AddClass(this SyntaxWriter writer, BindingClass member)
        {
            var classDeclaration = SyntaxFactory.ClassDeclaration(member.Name);
            var classKeyword = SyntaxFactory.Token(SyntaxKind.ClassKeyword);
            var openBrace = SyntaxFactory.Token(SyntaxKind.OpenBraceToken);
            var closeBrace = SyntaxFactory.Token(SyntaxKind.CloseBraceToken);
            var publicModifier = SyntaxFactory.Token(SyntaxKind.PublicKeyword);
            var partialModifier = SyntaxFactory.Token(SyntaxKind.PartialKeyword);
            var modifiers = SyntaxFactory.TokenList(new []
            {
                publicModifier,
                partialModifier
            });

            var cls = classDeclaration.WithKeyword(classKeyword)
                                      .WithModifiers(modifiers)
                                      .WithOpenBraceToken(openBrace)
                                      .WithCloseBraceToken(closeBrace);

            writer.Add(cls);
        }
    }
}
