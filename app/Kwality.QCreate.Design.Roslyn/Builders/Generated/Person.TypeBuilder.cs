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
namespace Kwality.QCreate.Builders.Generated;

[global::Kwality.QCreate.Attributes.TypeBuilder]
internal sealed class PersonTypeBuilder
    : global::Kwality.QCreate.Builders.Abstractions.ITypeBuilder<global::Kwality.QCreate.Models.Person>
{
    public global::Kwality.QCreate.Models.Person Create(
        global::Kwality.QCreate.Abstractions.IContainer container,
        global::Kwality.QCreate.Requests.Abstractions.Request? request
    )
    {
        var p1Request = new global::Kwality.QCreate.Requests.SeededRequest<string>(
            nameof(global::Kwality.QCreate.Models.Person.FirstName)
        );

        var p2Request = new global::Kwality.QCreate.Requests.SeededRequest<string>(
            nameof(global::Kwality.QCreate.Models.Person.LastName)
        );

        var p1 = container.Create<string>(p1Request);
        var p2 = container.Create<string>(p2Request);

        return new(p1, p2);
    }
}
