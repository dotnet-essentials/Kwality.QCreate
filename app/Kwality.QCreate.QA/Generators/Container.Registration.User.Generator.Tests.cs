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
namespace Kwality.QCreate.QA.Generators;

using Kwality.QCreate.QA.Verifiers;
using Kwality.QCreate.Roslyn.Generators;
using Xunit;

public sealed class ContainerRegistrationUserGeneratorTests
{
    [Fact(DisplayName = "The user-based 'Container' registration class is added (always).")]
    internal void The_container_is_added()
    {
        // ARRANGE.
        var sut = new SourceGeneratorVerifier<ContainerRegistrationUserGenerator>
        {
            InputSources = [],
            ExpectedGeneratedSources =
            [
                """
                    #nullable enable
                    namespace Kwality.QCreate;

                    internal static class UserBasedGeneratedTypeBuilders
                    {
                        public static global::System.Collections.Generic.IEnumerable<global::Kwality.QCreate.Data.TypeBuilderDefinition> GetTypeBuildersDefinition()
                        {
                            return [];
                        }
                    }
                    """,
            ],
        };

        // ACT & ASSERT.
        sut.Verify(["Provider.InitialExtraction", "Provider.NotNull"]);
    }

    [Fact(
        DisplayName = "A custom 'TypeBuilder' (marked, without the 'ITypeBuilder<T>' interface) does NOT generate code."
    )]
    internal void Marked_builder_without_interface_does_not_generate_code()
    {
        // ARRANGE.
        var sut = new SourceGeneratorVerifier<ContainerRegistrationUserGenerator>
        {
            InputSources =
            [
                """
                    [global::Kwality.QCreate.Attributes.TypeBuilder]
                    public sealed class FixedSquareTypeBuilder
                    {
                        public (int, int) Create(global::Kwality.QCreate.Requests.Abstractions.Request request, global::Kwality.QCreate.Abstractions.IContainer container)
                        {
                            return (0, 0);
                        }
                    }
                    """,
            ],
            ExpectedGeneratedSources =
            [
                """
                    #nullable enable
                    namespace Kwality.QCreate;

                    internal static class UserBasedGeneratedTypeBuilders
                    {
                        public static global::System.Collections.Generic.IEnumerable<global::Kwality.QCreate.Data.TypeBuilderDefinition> GetTypeBuildersDefinition()
                        {
                            return [];
                        }
                    }
                    """,
            ],
        };

        // ACT & ASSERT.
        sut.Verify(["Provider.InitialExtraction", "Provider.NotNull"]);
    }

    [Fact(DisplayName = "A custom 'TypeBuilder' (marked) is added to the registration class.")]
    internal void Marked_builder_for_tuple_is_added_to_the_registration_class()
    {
        // ARRANGE.
        var sut = new SourceGeneratorVerifier<ContainerRegistrationUserGenerator>
        {
            InputSources =
            [
                """
                    [global::Kwality.QCreate.Attributes.TypeBuilder]
                    public sealed class FixedSquareTypeBuilder : global::Kwality.QCreate.Builders.Abstractions.ITypeBuilder<(int, int)>
                    {
                        public (int, int) Create(global::Kwality.QCreate.Abstractions.IContainer container, global::Kwality.QCreate.Requests.Abstractions.Request request)
                        {
                            return (0, 0);
                        }
                    }
                    """,
            ],
            ExpectedGeneratedSources =
            [
                """
                    #nullable enable
                    namespace Kwality.QCreate;

                    internal static class UserBasedGeneratedTypeBuilders
                    {
                        public static global::System.Collections.Generic.IEnumerable<global::Kwality.QCreate.Data.TypeBuilderDefinition> GetTypeBuildersDefinition()
                        {
                            yield return new global::Kwality.QCreate.Data.TypeBuilderDefinition(typeof((int, int)), new global::FixedSquareTypeBuilder());
                        }
                    }
                    """,
            ],
        };

        // ACT & ASSERT.
        sut.Verify(["Provider.InitialExtraction", "Provider.NotNull"]);
    }

    [Fact(DisplayName = "A custom 'TypeBuilder' (marked, namespaced) is added to the registration class.")]
    internal void Marked_namespaced_builder_for_tuple_is_added_to_the_registration_class()
    {
        // ARRANGE.
        var sut = new SourceGeneratorVerifier<ContainerRegistrationUserGenerator>
        {
            InputSources =
            [
                """
                    namespace N1;

                    [global::Kwality.QCreate.Attributes.TypeBuilder]
                    public sealed class FixedSquareTypeBuilder : global::Kwality.QCreate.Builders.Abstractions.ITypeBuilder<(int, int)>
                    {
                        public (int, int) Create(global::Kwality.QCreate.Abstractions.IContainer container, global::Kwality.QCreate.Requests.Abstractions.Request request)
                        {
                            return (0, 0);
                        }
                    }
                    """,
            ],
            ExpectedGeneratedSources =
            [
                """
                    #nullable enable
                    namespace Kwality.QCreate;

                    internal static class UserBasedGeneratedTypeBuilders
                    {
                        public static global::System.Collections.Generic.IEnumerable<global::Kwality.QCreate.Data.TypeBuilderDefinition> GetTypeBuildersDefinition()
                        {
                            yield return new global::Kwality.QCreate.Data.TypeBuilderDefinition(typeof((int, int)), new global::N1.FixedSquareTypeBuilder());
                        }
                    }
                    """,
            ],
        };

        // ACT & ASSERT.
        sut.Verify(["Provider.InitialExtraction", "Provider.NotNull"]);
    }

    [Fact(DisplayName = "A custom 'TypeBuilder' (marked, nested) is added to the registration class.")]
    internal void Marked_nested_builder_for_tuple_is_added_to_the_registration_class()
    {
        // ARRANGE.
        var sut = new SourceGeneratorVerifier<ContainerRegistrationUserGenerator>
        {
            InputSources =
            [
                """
                    public sealed class Builders
                    {
                        [global::Kwality.QCreate.Attributes.TypeBuilder]
                        public sealed class FixedSquareTypeBuilder : global::Kwality.QCreate.Builders.Abstractions.ITypeBuilder<(int, int)>
                        {
                            public (int, int) Create(global::Kwality.QCreate.Abstractions.IContainer container, global::Kwality.QCreate.Requests.Abstractions.Request request)
                            {
                                return (0, 0);
                            }
                        }
                    }
                    """,
            ],
            ExpectedGeneratedSources =
            [
                """
                    #nullable enable
                    namespace Kwality.QCreate;

                    internal static class UserBasedGeneratedTypeBuilders
                    {
                        public static global::System.Collections.Generic.IEnumerable<global::Kwality.QCreate.Data.TypeBuilderDefinition> GetTypeBuildersDefinition()
                        {
                            yield return new global::Kwality.QCreate.Data.TypeBuilderDefinition(typeof((int, int)), new global::Builders.FixedSquareTypeBuilder());
                        }
                    }
                    """,
            ],
        };

        // ACT & ASSERT.
        sut.Verify(["Provider.InitialExtraction", "Provider.NotNull"]);
    }

    [Fact(DisplayName = "A custom 'TypeBuilder' (marked, namespaced, nested) is added to the registration class.")]
    internal void Marked_nested_namespaced_builder_for_tuple_is_added_to_the_registration_class()
    {
        // ARRANGE.
        var sut = new SourceGeneratorVerifier<ContainerRegistrationUserGenerator>
        {
            InputSources =
            [
                """
                    namespace N1;

                    public sealed class Builders
                    {
                        [global::Kwality.QCreate.Attributes.TypeBuilder]
                        public sealed class FixedSquareTypeBuilder : global::Kwality.QCreate.Builders.Abstractions.ITypeBuilder<(int, int)>
                        {
                            public (int, int) Create(global::Kwality.QCreate.Abstractions.IContainer container, global::Kwality.QCreate.Requests.Abstractions.Request request)
                            {
                                return (0, 0);
                            }
                        }
                    }
                    """,
            ],
            ExpectedGeneratedSources =
            [
                """
                    #nullable enable
                    namespace Kwality.QCreate;

                    internal static class UserBasedGeneratedTypeBuilders
                    {
                        public static global::System.Collections.Generic.IEnumerable<global::Kwality.QCreate.Data.TypeBuilderDefinition> GetTypeBuildersDefinition()
                        {
                            yield return new global::Kwality.QCreate.Data.TypeBuilderDefinition(typeof((int, int)), new global::N1.Builders.FixedSquareTypeBuilder());
                        }
                    }
                    """,
            ],
        };

        // ACT & ASSERT.
        sut.Verify(["Provider.InitialExtraction", "Provider.NotNull"]);
    }
}
