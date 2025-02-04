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
namespace Kwality.QCreate.QA.Generators.Builders.System;

using Kwality.QCreate.QA.Verifiers;
using Kwality.QCreate.Roslyn.Generators.Builders.System;
using Xunit;

public sealed class StringTypeBuilderGeneratorTests
{
    [Fact(DisplayName = "The 'ITypeBuilder<string>' implementation is added (always).")]
    internal void The_string_type_builder_is_added()
    {
        // ARRANGE.
        var sut = new SourceGeneratorVerifier<StringTypeBuilderGenerator>
        {
            InputSources = [],
            ExpectedGeneratedSources =
            [
                """
                    #nullable enable
                    namespace Kwality.QCreate.Builders.System;

                    internal sealed class StringTypeBuilder : global::Kwality.QCreate.Builders.Abstractions.ITypeBuilder<string>
                    {
                        public string Create(
                            global::Kwality.QCreate.Abstractions.IContainer container,
                            global::Kwality.QCreate.Requests.Abstractions.Request? request
                        )
                        {
                            var value = global::System.Guid.NewGuid().ToString();

                            return request is global::Kwality.QCreate.Requests.SeededRequest<string> seededRequest
                                ? $"{seededRequest.Value}_{value}"
                                : value;
                        }
                    }
                    """,
            ],
        };

        // ACT & ASSERT.
        sut.Verify();
    }
}
