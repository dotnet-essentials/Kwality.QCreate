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

using global::System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

[Generator]
public sealed class ContainerGenerator : IIncrementalGenerator
{
    private const string source = """
        #nullable enable
        namespace Kwality.QCreate;

        /// <summary>
        ///     Container used to create objects.
        /// </summary>
        public sealed class Container : global::Kwality.QCreate.Abstractions.IContainer
        {
            private readonly TypeBuilderMap typeBuilders = new()
            {
                { typeof(bool), new global::Kwality.QCreate.Builders.System.BoolTypeBuilder() },
                { typeof(string), new global::Kwality.QCreate.Builders.System.StringTypeBuilder() },
                { typeof(global::System.Guid), new global::Kwality.QCreate.Builders.System.GuidTypeBuilder() },
                { typeof(short), new global::Kwality.QCreate.Builders.System.Int16TypeBuilder() },
                { typeof(int), new global::Kwality.QCreate.Builders.System.Int32TypeBuilder() },
                { typeof(long), new global::Kwality.QCreate.Builders.System.Int64TypeBuilder() },
            };
            
            /// <summary>
            ///     Create a new <see cref="Container" /> instance.
            /// </summary>
            public Container()
            {
                this.RegisterGeneratorCreatedTypeBuilders();
                this.RegisterUserDefinedTypeBuilders();
            }

            /// <inheritdoc />
            /// <remarks>Defaults to 3.</remarks>
            public int RepeatCount { get; set; } = 3;

            /// <inheritdoc />
            public T Create<T>(global::Kwality.QCreate.Requests.Abstractions.Request? request = null)
            {
                return this.typeBuilders.TryGetValue(typeof(T), out var builder)
                    ? ((global::Kwality.QCreate.Builders.Abstractions.ITypeBuilder<T>)builder).Create(this, request)
                    : throw new global::Kwality.QCreate.Exceptions.QCreateException(
                        $"No builder registered for type '{typeof(T)}'."
                    );
            }

            /// <inheritdoc />
            public global::System.Collections.Generic.IEnumerable<T> CreateMany<T>(
                global::Kwality.QCreate.Requests.Abstractions.Request? request = null
            )
            {
                for (var i = 0; i < this.RepeatCount; i++)
                {
                    yield return this.Create<T>(request);
                }
            }

            /// <summary>
            ///     Register a custom <see cref="global::Kwality.QCreate.Builders.Abstractions.ITypeBuilder{T}" /> used to
            ///     create an instance of T. If another builder which create an instance of T is already registered, it gets
            ///     replaced with this one.
            /// </summary>
            /// <param name="builder">The builder used to create an instance of T.</param>
            /// <typeparam name="T">The type the builder can create.</typeparam>
            public void Register<T>(global::Kwality.QCreate.Builders.Abstractions.ITypeBuilder<T> builder) =>
                this.typeBuilders[typeof(T)] = builder;

            private void Register(global::System.Type type, global::System.Object builder) => this.typeBuilders[type] = builder;

            private void RegisterUserDefinedTypeBuilders()
            {
                global::System.Collections.Generic.IEnumerable<global::Kwality.QCreate.Data.TypeBuilderDefinition> userDefinedTypeBuilders =
                    global::Kwality.QCreate.UserBasedGeneratedTypeBuilders.GetTypeBuildersDefinition();

                foreach (global::Kwality.QCreate.Data.TypeBuilderDefinition? definition in userDefinedTypeBuilders)
                {
                    this.Register(definition.Type, definition.Builder);
                }
            }

            private void RegisterGeneratorCreatedTypeBuilders()
            {
                global::System.Collections.Generic.IEnumerable<global::Kwality.QCreate.Data.TypeBuilderDefinition> userDefinedTypeBuilders =
                    global::Kwality.QCreate.GeneratorBasedGeneratedTypeBuilders.GetTypeBuildersDefinition();

                foreach (global::Kwality.QCreate.Data.TypeBuilderDefinition? definition in userDefinedTypeBuilders)
                {
                    this.Register(definition.Type, definition.Builder);
                }
            }

            private sealed class TypeBuilderMap : global::System.Collections.Generic.Dictionary<global::System.Type, object>;
        }
        """;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(AddContainerSource);

        static void AddContainerSource(IncrementalGeneratorPostInitializationContext ctx) =>
            ctx.AddSource("ContainerGenerator.g.cs", SourceText.From(source, Encoding.UTF8));
    }
}
