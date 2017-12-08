using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

namespace VideoTools.Experimental
{
    [DisallowMultipleComponent]
    public class VideoClipEventController : MonoBehaviour
    {
        /// <summary>
        /// The list of events with associated data
        /// </summary>
        private VideoClipEvent[] videoClipEvents;
        
        /// <summary>
        /// dummy user data for testing
        /// </summary>
        private string dummyUserData;
        
        /// <summary>
        /// For keeping track if the data needs to be reinitialised
        /// </summary>
        private bool isDirty;

        private VideoPlayer player;
        private VideoClip clip;
        
#if UNITY_EDITOR
        private VideoClipImporter importer;
#endif
        
        public bool HasInitialised { get; private set; }

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
            if (!isDirty)
            {
                return;
            }
            player = GetComponent<VideoPlayer>();
            clip = player.clip;
            importer = (VideoClipImporter)AssetImporter.GetAtPath(clip.originalPath);
            Debug.Log("[VideoClipEventController] "+importer.userData);
            isDirty = true;
        }
#endif
    }
}