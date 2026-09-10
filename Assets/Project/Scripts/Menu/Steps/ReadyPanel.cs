using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CanvasGroup))]
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

    [Header("Panel (fade out une fois les 2 joueurs prêts)")]
    [SerializeField] private CanvasGroup readyGroup;
    [SerializeField, Min(0f)] private float fadeDuration = 0.25f;

    [Header("Joueurs")]
    [SerializeField] private PlayerSlot p1 = new PlayerSlot { displayName = "Joueur 1" };
    [SerializeField] private PlayerSlot p2 = new PlayerSlot { displayName = "Joueur 2" };

    [Header("Countdown")]
    [SerializeField] private TMP_Text countdownLabel;
    [SerializeField, Min(1)] private int countdownFrom = 3;
    [SerializeField, Min(0.1f)] private float countdownStep = 1f;
    [SerializeField] private string goText = "Fight !";

    [Header("Sons")]
    [SerializeField] private AudioClip readyClip;
    [SerializeField] private AudioClip tickClip;
    [SerializeField] private AudioClip goClip;

    [Header("Sortie (add ici Gameloop.StartGame dans l'inspecteur)")]
    [SerializeField] private UnityEvent onCountdownComplete;

    private PlayerInput inputs;

    private void Reset() => readyGroup = GetComponent<CanvasGroup>();

    private void Awake()
    {
        if (readyGroup == null) readyGroup = GetComponent<CanvasGroup>();

        Setup(p1);
        Setup(p2);

        if (countdownLabel != null) countdownLabel.gameObject.SetActive(false);

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

        yield return UIFade.FadeTo(readyGroup, 0f, fadeDuration);
        readyGroup.gameObject.SetActive(false);

        yield return CountdownRoutine();

        onCountdownComplete?.Invoke();
    }

    private IEnumerator CountdownRoutine()
    {
        if (countdownLabel != null) countdownLabel.gameObject.SetActive(true);

        for (int i = countdownFrom; i > 0; i--)
        {
            if (countdownLabel != null) countdownLabel.text = i.ToString();
            AudioManager.Instance?.PlaySfx(tickClip);
            yield return new WaitForSecondsRealtime(countdownStep);
        }

        if (countdownLabel != null) countdownLabel.text = goText;
        AudioManager.Instance?.PlaySfx(goClip);
        yield return new WaitForSecondsRealtime(countdownStep * 0.5f);

        if (countdownLabel != null) countdownLabel.gameObject.SetActive(false);
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