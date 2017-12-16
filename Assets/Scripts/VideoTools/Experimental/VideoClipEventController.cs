using UnityEngine;
using UnityEngine.Video;

namespace VideoTools.Experimental
{    
    [DisallowMultipleComponent]
    public class VideoClipEventController : MonoBehaviour
    {       
        [SerializeField, HideInInspector]
        protected VideoClipBakedMethods videoClipBakedMethods;
        
        private VideoPlayer player;
        private VideoClip clip;

        private bool isDirty;
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
            videoClipBakedMethods.AssignEvents(GetComponentsInChildren<Component>()); 
        }
#endif
        
        protected virtual void Awake()
        {
            videoClipBakedMethods.TestMethods();
        }
    }
}