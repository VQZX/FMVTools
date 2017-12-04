using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

namespace VideoTools.Experimental.Editor
{
    public class VideoWindow : EditorWindow
    {

        private static VideoClip editingClip;
        private static VideoClipImporter importer;
        
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
        private void DrawVideoClip()
        {
            GUIContent video = new GUIContent();
            Texture videoTexture = 
            
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
            }
            importer = new VideoClipImporter();
            importer.
        }

        [MenuItem("Assets/Edit Video", true)]
        private static bool ValidateMenuShowWindow()
        {
            return (Selection.activeObject is VideoClip);
        }
        #endregion
    }
}