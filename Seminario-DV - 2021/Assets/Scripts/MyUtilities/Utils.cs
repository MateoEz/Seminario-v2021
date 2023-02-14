using UnityEngine;

namespace MyUtilities
{
    public static class Utils
    {
        public static Vector3 GetDirIgnoringHeight(Vector3 from, Vector3 to)
        {
            var fromTo = to - from;
            fromTo = new Vector3(fromTo.x, 0, fromTo.z);
            return Vector3.Normalize(fromTo);
        }
        
        public static Vector3 GetVectorIgnoringHeight(Vector3 from, Vector3 to)
        {
            var fromTo = to - from;
            fromTo = new Vector3(fromTo.x, 0, fromTo.z);
            return fromTo;
        }
    }
}