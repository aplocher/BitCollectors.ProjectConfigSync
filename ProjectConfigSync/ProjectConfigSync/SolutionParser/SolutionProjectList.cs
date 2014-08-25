using System.Collections.Generic;
using System.Linq;

namespace ProjectConfigSync.SolutionParser
{
    public class SolutionProjectList : IList<SolutionProject>
    {
        private readonly List<SolutionProject> _rootList = new List<SolutionProject>();

        public int IndexOf(SolutionProject item)
        {
            return _rootList.IndexOf(item);
        }

        public void Insert(int index, SolutionProject item)
        {
            int originalCount = _rootList.Count;
            _rootList.Insert(index, item);

            if (originalCount != _rootList.Count)
            {
                _isDirty = true;
            }
        }

        public void RemoveAt(int index)
        {
            int originalCount = _rootList.Count;
            _rootList.RemoveAt(index);

            if (originalCount != _rootList.Count)
            {
                _isDirty = true;
            }
        }

        public SolutionProject this[int index]
        {
            get { return _rootList[index]; }
            set { _rootList[index] = value; }
        }

        public void Add(SolutionProject item)
        {
            _rootList.Add(item);

            _isDirty = true;
        }

        public void Clear()
        {
            if (_rootList.Any())
            {
                _isDirty = true;
            }

            _rootList.Clear();
        }

        public bool Contains(SolutionProject item)
        {
            return _rootList.Contains(item);
        }

        public void CopyTo(SolutionProject[] array, int arrayIndex)
        {
            _rootList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _rootList.Count; }
        }

        public bool IsReadOnly
        {
            get { return _isDirty; }
        }

        public bool Remove(SolutionProject item)
        {
            if (_rootList.Contains(item))
                _isDirty = true;

            return _rootList.Remove(item);
        }

        public IEnumerator<SolutionProject> GetEnumerator()
        {
            return _rootList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _rootList.GetEnumerator();
        }

        private bool _isDirty = false;
    }
}
