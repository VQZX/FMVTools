using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using UnityEngine;
using Object = System.Object;
using UnityComponent = UnityEngine.Component;
using Param = System.Object;

namespace VideoTools.Experimental.DataStructure.MemberInfoData
{
    [Serializable]
    public class UnityReifiedMethod : ReifiedMethodSerializedParam<UnityComponent, Param>
    {
        [SerializeField]
        protected MethodInfo methodInfo;
        public override MethodInfo MethodInfo
        {
            get { return methodInfo; }
            protected set { methodInfo = value; }
        }

        [SerializeField]
        protected UnityComponent callingInstance;
        public override UnityComponent CallingInstance
        {
            get { return callingInstance; }
        }

        [SerializeField]
        protected Param methodParam;
        public override Param MethodParam
        {
            get { return methodParam; }
        }

        private string methodName;

        public UnityReifiedMethod(MethodInfo info, UnityComponent instance, Object param) : base(info, instance, param)
        {
            methodInfo = info;
            callingInstance = instance;
            methodParam = param;
            methodName = info.Name;
        }

        [SuppressMessage("ReSharper", "IsExpressionAlwaysTrue")]
        public override void Invoke()
        {
            methodInfo.Invoke(
                callingInstance, 
                methodParam == null ? new object[] { } : new[] {methodParam});  
        }
    }
}