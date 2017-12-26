using System;
using System.Reflection;
using UnityEngine;

namespace VideoTools.Experimental.DataStructure.MemberInfoData
{
    [Serializable]
    public class ReifiedMethodParam<TInstance, TParam> : ReifiedMethod<TInstance> 
        where TInstance : class  
    {        
        /// <summary>
        /// The paramter that the method takes
        /// </summary>
        public virtual TParam MethodParam { get; protected set; }

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
            if (param == null)
            {
                base.Invoke();
                return;
            }
            object [] methodParams = {param};
            MethodInfo.Invoke(CallingInstance, methodParams);
        }
    }
}