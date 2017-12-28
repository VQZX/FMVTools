#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Video;

namespace VideoTools.Experimental
{    
    [RequireComponent(typeof(VideoPlayer)), DisallowMultipleComponent]
    public class VideoClipEventController : MonoBehaviour
    {       
        /// <summary>
        /// Controls the actual importing of events
        /// </summary>
        [SerializeField, HideInInspector]
        protected VideoClipBakedMethods videoClipBakedMethods;

        /// <summary>
        /// The video clip to gather data from
        /// </summary>
        [SerializeField, HideInInspector]
        protected VideoClip clip;

        /// <summary>
        /// The user data baked into the video clip
        /// </summary>
        [SerializeField, HideInInspector]
        protected string data;
        
        /// <summary>
        /// The video player associated with the video clip
        /// </summary>
        [SerializeField, HideInInspector]
        private VideoPlayer player;

        /// <summary>
        /// Used through reflection by UIWriter
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        protected string time;
        
        /// <summary>
        /// Tracks if anything has changed
        /// </summary>
        private bool isDirty;
        
        private bool IsPlaying
        {
            get
            {
                return player != null && player.isPlaying;
            }
        }
        
#if UNITY_EDITOR
        /// <summary>
        /// Changes to this property activate the initialisation
        /// if necessary
        /// </summary>
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
        /// <summary>
        /// On run, this reassigns all the necessary events based on the data from the clip
        /// </summary>
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

        /// <summary>
        /// Tracks the video player time and invokes events in necessary
        /// </summary>
        protected virtual void Update()
        {
            if (!IsPlaying)
            {
                return;
            }
            time = player.time.ToTime();
            videoClipBakedMethods.InvokeMethodsByTime(player.time);
        }
        
        
#if UNITY_EDITOR
        /// <summary>
        /// Gets all the user data from the video clip and caches the video player
        /// </summary>
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

        /// <summary>
        /// Initialises if possible
        /// </summary>
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