using UnityEngine;

namespace MilkShake.Demo
{
    public class ShakeClicker : MonoBehaviour
    {
        [SerializeField]
        private Shaker shakeManager;
        [SerializeField]
        private ShakeParameters idleShake;
        [SerializeField]
        private ShakeParameters clickedShake;

        private void Awake()
        {
            shakeManager.Shake(idleShake);
        }

        private void OnMouseDown()
        {
            shakeManager.Shake(clickedShake);
        }
    }
}