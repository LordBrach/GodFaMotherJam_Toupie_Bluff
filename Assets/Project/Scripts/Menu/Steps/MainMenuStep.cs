using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Écran titre : Jouer / Options / Quitter.
/// "Options" ouvre l'OptionsPanel (overlay indépendant) sans faire avancer le flow du menu.
/// </summary>
/// 
public class MainMenuStep : MenuStep
{
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [Header("Overlay")]
    [SerializeField] private SettingsPanel settingsPanel;

    private void Awake()
    {
        if (playButton != null) playButton.onClick.AddListener(CompleteStep);
        if (settingsButton != null) settingsButton.onClick.AddListener(OpenSettings);
        if (quitButton != null) quitButton.onClick.AddListener(Quit);
    }

    private void OnDestroy()
    {
        if (playButton != null) playButton.onClick.RemoveListener(CompleteStep);
        if (settingsButton != null) settingsButton.onClick.RemoveListener(OpenSettings);
        if (quitButton != null) quitButton.onClick.RemoveListener(Quit);
    }

    private void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.Open();
    }

    private void Quit()
    {
        Application.Quit();
    }
}