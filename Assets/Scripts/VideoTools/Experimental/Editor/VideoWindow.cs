using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
using VideoTools.Experimental.DataStructure;

namespace VideoTools.Experimental.Editor
{
    public class VideoWindow : EditorWindow
    {
        private static VideoClip editingClip;
        private static VideoClipImporter importer;
        private static Texture videoTexture;
        private static Rect rect = new Rect(0, 0, 300 , 300 );
        
        //For testing
        private static VideoClipEvent clipEvent;
        
        // TODO: Add ability to drag video clip into window
        /// <summary>
        /// Draws the video to the scene with a timeline
        /// </summary>
        protected virtual void OnGUI()
        {
            titleContent = new GUIContent("Video Editor");
            if (editingClip == null)
            {
                CheckDragging();
            }
            else
            {
                DrawVideoClip();
                DrawTimeline();
                DrawEventLine();
            }
        }

        protected virtual void Update()
        {
            if (importer.isPlayingPreview)
            {
                Repaint();
            }
        }

        /// <summary>
        /// Allows for a video to be dragged in to window
        /// if none exists
        /// </summary>
        private void CheckDragging()
        {
            
        }

        /// <summary>
        /// Draws the video clip in the window
        /// </summary>
        private static void DrawVideoClip()
        {
            videoTexture = importer.GetPreviewTexture();
            if (videoTexture == null)
            {
                return;
            }
            EditorGUI.DrawPreviewTexture(rect, videoTexture);
        }

        /// <summary>
        /// Draws the timeline (maybe with images) to allow for easy placement of events
        /// </summary>
        private void DrawTimeline()
        {
            
        }

        /// <summary>
        /// Draw the line for representing events
        /// </summary>
        private void DrawEventLine()
        {
           
        }

        #region Static Calls
        /// <summary>
        /// The method to dispaly the window
        /// </summary>
        [MenuItem("VideoTools/Video Window")]
        public static void ShowWindow()
        {
            GetWindow<VideoWindow>();
        }
        
        [MenuItem("Assets/Edit Video")]
        public static void MenuShowWindow()
        {
            ShowWindow();
            editingClip = (Selection.activeObject as VideoClip);
            if (editingClip == null)
            {
                Debug.LogWarning("No video clip selected");
                return;
            }
            importer = (VideoClipImporter)AssetImporter.GetAtPath(editingClip.originalPath);
            importer.PlayPreview();
            
            _TestData();
        }

        [MenuItem("Assets/Edit Video", true)]
        private static bool ValidateMenuShowWindow()
        {
            return (Selection.activeObject is VideoClip);
        }

        private static void _TestData()
        {
            clipEvent.MethodName = "MethodOneParamInt";
            clipEvent.IntParam = 7;

            VideoClipEvent otherEvent = new VideoClipEvent();
            otherEvent.MethodName  = "MethodOneParamString";
            otherEvent.StringParam = "ThisIsAString";
            
            VideoClipEvents events = new VideoClipEvents();
            events.Add(clipEvent);
            events.Add(otherEvent);

            string data = VideoClipEvents.GetXml(events);
            importer.userData = data;
        }
        #endregion
    }
}