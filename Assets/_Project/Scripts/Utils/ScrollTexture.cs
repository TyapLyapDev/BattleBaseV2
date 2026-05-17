using System;
using UnityEngine;

namespace BattleBase.Utils
{
    [RequireComponent(typeof(Renderer))]
    public class ScrollTexture : MonoBehaviour
    {
        [SerializeField] private string _textureSTPropertyName = "_BaseMap_ST";
        [SerializeField] private Vector2 _scrollSpeed = new(0.5f, 0.5f);

        private Renderer _renderer;
        private MaterialPropertyBlock _propertyBlock;
        private Vector2 _originalScale;
        private int _textureSTPropertyId;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            _propertyBlock = new MaterialPropertyBlock();
            _textureSTPropertyId = Shader.PropertyToID(_textureSTPropertyName);

            Material sharedMaterial = _renderer.sharedMaterial;

            if (sharedMaterial.HasProperty(_textureSTPropertyId) == false)
                throw new Exception($"Material '{sharedMaterial.name}' doesn't have property '{_textureSTPropertyName}'");

            Vector4 originalST = sharedMaterial.GetVector(_textureSTPropertyId);
            _originalScale = new Vector2(originalST.x, originalST.y);
        }

        private void Update()
        {
            Vector2 offset = new Vector2(
                (Time.unscaledTime * _scrollSpeed.x) % 1f,
                (Time.unscaledTime * _scrollSpeed.y) % 1f);

            _renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetVector(_textureSTPropertyId, new Vector4(_originalScale.x, _originalScale.y, offset.x, offset.y));
            _renderer.SetPropertyBlock(_propertyBlock);
        }
    }
}