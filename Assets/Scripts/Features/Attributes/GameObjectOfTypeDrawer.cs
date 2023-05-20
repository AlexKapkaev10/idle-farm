using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scripts.Features.Attributes
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(GameObjectOfTypeAttribute))]
    public class GameObjectOfTypeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!IsFieldGameObject())
            {
                DrawError(position);
                return;
            }

            var gameObjOfTypeAttribute = attribute as GameObjectOfTypeAttribute;
            var requiredType = gameObjOfTypeAttribute.Type;

            CheckDragAndDrop(position, requiredType);
            CheckValues(property, requiredType);
            DrawObjectField(property, position, label, gameObjOfTypeAttribute.AllowSceneObj);
        }

        private void CheckDragAndDrop(Rect position, Type requiredType)
        {
            if (!position.Contains(Event.current.mousePosition))
                return;

            var dragObjectsCount = DragAndDrop.objectReferences.Length;

            for (int i = 0; i < dragObjectsCount; i++)
            {
                if (!IsVallidObject(DragAndDrop.objectReferences[i], requiredType))
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                    break;
                }
            }
        }

        private bool IsVallidObject(Object o, Type requiredType)
        {
            var result = false;
            var go = o as GameObject;

            if (go != null)
            {
                result = go.GetComponent(requiredType) != null;
            }

            return result;
        }

        private bool IsFieldGameObject()
        {
            return fieldInfo.FieldType == typeof(GameObject)
                   || typeof(IEnumerable<GameObject>).IsAssignableFrom(fieldInfo.FieldType);
        }

        private void DrawError(Rect position)
        {
            EditorGUI.HelpBox(
                position,
                $"GameObjectOfTypeAttribute works only with GameObject references",
                MessageType.Error);
        }

        private void DrawObjectField(
            SerializedProperty property,
            Rect position,
            GUIContent label,
            bool allowSceneObjects)
        {
            property.objectReferenceValue = EditorGUI.ObjectField(
                position,
                label,
                property.objectReferenceValue,
                typeof(GameObject),
                allowSceneObjects);
        }

        private void CheckValues(SerializedProperty property, Type requiredType)
        {
            if (property.objectReferenceValue != null)
            {
                if (!IsVallidObject(property.objectReferenceValue, requiredType))
                {
                    property.objectReferenceValue = null;
                }
            }
        }
    }
#endif
}