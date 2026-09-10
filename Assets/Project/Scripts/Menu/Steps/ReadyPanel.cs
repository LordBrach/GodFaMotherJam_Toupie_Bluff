using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ReadyPanel : MonoBehaviour
{
    [Serializable]
    private class PlayerSlot
    {
        public string displayName = "Joueur";
        public TMP_Text nameLabel;
        public GameObject readyImg;
        public GameObject pressPrompt;

        [NonSerialized] public bool ready;
    }

    [SerializeField] private CanvasGroup readyGroup;
    [SerializeField, Min(0f)] private float fadeDuration = 0.25f;

    [Header("Container Players")]
    [SerializeField] private GameObject playersContainer;

    [Header("Players")]
    [SerializeField] private PlayerSlot p1 = new PlayerSlot { displayName = "Player 1" };
    [SerializeField] private PlayerSlot p2 = new PlayerSlot { displayName = "Player 2" };

    [SerializeField] private CanvasGroup countdownGroup;
    [SerializeField] private TMP_Text countdownLabel;
    [SerializeField, Min(1)] private int countdownFrom = 3;
    [Tooltip("Countdown speed of fade in/out of every numbers")]
    [SerializeField, Min(0.01f)] private float countdownFadeDuration = 0.6f;
    [Tooltip("Time where's the text stay before to fade out")]
    [SerializeField, Min(0f)] private float countdownPeakHold = 0.5f;
    [Tooltip("Time where last text is visible before fade out")]
    [SerializeField, Min(0f)] private float fightHold = 0.4f;
    [SerializeField] private string goText = "Fight !";

    [Header("Sounds")]
    [SerializeField] private AudioClip readyClip;
    [SerializeField] private AudioClip tickClip;
    [SerializeField] private AudioClip goClip;

    [Header("Add here Gameloop.StartGame on inspector)")]
    [SerializeField] private UnityEvent onCountdownComplete;

    private PlayerInput inputs;

    private void Awake()
    {
        if (readyGroup == null || countdownGroup == null)
        {
            Debug.LogError("Ready Group or countdownGroup missing.", this);
            enabled = false;
            return;
        }

        readyGroup.gameObject.SetActive(true);
        readyGroup.alpha = 1f;
        readyGroup.interactable = true;
        readyGroup.blocksRaycasts = true;

        countdownGroup.alpha = 0f;
        countdownGroup.interactable = false;
        countdownGroup.blocksRaycasts = false;

        if (playersContainer != null) playersContainer.SetActive(true);

        Setup(p1);
        Setup(p2);

        inputs = new PlayerInput();
        inputs.Default.PlayerOne.performed += OnPlayerOnePressed;
        inputs.Default.PlayerTwo.performed += OnPlayerTwoPressed;
        inputs.Enable();
    }

    private void OnDestroy() => ReleaseInputs();

    private void Setup(PlayerSlot slot)
    {
        slot.ready = false;
        if (slot.nameLabel != null) slot.nameLabel.text = slot.displayName;
        if (slot.readyImg != null) slot.readyImg.SetActive(false);
        if (slot.pressPrompt != null) slot.pressPrompt.SetActive(true);
    }

    private void OnPlayerOnePressed(InputAction.CallbackContext ctx) => SetReady(p1);
    private void OnPlayerTwoPressed(InputAction.CallbackContext ctx) => SetReady(p2);

    private void SetReady(PlayerSlot slot)
    {
        if (slot.ready) return;

        slot.ready = true;
        AudioManager.Instance?.PlaySfx(readyClip);
        if (slot.readyImg != null) slot.readyImg.SetActive(true);
        if (slot.pressPrompt != null) slot.pressPrompt.SetActive(false);

        if (p1.ready && p2.ready) StartCoroutine(ProceedRoutine());
    }

    private IEnumerator ProceedRoutine()
    {
        ReleaseInputs(); // le ready-check est fini

        if (playersContainer != null) playersContainer.SetActive(false); 

        yield return CountdownRoutine();

        onCountdownComplete?.Invoke();
    }

    private IEnumerator CountdownRoutine()
    {
        for (int i = countdownFrom; i > 0; i--)
            yield return PulseNumber(i.ToString(), tickClip);

        countdownLabel.text = goText;
        AudioManager.Instance?.PlaySfx(goClip);

        Coroutine fightIn = StartCoroutine(UIFade.FadeTo(countdownGroup, 1f, countdownFadeDuration));
        Coroutine contentOut = StartCoroutine(UIFade.FadeTo(readyGroup, 0f, fadeDuration));
        yield return fightIn;
        yield return contentOut;

        readyGroup.gameObject.SetActive(false);

        if (fightHold > 0f) yield return new WaitForSecondsRealtime(fightHold);

        yield return UIFade.FadeTo(countdownGroup, 0f, countdownFadeDuration);
    }

    private IEnumerator PulseNumber(string text, AudioClip clip)
    {
        countdownLabel.text = text;
        AudioManager.Instance?.PlaySfx(clip);

        yield return UIFade.FadeTo(countdownGroup, 1f, countdownFadeDuration);
        if (countdownPeakHold > 0f) yield return new WaitForSecondsRealtime(countdownPeakHold);
        yield return UIFade.FadeTo(countdownGroup, 0f, countdownFadeDuration);
    }

    private void ReleaseInputs()
    {
        if (inputs == null) return;

        inputs.Default.PlayerOne.performed -= OnPlayerOnePressed;
        inputs.Default.PlayerTwo.performed -= OnPlayerTwoPressed;
        inputs.Disable();
        inputs.Dispose();
        inputs = null;
    }
}