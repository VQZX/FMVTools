using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using UnityEngine;
using Object = System.Object;
using UnityComponent = UnityEngine.MonoBehaviour;

namespace VideoTools.Experimental.DataStructure.MemberInfoData
{
    [Serializable]
    public class UnityReifiedMethod : ReifiedMethodSerializedParam<UnityComponent, Object>
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
        protected Object methodParam;
        public override Object MethodParam
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
            if (callingInstance is MonoBehaviour)
            {
                callingInstance.Invoke(methodName, 0);
            }
            else if ( callingInstance is Component)
            {
                methodInfo.Invoke(
                    callingInstance, 
                    methodParam == null ? new object[] { } : new object[] {methodParam});
            }  
        }
    }
}