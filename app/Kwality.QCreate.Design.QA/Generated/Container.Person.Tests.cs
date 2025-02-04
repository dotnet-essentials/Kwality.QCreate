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
namespace Kwality.QCreate.Design.QA.Generated;

using Kwality.QCreate.Models;
using Kwality.QCreate.QA.Shared.Extensions;
using Xunit;

public sealed class ContainerTests
{
    [Fact(DisplayName = "'Create<T>': When 'T' is a 'Person' a unique 'Person' is returned.")]
    internal void Create_person_returns_a_unique_person()
    {
        // ARRANGE.
        var container = new Container();

        // ACT.
        Person r1 = container.Create<Person>();
        Person r2 = container.Create<Person>();

        // ASSERT.
        Assert.True(r1 != r2, "The generated values must be unique.");
        r1.FirstName.AssertHasPrefix("FirstName_");
        r2.FirstName.AssertEndsWithGuid("FirstName_");
        r1.LastName.AssertHasPrefix("LastName_");
        r2.LastName.AssertEndsWithGuid("LastName_");
    }
}
