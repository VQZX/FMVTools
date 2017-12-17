using Flusk.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

namespace VideoTools.Experimental.Editor
{
    public static class HierarchyChangedHandler
    {
        
        private static bool hasAssigned = false;
        [MenuItem("VideoTools/Force Hierarchy Check Assignment"), InitializeOnLoadMethod]
        private static void AssignCheck()
        {
            if (hasAssigned)
            {
                return;
            }
            EditorApplication.hierarchyWindowChanged += VideoPlayerCheck;
            hasAssigned = true;
        }

        [MenuItem("VideoTools/Force Hierarchy Check")]
        private static void ForceVideoPlayerCheck()
        {
            VideoPlayer [] players = Object.FindObjectsOfType<VideoPlayer>();
            foreach (VideoPlayer player in players)
            {
                VideoClipEventController controller = player.gameObject.AddSingleComponent<VideoClipEventController>();
#if UNITY_EDITOR
                controller.IsDirty = true;
#endif
                EditorUtility.SetDirty(controller);
            }
        }
        
        private static void VideoPlayerCheck()
        {
            VideoPlayer [] players = Object.FindObjectsOfType<VideoPlayer>();
            foreach (VideoPlayer player in players)
            {
#if UNITY_EDITOR
                VideoClipEventController controller;
                if (player.gameObject.AddSingleComponent(out controller))
                {
                    controller.Init();
                }
#endif
            }
        }  
    }
}
