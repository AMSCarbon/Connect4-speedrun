using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class humanAgent : PlayerAgent
{

    public MouseControl mouse;
       
    void Update()
    {
        if (!currentTurn) return;
    }

    public override void startTurn()
    {
        base.startTurn();
        mouse.active = true;
        mouse.currentToken = token;
    }

    public override void EndTurn()
    {
        base.EndTurn();
        mouse.active = false;
    }
}
