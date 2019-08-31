using System.Collections;
using UnityEngine;

namespace CGJ.ScenesManagement
{
	public class Fader : MonoBehaviour
	{
		[SerializeField] float fadeInTime = 1.0f;
		[SerializeField] float fadeOutTime = 2.0f;
		CanvasGroup canvasGroup;
		Coroutine currentActiveFade = null;

		void Awake()
		{
            canvasGroup = GetComponent<CanvasGroup>();
		}
		
		public IEnumerator FadeOutIn()
		{
			yield return FadeOut();
			yield return FadeIn();
		}
		
		public void BlackOutScreen()
		{
            canvasGroup.alpha = 1f;
		}

        public IEnumerator FadeOut()
        {
            yield return Fade(1.0f, fadeOutTime);
        }
        public IEnumerator FadeIn()
		{
			yield return Fade(0.0f, fadeInTime);
		}

		public IEnumerator Fade(float targetAlpha, float fadeTime)
		{
			if(currentActiveFade != null)
			{
				StopCoroutine(currentActiveFade);
			}
			currentActiveFade = StartCoroutine(FadeRoutine(targetAlpha, fadeTime));
			
			yield return currentActiveFade;
		}

		private IEnumerator FadeRoutine(float targetAlpha, float fadeTime)
		{
			while (!Mathf.Approximately(canvasGroup.alpha, targetAlpha))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, Time.deltaTime / fadeTime);
				yield return null;
            }
		}
	}
}