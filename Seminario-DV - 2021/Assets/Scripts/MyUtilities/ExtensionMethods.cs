using UnityEngine;

namespace MyUtilities
{
    public static class ExtensionMethods
    {
        public static bool IsAtRange(this Vector3 me, Vector3 other, float range)
        {
            return Vector3.Distance(me, other) <= range;
        }
    }
}