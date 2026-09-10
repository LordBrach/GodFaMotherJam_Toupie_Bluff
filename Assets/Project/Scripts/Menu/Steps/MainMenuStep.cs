using UnityEngine;
using UnityEngine.UI;

public class MainMenuStep : MonoBehaviour
{
    [Header("Boutons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [Header("Overlay")]
    [SerializeField] private SettingsPanel settingsPanel;

    [Header("Sortie")]
    [SerializeField] private string gameSceneName = "Game";

    private void Awake()
    {
        if (playButton != null) playButton.onClick.AddListener(Play);
        if (settingsButton != null) settingsButton.onClick.AddListener(OpenSettings);
        if (quitButton != null) quitButton.onClick.AddListener(Quit);
    }

    private void OnDestroy()
    {
        if (playButton != null) playButton.onClick.RemoveListener(Play);
        if (settingsButton != null) settingsButton.onClick.RemoveListener(OpenSettings);
        if (quitButton != null) quitButton.onClick.RemoveListener(Quit);
    }

    private void Play() => ScreenFader.GoToScene(gameSceneName);

    private void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.Open();
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