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
namespace Kwality.QCreate.Requests;

using Kwality.QCreate.Builders.Abstractions;
using Kwality.QCreate.Requests.Abstractions;

/// <summary>
///     A <see cref="Request" /> implementation that seeds a request to create an instance with a certain value.
///     This <see cref="Request" /> does NOT contain any logic on its own. It's the responsibility of the
///     <see cref="ITypeBuilder{T}" /> instances to handle this request and act accordingly.
/// </summary>
/// <param name="Value">The seed value.</param>
/// <typeparam name="T">The value of the seed.</typeparam>
public sealed record SeededRequest<T>(T? Value) : Request
{
    /// <summary>
    ///     The seed value.
    /// </summary>
    public T? Value { get; } = Value;
}
