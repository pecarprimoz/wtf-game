using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovermentController : MonoBehaviour {
    public GameObject PlayerHead;
    public float Speed = 2.0f;

    private bool SetPerfectAngles = false;
    // Allow 5 deg as a soft spot to reset angles
    private float AngleSoftSpot = 5.0f;
    // ((Mathf.Abs(gameObject.transform.localEulerAngles.z) - AngleSoftSpot) < 0) || ((Mathf.Abs(gameObject.transform.localEulerAngles.z) + AngleSoftSpot) > 360)
    void Update() {
        if (SetPerfectAngles) {
            var gameObjectAngles = gameObject.transform.localEulerAngles;
            var lerpedRotationX = Mathf.LerpAngle(gameObjectAngles.x, 0, Speed * Time.deltaTime);
            var lerpedRotationZ = Mathf.LerpAngle(gameObjectAngles.z, 0, Speed * Time.deltaTime);
            var lerpedPositionY = Mathf.Lerp(gameObject.transform.localPosition.y, 0, Speed * Time.deltaTime);
            gameObject.transform.localEulerAngles = new Vector3(lerpedRotationX, 0, lerpedRotationZ);
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, lerpedPositionY, gameObject.transform.localPosition.z);
            if (((Mathf.Abs(gameObject.transform.localEulerAngles.z) - AngleSoftSpot) < 0 || (Mathf.Abs(gameObject.transform.localEulerAngles.z) - AngleSoftSpot) > 360)) {
                gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0, gameObject.transform.localPosition.z);
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                SetPerfectAngles = false;
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
            }
        }
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
        if (Input.GetKey(KeyCode.Space)) {
            var gameObjectAngles = gameObject.transform.localEulerAngles;
            if (((Mathf.Abs(gameObjectAngles.x) - AngleSoftSpot) > 0) || ((Mathf.Abs(gameObjectAngles.z) - AngleSoftSpot) > 0)) {
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                SetPerfectAngles = true;
            }
        }
    }

}
