using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses
{
    public class CircularList<T> : List<T>, IEnumerable<T>
    {
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
