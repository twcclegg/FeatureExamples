using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FeatureExamples
{
    public interface IEnumeratingList<T> : IList<T>
    {
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class GrowingList<T> : IEnumeratingList<T>
    {
        private readonly List<T> _list = new();

        public IEnumerator<T> GetEnumerator()
            => _list.GetEnumerator();
        public void Add(T item)
            => _list.Add(item);
        public bool Contains(T item)
            => _list.Contains(item);
        public void CopyTo(T[] array, int arrayIndex)
            => _list.CopyTo(array, arrayIndex);
        public int Count => _list.Count;
        public bool IsReadOnly => false;
        public int IndexOf(T item)
            => _list.IndexOf(item);

        public void Insert(int index, T item)
            => throw new NotSupportedException();
        public void RemoveAt(int index)
            => throw new NotSupportedException();
        public bool Remove(T item)
            => throw new NotSupportedException();
        public void Clear()
            => throw new NotSupportedException();

        public T this[int index]
        {
            get => _list[index];
            set => throw new NotSupportedException();
        }

        public override string ToString()
            => string.Join("\n", _list);
    }
}