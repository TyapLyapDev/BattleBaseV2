using System;
using UnityEngine;
using UnityEngine.UI;

namespace BattleBase.Gameplay.Actors.HealthSystem
{
    [RequireComponent(typeof(Slider), typeof(CanvasGroup))]
    public class HealthBar : MonoBehaviour, IHealthViewComponent
    {
        private const float MaxValue = 1.0f;
        private const float MinValue = 0.0f;

        private IHealthEvents _healthEvents;
        private CanvasGroup _canvasGroup;
        private Slider _slider;

        private void OnEnable()
        {
            if (_healthEvents != null)
                _healthEvents.HealthChanged += OnUpdateDataHealth;
        }

        private void OnDisable()
        {
            if (_healthEvents != null)
                _healthEvents.HealthChanged -= OnUpdateDataHealth;

            _canvasGroup.alpha = MinValue;
        }

        public void Init(IHealthEvents healthEvents)
        {
            _healthEvents = healthEvents ?? throw new ArgumentNullException(nameof(healthEvents));

            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = MinValue;
            _canvasGroup.interactable = false;

            _slider = GetComponent<Slider>();
            _slider.maxValue = MaxValue;
            _slider.minValue = MinValue;

            if (gameObject.activeSelf)
                _healthEvents.HealthChanged += OnUpdateDataHealth;
        }

        private void OnUpdateDataHealth(float maxHealth, float currentHealth)
        {
            if (maxHealth <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxHealth));

            if (currentHealth > maxHealth || currentHealth < 0)
                throw new ArgumentOutOfRangeException(nameof(currentHealth));

            bool isHealthFull = Mathf.Approximately(currentHealth, maxHealth);

            if (isHealthFull)
                _canvasGroup.alpha = MinValue;
            else
                _canvasGroup.alpha = MaxValue;

            _slider.value = (float)currentHealth / maxHealth;
        }
    }
}