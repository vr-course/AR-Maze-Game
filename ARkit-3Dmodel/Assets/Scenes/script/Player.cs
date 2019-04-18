using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public float TurnRate;
    public float Force;
    public float Speed;
    public bool Die;
    public int Money;

    public Player()
    {
        Money = 0;
        Force = 10;
        Speed = 1f;
        TurnRate = 1;
        Die = false;
    }

}
