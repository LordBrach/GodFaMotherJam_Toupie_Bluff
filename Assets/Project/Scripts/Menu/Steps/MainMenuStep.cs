using UnityEngine;
using UnityEngine.UI;

public class MainMenuStep : MonoBehaviour
{
    [Header("Boutons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button IIMButton;
    [SerializeField] private Button ItchButton;

    [Header("Links")]
    [SerializeField] private string IIMLink = "";
    [SerializeField] private string ItchURL = "";

    [Header("Overlay")]
    [SerializeField] private SettingsPanel settingsPanel;

    [Header("Sortie")]
    [SerializeField] private string gameSceneName = "Game";

    private void Awake()
    {
        if (playButton != null) playButton.onClick.AddListener(Play);
        if (settingsButton != null) settingsButton.onClick.AddListener(OpenSettings);
        if (quitButton != null) quitButton.onClick.AddListener(Quit);
        if (IIMButton != null) IIMButton.onClick.AddListener(OpenIIMLink);
        if (ItchButton != null) ItchButton.onClick.AddListener(OpenItchLink);
    }

    private void OnDestroy()
    {
        if (playButton != null) playButton.onClick.RemoveListener(Play);
        if (settingsButton != null) settingsButton.onClick.RemoveListener(OpenSettings);
        if (quitButton != null) quitButton.onClick.RemoveListener(Quit);
        if (IIMButton != null) IIMButton.onClick.RemoveListener(OpenIIMLink);
        if (ItchButton != null) ItchButton.onClick.RemoveListener(OpenItchLink);
    }

    private void Play() => ScreenFader.GoToScene(gameSceneName);

    private void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.Open();
    }

    private void OpenIIMLink()
    {
        if (!string.IsNullOrEmpty(IIMLink))
        {
            Application.OpenURL(IIMLink);
        }
    }

    private void OpenItchLink()
    {
        if (!string.IsNullOrEmpty(ItchURL))
        {
            Application.OpenURL(ItchURL);
        }
    }

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}