namespace BL.Abstracts
{
    /// <summary>
    /// the creator of the Bl according to the singleton
    /// </summary>
    public static class BlFactory
    {
        /// <summary>
        /// the function returns an instance of bl.
        /// </summary>
        /// <returns></returns>
        public static IBl GetBl()
        {
            return Bl.GetInstance();
        }
    }
}
