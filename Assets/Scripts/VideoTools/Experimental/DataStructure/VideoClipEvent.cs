using System;
using System.Runtime.Serialization;
using Flusk.Serialization;
using Object = UnityEngine.Object;

namespace VideoTools.Experimental.DataStructure
{
    [DataContract, Serializable]
    public struct VideoClipEvent
    {
        [DataMember]
        public double Time;

        [DataMember]
        public string MethodName;

        [DataMember]
        public float FloatParam;

        [DataMember]
        public int IntParam;

        [DataMember]
        public string StringParam;

        [DataMember]
        public object ObjectParam;

        public Object UnityObjectParam
        {
            get
            {
                if (ObjectParam is Object)
                {
                    return (Object) ObjectParam;
                }
                return null;
            }
            set
            {
                if (value is Object)
                {
                    ObjectParam = value;
                }
            }
        }

        public override string ToString()
        {
            string output = MethodName;
            output = string.Format("{0}\n{1}", output, Time);
            output = string.Format("{0}\n{1}", output, FloatParam);
            output = string.Format("{0}\n{1}", output, IntParam);
            output = string.Format("{0}\n{1}", output, StringParam);
            output = string.Format("{0}\n{1}", output, ObjectParam);
            return output;
        }

        public object GetAppropriateValue(Type type)
        {
            if (type == typeof(float))
            {
                return FloatParam;
            }
            else if ( type == typeof(int))
            {
                return IntParam;
            }
            else if ( type == typeof(string))
            {
                return StringParam;
            }
            else
            {
                return ObjectParam;
            }
        }
        
        // ReSharper disable once InconsistentNaming
        public static string GetXMLFromVideoClip ( VideoClipEvent clipEvent)
        {
            return XMLHelpers.GetXMLFromObject(clipEvent);
        }
        
        public static VideoClipEvent ClipEventFromXml(string xml)
        {
            return XMLHelpers.XMLToObject<VideoClipEvent>(xml);
        }
    }
}