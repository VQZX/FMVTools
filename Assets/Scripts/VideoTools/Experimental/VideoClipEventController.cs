using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
using VideoTools.Experimental.DataStructure;
using VideoTools.Experimental.DataStructure.MemberInfoData;

namespace VideoTools.Experimental
{    
    [DisallowMultipleComponent]
    public class VideoClipEventController : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        protected List<TimeMethod> methods = new List<TimeMethod>();
        
        /// <summary>
        /// The list of events with associated data
        /// </summary>
        private VideoClipEvents clipEvents;
        
        /// <summary>
        /// The associated events with the information in clip events
        /// </summary>
        private MemberInfo[] memberInfos;
        
        /// <summary>
        /// dummy user data for testing
        /// </summary>
        private string dummyUserData;
        
        /// <summary>
        /// For keeping track if the data needs to be reinitialised
        /// </summary>
        private bool isDirty;

        private VideoPlayer player;
        private VideoClip clip;
        
#if UNITY_EDITOR
        private VideoClipImporter importer;
#endif
        
        public bool HasInitialised { get; private set; }

        public bool IsDirty
        {
            get { return isDirty; }
            set
            {
                isDirty = value;
                if (value)
                {
                    Init();
                }
            }
        }

#if UNITY_EDITOR
        public void Init()
        {
            if (!isDirty)
            {
                return;
            }
            player = GetComponent<VideoPlayer>();
            clip = player.clip;
            importer = (VideoClipImporter)AssetImporter.GetAtPath(clip.originalPath);
            clipEvents = VideoClipEvents.DeserializeFromXml(importer.userData);
            isDirty = false;
            AssignEvents();
        }


        private void AssignEvents()
        {
            Component [] attachedComponents = gameObject.GetComponentsInChildren<Component>();
            Type argumentType = default(Type);
            foreach (Component component in attachedComponents)
            {
                MethodInfo[] infos = component.GetType().GetMethods( BindingFlags.Public | BindingFlags.Instance );
                foreach (VideoClipEvent clipEvent in clipEvents)
                {
                    Debug.LogFormat("Find Method {0}...", clipEvent.MethodName);
                    foreach (MethodInfo info in infos)
                    {                      
                        string methodName = info.Name.Split('.').Last();
                        if (info.GetGenericArguments().Length == 0)
                        {
                            if (methodName == clipEvent.MethodName)
                            {
                                UnityReifiedMethod method = new UnityReifiedMethod(info, component, null);
                                TimeMethod timeMethod = new TimeMethod(clipEvent.Time, method);
                                methods.Add(timeMethod);
                                Debug.Log(methodName);
                                break;
                            }
                        }
                        else if ( IsValidArgument(info.GetGenericArguments(), ref argumentType))
                        {
                            if (methodName == clipEvent.MethodName)
                            {
                                UnityReifiedMethod method = new UnityReifiedMethod(info, component, 
                                    clipEvent.GetAppropriateValue(argumentType));
                                TimeMethod timeMethod = new TimeMethod(clipEvent.Time, method);
                                methods.Add(timeMethod);
                                Debug.Log(methodName);
                                break;
                            }
                        }
                        else
                        {
                            Debug.LogFormat("Failed To Find Method: {0}", clipEvent.MethodName);
                        }
                    }
                }
            }
        }

        private static bool IsValidArgument(Type[] arguments, ref Type argumentType)
        {
            if (arguments.Length == 1)
            {
                Type argument = arguments[0];
                bool validType = argument == typeof(float) ||
                                 argument == typeof(int) ||
                                 argument == typeof(string) ||
                                 argument == typeof(object); 
                // I just realised c# object is stupid
                // TODO: (2017/12/11) try to use UnityObject instead
                argumentType = argument;
                return validType;
            }
            else
            {
                return false;
            }
        }
#endif
    }
}