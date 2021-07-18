using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomColliderController : MonoBehaviour
{

    public bool movingPiece;
    public float duration = 10.0f;
    public Vector3 initialPosition;
    public float moveLength = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        duration += Time.deltaTime / moveLength;
        if (duration < moveLength) {
            transform.position = initialPosition + new Vector3(0, 0, 1);
        }
        else 
        {
            transform.position = initialPosition;
        }
    }

    public void open() {
        duration = 0;
    }
}
