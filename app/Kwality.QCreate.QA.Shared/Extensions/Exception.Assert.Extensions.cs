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
    ///     Assert that the given exception is of a certain type with a given message.
    /// </summary>
    /// <param name="ex">The exception to check.</param>
    /// <param name="message">The message of the exception.</param>
    /// <typeparam name="TException">The type that the given exception should be.</typeparam>
    public static void AssertType<TException>(this Exception? ex, string message)
    {
        Assert.True(ex != null, "The exception should NOT be null.");
        Assert.True(message == ex.Message, $"Exception should be '{message}', but found '{ex.Message}'.");

        _ = Assert.IsType<TException>(ex);
    }
}
