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

public sealed class ContainerRegistrationGeneratedGeneratorTests
{
    [Fact(DisplayName = "The generator-based 'Container' registration class is added (always).")]
    internal void The_container_is_added()
    {
        // ARRANGE.
        var sut = new SourceGeneratorVerifier<ContainerRegistrationGeneratedGenerator>
        {
            InputSources = [],
            ExpectedGeneratedSources =
            [
                """
                    #nullable enable
                    namespace Kwality.QCreate;

                    internal static class GeneratorBasedGeneratedTypeBuilders
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
        sut.Verify();
    }
}
