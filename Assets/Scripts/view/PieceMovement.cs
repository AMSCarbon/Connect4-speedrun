using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovement : MonoBehaviour
{
    public Vector3 previousPosition;
    public Vector3 targetPosition;
    public bool updatingPosition;
    public float t = 0;
    private float movementSpeed = 2.0f;
    private AnimationCurve movementCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public bool released = false;

    void Start()
    {
        previousPosition = GameObject.Find("SpawnPoint").transform.position;
        targetPosition =  GameObject.Find("SpawnPoint").transform.position;
     
        updatingPosition = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!updatingPosition || released) return;
        if (FindObjectOfType<PauseMenuManager>().isPaused) return;
        t += Time.deltaTime * movementSpeed;
        gameObject.transform.position = Vector3.Lerp(previousPosition, targetPosition, movementCurve.Evaluate(t));
        updatingPosition = (Vector3.Distance(transform.position, targetPosition) > 0.001f);
    }

    public void UpdateTargetPosition(Vector3 target) {
        //if the target is the same as the current one, don't do anything.
        if (Vector3.Distance(target, targetPosition) < 0.01) return;
        t = 0;
        updatingPosition = true;
        targetPosition = target;
        previousPosition = gameObject.transform.position;
    }

    public void ReleaseToBoard() {
        released = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
