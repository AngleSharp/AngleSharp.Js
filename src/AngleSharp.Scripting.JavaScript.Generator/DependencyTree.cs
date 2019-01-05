namespace AngleSharp.Scripting.JavaScript.Generator
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class DependencyTree<T> : IEnumerable<T>
        where T : IEquatable<T>
    {
        readonly List<DependencyNode> _branches;

        public DependencyTree()
        {
            _branches = new List<DependencyNode>();
        }

        public void Include(T dependency, T controller)
        {
            var parentNode = GetOrAdd(controller);
            var childNode = GetOrAdd(dependency);
            _branches.Remove(childNode);
            childNode.Parent = parentNode;
            parentNode.Children.Add(childNode);
        }

        public IEnumerable<T> Controllers()
        {
            return _branches.Select(m => m.Dependency);
        }

        public IEnumerable<T> Dependencies(T dependency)
        {
            var result = default(DependencyNode);

            foreach (var branch in _branches)
            {
                result = branch.Get(dependency);

                if (result != null)
                    return result.GetAll();
            }

            return Enumerable.Empty<T>();
        }

        DependencyNode GetOrAdd(T dependency)
        {
            var result = default(DependencyNode);

            foreach (var branch in _branches)
            {
                result = branch.Get(dependency);

                if (result != null)
                    return result;
            }

            result = new DependencyNode
            {
                Dependency = dependency
            };
            _branches.Add(result);
            return result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Controllers().SelectMany(Dependencies).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        sealed class DependencyNode
        {
            public T Dependency;
            public DependencyNode Parent = null;
            public List<DependencyNode> Children = new List<DependencyNode>();

            public DependencyNode Get(T dependency)
            {
                if (!Dependency.Equals(dependency))
                {
                    var result = default(DependencyNode);

                    foreach (var child in Children)
                    {
                        result = child.Get(dependency);

                        if (result != null)
                            break;
                    }

                    return result;
                }

                return this;
            }

            public IEnumerable<T> GetAll()
            {
                foreach (var child in Children)
                {
                    yield return child.Dependency;

                    foreach (var dependency in child.GetAll())
                        yield return dependency;
                }
            }
        }
    }
}
