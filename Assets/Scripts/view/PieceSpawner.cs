using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    public GameObject yellowPiece;
    public GameObject redPiece;

    public PieceMovement InstantiateNextPiece(bool redToken) {
        GameObject tokenPrefab = redToken ? redPiece : yellowPiece;
        return Instantiate(tokenPrefab).GetComponent<PieceMovement>();
    }

}
