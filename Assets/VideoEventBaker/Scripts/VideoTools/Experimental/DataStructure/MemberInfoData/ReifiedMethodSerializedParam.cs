using System;
   using System.Reflection;
   using System.Runtime.Serialization;
   
   namespace VideoTools.Experimental.DataStructure.MemberInfoData
   {
       [Serializable]
       public class ReifiedMethodSerializedParam<TInstance, TParam> : ReifiedMethodParam<TInstance, TParam> 
           where TInstance : class
           where TParam : class
       {
           public ReifiedMethodSerializedParam(MethodInfo info, TInstance instance, TParam param) : base(info, instance, param)
           {
           }
       }
   }