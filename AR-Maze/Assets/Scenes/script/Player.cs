using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public float TurnRate;
    public float Force;
    public float Speed;
    public bool Die;

    public Player()
    {
        Force = 10;
        Speed = 0.01f;
        TurnRate = 1;
        Die = false;
    }

}
