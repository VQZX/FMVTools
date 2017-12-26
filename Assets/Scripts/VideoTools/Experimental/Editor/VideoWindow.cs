using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
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
        public static Rect TimeLine
        {
            get { return timeLine; }
        }
        private static Rect manipTimeline = new Rect();

        public static Rect ManipTimeline
        {
            get { return manipTimeline; }
        }

        private static float multiplier = 0.5f;
        private static SerializedObject videoClipImporter;
        private static SerializedProperty userDataSerializedProperty;
        private static SerializedProperty assetPathSerializedProperty;
        
        private static VideoWindow current;
        public static VideoWindow Current
        {
            get { return current; }
        }

        private static bool mustPlayPreview;

        private static VideoClipEvents videoClipEvents;
        private static EditorVideoClipEvents editorVideoClipEvents;

        public static double TotalVideoTime { get; private set; }

        private static double currentTime;
        private static double timeAtPlayClicked;

        private double currentSelectedTime;
        private bool isAddingEvent;
        private EditorVideoClipEvent currentEvent;
        private Rect timePositionIndicatorTemplateRect = new Rect(10, 10, 10, 10);

        private Vector2 mousePosition;

        private static string VideoClipUserData
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

        private static string AssetPath
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
            mousePosition = Event.current.mousePosition;
            if (editingClip == null)
            {
                return;
            }
            DrawVideoClip();
            DrawTimeline();
            DrawEventLine();
        }

        protected virtual void Update()
        {
            if (importer != null && importer.isPlayingPreview)
            {
                Repaint();
            }
        }

        private void OnEnable()
        {
            videoClipEvents = new VideoClipEvents();
            editorVideoClipEvents = new EditorVideoClipEvents();
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
                ChangePlayState();
            }
            if (mustPlayPreview)
            {
                PlayPreview();
            }
            EditorGUI.DrawPreviewTexture(videoRect, videoTexture);
            
            if (GUI.Button(applyButtonRect, "Apply"))
            {
                importer.userData = VideoClipEvents.ObjectToJSON(videoClipEvents);
                videoClipImporter.ApplyModifiedPropertiesWithoutUndo();
                ReImportAssets(AssetPath);
            }
        }

        private static void PlayPreview()
        {
            currentTime = EditorApplication.timeSinceStartup - timeAtPlayClicked;
            string result = string.Format("{0:N2}", currentTime);
            if (currentTime >= TotalVideoTime)
            {
                importer.StopPreview();
                currentTime = 0;
            }
            result = string.Format("{0}/{1:N2}", result, TotalVideoTime);
            EditorGUI.LabelField(new Rect(10, current.position.height - 100, 100, 100), result);
        }

        private static void ChangePlayState()
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

        /// <summary>
        /// Draws the timeline (maybe with images) to allow for easy placement of events
        /// </summary>
        private void DrawTimeline()
        {
            timeLine = current.position;
            timeLine.x = timeLine.width * 0.1f;
            timeLine.width *= 0.7f;
            timeLine.height = 30f;
            timeLine.y = videoRect.yMax + 50f;
            string label = ((float) currentTime).ToTime();
            currentTime = EditorGUI.Slider(timeLine, label, (float)currentTime, 0f, (float)TotalVideoTime);
        }

        /// <summary>
        /// Draw the line for representing events
        /// </summary>
        private void DrawEventLine()
        {
            manipTimeline = timeLine;
            manipTimeline.y += 40f;
            string result = string.Format(" Events: {0}", ((float)currentSelectedTime).ToTime());
            EditorGUI.LabelField(manipTimeline, result);
            manipTimeline.y += 40f;
            currentSelectedTime = EditorGUI.Slider(manipTimeline, string.Empty, (float)currentSelectedTime, 0f,
                (float)TotalVideoTime);
            timePositionIndicatorTemplateRect.y = manipTimeline.y + 20f;
           
            Rect buttonRect = manipTimeline;
            buttonRect.y += 60f;
            buttonRect.height = 20f;
            buttonRect.width = 100f;

            Rect displayRect = buttonRect;
            displayRect.y += 30;
            editorVideoClipEvents.DrawClipEventIndicators(timePositionIndicatorTemplateRect);
            editorVideoClipEvents.InteractWithIndicators(mousePosition, displayRect );
            if (GUI.Button(buttonRect, !isAddingEvent ? "Add Event" : "Save Event"))
            {
                ChangeAddingEventState();
            }
            if (isAddingEvent)
            {
                DrawAddEventGUI(buttonRect, displayRect);
            }
        }

        private void DrawAddEventGUI(Rect buttonRect, Rect displayRect)
        {
            // draw the cancel button
            Rect cancelButton = buttonRect;
            cancelButton.x += cancelButton.width + 10f;
            if (GUI.Button(cancelButton, "Cancel"))
            {
                isAddingEvent = false;
            }

            // draw the event data
            currentEvent.clipEvent.Time = currentSelectedTime;
            currentEvent.DrawVideoClipEvent(ref displayRect);
        }

        private void ChangeAddingEventState()
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
                currentEvent = new EditorVideoClipEvent();
            }
            isAddingEvent = !isAddingEvent;
        }

        #region Static Calls
        /// <summary>
        /// The method to dispaly the window
        /// </summary>
        [MenuItem("VideoTools/Video Window")]
        public static void ShowWindow()
        {
           current = GetWindow<VideoWindow>();
           current.wantsMouseMove = true;
           Initialize();
        }
        
        [MenuItem("Assets/Edit Video")]
        public static void MenuShowWindow()
        {
            ShowWindow();
        }

        private static void Initialize()
        {
            current.wantsMouseMove = true;
            editingClip = (Selection.activeObject as VideoClip);
            if (editingClip == null)
            {
                var videoPlayer = GameObject.FindObjectOfType<VideoPlayer>();
                editingClip = videoPlayer.clip;
                if (editingClip == null)
                {
                    Debug.LogError("No clip selected");
                }
            }
            importer = (VideoClipImporter) AssetImporter.GetAtPath(editingClip.originalPath);
            videoClipImporter = new SerializedObject(importer);
            TotalVideoTime = importer.frameCount / importer.frameRate;

            string userData = importer.userData;
            videoClipEvents = VideoClipEvents.JSONToObject(userData);
            editorVideoClipEvents = new EditorVideoClipEvents(videoClipEvents);
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

            VideoClipEvents events = new VideoClipEvents {{clipEvent, otherEvent}};
            VideoClipUserData = Serialize(events);
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