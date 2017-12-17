using System;
using UnityEditor;
using UnityEngine;

namespace VideoTools.Experimental.Editor
{
    [CustomEditor(typeof(VideoClipEventController))]
    public class VideoClipEventControllerEditor : UnityEditor.Editor
    {
        private VideoClipEventController controller;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorUtility.SetDirty(controller);
        }
        
        protected virtual void OnEnable()
        {
            controller = (VideoClipEventController) target;
            controller.hideFlags = HideFlags.HideInInspector;
#if UNITY_EDITOR
            controller.Init();
#endif
        }

        private void OnDisable()
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}