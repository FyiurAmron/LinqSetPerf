// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace SetPerformance.Linq
{

using System.Collections.Generic;
using System.Drawing;

public static partial class Enumerable
    {
        private sealed partial class NewDistinctIterator<TSource> : IIListProvider<TSource>
        {
            private NewSet<TSource> FillSet() {
                NewSet<TSource> set;
                if ( _source is ICollection<TSource> src ) {
                    set = new NewSet<TSource>( _comparer, src.Count );
                } else {
                    set = new NewSet<TSource>( _comparer );
                }

                set.UnionWith(_source);
                return set;
            }

            public TSource[] ToArray() => FillSet().ToArray();

            public List<TSource> ToList() => FillSet().ToList();

            public int GetCount(bool onlyIfCheap) => onlyIfCheap ? -1 : FillSet().Count;
        }
    }
}
