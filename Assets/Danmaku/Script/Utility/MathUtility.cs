using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility {
    public class MathUtility
    {
        public static float VectorToAngle(Vector2 p_vector) {
            float angle = (Mathf.Atan2(p_vector.y, p_vector.x) * 180 / Mathf.PI);

            return angle;
        }
    }
}
