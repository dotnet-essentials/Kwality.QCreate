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
#pragma warning disable CA2201
namespace Kwality.QCreate.QA.Verifiers;

using System.Collections.Immutable;
using System.Globalization;
using Kwality.QCreate.QA.Shared.Extensions;
using Kwality.QCreate.QA.Verifiers.Abstractions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Xunit;

internal sealed class SourceGeneratorVerifier<TGenerator> : RoslynComponentVerifier
    where TGenerator : IIncrementalGenerator, new()
{
    public IIncrementalGenerator[]? AdditionalGenerators { get; init; }
    public string[]? ExpectedGeneratedSources { get; init; }

    public void Verify(string[]? trackingNames = null)
    {
        // ARRANGE.
        Compilation compilation = this.CreateCompilation();

        if (compilation.GetDiagnostics().Any())
        {
            DiagnosticData diagnostic = GetDiagnostic(compilation.GetDiagnostics().First());

            throw new(
                $"No diagnostics expected in the source. Found: {diagnostic.Id} - {diagnostic.Message} @ {diagnostic.File} ({diagnostic.Location})."
            );
        }

        IEnumerable<ISourceGenerator> generators = new List<ISourceGenerator>
        {
            new TGenerator().AsSourceGenerator(),
        }.Concat((this.AdditionalGenerators ?? []).Select(x => x.AsSourceGenerator()));

        var options = new GeneratorDriverOptions(IncrementalGeneratorOutputKind.None);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generators, driverOptions: options);

        // ACT (RUN 1).
        driver = driver.RunGeneratorsAndUpdateCompilation(
            compilation,
            out Compilation outputCompilation,
            out ImmutableArray<Diagnostic> _
        );

        if (outputCompilation.GetDiagnostics().Any())
        {
            DiagnosticData diagnostic = GetDiagnostic(outputCompilation.GetDiagnostics().First());

            throw new(
                $"No diagnostics expected in code has been generated. Found: {diagnostic.Id} - {diagnostic.Message} @ {diagnostic.File} ({diagnostic.Location})."
            );
        }

        GeneratorDriverRunResult runResult1 = driver.GetRunResult();

        // ACT (RUN 2).
        driver = driver.RunGeneratorsAndUpdateCompilation(
            compilation.Clone(),
            out Compilation outputCompilation2,
            out ImmutableArray<Diagnostic> _
        );

        if (outputCompilation2.GetDiagnostics().Any())
        {
            DiagnosticData diagnostic = GetDiagnostic(outputCompilation2.GetDiagnostics().First());

            throw new(
                $"No diagnostics expected in code has been generated. Found: {diagnostic.Id} - {diagnostic.Message} @ {diagnostic.File} ({diagnostic.Location})."
            );
        }

        GeneratorDriverRunResult runResult2 = driver.GetRunResult();

        // ASSERT.
        Assert.Empty(runResult1.Diagnostics);
        Assert.Empty(runResult2.Diagnostics);

        runResult1.GeneratedTrees.Select(x => x.ToString()).AssertContains(this.ExpectedGeneratedSources ?? []);

        if (trackingNames != null)
        {
            bool hasNonCachedSteps = runResult2
                .Results[0]
                .TrackedOutputSteps.SelectMany(x => x.Value)
                .SelectMany(x => x.Outputs)
                .Any(x => x.Reason != IncrementalStepRunReason.Cached);

            Assert.False(hasNonCachedSteps, "The second run should only contains 'cached' steps.");
        }
    }

    private static DiagnosticData GetDiagnostic(Diagnostic diagnostic)
    {
        var message = diagnostic.GetMessage(CultureInfo.InvariantCulture);
        var file = diagnostic.Location.SourceTree?.FilePath ?? string.Empty;
        LinePosition diagnosticSpan = diagnostic.Location.GetLineSpan().StartLinePosition;
        var location = $"{diagnosticSpan.Line + 1},{diagnosticSpan.Character + 1}";

        return new DiagnosticData(diagnostic.Id, message, file, location);
    }

    private sealed record DiagnosticData(string Id, string Message, string File, string Location);
}
