using Random = System.Random;

namespace Utils
{
    public class Randomizer
    {
        private static Random _random;
        
        static Randomizer()
        {
            _random = new Random();
        }

        public static float GetRandomInRange(float min, float max)
        {
            return (float)(_random.NextDouble() * (max - min) + min);
        }
    }
}