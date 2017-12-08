using System.Collections.Generic;
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
                controller.IsDirty = true;
            }
        }
        
        private static void VideoPlayerCheck()
        {
            VideoPlayer [] players = Object.FindObjectsOfType<VideoPlayer>();
            foreach (VideoPlayer player in players)
            {
                VideoClipEventController controller;
                if (player.gameObject.AddSingleComponent(out controller))
                {
                    controller.Init();
                }
            }
        }  
    }
}
