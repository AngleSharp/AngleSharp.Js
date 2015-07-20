namespace AngleSharp.Scripting.JavaScript.Generator
{
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using System;
    using System.Collections.Generic;

    sealed class SyntaxWriter
    {
        readonly NamespaceDeclarationSyntax _namespace;
        readonly List<MemberDeclarationSyntax> _declarations;

        public SyntaxWriter(String namespaceName)
        {
            var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(namespaceName));
            var namespaceKeyword = SyntaxFactory.Token(SyntaxKind.NamespaceKeyword);
            var openBrace = SyntaxFactory.Token(SyntaxKind.OpenBraceToken);
            var closeBrace = SyntaxFactory.Token(SyntaxKind.CloseBraceToken);

            _namespace = namespaceDeclaration.WithNamespaceKeyword(namespaceKeyword)
                                             .WithOpenBraceToken(openBrace)
                                             .WithCloseBraceToken(closeBrace);

            _declarations = new List<MemberDeclarationSyntax>();
        }

        public void Add(MemberDeclarationSyntax type)
        {
            _declarations.Add(type);
        }

        public String Serialize()
        {
            var eof = SyntaxFactory.Token(SyntaxKind.EndOfFileToken);
            var content = _namespace.WithMembers(SyntaxFactory.List<MemberDeclarationSyntax>(_declarations));
            var memberList = SyntaxFactory.List<MemberDeclarationSyntax>(new[] { content });
            var tree = SyntaxFactory.CompilationUnit()
                                    .WithMembers(memberList)
                                    .WithEndOfFileToken(eof);

            return tree.ToFullString();
        }
    }
}
