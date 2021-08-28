using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silver : Coin
{
    protected override void GetCoin()
    {
        GameManager.instance.AddPoint(100);
        base.GetCoin();
    }
}