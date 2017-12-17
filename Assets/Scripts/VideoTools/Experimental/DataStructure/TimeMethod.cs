using System;
using UnityEngine;
using VideoTools.Experimental.DataStructure.MemberInfoData;

namespace VideoTools.Experimental.DataStructure
{
    [Serializable]
    public class TimeMethod
    {
        [SerializeField]
        protected double time;
        public double Time {get { return time; }}

        [SerializeField]
        protected UnityReifiedMethod methodInfo;
        public UnityReifiedMethod MethodInfo
        {
            get { return methodInfo; }
        }

        public bool HasInvoked { get; set; }

        public TimeMethod(double time, UnityReifiedMethod method)
        {
            this.time = time;
            methodInfo = method;
        }

        public void Reset()
        {
            HasInvoked = false;
        }

        public void TryInvoke(double time)
        {
            if ( time <= Time || HasInvoked)
            {
                return;
            }
            Invoke();
            HasInvoked = true;
        }

        public void Invoke()
        {
            MethodInfo.Invoke();
        }
    }
}