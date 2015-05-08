namespace AngleSharp.Scripting.CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;

    public class DynamicNode : DynamicObject
    {
        readonly Object _node;

        public DynamicNode(Object node)
        {
            _node = node;
        }

        public override IEnumerable<String> GetDynamicMemberNames()
        {
            return base.GetDynamicMemberNames();
        }

        public override Boolean TryGetMember(GetMemberBinder binder, out Object result)
        {
            return base.TryGetMember(binder, out result);
        }

        public override Boolean TrySetMember(SetMemberBinder binder, Object value)
        {
            return base.TrySetMember(binder, value);
        }

        public override Boolean TryGetIndex(GetIndexBinder binder, Object[] indexes, out Object result)
        {
            return base.TryGetIndex(binder, indexes, out result);
        }

        public override Boolean TrySetIndex(SetIndexBinder binder, Object[] indexes, Object value)
        {
            return base.TrySetIndex(binder, indexes, value);
        }

        public override Boolean TryInvokeMember(InvokeMemberBinder binder, Object[] args, out Object result)
        {
            return base.TryInvokeMember(binder, args, out result);
        }
    }
}
