using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VideoTools.Experimental.DataStructure;

namespace VideoTools.Experimental.Editor.Data
{
    public class EditorVideoClipEvents : IList<EditorVideoClipEvent>
    {
        private VideoClipEvents clipEvents;

        private readonly List<EditorVideoClipEvent> editorClipEvents;
        
        public string UserData { get; private set; }

        private Texture indicatorTexture;

        private const string TEXTURE_PATH = "Indicator";

        public EditorVideoClipEvents()
        {
            clipEvents = new VideoClipEvents();
            editorClipEvents = new List<EditorVideoClipEvent>();
            LoadTexture();
        }

        public EditorVideoClipEvents(VideoClipEvents events)
        {
            clipEvents = events;
            editorClipEvents = new List<EditorVideoClipEvent>();
            foreach (VideoClipEvent clipEvent in clipEvents)
            {
                EditorVideoClipEvent editorClipEvent = new EditorVideoClipEvent(clipEvent);
                editorClipEvents.Add(editorClipEvent);
            }
            LoadTexture();
        }

        private void LoadTexture()
        {
            indicatorTexture = Resources.Load<Texture>(TEXTURE_PATH);
            foreach (EditorVideoClipEvent editorClipEvent in editorClipEvents)
            {
                editorClipEvent.SetTexture(indicatorTexture);
            }
        }

        /// <summary>
        /// Draws the indicators the represent the position 
        /// of the time related to the event
        /// </summary>
        /// <param name="template">
        /// A rect provided so that the dimensions can be externally controlled
        /// </param>
        public void DrawClipEventIndicators(Rect template)
        {
            foreach (EditorVideoClipEvent editorClipEvent in editorClipEvents)
            {
                editorClipEvent.DrawIndicator(template);
            }
        }

        public void InteractWithIndicators(Vector2 mousePosition, Rect displayRect)
        {
            foreach (EditorVideoClipEvent editorClipEvent in editorClipEvents)
            {
                editorClipEvent.InteractWithRect(mousePosition, ref displayRect);
            }
        }
        

        public VideoClipEvents GetClipEvents()
        {
            clipEvents = new VideoClipEvents(Count);
            foreach (EditorVideoClipEvent editorClipEvent in editorClipEvents)
            {
                clipEvents.Add(editorClipEvent.clipEvent);
            }
            return clipEvents;
        }
        
        public string Assign()
        {
            clipEvents = new VideoClipEvents(Count);
            foreach (EditorVideoClipEvent editorClip in editorClipEvents)
            {
                clipEvents.Add(editorClip.clipEvent);
            }
            UserData = VideoClipEvents.ObjectToJSON(clipEvents);
            return UserData;
        }

        public IEnumerator<EditorVideoClipEvent> GetEnumerator()
        {
            return editorClipEvents.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) editorClipEvents).GetEnumerator();
        }

        public void Add(EditorVideoClipEvent item)
        {
            editorClipEvents.Add(item);
            item.SetTexture(indicatorTexture);
            clipEvents.Add(item.clipEvent);
        }

        public void Add(VideoClipEvent item)
        {
            clipEvents.Add(item);
            EditorVideoClipEvent editorItem = new EditorVideoClipEvent(item);
            editorItem.SetTexture(indicatorTexture);
            editorClipEvents.Add(editorItem);
        }

        public void Clear()
        {
            editorClipEvents.Clear();
        }

        public bool Contains(EditorVideoClipEvent item)
        {
            return editorClipEvents.Contains(item);
        }

        public void CopyTo(EditorVideoClipEvent[] array, int arrayIndex)
        {
            editorClipEvents.CopyTo(array, arrayIndex);
        }

        public bool Remove(EditorVideoClipEvent item)
        {
            return editorClipEvents.Remove(item);
        }

        public int Count
        {
            get { return editorClipEvents.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<EditorVideoClipEvent>) editorClipEvents).IsReadOnly; }
        }

        public int IndexOf(EditorVideoClipEvent item)
        {
            return editorClipEvents.IndexOf(item);
        }

        public void Insert(int index, EditorVideoClipEvent item)
        {
            editorClipEvents.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            editorClipEvents.RemoveAt(index);
        }

        public EditorVideoClipEvent this[int index]
        {
            get { return editorClipEvents[index]; }
            set { editorClipEvents[index] = value; }
        }
    }
}