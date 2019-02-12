﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DirectX12GameEngine.Rendering.Shaders
{
    public class ShaderSyntaxRewriter : CSharpSyntaxRewriter
    {
        private readonly SemanticModel semanticModel;

        public ShaderSyntaxRewriter(SemanticModel semanticModel)
        {
            this.semanticModel = semanticModel;
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            node = (MethodDeclarationSyntax)base.VisitMethodDeclaration(node);

            SyntaxTokenList modifiers = new SyntaxTokenList();

            if (node.Modifiers.Any(SyntaxKind.StaticKeyword))
            {
                modifiers = modifiers.Add(SyntaxFactory.Token(default, SyntaxKind.StaticKeyword, SyntaxFactory.TriviaList(SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, " "))));
            }

            return node.ReplaceType(node.ReturnType).WithModifiers(modifiers);
        }

        public override SyntaxNode VisitParameter(ParameterSyntax node)
        {
            node = (ParameterSyntax)base.VisitParameter(node);
            return node.ReplaceType(node.Type);
        }

        public override SyntaxNode VisitAttribute(AttributeSyntax node)
        {
            node = (AttributeSyntax)base.VisitAttribute(node);
            return node.ReplaceType(node.Name);
        }

        public override SyntaxNode VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            node = (LocalDeclarationStatementSyntax)base.VisitLocalDeclarationStatement(node);
            return node.ReplaceType(node.Declaration.Type);
        }

        public override SyntaxNode VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            node = (ObjectCreationExpressionSyntax)base.VisitObjectCreationExpression(node);
            node = node.ReplaceType(node.Type);

            if (node.ArgumentList.Arguments.Count == 0)
            {
                return SyntaxFactory.CastExpression(node.Type, SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(0)));
            }
            else
            {
                return SyntaxFactory.InvocationExpression(node.Type, node.ArgumentList);
            }
        }

        public override SyntaxNode VisitDefaultExpression(DefaultExpressionSyntax node)
        {
            node = (DefaultExpressionSyntax)base.VisitDefaultExpression(node);
            node = node.ReplaceType(node.Type);
            return SyntaxFactory.CastExpression(node.Type, SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(0)));
        }

        public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            node = (MemberAccessExpressionSyntax)base.VisitMemberAccessExpression(node);
            return node.ReplaceMethod(semanticModel);
        }
    }
}