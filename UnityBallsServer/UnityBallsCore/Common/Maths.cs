using System;

namespace UnityBallsCore.Common
{
    public static class Maths
    {
        public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T> =>
            value.CompareTo(min) < 0 ? min : value.CompareTo(max) > 0 ? max : value;
    }
}
