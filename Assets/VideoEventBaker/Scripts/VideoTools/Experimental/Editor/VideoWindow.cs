using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
using VideoTools.Experimental.DataStructure;
using VideoTools.Experimental.Editor.Data;

namespace VideoTools.Experimental.Editor
{
    public class VideoWindow : EditorWindow
    {
        /// <summary>
        /// The current instance of the video window
        /// </summary>
        public static VideoWindow Current { get; private set; }
        
        /// <summary>
        /// The rect representing the timeline slider
        /// </summary>
        private Rect timeLine;
        public Rect TimeLine
        {
            get { return timeLine; }
        }
        
        /// <summary>
        /// The rect represeting the manipulation slider for events
        /// </summary>
        private Rect manipTimeline;
        public Rect ManipTimeline
        {
            get { return manipTimeline; }
        }
        
        /// <summary>
        /// The asset path of the video clip
        /// </summary>
        private string AssetPath
        {
            get
            {
                var clipImporter = videoClipImporter.targetObject as VideoClipImporter;
                return clipImporter != null ? clipImporter.assetPath : string.Empty;
            }
        }
        
        /// <summary>
        /// The length of the video clip
        /// </summary>
        public double TotalVideoTime { get; private set; }
        
        /// <summary>
        /// The clip we are editing
        /// </summary>
        private VideoClip editingClip;
        
        /// <summary>
        /// The importer used to bake information into the clip
        /// </summary>
        private VideoClipImporter importer;
        
        /// <summary>
        /// The list of video clip events to be tracked
        /// </summary>
        private VideoClipEvents videoClipEvents;
        
        /// <summary>
        /// The list of editor video clips which contain the video clip events
        /// as well as special logic for editor UI
        /// </summary>
        private EditorVideoClipEvents editorVideoClipEvents;
        
        /// <summary>
        /// The serialized object representing the <see cref="importer"/>
        /// </summary>
        private SerializedObject videoClipImporter;
        
        /// <summary>
        /// The serialized property representing the baked information
        /// </summary>
        private SerializedProperty userDataSerializedProperty;
        
        /// <summary>
        /// The serialized property represeting the video clip path
        /// </summary>
        private SerializedProperty assetPathSerializedProperty;
        
        /// <summary>
        /// The preview texture printed to the screen
        /// </summary>
        private Texture videoTexture;
        
        /// <summary>
        /// The rect that we draw the video texture to
        /// </summary>
        private Rect videoRect = new Rect(0, 0, 300 , 300 );
        
        /// <summary>
        /// The rect representing the play button position and dimensions
        /// </summary>
        private readonly Rect playButtonRect = new Rect(10, 10, 50, 30);
        
        /// <summary>
        /// The rect representing the apply button position and dimensions
        /// </summary>
        private readonly Rect applyButtonRect = new Rect(10, 50, 50, 30);

        /// <summary>
        /// Tracks if we have set the play the preview to play
        /// </summary>
        private bool mustPlayPreview;

        /// <summary>
        /// A multiplier for controlling how much bigger or smaller the displayed video rect must be compared 
        /// to the actual video
        /// </summary>
        private const float VIDEO_SIZE_MULTIPLIER = 0.5f;
        
        /// <summary>
        /// The time that the preview began playing
        /// </summary>
        private double timeAtPlayClicked;
        
        /// <summary>
        /// The current time of the playback preview
        /// </summary>
        private double currentTime;
        
        /// <summary>
        /// The currently selected time for adding events 
        /// </summary>
        private double currentSelectedTime;
        
        /// <summary>
        /// Tracks if the UI for adding an event is active
        /// </summary>
        private bool isAddingEvent;
        
        /// <summary>
        /// Tracks the current event being added or edited
        /// </summary>
        private EditorVideoClipEvent currentEvent;
        
        /// <summary>
        /// The template rect for the event point indicators
        /// </summary>
        private Rect timePositionIndicatorTemplateRect = new Rect(10, 10, 10, 10);

        /// <summary>
        /// The current mouse position
        /// </summary>
        private Vector2 mousePosition;

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

        /// <summary>
        /// When the preview is playing we need to constantly repaint
        /// </summary>
        protected virtual void Update()
        {
            if (importer != null && importer.isPlayingPreview)
            {
                Repaint();
            }
        }
        
        /// <summary>
        /// Initialisate the lists when the window is created
        /// </summary>
        protected virtual void OnEnable()
        {
            videoClipEvents = new VideoClipEvents();
            editorVideoClipEvents = new EditorVideoClipEvents();
        }

        /// <summary>
        /// Draws the video clip in the window
        /// And draws the Play/Pause buttons, the Apply button for saving the data to the video clip metadata
        /// </summary>
        private void DrawVideoClip()
        {
            videoTexture = importer.GetPreviewTexture();
            if (videoTexture == null)
            {
                return;
            }
            Vector2 size = new Vector2(videoTexture.width, videoTexture.height) * VIDEO_SIZE_MULTIPLIER;
            Vector2 videoPosition = new Vector2(100, 10);
            videoRect = new Rect(videoPosition, size);
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

        /// <summary>
        /// Plays the preview
        /// </summary>
        private void PlayPreview()
        {
            currentTime = EditorApplication.timeSinceStartup - timeAtPlayClicked;
            string result = string.Format("{0:N2}", currentTime);
            if (currentTime >= TotalVideoTime)
            {
                importer.StopPreview();
                currentTime = 0;
            }
            result = string.Format("{0}/{1:N2}", result, TotalVideoTime);
            EditorGUI.LabelField(new Rect(10, Current.position.height - 100, 100, 100), result);
        }

        /// <summary>
        /// Determines whether the video will play or not
        /// </summary>
        private void ChangePlayState()
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
            timeLine = Current.position;
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

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Draws the UI for adding or editing an event
        /// </summary>
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

        /// <summary>
        /// Determines whether the add event UI should be drawn
        /// </summary>
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
        [MenuItem("VideoTools/Video Window"), MenuItem("Assets/Edit Video")]
        public static void ShowWindow()
        {
           Current = GetWindow<VideoWindow>();
           Initialize();
        }

        /// <summary>
        /// Sets the video clip to edit,
        /// Sets the importer,
        /// Caches the video clip length
        /// Caches the user data in a serialized property
        /// Initializes the editor video clip events
        /// </summary>
        private static void Initialize()
        {
            Current.wantsMouseMove = true;
            Current.editingClip = (Selection.activeObject as VideoClip);
            if (Current.editingClip == null)
            {
                var videoPlayer = FindObjectOfType<VideoPlayer>();
                Current.editingClip = videoPlayer.clip;
                if (Current.editingClip == null)
                {
                    Debug.LogError("No clip selected");
                    return;
                }
            }
            Current.importer = (VideoClipImporter) AssetImporter.GetAtPath(Current.editingClip.originalPath);
            Current.videoClipImporter = new SerializedObject(Current.importer);
            Current.TotalVideoTime = Current.importer.frameCount / Current.importer.frameRate;

            string userData = Current.importer.userData;
            Current.videoClipEvents = VideoClipEvents.JSONToObject(userData);
            Current.editorVideoClipEvents = new EditorVideoClipEvents(Current.videoClipEvents);
        }

        /// <summary>
        /// A check for enabling/disabling the asset menu "Edit Video" option
        /// </summary>
        [MenuItem("Assets/Edit Video", true)]
        private static bool ValidateMenuShowWindow()
        {
            return (Selection.activeObject is VideoClip);
        }

        /// <summary>
        /// Reimports the video clip asset if it is dirty,
        /// saving any changed userData in the process
        /// </summary>
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
        
        /// <summary>
        /// Callback when the import process has completed
        /// </summary>
        private static void OnAssetImportDone(string path)
        {
            Debug.LogFormat("Asset import of {0} complete.", path);
        }
        #endregion
    }
}