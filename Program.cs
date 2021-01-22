namespace SetPerformance {

using System;
using System.Collections.Generic;
using Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

[MemoryDiagnoser]
public class Program {
    [Benchmark]
    public void CoreLinqSet() {
        IEnumerable<int> d = System.Linq.Enumerable.ToArray( System.Linq.Enumerable.Distinct( getData() ) );
    }

    [Benchmark]
    public void OldSet() {
        IEnumerable<int> d = getData().Distinct().ToArray();
    }

    [Benchmark]
    public void NewSet() {
        IEnumerable<int> d = getData().NewDistinct().ToArray();
    }

    private static int[] data = null;
    // private static int[] data = new int[260];
    // private static int[] data = new int[1030];
    // private static int[] data = new int[200_000];
    
    private static int[] getData() {
        if ( data != null ) {
            return data;
        }

        // int size = 200000;
        int size = 1030;
        // int size = 260;
        int reps = 1;
        // int reps = 4;
        int realSize = size / reps;
        data = new int[size];
        Random rng = new Random( 9001 );
        int i = 0;
        for ( ; i < realSize; i++ ) {
            data[i] = rng.Next();
        }

        for ( int rep = 1; rep < reps; rep++ ) {
            for ( ; i < realSize; i++ ) {
                data[i] = data[i + rep * realSize];
            }
        }

        return data;
    }

    private static void Main( string[] args ) {
        // new Program().CoreLinqSet();
        // new Program().NewSet();
        // Random rng = new Random();
        // List<int> ints = new();
        // for ( int i = 0; i < 1030; i++ ) ints.Add( rng.Next() );
        // Console.WriteLine( $"{{ {string.Join( ",\n ", ints )} }}" );
        getData();
        Summary summary = BenchmarkRunner.Run<Program>();
    }
}

}
