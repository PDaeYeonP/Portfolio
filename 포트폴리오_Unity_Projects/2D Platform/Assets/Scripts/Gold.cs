using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Coin
{
    protected override void GetCoin()
    {
        GameManager.instance.AddPoint(150);
        base.GetCoin();
    }
}
