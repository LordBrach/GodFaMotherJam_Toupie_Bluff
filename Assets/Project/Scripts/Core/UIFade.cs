using System.Collections;
using UnityEngine;

public static class UIFade 
{

    // Fais un fade in d'un CanvasGroup ou un fade out selon la valeur de targetAlpha (0 = transparent, 1 = opaque)
    public static IEnumerator FadeTo(CanvasGroup canvasGroup, float targetAlpha, float duration)
    {

        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup is null. Can't fade");
            yield break;
        }

        if(duration <= 0f)
        {
            canvasGroup.alpha = targetAlpha;
            yield break;
        }

        float startAlpha = canvasGroup.alpha;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime / duration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, Ease(time));
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }

    // Glisse un RectTransform vers une position cible sur une durée donnée 
    public static IEnumerator SlideTo(RectTransform rectTransform, Vector2 targetPosition, float duration)
    {
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform is null. Can't slide");
            yield break;
        }

        if (duration <= 0f)
        {
            rectTransform.anchoredPosition = targetPosition;
            yield break;
        }

        Vector2 startPosition = rectTransform.anchoredPosition;

        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime / duration;
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, Ease(time));
            yield return null;
        }
    }

    // Fonction d'easing pour rendre le fade plus fluide
    private static float Ease(float progress) => Mathf.SmoothStep(0f,1f,Mathf.Clamp01(progress));
}
