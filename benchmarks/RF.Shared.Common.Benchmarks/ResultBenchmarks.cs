using BenchmarkDotNet.Attributes;
using RF.Shared.Common.Models.V1;

namespace RF.Shared.Common.Benchmarks;

[MemoryDiagnoser]
public class ResultBenchmarks
{
    [Benchmark]
    public void ExceptionControlFlow()
    {
        try
        {
            throw new InvalidOperationException();
        }
        catch (Exception) { }
    }

    [Benchmark]
    public void ResultControlFlow()
    {
        var error = new Error("Message", 500, "Message");
        var result = Result.Failure(error);
    }

    [Benchmark]
    public void ResultControlFlowWithExtensions()
    {
        var error = new Error("Message", 500, "Message", new Dictionary<string, string>() { { "Key", "Value" } });
        var result = Result.Failure(error);
    }
}
