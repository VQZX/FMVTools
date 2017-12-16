using System.Reflection;

namespace VideoTools.Experimental.DataStructure.MemberInfoData
{
    public class ReifiedMethod<T> where T : class
    {
        /// <summary>
        /// The specific method to call
        /// </summary>
        public virtual MethodInfo MethodInfo { get; protected set; }

        /// <summary>
        /// The instance of the object the method resides on
        /// </summary>
        public virtual T CallingInstance { get; protected set; }

        public ReifiedMethod(MethodInfo methodInfo, T instance)
        {
            MethodInfo = methodInfo;
            CallingInstance = instance;
        }

        public virtual void Invoke()
        {
            MethodInfo.Invoke(CallingInstance, new object[]{});
        }
    }
}