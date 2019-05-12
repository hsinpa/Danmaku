using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertiesUtility : MonoBehaviour
{

    public static float time;
    public static float deltaTime;

    // Update is called once per frame
    void Update()
    {
        time = Time.time;
        deltaTime = Time.deltaTime;
    }

}
