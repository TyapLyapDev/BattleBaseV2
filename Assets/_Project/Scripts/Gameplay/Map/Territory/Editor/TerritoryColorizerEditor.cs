#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BattleBase.Gameplay.Map.Editor
{
    [InitializeOnLoad]
    public static class TerritoryColorizerEditor
    {
        private const string ColorProperty = "_BaseColor";
        private const string AdjacentsField = "_adjacents";
        private const string UndoMessage = "Toggle Adjacent Territory";

        private const int MaterialIndex = 1;
        private const bool TwoWayDependency = true;

        private static readonly int ColorPropertyID = Shader.PropertyToID(ColorProperty);

        private static readonly Color SelectedColor = Color.green;
        private static readonly Color AdjacentColor = Color.yellow;

        private readonly static HashSet<MeshRenderer> s_affectedRenderers = new();
        private readonly static FieldInfo s_adjacentsField;

        private static Territory s_currentSelectedTerritory;

        static TerritoryColorizerEditor()
        {
            s_adjacentsField = typeof(Territory).GetField(AdjacentsField,
                BindingFlags.NonPublic | BindingFlags.Instance);

            Selection.selectionChanged += OnSelectionChanged;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSelectionChanged()
        {
            if (Application.isPlaying) 
                return;

            ResetColors();
            GameObject selected = Selection.activeGameObject;

            if (selected != null && selected.TryGetComponent(out Territory territory))
            {
                s_currentSelectedTerritory = territory;
                ApplyColors(territory);
            }
            else
            {
                s_currentSelectedTerritory = null;
            }
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            if (Application.isPlaying)
                return;

            if (s_currentSelectedTerritory == null)
                return;

            Event tempEvent = Event.current;

            if (tempEvent.type == EventType.MouseDown && tempEvent.button == 0 && tempEvent.control && !tempEvent.alt)
            {
                GameObject clickedObject = HandleUtility.PickGameObject(tempEvent.mousePosition, false);

                if (clickedObject != null && clickedObject.TryGetComponent(out Territory clickedTerritory))
                {
                    if (clickedTerritory == s_currentSelectedTerritory)
                        return;

                    ProcessAdjacent(s_currentSelectedTerritory, clickedTerritory);

                    ResetColors();
                    ApplyColors(s_currentSelectedTerritory);

                    EditorUtility.SetDirty(s_currentSelectedTerritory);
                    tempEvent.Use();
                }
            }
        }

        private static void ProcessAdjacent(Territory owner, Territory clicked)
        {
            Undo.RecordObject(owner, UndoMessage);

            if (TwoWayDependency)
                Undo.RecordObject(clicked, UndoMessage);

            bool alreadyConnected = owner.Adjacents.Contains(clicked); 

            if (alreadyConnected)
                owner.RemoveAdjacent(clicked);
            else
                owner.AddAdjacent(clicked);

            EditorUtility.SetDirty(owner);

            if (TwoWayDependency)
                EditorUtility.SetDirty(clicked);
        }

        private static void ApplyColors(Territory territory)
        {
            if (territory.TryGetComponent(out MeshRenderer selectedRenderer))
                SetColor(selectedRenderer, SelectedColor);

            if (s_adjacentsField?.GetValue(territory) is List<Territory> adjacents)
            {
                foreach (Territory adjacent in adjacents)
                {
                    if (adjacent != null)
                    {
                        if (adjacent.TryGetComponent<MeshRenderer>(out var adjRenderer))
                            SetColor(adjRenderer, AdjacentColor);
                    }
                }
            }
        }

        private static void SetColor(MeshRenderer renderer, Color color)
        {
            MaterialPropertyBlock materialPropertyBlock = new();
            renderer.GetPropertyBlock(materialPropertyBlock, MaterialIndex);
            materialPropertyBlock.SetColor(ColorPropertyID, color);
            renderer.SetPropertyBlock(materialPropertyBlock, MaterialIndex);
            s_affectedRenderers.Add(renderer);
        }

        private static void ResetColors()
        {
            foreach (MeshRenderer renderer in s_affectedRenderers)
            {
                if (renderer != null)
                    renderer.SetPropertyBlock(null, MaterialIndex);
            }

            s_affectedRenderers.Clear();
        }
    }
}
#endif