using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {
    public GameObject Player;
    public float LookSpeed = 2.0f;
    private Vector2 Rotation = Vector2.zero;
    
    private float MinimumYDeg = -20.0f;
    private float MaximumYDeg = 40.0f;

    void Update () {
        Rotation.x += Input.GetAxis("Mouse X");
        Rotation.y += Input.GetAxis("Mouse Y");
        Rotation.y = Mathf.Clamp(Rotation.y, MinimumYDeg, MaximumYDeg);
        transform.localEulerAngles = new Vector3(0, Rotation.x * LookSpeed, Rotation.y * LookSpeed);
    }
}
