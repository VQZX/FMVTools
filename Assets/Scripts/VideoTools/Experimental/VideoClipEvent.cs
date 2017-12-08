using System;

namespace VideoTools.Experimental
{
    [Serializable]
    public struct VideoClipEvent
    {
        public float Time;

        public string MetodName;

        public float FloatParam;

        public int IntParam;

        public string StringParam;

        public object ObjectParam;
    }
}