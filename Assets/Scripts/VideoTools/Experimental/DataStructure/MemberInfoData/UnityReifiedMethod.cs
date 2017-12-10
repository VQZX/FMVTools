using System;
using System.Reflection;
using UnityEngine;

namespace VideoTools.Experimental.DataStructure.MemberInfoData
{
    [Serializable]
    public class UnityReifiedMethod : ReifiedMethodParam<Component, object>
    {
        public UnityReifiedMethod(MethodInfo info, Component instance, object param) : base(info, instance, param)
        {
        }
    }
}