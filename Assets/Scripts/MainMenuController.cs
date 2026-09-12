using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject _settingsPanel;
    public GameObject _settingsButton;
    public GameObject _playGameButton;
    private GameAudioManager _gameAudioManager;
    //private bool _isMuted = false;
    [SerializeField]private Sprite _volumeOn;
    [SerializeField]private Sprite _volumeOff;
    [SerializeField]private UnityEngine.UI.Image _volumeButtonImage;
    public void Awake()
    {
        if(_settingsPanel != null) _settingsPanel.SetActive(false);
        if(_settingsButton != null) _settingsButton.SetActive(true);
        if(_playGameButton != null) _playGameButton.SetActive(true);

        _gameAudioManager = FindAnyObjectByType<GameAudioManager>();
    }
    public void Start()
    {
        if (_gameAudioManager != null) _gameAudioManager.PlayMusic();
        if (_volumeButtonImage != null) _volumeButtonImage.sprite = _volumeOn;
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

        public void ToggleMusicUI()
    {
        if (_gameAudioManager == null) return;

        // 1. Toggle the music first
        _gameAudioManager.ToggleMusic();

        // 2. Then update the sprite based on the NEW state
        if (_gameAudioManager.IsMuted)
        {
            if (_volumeButtonImage != null) _volumeButtonImage.sprite = _volumeOff;
        }
        else
        {
            if (_volumeButtonImage != null) _volumeButtonImage.sprite = _volumeOn;
        }
    }

    
}
