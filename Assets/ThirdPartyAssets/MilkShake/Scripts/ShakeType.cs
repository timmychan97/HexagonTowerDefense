namespace MilkShake
{
    /// <summary>
    /// Defines the behavior of a shake.
    /// </summary>
    public enum ShakeType
    {
        /// <summary>
        /// Will fade in and fade out automatically.
        /// </summary>
        OneShot,

        /// <summary>
        /// Will fade in automatically, but will continue to shake and will not fade out until told to stop.
        /// </summary>
        Sustained
    }
}
