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
namespace Kwality.QCreate.Design.QA;

using Kwality.QCreate.Design.QA.Extensions;
using Kwality.QCreate.Requests;
using Xunit;

public sealed partial class ContainerTests
{
    [Fact(DisplayName = "'Create<T>': When 'T' is a 'string' a unique 'string' is returned.")]
    internal void Create_string_returns_a_unique_string()
    {
        // ARRANGE.
        var container = new Container();

        // ACT.
        var r1 = container.Create<string>();
        var r2 = container.Create<string>();

        // ASSERT.
        Assert.True(Guid.TryParse(r1, out _), "The generated string must be a 'GUID'.");
        Assert.True(Guid.TryParse(r2, out _), "The generated string must be a 'GUID'.");
        Assert.True(r1 != r2, "The generated strings must be unique.");
    }

    [Fact(DisplayName = "'Create<T> (seeded)': When 'T' is a 'string' the seed is used as a prefix.")]
    internal void Create_string_with_seed_uses_the_seed_as_prefix()
    {
        // ARRANGE.
        var container = new Container();

        // ACT.
        var r1 = container.Create<string>(new SeededRequest<string>("Hello"));

        // ASSERT.
        r1.AssertHasPrefix("Hello_");
        r1.AssertEndsWithGuid("Hello_");
    }
}
