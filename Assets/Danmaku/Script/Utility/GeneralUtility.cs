using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility {

    public static class ArrayExtensions
    {
        public static void Fill<T>(this T[] originalArray, T with)
        {
            for (int i = 0; i < originalArray.Length; i++)
            {
                originalArray[i] = with;
            }
        }
    }

    public class GeneralUtility
    {

        public static IEnumerator DoDelayWork(float p_delay, System.Action p_action) {
            yield return new WaitForSeconds(p_delay);

            if (p_action != null)
                p_action();
        }


    }
}
