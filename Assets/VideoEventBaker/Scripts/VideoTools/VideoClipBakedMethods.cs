﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Video;
using VideoTools.Experimental.DataStructure;
using VideoTools.Experimental.DataStructure.MemberInfoData;

namespace VideoTools
{
    [Serializable]
    public class VideoClipBakedMethods
    {
        /// <summary>
        /// The video clip containing the data
        /// </summary>
        [SerializeField]
        protected VideoClip clip;

        [SerializeField]
        protected string clipEventData;
        public string ClipEventData { get { return clipEventData; }}
        
#if UNITY_EDITOR
        /// <summary>
        /// The video clip importer that holds the meta data of
        /// the clip
        /// </summary>
        [SerializeField]
        protected VideoClipImporter importer;
#endif       
        /// <summary>
        /// The list of events with their associated data
        /// </summary>
        [SerializeField]
        protected VideoClipEvents clipEvents;

        /// <summary>
        /// The methods with their fire off time
        /// </summary>
        [SerializeField]
        protected List<TimeMethod> methods = new List<TimeMethod>();
        
        public bool IsDirty { get; protected set; }

        public VideoClipBakedMethods(VideoClip videoClip)
        {
            clip = videoClip;
            Initialize();
            IsDirty = false;
        }

        public VideoClipBakedMethods(VideoClip videoClip, string data)
        {
            clip = videoClip;
            clipEventData = data;
        }

        public string MethodData()
        {
            return clipEvents.ToString();
        }

        public void Initialize()
        {
#if UNITY_EDITOR
            importer = (VideoClipImporter)AssetImporter.GetAtPath(clip.originalPath);
            clipEventData = importer.userData;
            GetVideoClipEvents();
#endif
            IsDirty = false;
        }

        public void GetVideoClipEvents()
        {
            clipEvents = GetVideoClipEvents(clipEventData);
        }
        
        public void AssignEvents(Component [] attachedComponents)
        {
            methods = new List<TimeMethod>();
            Type argumentType = default(Type);
            if (clipEvents == null)
            {
                Initialize();
            }
            foreach (Component component in attachedComponents)
            {
                MethodInfo[] infos = component.GetType().GetMethods( BindingFlags.Public | BindingFlags.Instance );
                foreach (VideoClipEvent clipEvent in clipEvents)
                {
                    argumentType = SearchMethods(infos, clipEvent, component, argumentType);
                }
            }
        }
        
        /// <summary>
        /// Tries to invoke all methods if they are valid
        /// </summary>
        public void InvokeMethodsByTime(double time)
        {
            foreach (TimeMethod timeMethod in methods)
            {
                timeMethod.TryInvoke(time);
            }
        }

        /// <summary>
        /// Invokes the method closest to the time (before the time)
        /// </summary>
        public void InvokeClosestMethodByTime(double time)
        {
            TimeMethod selected = default(TimeMethod);
            foreach (TimeMethod timeMethod in methods)
            {
                if (timeMethod.Time < time)
                {
                    selected = timeMethod;
                }
                else
                {
                    break;
                }
            }
            if (selected != null)
            {
                selected.Invoke();
            }
        }

        public void TestMethods()
        {
            foreach (TimeMethod method in methods)
            {
                method.Invoke();
            }
        }
        
        private Type SearchMethods(MethodInfo[] infos, VideoClipEvent clipEvent, Component component, Type argumentType)
        {
            foreach (var methodBase in infos)
            {
                var info = methodBase;
                string methodName = info.Name.Split('.').Last();
                if (TryToFindMethod(info, methodName, clipEvent, component, ref argumentType))
                {
                    continue;
                }
                break;
            }
            return argumentType;
        }

        private bool TryToFindMethod(MethodInfo info, string methodName, VideoClipEvent clipEvent, Component component, ref Type argumentType)
        {
            if (info.GetParameters().Length == 0)
            {
                if (ExtractParamaterlessMethod(methodName, clipEvent, info, component))
                {
                    return true;
                }
                return false;
            }
            if (!IsValidArgument(info.GetParameters(), ref argumentType))
            {
                return true;
            }
            return ExtractMethod(methodName, clipEvent, info, component, argumentType);
        }

        private bool ExtractMethod(string methodName, VideoClipEvent clipEvent, MethodInfo info, Component component, Type argumentType)
        {
            if (methodName != clipEvent.MethodName)
            {
                return true;
            }
            UnityReifiedMethod method = new UnityReifiedMethod(info, component,
                clipEvent.GetAppropriateValue(argumentType));
            TimeMethod timeMethod = new TimeMethod(clipEvent.Time, method);
            methods.Add(timeMethod);
            return false;
        }

        private bool ExtractParamaterlessMethod(string methodName, VideoClipEvent clipEvent, MethodInfo info, Component component)
        {
            if (methodName != clipEvent.MethodName)
            {
                return true;
            }
            Debug.Log("Method: "+info.GetType());
            UnityReifiedMethod method = new UnityReifiedMethod(info, component, null);
            TimeMethod timeMethod = new TimeMethod(clipEvent.Time, method);
            methods.Add(timeMethod);
            return false;
        }

        private static bool IsValidArgument(ParameterInfo[] arguments, ref Type argumentType)
        {
            if (arguments.Length == 1)
            {
                Type argument = arguments[0].ParameterType;
                bool validType = argument == typeof(float) ||
                                 argument == typeof(int) ||
                                 argument == typeof(string) ||
                                 argument == typeof(object); 
                // I just realised c# object is stupid
                // TODO: (2017/12/11) try to use UnityObject instead
                argumentType = argument;
                return validType;
            }
            return false;
        }
        
        protected static VideoClipEvents GetVideoClipEvents(string data)
        {
            VideoClipEvents clipEvents = new VideoClipEvents();
            JsonUtility.FromJsonOverwrite(data, clipEvents);
            return clipEvents;
        }
    }
}