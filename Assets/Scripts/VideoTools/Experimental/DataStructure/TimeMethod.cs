using System;
using UnityEngine;
using VideoTools.Experimental.DataStructure.MemberInfoData;

namespace VideoTools.Experimental.DataStructure
{
    [Serializable]
    public class TimeMethod
    {
        [SerializeField]
        public double Time { get; protected set; }
        
        [SerializeField]
        public UnityReifiedMethod MethodInfo { get; protected set; }

        public TimeMethod(double time, UnityReifiedMethod method)
        {
            Time = time;
            MethodInfo = method;
        }
    }
}