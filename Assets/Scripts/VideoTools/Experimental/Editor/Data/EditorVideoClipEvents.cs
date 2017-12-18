using System.Collections;
using System.Collections.Generic;
using VideoTools.Experimental.DataStructure;

namespace VideoTools.Experimental.Editor.Data
{
    public class EditorVideoClipEvents : IList<EditorVideoClipEvent>
    {
        private VideoClipEvents clipEvents;

        private List<EditorVideoClipEvent> editorClipEvents;
        
        public string UserData { get; private set; }

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