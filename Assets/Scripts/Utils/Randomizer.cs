using System;
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
        
        public static T GetRandomEnumValue<T>() where T : Enum
        {
            var enumValues = (T[])Enum.GetValues(typeof(T));
            var randomIndex = _random.Next(0, enumValues.Length);

            return enumValues[randomIndex];
        }

        public static T GetRandomArrayElement<T>(T[] array)
        {
            return array[_random.Next(array.Length)];
        }

        public static float GetRandomInRange(float min, float max)
        {
            return (float)(_random.NextDouble() * (max - min) + min);
        }

        public static bool GetRandomBool()
        {
            return GetRandomBoolWithProbability(50);
        }

        private static bool GetRandomBoolWithProbability(byte probability)
        {
            return _random.Next(1, 100) < probability;
        }
    }
}