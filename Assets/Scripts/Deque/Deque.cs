using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deque<T> : IEnumerable<T>
    {
        public int size;
        private LinkedList<T> _buffer;
        
        public IEnumerator<T> GetEnumerator()
        {
            return _buffer.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _buffer.GetEnumerator();
        }

        public void Remove (T value) 
        {
            T to_remove = default(T);
            foreach (T val in _buffer) {
                if (val.Equals(value)) {
                    to_remove = val;
                }
            }
            
            _buffer.Remove(to_remove);
            size--;
            
        }

        
        public void AddFirst(T item)
        {
            _buffer.AddFirst(item);
            size++;
        }

        public void AddLast(T item)
        {
            _buffer.AddLast(item);
            size++;
        }
        
        public T PeekFirst()
        {
            if (_buffer.First != null)
            {
                T result = _buffer.First.Value;
                return result;
            }
            throw new InvalidOperationException("Deque empty");
        }

        public T PeekLast()
        {
            if (_buffer.Last != null)
            {
                T result = _buffer.Last.Value;
                return result;
            }
            throw new InvalidOperationException("Deque empty");
        }

        public bool TryPeekFirst(out T result)
        {
            if (_buffer.First != null)
            {
                result = _buffer.First.Value;
                return true;
            }
            result = default;
            return false;
        }
        
        public bool TryPeekLast(out T result)
        {
            if (_buffer.Last != null)
            {
                result = _buffer.Last.Value;
                return true;
            }
            result = default;
            return false;
        }
        
        public T DequeueFirst()
        {
            T result = PeekFirst();
            _buffer.RemoveFirst();
            size--;
            return result;
        }
        
        public T DequeueLast()
        {
            T result = PeekLast();
            _buffer.RemoveLast();
            size--;
            return result;
        }

        public bool TryDequeueFirst(out T result)
        {
            var canDeque = TryPeekFirst(out result);
            if (canDeque)
            {
                _buffer.RemoveFirst();
                size--;
            }
            return canDeque;
        }
        
        public bool TryDequeueLast(out T result)
        {
            var canDeque = TryPeekLast(out result);
            if (canDeque)
            {
                _buffer.RemoveLast();
                size--;
            }
            return canDeque;
        }

        public void Clear()
        {
            _buffer.Clear();
            size = 0;
        }

        public bool Contains(T item)
        {
            return _buffer.Contains(item);
        }
        public Deque()
        {
            size = 0;
            _buffer = new LinkedList<T>();
        }
    }
