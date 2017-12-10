using System;
using System.Reflection;
using UnityEngine;

namespace VideoTools.Experimental.DataStructure.MemberInfoData
{
    public class ReifiedMethodParam<TInstance, TParam> : ReifiedMethod<TInstance> 
        where TInstance : class  
    {        
        /// <summary>
        /// The paramter that the method takes
        /// </summary>
        public TParam MethodParam { get; protected set; }

        public ReifiedMethodParam(MethodInfo info, TInstance instance, TParam param) : base(info, instance)
        {
            MethodParam = param;
        }

        public override void Invoke()
        {
            Invoke(MethodParam);
        }

        public virtual void Invoke(TParam param)
        {
            object [] methodParams = new object[] {param};
            MethodInfo.Invoke(CallingInstance, methodParams);
        }
    }
}