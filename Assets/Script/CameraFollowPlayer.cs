using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private GameObject Player;
    private Vector3 offset = new Vector3(17.39f, 24.48f, -14.59f);

    void Start()
    {
        Player = GameObject.Find("Player");
    }

    void Update()
    {
        transform.position = Player.transform.position + offset ;
    }
}
