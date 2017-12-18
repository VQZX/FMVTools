using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Flusk.Serialization;
using UnityEngine;

namespace VideoTools.Experimental.DataStructure
{
    [DataContract, Serializable]
    public class VideoClipEvents : IList<VideoClipEvent>
    {
        [DataMember]
        public List<VideoClipEvent> videoClips = new List<VideoClipEvent>();

        public static string GetXml(VideoClipEvents events)
        {
            return XMLHelpers.GetXMLFromObject(events);
        }

        public static VideoClipEvents DeserializeFromXml(string xml)
        {
            return XMLHelpers.XMLToObject<VideoClipEvents>(xml);
        }

        public static string ObjectToJSON(VideoClipEvents events)
        {
            return JsonUtility.ToJson(events);
        }

        public VideoClipEvents JSONToObject(string json)
        {
            return JsonUtility.FromJson<VideoClipEvents>(json);
        }
       
        public override string ToString()
        {
            string output = base.ToString();
            foreach (VideoClipEvent videoClipEvent in videoClips)
            {
                output = string.Format("{0}\n{1}", output, videoClipEvent.ToString());
            }
            return output;
        }

        public VideoClipEvents(int count)
        {
            videoClips = new List<VideoClipEvent>(count);
        }

        public VideoClipEvents(VideoClipEvents clipEvents)
        {
            int count = clipEvents.Count;
            videoClips = new List<VideoClipEvent>(count);
            foreach (VideoClipEvent clipEvent in clipEvents)
            {
                videoClips.Add(clipEvent);
            }
        }

        public VideoClipEvents()
        {
            videoClips = new List<VideoClipEvent>();
        }
        
        #region IList implementation
        public IEnumerator<VideoClipEvent> GetEnumerator()
        {
            return videoClips.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) videoClips).GetEnumerator();
        }

        public void Add(VideoClipEvent item)
        {
            videoClips.Add(item);
        }

        public void Add(params VideoClipEvent[] items)
        {
            int count = items.Length;
            for (int i = 0; i < count; i++)
            {
                Add(items[i]);
            }
        }
        

        public void Clear()
        {
            videoClips.Clear();
        }

        public bool Contains(VideoClipEvent item)
        {
            return videoClips.Contains(item);
        }

        public void CopyTo(VideoClipEvent[] array, int arrayIndex)
        {
            videoClips.CopyTo(array, arrayIndex);
        }

        public bool Remove(VideoClipEvent item)
        {
            return videoClips.Remove(item);
        }

        public int Count
        {
            get { return videoClips.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<VideoClipEvent>) videoClips).IsReadOnly; }
        }

        public int IndexOf(VideoClipEvent item)
        {
            return videoClips.IndexOf(item);
        }

        public void Insert(int index, VideoClipEvent item)
        {
            videoClips.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            videoClips.RemoveAt(index);
        }

        public VideoClipEvent this[int index]
        {
            get { return videoClips[index]; }
            set { videoClips[index] = value; }
        }
        #endregion
    }
}