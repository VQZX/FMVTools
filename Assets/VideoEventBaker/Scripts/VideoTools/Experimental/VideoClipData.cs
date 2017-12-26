using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

namespace VideoTools.Experimental
{
    [Serializable]
    [CreateAssetMenu(fileName = "VideoClipData", menuName = "Video Clip Data", order = 0)]
    public class VideoClipData : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        protected VideoClip videoClip;

        public VideoClip VideoClip
        {
            get { return videoClip; }
        }

        [SerializeField, HideInInspector]
        protected string userData;
        public string UserData
        {
            get { return userData; }
        }
        
        [SerializeField]
        protected VideoClipBakedMethods bakedMethods;
        public VideoClipBakedMethods BakedMethods
        {
            get { return bakedMethods; }
        }

        [SerializeField]
        protected GameObject calllingPrefab;
        public GameObject CalllingPrefab
        {
            get { return calllingPrefab; }
        }

        public void OnBeforeSerialize()
        {
            if (videoClip == null)
            {
                return;
            }
            VideoClipImporter importer = (VideoClipImporter) AssetImporter.GetAtPath(videoClip.originalPath);
            userData = importer.userData;
            bakedMethods = new VideoClipBakedMethods(videoClip, userData);
            if (calllingPrefab != null)
            {
                bakedMethods.AssignEvents(calllingPrefab.GetComponentsInChildren<Component>());
            }
        }

        public void OnAfterDeserialize()
        {
        }
    }
}