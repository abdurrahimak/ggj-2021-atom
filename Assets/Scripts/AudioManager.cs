using CoreProject.Singleton;
using UnityEngine;

public class AudioManager : SingletonComponent<AudioManager>
{
    [Header("Click Sounds")]
    public AudioClip ButtonSwitchSoundClip;
    public AudioClip ButtonClick1;
    public AudioClip ButtonClick2;

    [Header("Ship")]
    public AudioClip ShipMovement;
    public AudioClip ShipTeleport;

    [Header("Player")]
    public AudioClip PlayerMovementSound;

    [Header("Collect")]
    public AudioClip CollectSound;

    [Header("End Game")]
    public AudioClip WinSound;

    [Header("Looping")]
    public AudioSource LoopingAudioSource;
    public AudioClip MenuLoopingSound;

    [Header("Temp")]
    public AudioSource TempAudioSource;

    private bool _isActive = true;
    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
            Camera.main.GetComponent<AudioListener>().enabled = _isActive;
        }
    }

    private void Start()
    {
        LoopingAudioSource.loop = true;
        LoopingAudioSource.clip = MenuLoopingSound;
        LoopingAudioSource.Play();
    }

    public void PlayWinSound()
    {
        TempAudioSource.clip = WinSound;
        TempAudioSource.Play();
    }
}
