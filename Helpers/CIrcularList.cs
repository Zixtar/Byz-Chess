using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace Helpers
{
    public class CircularList<T> : List<T>, IEnumerable<T>
    {
        private T[] _items
        {
            get
            {
                var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                var field = typeof(List<T>).GetField("_items", bindingFlags);
                return (T[])field?.GetValue(this);
            }
            set
            {
                var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                var field = typeof(CircularList<T>).GetField("_items", bindingFlags);
                field?.SetValue(this, value);
            }
        }
        private int _version
        {
            get
            {
                var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                var field = typeof(CircularList<T>).GetField("_version", bindingFlags);
                return (int)field?.GetValue(this);
            }
            set
            {
                var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                var field = typeof(CircularList<T>).GetField("_version", bindingFlags);
                field?.SetValue(this, value);
            }
        }
        public new T this[int index]
        {
            get
            {
                return _items[Mod(index, Count)];
            }

            set
            {
                _items[Mod(index,Count)] = value;
                _version++;
            }
        }

        public static int Mod(int x, int m)
        {
            var r = x % m;
            return r < 0 ? r + m : r;
        }

        public new IEnumerator<T> GetEnumerator()
        {
            return new CircularEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CircularEnumerator<T>(this);
        }

        private class CircularEnumerator<T> : IEnumerator<T>
        {
            private readonly List<T> _list;
            private int _index;

            public CircularEnumerator(List<T> list)
            {
                this._list = list;
                _index = 0;
            }

            public T Current => _list[_index];

            object IEnumerator.Current => this;

            public void Dispose()
            {

            }

            public bool MoveNext()
            {
                _index = (_index + 1) % _list.Count;
                return true;
            }

            public void Reset()
            {
                _index = 0;
            }
        }
    }
}
