using System.Collections.Generic;
using UnityEngine;

namespace MilkShake
{
    /// <summary>
    /// Component that handles shaking of a Transform.
    /// </summary>
    [AddComponentMenu("MilkShake/Shaker")]
    public class Shaker : MonoBehaviour
    {
        /// <summary>
        /// A static list of Shakers.
        /// Shakers are added to this list on Awake if Add To Global Shakers is set to true.
        /// </summary>
        public static List<Shaker> GlobalShakers = new List<Shaker>();

        /// <summary>
        /// Shakes all global Shakers.
        /// Note that all shakers will get the same ShakeInstance, and thus will have the same shake movement.
        /// </summary>
        /// <param name="shakeData">The shake parameters such as strength, roughness and fade in/out times.</param>
        /// <param name="seed">An optional seed to use for the shake. Shakes that have the same seed will have the same movement.</param>
        /// <returns>A single ShakeInstance that all global shakers will be using for the shake. Modifying this will change it for all shakers.</returns>
        public static ShakeInstance ShakeAll(IShakeParameters shakeData, int? seed = null)
        {
            ShakeInstance shakeInstance = new ShakeInstance(shakeData, seed);
            AddShakeAll(shakeInstance);
            return shakeInstance;
        }

        /// <summary>
        /// Shakes all global Shakers, using a separate ShakeInstance for each Shaker.
        /// </summary>
        /// <param name="shakeData">The shake parameters such as strength, roughness and fade in/out times.</param>
        /// <param name="shakeInstances">An optional list that will be populated with all of the created ShakeInstances.</param>
        /// <param name="seed">An optional seed to use for the shake. Shakes that have the same seed will have the same movement.</param>
        /// <returns>A single ShakeInstance that all global shakers will be using for the shake. Modifying this will change it for all shakers.</returns>
        public static void ShakeAllSeparate(IShakeParameters shakeData, List<ShakeInstance> shakeInstances = null, int? seed = null)
        {
            if (shakeInstances != null)
                shakeInstances.Clear();

            for (int i = 0; i < GlobalShakers.Count; i++)
            {
                if (!GlobalShakers[i].gameObject.activeInHierarchy)
                    continue;

                ShakeInstance shakeInstance = GlobalShakers[i].Shake(shakeData, seed);

                if (shakeInstances != null && shakeInstance != null)
                    shakeInstances.Add(shakeInstance);
            }
        }

        /// <summary>
        /// Starts a shake for all Global Shakers, using a position and distance to scale the strength of the shake.
        /// If the distance between the shaker and the point is greater than the max distance, no shake will be applied.
        /// </summary>
        /// <param name="point">The poisition that the shake originated from.</param>
        /// <param name="maxDistance">The maximum distance the shake should have an affect.</param>
        /// <param name="shakeData">The shake parameters such as strength and roughness and fade in/out times.</param>
        /// <param name="shakeInstances">An optional list that will be populated with all of the created ShakeInstances.</param>
        /// <param name="seed">A optional seed to use for the shake. Shakes that have the same seed will have the same movement.</param>
        /// If the distance between this Shaker and the point is greater than the max distance, null will be returned.</returns>
        public static void ShakeAllFromPoint(Vector3 point, float maxDistance, IShakeParameters shakeData, List<ShakeInstance> shakeInstances = null, int? seed = null)
        {
            if (shakeInstances != null)
                shakeInstances.Clear();

            for (int i = 0; i < GlobalShakers.Count; i++)
            {
                if (!GlobalShakers[i].gameObject.activeInHierarchy)
                    continue;

                ShakeInstance shakeInstance = GlobalShakers[i].ShakeFromPoint(point, maxDistance, shakeData, seed);

                if (shakeInstances != null && shakeInstance != null)
                    shakeInstances.Add(shakeInstance);
            }
        }

        /// <summary>
        /// Adds an already existing ShakeInstance to all global shakers.
        /// </summary>
        /// <param name="shakeInstance">The ShakeInstance to add.</param>
        public static void AddShakeAll(ShakeInstance shakeInstance)
        {
            for (int i = 0; i < GlobalShakers.Count; i++)
            {
                if (!GlobalShakers[i].gameObject.activeInHierarchy)
                    continue;

                GlobalShakers[i].AddShake(shakeInstance);
            }
        }

        [SerializeField]
        private bool addToGlobalShakers;

        private List<ShakeInstance> activeShakes = new List<ShakeInstance>();

        private void Awake()
        {
            if (addToGlobalShakers)
            {
                GlobalShakers.Add(this);
            }
        }

        private void OnDestroy()
        {
            if (addToGlobalShakers)
            {
                GlobalShakers.Remove(this);
            }
        }

        private void Update()
        {
            ShakeResult shake = new ShakeResult();

            for (int i = 0; i < activeShakes.Count; i++)
            {
                if (activeShakes[i].IsFinished)
                {
                    activeShakes.RemoveAt(i);
                    i--;
                    continue;
                }

                shake += activeShakes[i].UpdateShake(Time.deltaTime);
            }

            transform.localPosition = shake.PositionShake;
            transform.localEulerAngles = shake.RotationShake;
        }

        /// <summary>
        /// Starts a shake.
        /// </summary>
        /// <param name="shakeData">The shake parameters such as strength and roughness and fade in/out times.</param>
        /// <param name="seed">An optional seed to use for the shake. Shakes that have the same seed will have the same movement.</param>
        /// <returns>A ShakeInstance which can be used to stop the shake or modify the shake parameters.</returns>
        public ShakeInstance Shake(IShakeParameters shakeData, int? seed = null)
        {
            ShakeInstance shakeInstance = new ShakeInstance(shakeData, seed);
            AddShake(shakeInstance);
            return shakeInstance;
        }

        /// <summary>
        /// Starts a shake, using a position and distance to scale the strength of the shake.
        /// If the distance between this Shaker and the point is greater than the max distance, no shake will be applied.
        /// </summary>
        /// <param name="point">The poisition that the shake originated from.</param>
        /// <param name="maxDistance">The maximum distance the shake should have an affect.</param>
        /// <param name="shakeData">The shake parameters such as strength and roughness and fade in/out times.</param>
        /// <param name="seed">A optional seed to use for the shake. Shakes that have the same seed will have the same movement.</param>
        /// <returns>A ShakeInstance which can be used to stop the shake or modify the shake parameters. 
        /// If the distance between this Shaker and the point is greater than the max distance, null will be returned.</returns>
        public ShakeInstance ShakeFromPoint(Vector3 point, float maxDistance, IShakeParameters shakeData, int? seed = null)
        {
            float distance = Vector3.Distance(transform.position, point);

            if (distance < maxDistance)
            {
                ShakeInstance shakeInstance = new ShakeInstance(shakeData, seed);
                float scale = 1 - Mathf.Clamp01(distance / maxDistance);
                shakeInstance.StrengthScale = scale;
                shakeInstance.RoughnessScale = scale;
                AddShake(shakeInstance);
                return shakeInstance;
            }

            return null;
        }

        /// <summary>
        /// Adds an already existing ShakeInstance to this Shaker's active shakes.
        /// </summary>
        /// <param name="shakeInstance">The ShakeInstance to add.</param>
        public void AddShake(ShakeInstance shakeInstance)
        {
            activeShakes.Add(shakeInstance);
        }
    }
}