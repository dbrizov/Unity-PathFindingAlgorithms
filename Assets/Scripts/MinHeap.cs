using System;
using System.Collections.Generic;

namespace PathFinding
{
    public class MinHeap<T>
    {
        private const int InitialCapacity = 4;

        private T[] _arr;
        private int _lastItemIndex;
        private IComparer<T> _comparer;

        public MinHeap()
            : this(InitialCapacity, Comparer<T>.Default)
        {
        }

        public MinHeap(int capacity)
            : this(capacity, Comparer<T>.Default)
        {
        }

        public MinHeap(Comparison<T> comparison)
            : this(InitialCapacity, Comparer<T>.Create(comparison))
        {
        }

        public MinHeap(IComparer<T> comparer)
            : this(InitialCapacity, comparer)
        {
        }

        public MinHeap(int capacity, IComparer<T> comparer)
        {
            _arr = new T[capacity];
            _lastItemIndex = -1;
            _comparer = comparer;
        }

        public int Count
        {
            get
            {
                return _lastItemIndex + 1;
            }
        }

        public void Add(T item)
        {
            if (_lastItemIndex == _arr.Length - 1)
            {
                Resize();
            }

            _lastItemIndex++;
            _arr[_lastItemIndex] = item;

            MinHeapifyUp(_lastItemIndex);
        }

        public T Remove()
        {
            if (_lastItemIndex == -1)
            {
                throw new InvalidOperationException("The heap is empty");
            }

            T removedItem = _arr[0];
            _arr[0] = _arr[_lastItemIndex];
            _lastItemIndex--;

            MinHeapifyDown(0);

            return removedItem;
        }

        public T Peek()
        {
            if (_lastItemIndex == -1)
            {
                throw new InvalidOperationException("The heap is empty");
            }

            return _arr[0];
        }

        public void Clear()
        {
            _lastItemIndex = -1;
        }

        private void MinHeapifyUp(int index)
        {
            if (index == 0)
            {
                return;
            }

            int childIndex = index;
            int parentIndex = (index - 1) / 2;

            if (_comparer.Compare(_arr[childIndex], _arr[parentIndex]) < 0)
            {
                // swap the parent and the child
                T temp = _arr[childIndex];
                _arr[childIndex] = _arr[parentIndex];
                _arr[parentIndex] = temp;

                MinHeapifyUp(parentIndex);
            }
        }

        private void MinHeapifyDown(int index)
        {
            int leftChildIndex = index * 2 + 1;
            int rightChildIndex = index * 2 + 2;
            int smallestItemIndex = index; // The index of the parent

            if (leftChildIndex <= _lastItemIndex &&
                _comparer.Compare(_arr[leftChildIndex], _arr[smallestItemIndex]) < 0)
            {
                smallestItemIndex = leftChildIndex;
            }

            if (rightChildIndex <= _lastItemIndex &&
                _comparer.Compare(_arr[rightChildIndex], _arr[smallestItemIndex]) < 0)
            {
                smallestItemIndex = rightChildIndex;
            }

            if (smallestItemIndex != index)
            {
                // swap the parent with the smallest of the child items
                T temp = _arr[index];
                _arr[index] = _arr[smallestItemIndex];
                _arr[smallestItemIndex] = temp;

                MinHeapifyDown(smallestItemIndex);
            }
        }

        private void Resize()
        {
            T[] newArr = new T[_arr.Length * 2];
            for (int i = 0; i < _arr.Length; i++)
            {
                newArr[i] = _arr[i];
            }

            _arr = newArr;
        }
    }
}
