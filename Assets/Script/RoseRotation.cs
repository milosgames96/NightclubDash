using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoseRotation : MonoBehaviour
{
    void Update()
    {
        gameObject.transform.Rotate(0f, 0.3f, 0f, Space.World);
    }
}
