using System;

namespace Utils
{
    public static class Randomizer
    {
        private static readonly Random Random;

        static Randomizer()
        {
            Random = new Random();
        }

        public static T GetRandomEnumValue<T>() where T : Enum
        {
            var enumValues = (T[])Enum.GetValues(typeof(T));
            var randomIndex = Random.Next(0, enumValues.Length);

            return enumValues[randomIndex];
        }

        public static T GetRandomArrayElement<T>(T[] array)
        {
            return array[Random.Next(array.Length)];
        }

        public static float GetRandomInRange(float min, float max)
        {
            return (float)(Random.NextDouble() * (max - min) + min);
        }

        public static bool GetRandomBool()
        {
            return GetRandomBoolWithProbability(50);
        }

        private static bool GetRandomBoolWithProbability(byte probability)
        {
            return Random.Next(1, 100) < probability;
        }
    }
}