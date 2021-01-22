namespace SetPerformance {

using System;
using System.Collections.Generic;
using Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

public class Program {
    [Benchmark]
    public void CoreLinqSet() {
        IEnumerable<int> d = System.Linq.Enumerable.ToArray( System.Linq.Enumerable.Distinct( Data.INTS ) );
    }

    [Benchmark]
    public void OldSet() {
        IEnumerable<int> d = Data.INTS.Distinct().ToArray();
    }

    [Benchmark]
    public void NewSet() {
        IEnumerable<int> d = Data.INTS.NewDistinct().ToArray();
    }

    private static void Main( string[] args ) {
        // new Program().CoreLinqSet();
        // new Program().NewSet();
        // Random rng = new Random();
        // List<int> ints = new();
        // for ( int i = 0; i < 1030; i++ ) ints.Add( rng.Next() );
        // Console.WriteLine( $"{{ {string.Join( ",\n ", ints )} }}" );
        Summary summary = BenchmarkRunner.Run<Program>();
    }
}

}
