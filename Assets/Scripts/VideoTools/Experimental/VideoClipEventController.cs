#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Video;

namespace VideoTools.Experimental
{    
    [DisallowMultipleComponent]
    public class VideoClipEventController : MonoBehaviour
    {       
        [SerializeField, HideInInspector]
        protected VideoClipBakedMethods videoClipBakedMethods;

        [SerializeField, HideInInspector]
        protected VideoClip clip;

        [SerializeField, HideInInspector]
        protected string data;
        
        private VideoPlayer player;
        private bool isDirty;
        
#if UNITY_EDITOR
        public bool IsDirty
        {
            get { return isDirty; }
            set
            {
                isDirty = value;
                if (value)
                {
                    Init();
                }
            }
        }
#endif

#if UNITY_EDITOR
        public void Init()
        {
            if (player == null)
            {
                player = GetComponent<VideoPlayer>();
            }
            clip = player.clip;
            if (clip == null)
            {
                Debug.LogErrorFormat("[VideoClipEventController ({0})] No video clip assigned", gameObject.name);
                return;
            }
            
            videoClipBakedMethods = new VideoClipBakedMethods(clip);
            data = videoClipBakedMethods.ClipEventData;
            videoClipBakedMethods.AssignEvents(GetComponentsInChildren<Component>());   
            EditorUtility.SetDirty(this);
        }
#endif        
        
        protected virtual void Start()
        {
            Debug.Log(videoClipBakedMethods.ClipEventData);
            videoClipBakedMethods = new VideoClipBakedMethods(clip, data);
            videoClipBakedMethods.GetVideoClipEvents();
            videoClipBakedMethods.AssignEvents(GetComponentsInChildren<Component>());
            videoClipBakedMethods.TestMethods();
            Debug.Log(videoClipBakedMethods.ClipEventData);
        }
    }
}