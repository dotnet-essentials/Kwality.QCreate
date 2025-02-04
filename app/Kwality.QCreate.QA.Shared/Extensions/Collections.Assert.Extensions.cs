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
    ///     Assert that a given collection contains the elements from another collection.
    /// </summary>
    /// <param name="source">The collection that contains the actual output.</param>
    /// <param name="expected">The expected elements that should be available in source.</param>
    public static void AssertContains<T>(this IEnumerable<T> source, IEnumerable<T> expected)
    {
        T[] sourceSet = source.ToArray();

        foreach (T item in expected)
        {
            Assert.True(sourceSet.Contains(item), $"The item '{item}' is not present in the collection.");
        }
    }
}
