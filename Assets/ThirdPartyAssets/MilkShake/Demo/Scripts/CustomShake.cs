using UnityEngine;
using UnityEngine.UI;

namespace MilkShake.Demo
{
    public class CustomShake : MonoBehaviour
    {
        [SerializeField]
        private Slider strengthSlider;
        [SerializeField]
        private Text strengthText;
        [SerializeField]
        private Slider roughnessSlider;
        [SerializeField]
        private Text roughnessText;
        [SerializeField]
        private Slider fadeInSlider;
        [SerializeField]
        private Text fadeInText;
        [SerializeField]
        private Slider fadeOutSlider;
        [SerializeField]
        private Text fadeOutText;
        [SerializeField]
        private GameObject sustainedShake;
        [SerializeField]
        private GameObject oneShotShake;

        private ShakeInstance shakeInstance;

        private ShakeParameters shakeData;
        private int seed;

        private void Awake()
        {
            shakeData = new ShakeParameters();
            shakeData.Strength = 0.5f;
            shakeData.Roughness = 5;
            shakeData.FadeIn = 0.1f;
            shakeData.FadeOut = 1f;
            shakeData.PositionInfluence = Vector3.one;
            shakeData.RotationInfluence = Vector3.one * 10;

            strengthSlider.value = shakeData.Strength;
            roughnessSlider.value = shakeData.Roughness;
            fadeInSlider.value = shakeData.FadeIn;
            fadeOutSlider.value = shakeData.FadeOut;

            UISetShakeType(0);
        }

        public void UIShakeOnce()
        {
            Shaker.ShakeAllSeparate(shakeData, null, seed);
        }

        public void UIToggleShake(bool enabled)
        {
            if (shakeInstance == null)
                shakeInstance = Shaker.ShakeAll(shakeData);
            else
            {
                shakeInstance.Stop(shakeData.FadeOut, true);
                shakeInstance = null;
            }
        }

        public void UITogglePause(bool enabled)
        {
            if (shakeInstance != null)
                shakeInstance.TogglePaused(0.1f);
        }

        public void UISetStrength(float value)
        {
            shakeData.Strength = value;
            strengthText.text = value.ToString("0.##");

            if (shakeInstance != null)
                shakeInstance.ShakeParameters.Strength = value;
        }

        public void UISetRoughness(float value)
        {
            shakeData.Roughness = value;
            roughnessText.text = value.ToString("0.##");

            if (shakeInstance != null)
                shakeInstance.ShakeParameters.Roughness = value;
        }

        public void UISetFadeIn(float value)
        {
            shakeData.FadeIn = value;
            fadeInText.text = value.ToString("0.##");
        }

        public void UISetFadeOut(float value)
        {
            shakeData.FadeOut = value;
            fadeOutText.text = value.ToString("0.##");
        }

        public void UISetSeed(string value)
        {
            int v;
            if (int.TryParse(value, out v))
                seed = v;
        }

        public void UISetShakeType(int value)
        {
            shakeData.ShakeType = (ShakeType)value;

            sustainedShake.SetActive(shakeData.ShakeType == ShakeType.Sustained);
            oneShotShake.SetActive(shakeData.ShakeType == ShakeType.OneShot);
        }

        public void UISetPositionInfluenceX(string value)
        {
            float v;
            if (float.TryParse(value, out v))
                shakeData.PositionInfluence = new Vector3(v, shakeData.PositionInfluence.y, shakeData.PositionInfluence.z);
        }

        public void UISetPositionInfluenceY(string value)
        {
            float v;
            if (float.TryParse(value, out v))
                shakeData.PositionInfluence = new Vector3(shakeData.PositionInfluence.x, v, shakeData.PositionInfluence.z);
        }

        public void UISetPositionInfluenceZ(string value)
        {
            float v;
            if (float.TryParse(value, out v))
                shakeData.PositionInfluence = new Vector3(shakeData.PositionInfluence.x, shakeData.PositionInfluence.y, v);
        }

        public void UISetRotationInfluenceX(string value)
        {
            float v;
            if (float.TryParse(value, out v))
                shakeData.RotationInfluence = new Vector3(v, shakeData.RotationInfluence.y, shakeData.RotationInfluence.z);
        }

        public void UISetRotationInfluenceY(string value)
        {
            float v;
            if (float.TryParse(value, out v))
                shakeData.RotationInfluence = new Vector3(shakeData.RotationInfluence.x, v, shakeData.RotationInfluence.z);
        }

        public void UISetRotationInfluenceZ(string value)
        {
            float v;
            if (float.TryParse(value, out v))
                shakeData.RotationInfluence = new Vector3(shakeData.RotationInfluence.x, shakeData.RotationInfluence.y, v);
        }
    }
}

