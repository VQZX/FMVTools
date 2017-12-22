﻿using System;
using UnityEditor;
using UnityEngine;
using System.IO;
using VideoTools.Experimental.DataStructure;
using Object = UnityEngine.Object;

namespace VideoTools.Experimental.Editor.Data
{
    public class EditorVideoClipEvent
    {
        public bool isDisplayed;

        public VideoClipEvent clipEvent;
        
        public EditorVideoClipEvent(VideoClipEvent clipEvent)
        {
            this.clipEvent = clipEvent;
        }

        public EditorVideoClipEvent()
        {
            clipEvent = default(VideoClipEvent);
        }

        public void DrawVideoClipEvent(ref Rect rect, float spacing = 25f)
        {
            // Method Name
            rect.width = 300f;    
            clipEvent.MethodName = EditorGUI.TextField(rect, "Method Name", clipEvent.MethodName);
            if (string.IsNullOrEmpty(clipEvent.MethodName) || !ValidMethodName(clipEvent.MethodName))
            {
                clipEvent.MethodName = string.Empty;
            }
            rect.y += spacing;
            string prettyTime = clipEvent.Time.ToTime();
            
            // Time
            EditorGUI.TextField(rect, "Time", prettyTime);
            spacing *= 1.2f;
            rect.y += spacing;
            
            // float parameter
            clipEvent.FloatParam = EditorGUI.FloatField(rect, "Float Param", clipEvent.FloatParam);
            rect.y += spacing;
            
            // Int param
            clipEvent.IntParam = EditorGUI.IntField(rect, "Int Param", clipEvent.IntParam);
            rect.y += spacing;
            
            // string param
            clipEvent.StringParam = EditorGUI.TextField(rect, "String Param", clipEvent.StringParam);
            rect.y += spacing;
            
            // Object param
            clipEvent.ObjectParam = EditorGUI.ObjectField(rect, "Object Param", (Object)clipEvent.ObjectParam, typeof(Object), false);
            rect.y += spacing;
        }

        private static bool ValidMethodName(string name)
        {
            return name.Split(Path.GetInvalidFileNameChars()).Length <= 1;
        }
    }
}