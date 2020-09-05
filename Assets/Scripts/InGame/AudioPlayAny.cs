using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayAny : MonoBehaviour
{
    public AudioClip[] audioClips;

    void Start()
    {
        int random = Random.Range(0, audioClips.Length);
        AudioClip selectedAudioClip = audioClips[random];
        GetComponent<AudioSource>().PlayOneShot(selectedAudioClip);
    }
}
