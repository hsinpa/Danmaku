using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using UnityEditor;
using XNodeEditor;

namespace DanmakuEditor
{
    [CustomNodeEditor(typeof(BulletPath))]

    public class BulletPathEditor : NodeEditor
    {
        protected bool _showSpawnProperties = true;
        protected bool _showContinuousProperties = true;
        protected bool _showFalloutProperties = true;

        public override void OnBodyGUI()
        {
            // Update serialized object's representation
            serializedObject.Update();

            GUIStyle myFoldoutStyle = GetFoldOutStyle();

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("node"));

            _showSpawnProperties = EditorGUILayout.Foldout(_showSpawnProperties, "Spawn Properties", myFoldoutStyle);
            if (_showSpawnProperties)
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("angle_formula"), true, GUILayout.MinWidth(500));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("angleOnTarget"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("start_delay"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("transition"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("range"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("radius"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("numberOfBullet"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("spawnOffset"));
            }

            _showContinuousProperties = EditorGUILayout.Foldout(_showContinuousProperties, "Continuous Properties", myFoldoutStyle);
            if (_showContinuousProperties) {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("velocity"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("angular_velocity_formula"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("followTarget"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("lerpPercent"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("frequency"));
            }

            _showFalloutProperties = EditorGUILayout.Foldout(_showFalloutProperties, "Fallout Properties", myFoldoutStyle);
            if (_showFalloutProperties)
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("duration"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("constraint"));
            }

            // Apply property modifications
            serializedObject.ApplyModifiedProperties();
        }


        private GUIStyle GetFoldOutStyle() {
            GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            myFoldoutStyle.fontStyle = FontStyle.Bold;
            myFoldoutStyle.fontSize = 14;
            Color myStyleColor = Color.black;
            myFoldoutStyle.normal.textColor = myStyleColor;
            myFoldoutStyle.onNormal.textColor = myStyleColor;
            myFoldoutStyle.hover.textColor = myStyleColor;
            myFoldoutStyle.onHover.textColor = myStyleColor;
            myFoldoutStyle.focused.textColor = myStyleColor;
            myFoldoutStyle.onFocused.textColor = myStyleColor;
            myFoldoutStyle.active.textColor = myStyleColor;
            myFoldoutStyle.onActive.textColor = myStyleColor;

            return myFoldoutStyle;
        }
    }

}