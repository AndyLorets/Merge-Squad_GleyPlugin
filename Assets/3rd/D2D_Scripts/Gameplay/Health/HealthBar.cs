using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace D2D.Gameplay
{
	public class HealthBar : MonoBehaviour
	{
		[SerializeField] private Health targetHealth;
		[SerializeField] private Gradient gradient;
		[SerializeField] private Image fill;
		[SerializeField] private float fadeDuration = .7f;
		[SerializeField] private float showDuration = 2f;
		[SerializeField] private Slider slider;
		
		private CanvasGroup canvasGroup;

		private Tween fadeTween;

		private bool isInited = false;

		private void Awake()
		{
			slider = GetComponent<Slider>();
			canvasGroup = GetComponent<CanvasGroup>();
		}

		private void Start()
		{
			if (targetHealth != null && !isInited)
            {
				InitSlider();
            }
		}

		private void OnDisable()
		{
			if (targetHealth != null)
            {
				targetHealth.PointsChanged -= UpdateSlider;
            }
		}

		private void InitSlider()
		{
			isInited = true;

			targetHealth.PointsChanged += UpdateSlider;

			slider.maxValue = targetHealth.MaxPoints;
			slider.value = targetHealth.MaxPoints;

			fill.color = gradient.Evaluate(1f);
		}

		private void UpdateSlider()
		{
			fadeTween.KillTo0();
			
			if (targetHealth.CurrentPoints <= 0)
            {
				return;
            }

			canvasGroup.DOFade(1, 0);
			fadeTween = canvasGroup.DOFade(0, fadeDuration).SetDelay(showDuration);

			slider.maxValue = targetHealth.MaxPoints;
			slider.value = targetHealth.CurrentPoints;
			
			fill.color = gradient.Evaluate(slider.normalizedValue);
		}

		public void SetHealth(Health health)
        {
			targetHealth = health;

			if (canvasGroup == null)
            {
				canvasGroup = GetComponent<CanvasGroup>();
			}

			InitSlider();
        }
	}
}
