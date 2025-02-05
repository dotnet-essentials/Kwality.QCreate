﻿// =====================================================================================================================
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
namespace Kwality.QCreate.Design.QA.System;

using Xunit;

public sealed partial class ContainerTests
{
    [Fact(DisplayName = "'Create<T>': When 'T' is a 'bool' the results alternate between 'true' and 'false'.")]
    internal void Create_bool_alternates_between_true_and_false()
    {
        // ARRANGE.
        var container = new Container();

        // ACT.
        var r1 = container.Create<bool>();
        var r2 = container.Create<bool>();
        var r3 = container.Create<bool>();
        var r4 = container.Create<bool>();

        // ASSERT.
        Assert.True(r1, "The 1st time a 'bool' is created, it should have the value 'true'.");
        Assert.False(r2, "The 2nd time a 'bool' is created, it should have the value 'false'.");
        Assert.True(r3, "The 3rd time a 'bool' is created, it should have the value 'true'.");
        Assert.False(r4, "The 4th time a 'bool' is created, it should have the value 'false'.");
    }
}
