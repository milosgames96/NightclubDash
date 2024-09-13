using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : MonoBehaviour
{
    readonly float enrageIncrement = 1.15f;
    public int enrageStack = 0;
    [SerializeField] private GirlScript girlScript;

    public void Enrage()
    {
        transform.localScale *= enrageIncrement;
        ++enrageStack;
    }
    public void DeEnrage()
    {
        transform.localScale /= enrageIncrement;
        --enrageStack;
    }
}
