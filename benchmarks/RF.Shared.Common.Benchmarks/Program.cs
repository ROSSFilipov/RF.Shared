using BenchmarkDotNet.Running;
using RF.Shared.Common.Benchmarks;

var benchmark = BenchmarkRunner.Run<ResultBenchmarks>();