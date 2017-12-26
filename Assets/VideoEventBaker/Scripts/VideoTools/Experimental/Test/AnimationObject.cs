using UnityEngine;

namespace VideoTools.Experimental.Test
{
    public class AnimationObject : MonoBehaviour
    {
        [SerializeField]
        protected GameObject prefabInt;

        [SerializeField]
        protected GameObject prefabString;
        
        public void MethodNoParam ()
        {
            Debug.Log("MethodNoParam");
        }

        public void MethodOneParamInt(int a)
        {
            Debug.LogFormat("MethodOneParam {0}", a);
            Instantiate(prefabInt, Vector3.right, Quaternion.identity);
        }

        public void MethodOneParamString(string b)
        {
            Debug.LogFormat("MethodOneParam {0}", b);
            Instantiate(prefabString, Vector3.left, Quaternion.identity);
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