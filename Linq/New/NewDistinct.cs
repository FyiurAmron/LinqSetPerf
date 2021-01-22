// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace SetPerformance.Linq
{

using System.Collections.Generic;
using System.Diagnostics;

public static partial class Enumerable
    {
        public static IEnumerable<TSource> NewDistinct<TSource>(this IEnumerable<TSource> source) => Enumerable.NewDistinct(source, null);

        public static IEnumerable<TSource> NewDistinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource>? comparer)
        {
            if (source == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.source);
            }

            return new NewDistinctIterator<TSource>(source, comparer);
        }

        /// <summary>
        /// An iterator that yields the distinct values in an <see cref="IEnumerable{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source enumerable.</typeparam>
        private sealed partial class NewDistinctIterator<TSource> : Enumerable.Iterator<TSource>
        {
            private readonly IEnumerable<TSource> _source;
            private readonly IEqualityComparer<TSource>? _comparer;
            private NewSet<TSource>? _set;
            private IEnumerator<TSource>? _enumerator;

            public NewDistinctIterator(IEnumerable<TSource> source, IEqualityComparer<TSource>? comparer)
            {
                Debug.Assert(source != null);
                _source = source;
                _comparer = comparer;
            }

            public override Enumerable.Iterator<TSource> Clone() => new Enumerable.NewDistinctIterator<TSource>(_source, _comparer);

            public override bool MoveNext()
            {
                switch (_state)
                {
                    case 1:
                        _enumerator = _source.GetEnumerator();
                        if (!_enumerator.MoveNext())
                        {
                            Dispose();
                            return false;
                        }

                        TSource element = _enumerator.Current;
                        _set = new NewSet<TSource>(_comparer);
                        _set.Add(element);
                        _current = element;
                        _state = 2;
                        return true;
                    case 2:
                        Debug.Assert(_enumerator != null);
                        Debug.Assert(_set != null);
                        while (_enumerator.MoveNext())
                        {
                            element = _enumerator.Current;
                            if (_set.Add(element))
                            {
                                _current = element;
                                return true;
                            }
                        }

                        break;
                }

                Dispose();
                return false;
            }

            public override void Dispose()
            {
                if (_enumerator != null)
                {
                    _enumerator.Dispose();
                    _enumerator = null;
                    _set = null;
                }

                base.Dispose();
            }
        }
    }
}
