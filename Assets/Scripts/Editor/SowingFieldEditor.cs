using UnityEditor;
using UnityEngine;

namespace Scripts.Editor
{
    [CustomEditor(typeof(SowingField))]
    public class SowingFieldEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SowingField sowingField = (SowingField)target;

            if (GUILayout.Button("Generate"))
            {
                sowingField.BuildField();
            }
        }
    }
}
