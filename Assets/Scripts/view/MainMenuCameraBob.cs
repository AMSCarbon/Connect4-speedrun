using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraBob : MonoBehaviour
{
    // Start is called before the first frame update
    public float frequency_height;
    float t_height;

    public float frequency_pan;
    float t_pan;
    public float height;
    public Vector3 originalTransofrm;
    public float r;
    public float theta;
    public float theta_offset;

    public Transform target;
    void Start()
    {
        originalTransofrm = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        t_height += Time.deltaTime*frequency_height;
        t_pan += Time.deltaTime * frequency_pan;
        Vector3 pos = transform.position;
        pos.x = originalTransofrm.x + r * Mathf.Cos(Mathf.Sin(t_pan) * theta+ theta_offset);
        pos.y = originalTransofrm.y + Mathf.Sin(t_height)*height;
        pos.z = originalTransofrm.z + r * Mathf.Sin(Mathf.Sin(t_pan) *theta + theta_offset);
        transform.position = pos;

        transform.LookAt(target);
    }
}
