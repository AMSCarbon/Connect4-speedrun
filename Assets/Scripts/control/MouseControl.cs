using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    
    public Camera cam;
    GameManager gm;
    public List<GameObject> columnMarkers;
    public GameObject closestMarker;
    private GameObject previousClosestMarker;
    private float matchCount = 0;
    private float matchThreshold = 0.1f; //duration of time index has to be the same for before updating the position.
    public bool updated = false;
    public PieceMovement currentToken;
    public float followThreshold;
    public bool active;
    private float averageDelta;
    float distanceBetweenMarkers = 0.0f;


    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        closestMarker = columnMarkers[0];
    }

    void Update()
    {
        if (!active) return;
        if (FindObjectOfType<PauseMenuManager>().isPaused) return;

        //left click to drop the token into the board.
        if (Input.GetMouseButtonDown(0) && currentToken && !currentToken.updatingPosition)
        {
            if (gm.AddPiece(int.Parse(closestMarker.name)))
            {
                currentToken.ReleaseToBoard();
                gm.SwitchTurn();
                return;
            }
        }

        //determine which markers the mouse is closest to.
        float mouse_x = Input.mousePosition.x;
        GameObject first_closest = columnMarkers[0];
        GameObject second_closest = columnMarkers[0];
        foreach (GameObject marker in columnMarkers)
        {
            float screen_x = cam.WorldToScreenPoint(marker.transform.position).x;
            if (Mathf.Abs(screen_x - mouse_x) <
             Mathf.Abs(cam.WorldToScreenPoint(closestMarker.transform.position).x - mouse_x))
            {
                second_closest = closestMarker;
                closestMarker = marker;
            }
        }

        //decide if the token should be moved. 
        if (closestMarker == previousClosestMarker)
        {
            matchCount += Time.deltaTime;
            if (matchCount > matchThreshold)
            {
                UpdatePiece(closestMarker.transform.position);
                matchCount = 0;
            }
        }
        else
        {
            matchCount = 0;
            updated = false;
        }
        previousClosestMarker = closestMarker;
    }


    public void UpdatePiece(Vector3 targetPosition)
    {
        if (gm.restarting || gm.gameOver ) return;
        currentToken.UpdateTargetPosition(targetPosition);
    }
}

