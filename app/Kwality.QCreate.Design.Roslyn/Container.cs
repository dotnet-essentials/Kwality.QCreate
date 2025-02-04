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
    };

    /// <summary>
    ///     Create an instance of T.
    /// </summary>
    /// <typeparam name="T">The type to create.</typeparam>
    /// <returns>An instance of T.</returns>
    /// <exception cref="global::Kwality.QCreate.Exceptions.QCreateException">An instance of T couldn't be created.</exception>
    public T Create<T>()
    {
        return this.typeBuilders.TryGetValue(typeof(T), out var builder)
            ? ((global::Kwality.QCreate.Builders.Abstractions.ITypeBuilder<T>)builder).Create()
            : throw new global::Kwality.QCreate.Exceptions.QCreateException(
                $"No builder registered for type '{typeof(T)}'."
            );
    }

    /// <summary>
    ///     Create 3 instances of T.
    /// </summary>
    /// <typeparam name="T">The type to create.</typeparam>
    /// <returns>A collection containing 3 instances of T.</returns>
    /// <exception cref="global::Kwality.QCreate.Exceptions.QCreateException">An instance of T couldn't be created.</exception>
    public global::System.Collections.Generic.IEnumerable<T> CreateMany<T>()
    {
        for (var i = 0; i < 3; i++)
        {
            yield return this.Create<T>();
        }
    }

    private sealed class TypeBuilderMap : System.Collections.Generic.Dictionary<global::System.Type, object>;
}
