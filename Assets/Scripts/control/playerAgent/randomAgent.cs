using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomAgent : PlayerAgent
{
    //wait for x seconds to make it look like it's thinking. less jaring.
    public float thinkTime = 0.1f;

    public override void startTurn()
    {
        Invoke("PlaceToken", thinkTime);
        base.startTurn();
    }

    public void PlaceToken() {
        int column = Random.Range(0, 6);
        while (!gm.CanAddAtColumn(column)) {
            column = Random.Range(0, 6);
        }
        GameObject columnMarker = GameObject.Find(column.ToString());
        token.transform.position = columnMarker.transform.position;
        FindObjectOfType<GameManager>().AddPiece(column);
        token.ReleaseToBoard();
        gm.SwitchTurn();
    }
}

