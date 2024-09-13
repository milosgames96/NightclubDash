using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToPlayer : MonoBehaviour
{
    private GameObject Player;
    private readonly float turnDistance = 7;

    void Start()
    {
        Player = GameObject.Find("Player"); 
    }

    void Update()
    {
        if (Vector3.Distance(transform.position,Player.transform.position) < turnDistance)
        {
           transform.LookAt(Player.transform);
        }
    }
}
