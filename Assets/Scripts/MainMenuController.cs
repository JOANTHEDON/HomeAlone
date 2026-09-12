using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject _settingsPanel;
    public GameObject _settingsButton;
    public GameObject _playGameButton;
    public void Awake()
    {
        if(_settingsPanel == null) return;
        _settingsPanel.SetActive(false);
        if(_settingsButton ==null)return;
        _settingsButton.SetActive(true);
        if(_playGameButton == null) return;
        _playGameButton.SetActive(true);
    }

    public void PlayGameButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }

    public void OpenSettingsPanel()
    {
        _settingsPanel.SetActive(true);
        _settingsButton.SetActive(false);
        _playGameButton.SetActive(false);
    }

    public void CloseSettingsPanel()
    {
        _settingsPanel.SetActive(false);
        _settingsButton.SetActive(true);
        _playGameButton.SetActive(true);
    }

    
}
