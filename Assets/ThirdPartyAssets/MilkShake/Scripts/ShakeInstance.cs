using System;
using UnityEngine;

namespace MilkShake
{
    /// <summary>
    /// A container that holds data for a shake instance.
    /// You can modify shake data here and any Shakers that use this instance will buse the updated data.
    /// </summary>
    [Serializable]
    public class ShakeInstance
    {
        /// <summary>
        /// The original shake parameters.
        /// Note that modifying these parameters will overwrite the original values.
        /// </summary>
        public ShakeParameters ShakeParameters;

        /// <summary>
        /// A scalar value for the shake strength.
        /// You can modify this if you want to adjust the strength of the shake wihout overwriting the original strength.
        /// </summary>
        public float StrengthScale;

        /// <summary>
        /// A scalar value for the roughness.
        /// You can modify this if you want to adjust the roughness of the shake wihout overwriting the original roughness.
        /// </summary>
        public float RoughnessScale;

        /// <summary>
        /// Should this shake instance be removed from its Shaker when it is stopped?
        /// You can set this to false if you want to start a shake again after it has been stopped.
        /// </summary>
        public bool RemoveWhenStopped;

        /// <summary>
        /// The current state of the shake.
        /// </summary>
        public ShakeState State { get; private set; }

        /// <summary>
        /// Is the shake currently paused?
        /// </summary>
        public bool IsPaused { get; private set; }

        /// <summary>
        /// Is the shake completely finished?
        /// Once a shake is finished, any Shakers that use it will remove it from their active shakes.
        /// </summary>
        public bool IsFinished
        {
            get { return State == ShakeState.Stopped && RemoveWhenStopped; }
        }

        /// <summary>
        /// The current strength of the shake, based on the original strength, fading, and StrengthScale.
        /// </summary>
        public float CurrentStrength
        {
            get { return ShakeParameters.Strength * fadeTimer * StrengthScale; }
        }

        /// <summary>
        /// The current roughness of the shake, based on the original roughness, fading, and RoughnessScale.
        /// </summary>
        public float CurrentRoughness
        {
            get { return ShakeParameters.Roughness * fadeTimer * RoughnessScale; }
        }

        private int baseSeed;
        private float seed1;
        private float seed2;
        private float seed3;

        private float noiseTimer;

        private float fadeTimer;
        private float fadeInTime;
        private float fadeOutTime;

        private float pauseTimer;
        private float pauseFadeTime;

        private int lastUpdatedFrame;

        /// <summary>
        /// Creates a new ShakeInstance.
        /// </summary>
        /// <param name="seed">An optional seed to use for the shake. Using the same seed will result in the same shake movement.</param>
        public ShakeInstance(int? seed = null)
        {
            if (!seed.HasValue)
                seed = UnityEngine.Random.Range(-10000, 10000);

            baseSeed = seed.Value;
            seed1 = baseSeed / 2f;
            seed2 = baseSeed / 3f;
            seed3 = baseSeed / 4f;

            noiseTimer = this.baseSeed;
            fadeTimer = 0;
            pauseTimer = 0;

            StrengthScale = 1;
            RoughnessScale = 1;
        }

        /// <summary>
        /// Creates a new ShakeInstance using the given ShakeData.
        /// </summary>
        /// <param name="shakeData">The ShakeData containing the shake strength, roughness, fading, and influences.</param>
        /// <param name="seed">The seed to use for the shake. Using the same seed will result in the same shake movement.</param>
        public ShakeInstance(IShakeParameters shakeData, int? seed = null) : this(seed)
        {
            //Create a copy of the shake data.
            ShakeParameters = new ShakeParameters(shakeData);

            fadeInTime = shakeData.FadeIn;
            fadeOutTime = shakeData.FadeOut;

            State = ShakeState.FadingIn;
        }

        /// <summary>
        /// Updates the shake timers and returns the resulting shake values.
        /// </summary>
        /// <param name="deltaTime">The delta time value to use. Typically this will be Time.deltaTime.</param>
        /// <returns>A ShakeResult containing the position and rotation shake amounts.</returns>
        public ShakeResult UpdateShake(float deltaTime)
        {
            ShakeResult result = new ShakeResult();

            //Get shake values
            result.PositionShake = getPositionShake();
            result.RotationShake = getRotationShake();

            //Update timers
            //Protection for updating timers more than once per frame.
            if (Time.frameCount == lastUpdatedFrame)
                return result;

            //Update pause timer
            if (pauseFadeTime > 0)
            {
                if (IsPaused)
                    pauseTimer += deltaTime / pauseFadeTime;
                else
                    pauseTimer -= deltaTime / pauseFadeTime;
            }
            pauseTimer = Mathf.Clamp01(pauseTimer);

            //Update noise timer
            noiseTimer += (1 - pauseTimer) * deltaTime * CurrentRoughness;

            //Update fade timer
            if (State == ShakeState.FadingIn)
            {
                if (fadeInTime > 0)
                    fadeTimer += deltaTime / fadeInTime;
                else
                    fadeTimer = 1;
            }
            else if (State == ShakeState.FadingOut)
            {
                if (fadeOutTime > 0)
                    fadeTimer -= deltaTime / fadeOutTime;
                else
                    fadeTimer = 0;
            }
            fadeTimer = Mathf.Clamp01(fadeTimer);

            //Update the state if needed
            if (fadeTimer == 1)
            {
                if (ShakeParameters.ShakeType == ShakeType.Sustained)
                    State = ShakeState.Sustained;
                else if (ShakeParameters.ShakeType == ShakeType.OneShot)
                    Stop(ShakeParameters.FadeOut, true);
            }
            else if (fadeTimer == 0)
                State = ShakeState.Stopped;

            lastUpdatedFrame = Time.frameCount;

            return result;
        }

        /// <summary>
        /// Start the shake.
        /// <paramref name="fadeTime">The time in seconds for the shake to fade in. If 0 the shake will be started immediately with no fade in.</paramref>
        /// </summary>
        public void Start(float fadeTime)
        {
            this.fadeInTime = fadeTime;
            State = ShakeState.FadingIn;
        }

        /// <summary>
        /// Stop the shake.
        /// <paramref name="fadeTime">The time in seconds for the shake to fade out. If 0 the shake will be stopped immediately with no fade out.</paramref>
        /// <paramref name="removeWhenStopped">Should the Shakers that use this shake remove it when it is fully stopped? You can set this to false if you want to restart the shake later.</paramref>
        /// </summary>
        public void Stop(float fadeTime, bool removeWhenStopped)
        {
            this.fadeOutTime = fadeTime;
            this.RemoveWhenStopped = removeWhenStopped;
            State = ShakeState.FadingOut;
        }

        /// <summary>
        /// Pause the shake. Unpause with the Resume function.
        /// <paramref name="fadeTime">The time in seconds for the shake to fully pause. If 0 the shake will be paused immediately with no fade out.</paramref>
        /// </summary>
        public void Pause(float fadeTime)
        {
            IsPaused = true;
            pauseFadeTime = fadeTime;
            if (fadeTime <= 0)
                pauseTimer = 1;
        }

        /// <summary>
        /// Resume a paused shake.
        /// <paramref name="fadeTime">The time in seconds for the shake to fully resume. If 0 the shake will be resumed immediately with no fade in.</paramref>
        /// </summary>
        public void Resume(float fadeTime)
        {
            IsPaused = false;
            pauseFadeTime = fadeTime;

            if (fadeTime <= 0)
                pauseTimer = 0;
        }

        /// <summary>
        /// If the shake is paused, resume it.
        /// If the shake is not paused, pause it.
        /// </summary>
        public void TogglePaused(float fadeTime)
        {
            if (IsPaused)
                Resume(fadeTime);
            else
                Pause(fadeTime);
        }

        private Vector3 getPositionShake()
        {
            Vector3 v = Vector3.zero;

            v.x = getNoise(noiseTimer + seed1, baseSeed);
            v.y = getNoise(baseSeed, noiseTimer);
            v.z = getNoise(seed3 + noiseTimer, baseSeed + noiseTimer);

            return Vector3.Scale(v * CurrentStrength, ShakeParameters.PositionInfluence);
        }

        private Vector3 getRotationShake()
        {
            Vector3 v = Vector3.zero;

            v.x = getNoise(noiseTimer - baseSeed, seed3);
            v.y = getNoise(baseSeed, noiseTimer + seed2);
            v.z = getNoise(baseSeed + noiseTimer, seed1 + noiseTimer);

            return Vector3.Scale(v * CurrentStrength, ShakeParameters.RotationInfluence);
        }

        private float getNoise(float x, float y)
        {
            return (Mathf.PerlinNoise(x, y) - 0.5f) * 2f;
        }
    }
}
