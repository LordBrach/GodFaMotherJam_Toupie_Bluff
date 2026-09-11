using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{
    [SerializeField] private RectTransform pointeur;
    [SerializeField] private float TimeUpdate = 0.5f;
    private float target = 0f;
    private float current = 0f;
    private float sliderCurrent = 0f;
    private IEnumerator barCoroutine;
    private Slider slider;
    public UnityEvent OnBeginUpdateScore;
    public UnityEvent OnEndUpdateScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetupScoreBar()
    {
        target = 0f;
        slider = GetComponent<Slider>();
        slider.value = 0f;
        if (pointeur != null)
        {
            pointeur.anchoredPosition = new Vector2 (0f, 0f);
        }
    }

    private void Start()
    {
        SetupScoreBar();
        slider.value = 0.5f;
    }

    public void UpdateScoreBar(int currentScore)
    {
        Debug.Log("Score:" + currentScore);
        OnBeginUpdateScore.Invoke();
        target = currentScore * 5;
        if(barCoroutine != null)
            StopCoroutine(barCoroutine);
            
        barCoroutine = LerpBar();
        StartCoroutine(barCoroutine);
    }

    IEnumerator LerpBar()
    {
        float a = current;  // start
        float b = target;  // end
        float x = TimeUpdate;  // time frame
        float n = 0;  // lerped value

        for (float f = 0; f <= x; f += Time.deltaTime)
        {
            n = Mathf.Lerp(a, b, f / x);
            current = n;
            pointeur.anchoredPosition = new Vector2(n, 0f);
            slider.value = n;
            yield return null;
        }
        OnEndUpdateScore.Invoke();

    }

}
