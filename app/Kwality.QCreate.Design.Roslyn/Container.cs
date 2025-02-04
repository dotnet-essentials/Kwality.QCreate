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
namespace Kwality.QCreate;

/// <summary>
///     Container used to create objects.
/// </summary>
public sealed class Container
{
    private readonly TypeBuilderMap typeBuilders = new()
    {
        { typeof(bool), new global::Kwality.QCreate.Builders.System.BoolTypeBuilder() },
        { typeof(string), new global::Kwality.QCreate.Builders.System.StringTypeBuilder() },
        { typeof(global::System.Guid), new global::Kwality.QCreate.Builders.System.GuidTypeBuilder() },
        { typeof(short), new global::Kwality.QCreate.Builders.System.Int16TypeBuilder() },
        { typeof(int), new global::Kwality.QCreate.Builders.System.Int32TypeBuilder() },
        { typeof(long), new global::Kwality.QCreate.Builders.System.Int64TypeBuilder() },
    };

    /// <summary>
    ///     The total number of elements to create when using <see cref="CreateMany{T}" />.
    /// </summary>
    /// <remarks>Defaults to 3.</remarks>
    public int RepeatCount { get; set; } = 3;

    /// <summary>
    ///     Create an instance of T.
    /// </summary>
    /// <typeparam name="T">The type to create.</typeparam>
    /// <param name="request">The request that describes how to create an instance of T.</param>
    /// <returns>An instance of T.</returns>
    /// <exception cref="global::Kwality.QCreate.Exceptions.QCreateException">An instance of T couldn't be created.</exception>
    public T Create<T>(global::Kwality.QCreate.Requests.Abstractions.Request? request = null)
    {
        return this.typeBuilders.TryGetValue(typeof(T), out var builder)
            ? ((global::Kwality.QCreate.Builders.Abstractions.ITypeBuilder<T>)builder).Create(request)
            : throw new global::Kwality.QCreate.Exceptions.QCreateException(
                $"No builder registered for type '{typeof(T)}'."
            );
    }

    /// <summary>
    ///     Create multiple instances of T.
    ///     The amount of instances that's returned is equal to <see cref="RepeatCount" />, which defaults to 3.
    /// </summary>
    /// <typeparam name="T">The type to create.</typeparam>
    /// <param name="request">The request that describes how to create an instance of T.</param>
    /// <returns>A collection containing multiple instances of T.</returns>
    /// <exception cref="global::Kwality.QCreate.Exceptions.QCreateException">An instance of T couldn't be created.</exception>
    public global::System.Collections.Generic.IEnumerable<T> CreateMany<T>(
        global::Kwality.QCreate.Requests.Abstractions.Request? request = null
    )
    {
        for (var i = 0; i < this.RepeatCount; i++)
        {
            yield return this.Create<T>(request);
        }
    }

    private sealed class TypeBuilderMap : System.Collections.Generic.Dictionary<global::System.Type, object>;
}
