using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgent : MonoBehaviour
{
    public bool currentTurn;
    protected GameManager gm;
    public PieceMovement token;
    public GameObject tokenPrefab;
    public float score = 0.0f;
    public GameObject tokenSpawnPoint;

    public void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public virtual void startTurn() {
        token = Instantiate(tokenPrefab,
            tokenSpawnPoint.transform.position,
            tokenSpawnPoint.transform.rotation).GetComponent<PieceMovement>();
        currentTurn = true;
    }

    public virtual void EndTurn()
    {
        currentTurn = false;
    }

    public virtual void AlertEnd(bool won) {
    }

}
