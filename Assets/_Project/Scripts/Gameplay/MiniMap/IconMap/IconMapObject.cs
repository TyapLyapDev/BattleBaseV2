using System;
using BattleBase.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BattleBase.Gameplay.MiniMap
{
    public class IconMapObject : MonoBehaviour, IPoolable<IconMapObject>
    {
        [SerializeField] private Image _image;
        [SerializeField] private RectTransform _rectTransform;

        public event Action<IconMapObject> Deactivated;

        public void Show() =>
            gameObject.SetActive(true);

        public void Hide() =>
            gameObject.SetActive(false);

        public void SetAnchoredPosition(Vector2 position) =>
            _rectTransform.anchoredPosition = position;

        public void SetParent(Transform parent) =>
            transform.SetParent(parent);

        public void SetColor(Color color) =>
            _image.color = color;

        public void SetSize(Vector2 size) =>
            _rectTransform.sizeDelta = size;

        public void SetRotation(float rotation) =>
            _rectTransform.rotation = Quaternion.Euler(0, 0, -rotation);

        public void ResetScale() =>
            transform.localScale = Vector3.one;

        public void Deactivate()
        {
            SetParent(null);
            Deactivated?.Invoke(this);
        }
    }
}