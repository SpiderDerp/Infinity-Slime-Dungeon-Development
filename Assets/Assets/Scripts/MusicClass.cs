using UnityEngine;

public class MusicClass : MonoBehaviour
{
    public static MusicClass Instance { get; private set; }
    private AudioSource _audioSource;
    
    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy this GameObject
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        // Set this as the instance and make it persist across scenes
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}