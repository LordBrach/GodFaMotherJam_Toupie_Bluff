using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class SettingsPanel : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private CanvasGroup group;
    [SerializeField] private Button closeButton;

    [Header("Sliders (0 à 1, onValueChanged branché en inspecteur sur AudioManager)")]
    [SerializeField] private Slider generalSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField, Min(0f)] private float fadeDuration = 0.25f;

    private Coroutine current;

    private void Reset() => group = GetComponent<CanvasGroup>();

    private void Awake()
    {
        if (group == null) group = GetComponent<CanvasGroup>();

        if (closeButton != null) closeButton.onClick.AddListener(Close);

        group.alpha = 0f;
        group.blocksRaycasts = false;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (closeButton != null) closeButton.onClick.RemoveListener(Close);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        SyncSlidersFromAudio(); // le panel doit refléter les volumes actuels, pas repartir à 1

        if (current != null) StopCoroutine(current);
        current = StartCoroutine(FadeRoutine(1f));
    }

    public void Close()
    {
        if (current != null) StopCoroutine(current);
        current = StartCoroutine(FadeRoutine(0f));
    }

    private IEnumerator FadeRoutine(float target)
    {
        group.blocksRaycasts = true;
        group.interactable = true;
        yield return UIFade.FadeTo(group, target, fadeDuration);

        bool visible = target > 0.5f;
        group.blocksRaycasts = visible;
        group.interactable = visible;
        if (!visible) gameObject.SetActive(false);

        current = null;
    }

    private void SyncSlidersFromAudio()
    {
        AudioManager audio = AudioManager.Instance;
        if (audio == null) return;

        // SetValueWithoutNotify évite de redéclencher onValueChanged pour rien juste au moment de l'ouverture.

        if (generalSlider != null) generalSlider.SetValueWithoutNotify(audio.GeneralVolume);
        if (musicSlider != null) musicSlider.SetValueWithoutNotify(audio.MusicVolume);
        if (sfxSlider != null) sfxSlider.SetValueWithoutNotify(audio.SfxVolume);
    }
}