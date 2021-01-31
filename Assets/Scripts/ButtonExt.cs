using UnityEngine;
using UnityEngine.UI;

public class ButtonExt : Button
{
    private AudioSource _audioSource;
    bool _canPlayHighlighted = true;
    bool _canPlayPressed = true;

    private void Update()
    {
        if (IsHighlighted() && _canPlayHighlighted)
        {
            _canPlayHighlighted = false;
            CheckAudioSource();
            _audioSource.clip = AudioManager.Instance.ButtonSwitchSoundClip;
            _audioSource.Play();
        }
        else if (!IsHighlighted())
        {
            _canPlayHighlighted = true;
        }

        if (IsPressed() && _canPlayPressed)
        {
            _canPlayPressed = false;
            CheckAudioSource();
            _audioSource.clip = AudioManager.Instance.ButtonClick1;
            _audioSource.Play();
        }
        else if (!IsPressed())
        {
            _canPlayPressed = true;
        }

    }

    private void CheckAudioSource()
    {
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
}