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
        
        [SerializeField, HideInInspector]
        private VideoPlayer player;

        /// <summary>
        /// Used through reflection by UIWriter
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        protected string time;
        
        private bool isDirty;

        private bool IsPlaying
        {
            get
            {
                return player != null && player.isPlaying;
            }
        }
        
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
        protected virtual void Awake()
        {
            if (player == null)
            {
                player = GetComponent<VideoPlayer>();
            }
            videoClipBakedMethods = new VideoClipBakedMethods(clip, data);
            videoClipBakedMethods.GetVideoClipEvents();
            videoClipBakedMethods.AssignEvents(GetComponentsInChildren<Component>());
            time = player.time.ToTime();
            Debug.Log(videoClipBakedMethods.MethodData());
        }

        protected virtual void Update()
        {
            if (IsPlaying)
            {
                time = player.time.ToTime();
                videoClipBakedMethods.InvokeMethodsByTime(player.time);
            }
        }
        
        
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
                return;
            }
            
            videoClipBakedMethods = new VideoClipBakedMethods(clip);
            data = videoClipBakedMethods.ClipEventData;
            videoClipBakedMethods.AssignEvents(GetComponentsInChildren<Component>());   
            EditorUtility.SetDirty(this);
        }

        private void OnValidate()
        {
            if (clip == null || videoClipBakedMethods == null || string.IsNullOrEmpty(data))
            {
                Init();
            }
        }
#endif    
    }
}