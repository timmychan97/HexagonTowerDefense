using UnityEngine;

namespace MilkShake
{
    /// <summary>
    /// A data structure for holding shake position and rotation.
    /// </summary>
    public struct ShakeResult
    {
        /// <summary>
        /// The amount of position shake.
        /// </summary>
        public Vector3 PositionShake;
        /// <summary>
        /// The amount of rotation shake.
        /// </summary>
        public Vector3 RotationShake;

        public static ShakeResult operator +(ShakeResult a, ShakeResult b)
        {
            ShakeResult c = new ShakeResult();

            c.PositionShake = a.PositionShake + b.PositionShake;
            c.RotationShake = a.RotationShake + b.RotationShake;

            return c;
        }
    }
}
