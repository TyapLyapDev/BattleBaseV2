using UnityEngine;

namespace BattleBase.Gameplay.Map
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(Territory))]
    public class TerritoryColorIndicator : MonoBehaviour
    {
        private const string ColorProperty = "_BaseColor";
        private static readonly int ColorPropertyID = Shader.PropertyToID(ColorProperty);

        [SerializeField] private int _surfaceMaterialIndex = 1;

        private MeshRenderer _meshRenderer;
        private Territory _territory;
        private MaterialPropertyBlock _materialPropertyBlock;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _territory = GetComponent<Territory>();

            _materialPropertyBlock = new();
        }

        private void OnEnable()
        {
            _territory.ColorChanged += OnTerritoryChanged;
            OnTerritoryChanged();
        }

        private void OnDisable() =>
            _territory.ColorChanged -= OnTerritoryChanged;

        private void OnTerritoryChanged()
        {
            ApplyColor(_territory.Color);
        }

        private void ApplyColor(Color? color)
        {
            if (color.HasValue == false)
                return;

            _meshRenderer.GetPropertyBlock(_materialPropertyBlock, _surfaceMaterialIndex);
            _materialPropertyBlock.SetColor(ColorPropertyID, color.Value);
            _meshRenderer.SetPropertyBlock(_materialPropertyBlock, _surfaceMaterialIndex);
        }
    }
}