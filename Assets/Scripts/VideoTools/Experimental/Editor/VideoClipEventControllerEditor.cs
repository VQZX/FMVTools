using UnityEditor;
using UnityEngine;

namespace VideoTools.Experimental.Editor
{
    [CustomEditor(typeof(VideoClipEventController))]
    public class VideoClipEventControllerEditor : UnityEditor.Editor
    {
        private VideoClipEventController controller;
        protected virtual void OnEnable()
        {
            controller = (VideoClipEventController) target;
            controller.hideFlags = HideFlags.HideInInspector;
            controller.Init();
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(controller);
        }
    }
}