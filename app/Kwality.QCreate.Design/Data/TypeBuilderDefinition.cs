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
namespace Kwality.QCreate.Data;

using Kwality.QCreate.Builders.Abstractions;

/// <summary>
///     A definition for an <see cref="ITypeBuilder{T}" /> instance.
/// </summary>
/// <param name="Type">The type the associated <see cref="ITypeBuilder{T}" /> can create.</param>
/// <param name="Builder">The <see name="ITypeBuilder{T}" /> instance.</param>
public sealed record TypeBuilderDefinition(Type Type, object Builder)
{
    /// <summary>
    ///     The type the associated <see cref="ITypeBuilder{T}" /> can create.
    /// </summary>
    public Type Type { get; } = Type;

    /// <summary>
    ///     The <see name="ITypeBuilder{T}" /> instance.
    /// </summary>
    public object Builder { get; } = Builder;
}
