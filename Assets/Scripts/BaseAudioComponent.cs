using System;
using UnityEngine;

public class BaseAudioComponent : MonoBehaviour
{
    protected AudioSource AudioSource;

    internal void InitializeAudioComp()
    {
        AudioSource = new GameObject("_AuiodSource", typeof(AudioSource)).GetComponent<AudioSource>();
        AudioSource.playOnAwake = false;
        AudioSource.transform.SetParent(transform);
    }
}
