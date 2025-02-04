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
#pragma warning disable CA1716
#pragma warning disable CA1062
namespace Kwality.QCreate.QA.Shared.Extensions;

using Xunit;

/// <summary>
///     Contain extensions for xUnit's 'Assert' functionality.
/// </summary>
public static partial class AssertExtensions
{
    /// <summary>
    ///     Assert that a given string starts with a certain prefix.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <param name="prefix">The prefix the given string should start with.</param>
    public static void AssertHasPrefix(this string value, string prefix)
    {
        var startsWith = value.StartsWith(prefix, StringComparison.CurrentCulture);

        Assert.True(startsWith, $"The string should start with '{prefix}'.");
    }

    /// <summary>
    ///     Assert that a given string starts with a certain prefix followed by a GUID.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <param name="prefix">The prefix the given string should start with.</param>
    public static void AssertEndsWithGuid(this string value, string prefix)
    {
        var valueWithoutPrefix = value.Replace(prefix, "");

        Assert.True(Guid.TryParse(valueWithoutPrefix, out _), "The string should end with a GUID.");
    }
}
