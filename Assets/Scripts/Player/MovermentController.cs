using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovermentController : MonoBehaviour {
    public GameObject PlayerHead;
    public float Speed = 2.0f; 
    void Start() {
    }

    void Update() {
        if (Input.GetKey(KeyCode.W)) {

            gameObject.transform.localPosition += PlayerHead.transform.right * Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S)) {
            gameObject.transform.localPosition -= PlayerHead.transform.right * Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A)) {
            gameObject.transform.localPosition += PlayerHead.transform.forward * Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D)) {
            gameObject.transform.localPosition -= PlayerHead.transform.forward * Speed * Time.deltaTime;
        }
    }
}
