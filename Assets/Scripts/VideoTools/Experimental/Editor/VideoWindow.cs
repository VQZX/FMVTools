using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
using VideoTools.Experimental.DataStructure;
using VideoTools.Experimental.Editor.Data;

namespace VideoTools.Experimental.Editor
{
    public class VideoWindow : EditorWindow
    {
        private static VideoClip editingClip;
        private static VideoClipImporter importer;
        private static Texture videoTexture;
        private static Rect videoRect = new Rect(0, 0, 300 , 300 );
        private static Rect playButtonRect = new Rect(10, 10, 50, 30);
        private static Rect applyButtonRect = new Rect(10, 50, 50, 30);
        
        private static Rect timeLine = new Rect();

        private static float multiplier = 0.5f;
        private static SerializedObject videoClipImporter;
        private static SerializedProperty userDataSerializedProperty;
        private static SerializedProperty assetPathSerializedProperty;
        private static VideoWindow current;
        private static bool mustPlayPreview;

        private static VideoClipEvents videoClipEvents = new VideoClipEvents();
        private static EditorVideoClipEvents editorVideoClipEvents = new EditorVideoClipEvents();

        private static double totalVideoTime;
        private static double currentTime;
        private static double timeAtPlayClicked;

        private double currentSelectedTime;
        private bool isAddingEvent;
        private EditorVideoClipEvent currentEvent;

        private static string videoClipUserData
        {
            get
            {
                if (videoClipImporter == null)
                {
                    return string.Empty;
                }
                VideoClipImporter clipImporter = videoClipImporter.targetObject as VideoClipImporter;
                return clipImporter != null ? clipImporter.userData : string.Empty;
            }
            set
            {
                VideoClipImporter clipImporter = videoClipImporter.targetObject as VideoClipImporter;
                if (clipImporter != null)
                {
                    clipImporter.userData = value;
                }
            }
        }

        private static string assetPath
        {
            get
            {
                var clipImporter = videoClipImporter.targetObject as VideoClipImporter;
                return clipImporter != null ? clipImporter.assetPath : string.Empty;
            }
        }
        //For testing
        private static VideoClipEvent clipEvent;
        
        // TODO: Add ability to drag video clip into window
        /// <summary>
        /// Draws the video to the scene with a timeline
        /// </summary>
        protected virtual void OnGUI()
        {
            titleContent = new GUIContent("Video Editor");
            DrawVideoClip();
            DrawTimeline();
            DrawEventLine();
        }

        protected virtual void Update()
        {
            if (importer.isPlayingPreview)
            {
                Repaint();
            }
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
            Vector2 size = new Vector2(videoTexture.width, videoTexture.height) * multiplier;
            //float xPosition = (current.position.width * 0.5f - size.x * 0.5f);
            Vector2 position = new Vector2(100, 10);
            videoRect = new Rect(position, size);
            bool mustPlay = GUI.Button(playButtonRect, mustPlayPreview ? "Pause" : "Play");
            
            if (mustPlay)
            {
                mustPlayPreview = !mustPlayPreview;
                if (mustPlayPreview)
                {
                    importer.PlayPreview();
                    timeAtPlayClicked = EditorApplication.timeSinceStartup;
                }
                else
                {
                    importer.StopPreview();
                }
            }
            if (mustPlayPreview)
            {
                currentTime = EditorApplication.timeSinceStartup - timeAtPlayClicked;
                string result = string.Format("{0:N2}", currentTime);
                if (currentTime >= totalVideoTime)
                {
                    importer.StopPreview();
                    currentTime = 0;
                }
                result = string.Format("{0}/{1:N2}", result, totalVideoTime);
                
                EditorGUI.LabelField(new Rect(10, current.position.height - 100, 100, 100), result);
            }
            EditorGUI.DrawPreviewTexture(videoRect, videoTexture);
            
            if (GUI.Button(applyButtonRect, "Apply"))
            {
                importer.userData = VideoClipEvents.ObjectToJSON(videoClipEvents);
                videoClipImporter.ApplyModifiedPropertiesWithoutUndo();
                ReImportAssets(assetPath);
            }
        }

        /// <summary>
        /// Draws the timeline (maybe with images) to allow for easy placement of events
        /// </summary>
        private void DrawTimeline()
        {
            timeLine = current.position;
            timeLine.x += timeLine.width * 0.05f;
            timeLine.width *= 0.7f;
            timeLine.height = 30f;
            timeLine.y = videoRect.yMax + 50f;
            string label = ((float) currentTime).ToTime();
            currentTime = EditorGUI.Slider(timeLine, label, (float)currentTime, 0f, (float)totalVideoTime);
        }

        /// <summary>
        /// Draw the line for representing events
        /// </summary>
        private void DrawEventLine()
        {
            Rect manipTimeline = timeLine;
            manipTimeline.y += 40;
            string result = string.Format(" Events: {0}", ((float)currentSelectedTime).ToTime());
            currentSelectedTime = EditorGUI.Slider(manipTimeline, result, (float)currentSelectedTime, 0f,
                (float)totalVideoTime);
           
            Rect buttonRect = manipTimeline;
            buttonRect.y += 40f;
            buttonRect.height = 20f;
            buttonRect.width = 100f;
            
            if (editorVideoClipEvents == null || editorVideoClipEvents.Count > 0)
            {
                
            }
            if (GUI.Button(buttonRect, !isAddingEvent ? "Add Event" : "Save Event"))
            {
                if (isAddingEvent)
                {
                    // save the event
                    if (editorVideoClipEvents == null)
                    {
                        editorVideoClipEvents = new EditorVideoClipEvents();
                    }
                    editorVideoClipEvents.Add(currentEvent);
                    videoClipEvents = editorVideoClipEvents.GetClipEvents();
                }
                else
                {
                    currentEvent = new EditorVideoClipEvent();                }
                isAddingEvent = !isAddingEvent;
            }
            if (isAddingEvent)
            {
                // draw the cancel button
                Rect cancelButton = buttonRect;
                cancelButton.x += cancelButton.width + 10f;
                if (GUI.Button(cancelButton, "Cancel"))
                {
                    isAddingEvent = false;
                }
                
                // draw the event data
                buttonRect.y += 30;
                currentEvent.clipEvent.Time = currentSelectedTime;
                currentEvent.DrawVideoClipEvent(ref buttonRect);
            }
        }

        #region Static Calls
        /// <summary>
        /// The method to dispaly the window
        /// </summary>
        [MenuItem("VideoTools/Video Window")]
        public static void ShowWindow()
        {
           current = GetWindow<VideoWindow>();
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
            videoClipImporter = new SerializedObject(importer);
            totalVideoTime = importer.frameCount / importer.frameRate;
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
            clipEvent.Time = 10;

            VideoClipEvent otherEvent = new VideoClipEvent
            {
                Time = 5,
                MethodName = "MethodOneParamString",
                StringParam = "ThisIsAString"
            };

            VideoClipEvents events = new VideoClipEvents();
            events.Add(clipEvent, otherEvent);
            videoClipUserData = Serialize(events);
        }

        private static void ReImportAssets(string path)
        {
            AssetDatabase.WriteImportSettingsIfDirty(path);   
            try
            {
                AssetDatabase.StartAssetEditing();
                AssetDatabase.ImportAsset(path);       
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }
            OnAssetImportDone(path);
            HierarchyChangedHandler.ReInit();
        }

        private static string Serialize([NotNull] VideoClipEvents events)
        {
            if (events == null)
            {
                throw new ArgumentNullException("events");
            }
            string output = JsonUtility.ToJson(events);
            Debug.Log(output);
            return output;
        }

        private static void OnAssetImportDone(string path)
        {
            Debug.LogFormat("Asset import of {0} complete.", path);
        }
        #endregion
    }
}