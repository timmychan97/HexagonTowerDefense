using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MilkShake.Demo
{
    public class ShakeButton : MonoBehaviour
    {
        [SerializeField]
        private ShakePreset shakePreset;
        [SerializeField]
        private Transform fromPosition;
        [SerializeField]
        private float fromPositionDistance;

        private ShakeInstance shakeInstance;

        public void UIShakeOnce()
        {
            if (fromPosition != null)
                Shaker.ShakeAllFromPoint(fromPosition.position, fromPositionDistance, shakePreset);
            else
                Shaker.ShakeAllSeparate(shakePreset);
        }

        public void UIToggleShake(bool enabled)
        {
            if (shakeInstance == null)
                shakeInstance = Shaker.ShakeAll(shakePreset);
            else
            {
                shakeInstance.Stop(shakePreset.FadeOut, true);
                shakeInstance = null;
            }
        }

        public void UITogglePause(bool enabled)
        {
            if (shakeInstance != null)
                shakeInstance.TogglePaused(0.1f);
        }
    }
}