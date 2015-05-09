namespace AngleSharp.Scripting.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Reflection;
    using AngleSharp.Attributes;

    sealed class DynamicDomObject : DynamicObject
    {
        readonly Object _node;
        readonly Dictionary<String, MemberInfo> _members;

        public DynamicDomObject(Object node)
        {
            _node = node;
            _members = new Dictionary<String, MemberInfo>();

            foreach (var method in node.GetType().GetRuntimeMethods())
            {
                var attr = method.GetCustomAttribute<DomNameAttribute>();

                if (attr == null)
                    break;

                _members.Add(attr.OfficialName, method);
            }

            foreach (var property in node.GetType().GetRuntimeProperties())
            {
                var attr = property.GetCustomAttribute<DomNameAttribute>();

                if (attr == null)
                    break;

                _members.Add(attr.OfficialName, property);
            }
        }

        public override IEnumerable<String> GetDynamicMemberNames()
        {
            return _members.Keys;
        }

        public override Boolean TryGetMember(GetMemberBinder binder, out Object result)
        {
            var info = default(MemberInfo);
            result = null;

            if (_members.TryGetValue(binder.Name, out info) && info is PropertyInfo)
            {
                var property = (PropertyInfo)info;

                if (property.CanRead)
                    result = property.GetValue(_node);

                return true;
            }

            return false;
        }

        public override Boolean TrySetMember(SetMemberBinder binder, Object value)
        {
            var info = default(MemberInfo);

            if (_members.TryGetValue(binder.Name, out info) && info is PropertyInfo)
            {
                var property = (PropertyInfo)info;

                if (property.CanWrite)
                    property.SetValue(_node, value);

                return true;
            }

            return false;
        }

        public override Boolean TryGetIndex(GetIndexBinder binder, Object[] indexes, out Object result)
        {
            //TODO
            return base.TryGetIndex(binder, indexes, out result);
        }

        public override Boolean TrySetIndex(SetIndexBinder binder, Object[] indexes, Object value)
        {
            //TODO
            return base.TrySetIndex(binder, indexes, value);
        }

        public override Boolean TryInvokeMember(InvokeMemberBinder binder, Object[] args, out Object result)
        {
            var info = default(MemberInfo);
            result = null;

            if (_members.TryGetValue(binder.Name, out info) && info is MethodInfo)
            {
                var method = (MethodInfo)info;
                result = method.Invoke(_node, args);
                return true;
            }

            return false;
        }
    }
}
