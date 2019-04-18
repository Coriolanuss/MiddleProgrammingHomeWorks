``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17763.437 (1809/October2018Update/Redstone5)
Intel Core2 Quad CPU Q9550 2.83GHz, 1 CPU, 4 logical and 4 physical cores
  [Host]     : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.3362.0
  DefaultJob : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.3362.0


```
|         Method |     Mean |     Error |    StdDev |
|--------------- |---------:|----------:|----------:|
| SelectUnionAll | 1.239 ms | 0.0179 ms | 0.0167 ms |
|    SelectUnion | 1.120 ms | 0.0192 ms | 0.0180 ms |
