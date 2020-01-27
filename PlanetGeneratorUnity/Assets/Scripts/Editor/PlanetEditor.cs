using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    private Planet _planet;

    private Editor _shapeEditor;
    private Editor _colourEditor;

    private void OnEnable()
    {
        _planet = (Planet) target;
    }

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                _planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate Planet"))
        {
            _planet.GeneratePlanet();
        }

        DrawSettingsEditor(_planet.shapeSettings, _planet.OnShapeSettingsUpdated, ref _planet.shapeFoldout,
            ref _shapeEditor);
        DrawSettingsEditor(_planet.colourSettings, _planet.OnColourSettingsUpdated, ref _planet.colorFoldout,
            ref _colourEditor);
    }

    private static void DrawSettingsEditor(Object settings, Action onSettingsUpdated, ref bool foldout,
        ref Editor editor)
    {
        if (settings == null)
        {
            return;
        }

        foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            if (!foldout)
            {
                return;
            }

            CreateCachedEditor(settings, null, ref editor);
            editor.OnInspectorGUI();
            if (check.changed)
            {
                onSettingsUpdated?.Invoke();
            }
        }
    }
}