using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryPanel : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private CanvasGroup group;
    [SerializeField, Min(0f)] private float fadeDuration = 0.25f;

    [Header("Texte")]
    [SerializeField] private TMP_Text winnerLabel;
    [SerializeField] private string playerOneWinText = "Player 1 win !";
    [SerializeField] private string playerTwoWinText = "Player 2 win !";

    [Header("Boutons")]
    [SerializeField] private Button replayButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        if (group == null) group = GetComponent<CanvasGroup>();

        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;
        gameObject.SetActive(false);

        if (replayButton != null) replayButton.onClick.AddListener(Replay);
        if (quitButton != null) quitButton.onClick.AddListener(Quit);
    }

    private void OnDestroy()
    {
        if (replayButton != null) replayButton.onClick.RemoveListener(Replay);
        if (quitButton != null) quitButton.onClick.RemoveListener(Quit);
    }

    public void ShowWinner(Player winner)
    {
        if (winnerLabel != null)
            winnerLabel.text = winner == Player.PlayerOne ? playerOneWinText : playerTwoWinText;

        gameObject.SetActive(true);
        group.interactable = true;
        group.blocksRaycasts = true;
        StartCoroutine(UIFade.FadeTo(group, 1f, fadeDuration));
    }

    private void Replay() => ScreenFader.GoToScene(SceneManager.GetActiveScene().name);

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}