using UnityEngine;

namespace MilkShake
{
    /// <summary>
    /// Interface for shake parameters.
    /// </summary>
    public interface IShakeParameters
    {
        /// <summary>
        /// The type of shake (One-Shot or Sustained)
        /// </summary>
        ShakeType ShakeType { get; set; }

        /// <summary>
        /// The intensity / magnitude of the shake.
        /// </summary>
        float Strength { get; set; }
        /// <summary>
        /// The roughness of the shake.
        /// Lower values are slower and smoother, higher values are faster and noisier.
        /// </summary>
        float Roughness { get; set; }

        /// <summary>
        /// The time, in seconds, for the shake to fade in.
        /// </summary>
        float FadeIn { get; set; }
        /// <summary>
        /// The time, in seconds, for the shake to fade out.
        /// </summary>
        float FadeOut { get; set; }

        /// <summary>
        /// How much influence the shake has over the camera's position.
        /// All values are valid, even numbers greater than 1 and negative numbers.
        /// </summary>
        Vector3 PositionInfluence { get; set; }
        /// <summary>
        /// How much influence the shake has over the camera's rotation.
        /// All values are valid, even numbers greater than 1 and negative numbers.
        /// </summary>
        Vector3 RotationInfluence { get; set; }
    }
}
