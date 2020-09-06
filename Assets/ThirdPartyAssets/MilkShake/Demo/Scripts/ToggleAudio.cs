using UnityEngine;

namespace MilkShake.Demo
{
    public class ToggleAudio : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;

        public void UIToggle(bool enabled)
        {
            if (enabled)
                audioSource.Play();
            else
                audioSource.Stop();
        }
    }
}

