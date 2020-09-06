namespace MilkShake
{
    /// <summary>
    /// Represents the current state of a shake.
    /// </summary>
    public enum ShakeState
    {
        /// <summary>
        /// The shake is starting / fading in.
        /// </summary>
        FadingIn = 0,

        /// <summary>
        /// The shake has reached its full strength and is now constant.
        /// </summary>
        Sustained = 1,

        /// <summary>
        /// The shake is stopping / fading out.
        /// </summary>
        FadingOut = 2,

        /// <summary>
        /// The shake has fully stopped.
        /// </summary>
        Stopped = 3
    }
}
