using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bronze : Coin
{
    protected override void GetCoin()
    {
        GameManager.instance.AddPoint(50);
        base.GetCoin();
    }
}