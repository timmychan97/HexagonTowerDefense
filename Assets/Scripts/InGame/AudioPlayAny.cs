using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayAny : MonoBehaviour
{
    public AudioClip[] audioClips;
    // Start is called before the first frame update
    void Start()
    {
        int random = Random.Range(0, audioClips.Length);
        AudioClip selectedAudioClip = audioClips[random];
        GetComponent<AudioSource>().PlayOneShot(selectedAudioClip);
    }
}
