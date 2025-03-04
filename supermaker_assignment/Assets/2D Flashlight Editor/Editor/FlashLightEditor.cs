using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FlashLight2D)), CanEditMultipleObjects]
public class EnemySightEditor : Editor
{
    static internal FlashLight2D sight;
    private bool hideSelection = true;

    [MenuItem("GameObject/FlashLight2D", false, 1)]
    public static void CreateEnemySight()
    {
        GameObject obj = new GameObject("EnemySight2D");
        obj.AddComponent<FlashLight2D>();
        Vector3 position = SceneView.lastActiveSceneView.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1.0f)).origin;
        obj.transform.position = new Vector3(position.x, position.y, 0);
        Selection.activeGameObject = obj;
    }

    public override void OnInspectorGUI()
    {
        if (sight == null)
            return;

        SerializedObject serializedGradient = null;
        SerializedObject serializedMaterial = null;
        float ray_width = sight.ray_width, ray_step = sight.ray_step, distance = sight.distance, light_intensity = sight.light_intensity, 
            light_range = sight.light_range;
        int raysNumber = sight.raysNumber;

        EditorGUI.BeginChangeCheck();

        //Rays
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        bool drawRays = EditorGUILayout.BeginToggleGroup("Draw rays", sight.drawRays);
        if (drawRays)
        {
            serializedGradient = new SerializedObject(target);
            SerializedProperty gradient = serializedGradient.FindProperty("ray_gradient");
            EditorGUILayout.PropertyField(gradient, true, null);
            ray_width = EditorGUILayout.FloatField("Ray width", sight.ray_width);
            ray_step = EditorGUILayout.FloatField("Ray step", sight.ray_step);
            raysNumber = EditorGUILayout.IntField("Ray number", sight.raysNumber);
            distance = EditorGUILayout.FloatField("Ray distance", sight.distance);
            serializedMaterial = new SerializedObject(target);
            SerializedProperty material = serializedMaterial.FindProperty("material");
            EditorGUILayout.PropertyField(material, true, null);
        }
        EditorGUILayout.EndToggleGroup();
        bool debugLight = EditorGUILayout.Toggle("Debug lights", sight.debugLight);

        EditorGUILayout.EndVertical();

        //Light
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        bool enableLight = EditorGUILayout.BeginToggleGroup("Enable Light", sight.enableLight);
        if (sight.enableLight)
        {
            light_intensity = EditorGUILayout.FloatField("Light intensity", sight.light_intensity);
            light_range = EditorGUILayout.FloatField("Light range", sight.light_range);

        }
        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.EndVertical();
        //Editor
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        hideSelection = EditorGUILayout.Toggle("Hide Selections", hideSelection);
        EditorGUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(sight);

            if (serializedGradient != null)
                serializedGradient.ApplyModifiedProperties();
            if(serializedMaterial != null)
                serializedMaterial.ApplyModifiedProperties();

            Undo.RecordObject(sight, "Apply changes");
            sight.drawRays = drawRays;
            sight.ray_width = ray_width;
            sight.ray_step = ray_step;
            sight.distance = distance;
            sight.debugLight = debugLight;
            sight.enableLight = enableLight;
            sight.light_intensity = light_intensity;
            sight.light_range = light_range;

            if (sight.raysNumber != raysNumber) 
            {
                sight.raysNumber = raysNumber;
                sight.DestroyObject();
                sight.CreateObject();
            }
            else
                sight.raysNumber = raysNumber;
        }

    }

       

    private void OnSceneGUI()
    {
        if (sight == null) 
            return;
        sight.CastRays();
        if (hideSelection)
            HideSelection();
        else
            RestoreSelection();
    }

    private void HideSelection()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            LineRenderer[] rends = obj.GetComponentsInChildren<LineRenderer> ();

            foreach(var rend in rends)
                 EditorUtility.SetSelectedRenderState(rend, EditorSelectedRenderState.Hidden);
        }
    }

    private void RestoreSelection()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            LineRenderer[] rends = obj.GetComponentsInChildren<LineRenderer>();

            foreach (var rend in rends)
                EditorUtility.SetSelectedRenderState(rend, EditorSelectedRenderState.Highlight);
        }
    }


    private void OnEnable()
    {
        sight = target as FlashLight2D;
    }
}
