using UnityEngine;

namespace VideoTools.Experimental
{
    public class AnimationObject : MonoBehaviour
    {
        public void MethodNoParam ()
        {
            Debug.Log("MethodNoParam");
        }

        public void MethodOneParamInt(int a)
        {
            Debug.LogFormat("MethodOneParam {0}", a);
        }

        public void MethodOneParaString(string b)
        {
            Debug.LogFormat("MethodOneParam {0}", b);
        }

        public void MethodTwoParam(string a, int c)
        {
            Debug.LogFormat("MethodTwoParam {0} {1}",a, c );
        }
        

        public void MethodParamObject(object c)
        {
            Debug.LogFormat("MethodParamObject {0}", c.GetType());
        }
        
        public void MethodParamUnityObject(Object c)
        {
            Debug.LogFormat("MethodParamObject {0}", c.GetType());
        }

    }
}