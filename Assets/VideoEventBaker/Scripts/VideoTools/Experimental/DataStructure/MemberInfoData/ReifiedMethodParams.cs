using System;
using System.Reflection;

namespace VideoTools.Experimental.DataStructure.MemberInfoData
{
    public class ReifiedMethodParams<T> : ReifiedMethod<T> 
        where T : class 
    {
        public object [] MethodParams { get; protected set; }
        
        public ReifiedMethodParams(MethodInfo methodInfo, T instance) : base(methodInfo, instance)
        {
        }

        public override void Invoke()
        {
            MethodInfo.Invoke(CallingInstance, MethodParams);
        }
    }
}