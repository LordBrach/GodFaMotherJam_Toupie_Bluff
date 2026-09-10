using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.35f;

    [SerializeField] private bool startOpaque = true;

    private Coroutine currentFade;

    private bool isFading { get; set; }

    private void Reset() => canvasGroup = GetComponent<CanvasGroup>();

    private void Awake()
    {

        Instance = this;

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        ApplyCG(startOpaque ? 1f : 0f);
    }

    private void Start()
    {
        if (startOpaque) 
            FadeIn();   
    }

    public Coroutine FadeIn(float duration = -1f) => Run(0f, duration);
    public Coroutine FadeOut(float duration = -1f) => Run(1f, duration);

    private Coroutine Run(float target, float duration)
    {
        if(currentFade != null) 
            StopCoroutine(currentFade);

        float actualDuration = duration < 0f ? fadeDuration : duration;

        currentFade = StartCoroutine(FadeRoutine(target, actualDuration));
        return currentFade;
    }

    private IEnumerator FadeRoutine(float target, float duration)
    {
        isFading = true;
        
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.blocksRaycasts = true;

        yield return UIFade.FadeTo(canvasGroup, target, duration);

        ApplyCG(target);
        isFading = false;
        currentFade = null;
    }

    private void ApplyCG(float alpha)
    {
        bool visible = alpha > 0.0001f;
        canvasGroup.alpha = alpha;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
        canvasGroup.gameObject.SetActive(visible);
    }

    public static void GoToScene(string sceneName)
    {
        if(string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name is null or empty. Can't load scene.");
            return;
        }

        if(Instance == null) { SceneManager.LoadScene(sceneName); return; }
        if (Instance.isFading) return; 

        Instance.StartCoroutine(Instance.LoadRoutine(sceneName));
    }

    private IEnumerator LoadRoutine(string sceneName)
    {
        yield return FadeOut();
        SceneManager.LoadScene(sceneName);
    }
}
