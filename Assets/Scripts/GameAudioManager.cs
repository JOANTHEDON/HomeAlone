using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    public static GameAudioManager Instance{get; private set;}
    [SerializeField]private AudioSource _gameAudioSource;
    [SerializeField]private AudioClip _musicAudioClip;
    
    private bool _isMuted = false;

    public bool IsMuted => _isMuted;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        if (_gameAudioSource == null) 
        {
            _gameAudioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayMusic()
    {
        // Add the !isPlaying check here:
        if(_gameAudioSource != null && !_gameAudioSource.isPlaying)
        {
            _gameAudioSource.clip = _musicAudioClip;
            //_gameAudioSource.volume = 0.3f;
            _gameAudioSource.Play();
        }
    }

    
    public void ToggleMusic()
    {
        _isMuted = !_isMuted;

        if(_gameAudioSource != null)
        {
            _gameAudioSource.mute = _isMuted;
        }
    }
}
