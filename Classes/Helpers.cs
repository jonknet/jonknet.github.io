namespace TreeBuilder.Classes {
    public static class Helpers {
        /// <summary>
        ///     Helper method to determine class type
        /// </summary>
        /// <param name="Base">Class you want to check</param>
        /// <typeparam name="T">Type to check for</typeparam>
        /// <returns></returns>
        public static bool Is<T>(this BaseClass Base) {
            return Base is T;
        }
    }
}