// =====================================================================================================================
// == LICENSE:       Copyright (c) 2025 Kevin De Coninck
// ==
// ==                Permission is hereby granted, free of charge, to any person
// ==                obtaining a copy of this software and associated documentation
// ==                files (the "Software"), to deal in the Software without
// ==                restriction, including without limitation the rights to use,
// ==                copy, modify, merge, publish, distribute, sublicense, and/or sell
// ==                copies of the Software, and to permit persons to whom the
// ==                Software is furnished to do so, subject to the following
// ==                conditions:
// ==
// ==                The above copyright notice and this permission notice shall be
// ==                included in all copies or substantial portions of the Software.
// ==
// ==                THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// ==                EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// ==                OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// ==                NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// ==                HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// ==                WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// ==                FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// ==                OTHER DEALINGS IN THE SOFTWARE.
// =====================================================================================================================
#pragma warning disable CS1591
namespace Kwality.QCreate.Roslyn.Generators;

using System.Collections.Immutable;
using System.Text;
using Kwality.QCreate.Roslyn.Extensions.Symbols;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

[Generator]
public sealed class ContainerRegistrationUserGenerator : IIncrementalGenerator
{
    private const string typeBuilderInterfaceFqName = "Kwality.QCreate.Builders.Abstractions.ITypeBuilder<T>";

    private const string sourceTemplate = """
        #nullable enable
        namespace Kwality.QCreate;

        internal static class UserBasedGeneratedTypeBuilders
        {
            public static global::System.Collections.Generic.IEnumerable<global::Kwality.QCreate.Data.TypeBuilderDefinition> GetTypeBuildersDefinition()
            {
        [[#[[TYPE_BUILDERS_YIELD]]#]]    }
        }
        """;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValueProvider<ImmutableArray<TypeBuilderDefinition?>> provider = GetProvider(context).Collect();

        context.RegisterSourceOutput(provider, this.GenerateRegistrationClass);
    }

    private static IncrementalValuesProvider<TypeBuilderDefinition?> GetProvider(
        IncrementalGeneratorInitializationContext context
    )
    {
        const string markerAttribute = "Kwality.QCreate.Attributes.TypeBuilderAttribute";

        return context
            .SyntaxProvider.ForAttributeWithMetadataName(markerAttribute, Predicate, Transformation)
            .WithTrackingName("Provider.InitialExtraction")
            .Where(builder => builder is not null)
            .WithTrackingName("Provider.NotNull");

        bool Predicate(SyntaxNode node, CancellationToken _) => node is ClassDeclarationSyntax;

        TypeBuilderDefinition? Transformation(GeneratorAttributeSyntaxContext ctx, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (ctx.TargetSymbol is not INamedTypeSymbol classSymbol)
            {
                return null;
            }

            INamedTypeSymbol? interfaceSymbol = classSymbol.GetImplementingInterface(typeBuilderInterfaceFqName);

            return interfaceSymbol == null ? null : NewTypeBuilderDefinition(classSymbol, interfaceSymbol);
        }
    }

    private void GenerateRegistrationClass(
        SourceProductionContext ctx,
        ImmutableArray<TypeBuilderDefinition?> definitions
    )
    {
        var sBuilder = new StringBuilder();

        if (definitions.IsDefaultOrEmpty)
        {
            _ = sBuilder.AppendLine("        return [];");
        }

        foreach (TypeBuilderDefinition? definition in Enumerable.OfType<TypeBuilderDefinition>(definitions))
        {
            _ = sBuilder.AppendLine(
                $"        yield return new global::Kwality.QCreate.Data.TypeBuilderDefinition(typeof({definition.ForFqTypeName}), new {definition.FqName}());"
            );
        }

        var source = sourceTemplate.Replace("[[#[[TYPE_BUILDERS_YIELD]]#]]", sBuilder.ToString());
        ctx.AddSource("UserDefined.TypeBuilders.Registration.g.cs", SourceText.From(source, Encoding.UTF8));
    }

    private static TypeBuilderDefinition NewTypeBuilderDefinition(INamedTypeSymbol @class, INamedTypeSymbol @interface)
    {
        ITypeSymbol forType = @interface.TypeArguments.First();
        var forFqTypeName = forType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var fqName = @class.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        return new(forFqTypeName, fqName);
    }

    private sealed record TypeBuilderDefinition(string ForFqTypeName, string FqName)
    {
        public string ForFqTypeName { get; } = ForFqTypeName;

        public string FqName { get; } = FqName;
    }
}
