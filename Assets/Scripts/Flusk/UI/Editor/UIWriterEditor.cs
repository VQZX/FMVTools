using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;
using System.Reflection;

namespace Flusk.UI.Editor
{
    [CustomEditor(typeof(UIWriter))]
    // ReSharper disable once InconsistentNaming
    public class UIWriterEditor : UnityEditor.Editor
    {
        private UIWriter writer;

        private SerializedProperty serializedCallingInstance;
        private SerializedProperty serializedDataName;
        private SerializedProperty serializedIndexName;

        private const string CALLING_INSTANCE = "objectToWriteFrom";
        private const string DATA = "dataToWriteFrom";
        private const string DATA_NAME = "dataObjectName";
        private const string INDEX_PROP = "index";

        private List<FieldData> attachedFieldData = new List<FieldData>();
        private string[] fieldNames;

        private int selectedIndex = 0;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            selectedIndex = serializedIndexName.intValue;
            if (serializedCallingInstance.objectReferenceValue != null)
            {
                Component[] attachedComponents = ((Component)serializedCallingInstance.objectReferenceValue).GetComponentsInChildren<Component>();
                foreach (Component component in attachedComponents)
                {
                   BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | 
                                                              BindingFlags.Static | BindingFlags.Instance | 
                                                              BindingFlags.DeclaredOnly;
                    var currentFields = component.GetType().GetFields(flags);
                    foreach (FieldInfo fieldInfo in currentFields)
                    {
                        FieldData data = new FieldData
                        {
                            Component = component,
                            FieldName = fieldInfo.Name,
                            Info = fieldInfo
                        };
                        attachedFieldData.Add(data);
                    }
                }
                fieldNames = attachedFieldData.Select(x => x.FieldName).ToArray();
                selectedIndex = EditorGUILayout.Popup("Writing Field", selectedIndex, fieldNames);
                serializedIndexName.intValue = selectedIndex;
            }
            
            if (attachedFieldData.Count > selectedIndex)
            {
                serializedCallingInstance.objectReferenceValue = attachedFieldData[selectedIndex].Component;
                serializedDataName.stringValue = attachedFieldData[selectedIndex].FieldName;
            }
           
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(writer);
        }

        private void OnEnable()
        {
            writer = (UIWriter) target;
            serializedCallingInstance = serializedObject.FindProperty(CALLING_INSTANCE);
            serializedDataName = serializedObject.FindProperty(DATA_NAME);
            serializedIndexName = serializedObject.FindProperty(INDEX_PROP);
        }
    }
    
    public class FieldData
    {
        public string FieldName;
        public FieldInfo Info;
        public Component Component;
    }
}