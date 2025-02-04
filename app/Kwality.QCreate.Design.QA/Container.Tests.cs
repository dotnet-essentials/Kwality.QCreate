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

using Kwality.QCreate.Abstractions;
using Kwality.QCreate.Builders.Abstractions;
using Kwality.QCreate.Design.QA.Extensions;
using Kwality.QCreate.Exceptions;
using Kwality.QCreate.Models;
using Kwality.QCreate.Requests.Abstractions;
using Xunit;

public sealed class ContainerTests
{
    [Fact(DisplayName = "'Create<T>': When 'T' is a type for which NO builder is present, an exception is raised.")]
    internal void Create_unsupported_throws()
    {
        // ARRANGE.
        var container = new Container();

        // ACT.
        Exception? ex = Record.Exception(() => container.Create<(int, int)>());

        // ASSERT.
        ex.AssertType<QCreateException>($"No builder registered for type '{typeof((int, int))}'.");
    }

    [Fact(DisplayName = "'CreateMany<T>': Returns 3 elements by default.")]
    internal void Create_multiple_returns_3_elements_by_default()
    {
        // ARRANGE.
        var container = new Container();

        // ACT.
        var result = container.CreateMany<bool>().ToArray();

        // ASSERT.
        Assert.True(3 == result.Length, "Exactly 3 elements should be returned.");
        Assert.True(result[0], "The 1st created 'bool' should have the value 'true'.");
        Assert.False(result[1], "The 2nd created 'bool' should have the value 'false'.");
        Assert.True(result[2], "The 3rd created 'bool' should have the value 'true'.");
    }

    [Fact(DisplayName = "'CreateMany<T>': Returns the requested amount of elements.")]
    internal void Create_multiple_returns_requested_elements_by_default()
    {
        // ARRANGE.
        var container = new Container { RepeatCount = 4 };

        // ACT.
        var result = container.CreateMany<bool>().ToArray();

        // ASSERT.
        Assert.True(4 == result.Length, "Exactly 4 elements should be returned.");
        Assert.True(result[0], "The 1st created 'bool' should have the value 'true'.");
        Assert.False(result[1], "The 2nd created 'bool' should have the value 'false'.");
        Assert.True(result[2], "The 3rd created 'bool' should have the value 'true'.");
        Assert.False(result[1], "The 4th created 'bool' should have the value 'false'.");
    }

    [Fact(DisplayName = "'Register<T>': Overwrites the built-in builder for 'T'.")]
    internal void Register_custom_builder_overwrites_the_built_in_builder()
    {
        // ARRANGE.
        var container = new Container();

        // ACT.
        container.Register(new FixedStringTypeBuilder());
        var result = container.Create<string>();

        // ASSERT.
        Assert.True("Hello, World!" == result, "The generated string must be 'Hello, World!'.");
    }

    [Fact(DisplayName = "'Register<T>': Adds the new builder for 'T'.")]
    internal void Register_custom_builder_adds_new_builder()
    {
        // ARRANGE.
        var container = new Container();

        // ACT.
        container.Register(new CoordinateTypeBuilder());
        (int, int) r1 = container.Create<(int, int)>();
        (int, int) r2 = container.Create<(int, int)>();

        // ASSERT.
        Assert.True(r1 != r2, "The generated values must be unique.");
    }

    [Fact(DisplayName = "A user-defined builder for 'T' has priority over a generated builder for 'T'.")]
    internal void User_defined_builders_have_priority_over_the_generated_builders()
    {
        // ARRANGE.
        var container = new Container();
        container.Register(new FixedPersonTypeBuilder());

        // ACT.
        Person r1 = container.Create<Person>();

        // ASSERT.
        Assert.True(r1 == new Person("Donald", "Trump"), "The generated value must be 'Donald Trump'.");
    }

    private sealed class FixedStringTypeBuilder : ITypeBuilder<string>
    {
        public string Create(IContainer container, Request? request) => "Hello, World!";
    }

    private sealed class CoordinateTypeBuilder : ITypeBuilder<(int, int)>
    {
        public (int, int) Create(IContainer container, Request? _) =>
            (container.Create<int>(), container.Create<int>());
    }

    private sealed class FixedPersonTypeBuilder : ITypeBuilder<Person>
    {
        public Person Create(IContainer container, Request? request) => new("Donald", "Trump");
    }
}
