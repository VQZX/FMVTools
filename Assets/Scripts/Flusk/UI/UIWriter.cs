using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityObject = UnityEngine.Component;

namespace Flusk.UI
{
    [RequireComponent(typeof(Text))]
    public class UIWriter : MonoBehaviour
    {
        [SerializeField]
        private UnityObject objectToWriteFrom;

        [SerializeField]
        private FieldInfo dataToWriteFrom;

        [HideInInspector, SerializeField]
        private string dataObjectName;

        [HideInInspector, SerializeField]
        private int index;

        private Text text;

        protected virtual void Awake()
        {
            text = GetComponent<Text>();  
            Component[] attachedComponents = objectToWriteFrom.GetComponentsInChildren<Component>();
            foreach (Component component in attachedComponents)
            {
                BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | 
                                     BindingFlags.Static | BindingFlags.Instance | 
                                     BindingFlags.DeclaredOnly;
                var currentFields = component.GetType().GetFields(flags);
                if (TryFindField(component, currentFields))
                {
                    break;
                }
            }
        }

        private bool TryFindField(Component component, FieldInfo [] currentFields)
        {
            foreach (FieldInfo fieldInfo in currentFields)
            {
                if (component == objectToWriteFrom && fieldInfo.Name == dataObjectName)
                {
                    dataToWriteFrom = fieldInfo;
                    return true;
                }
            }
            return false;
        }
        
        protected virtual void Update()
        {
            string result = dataToWriteFrom.GetValue(objectToWriteFrom).ToString();
            text.text = string.Format("{0}", result);
        }
    }
}