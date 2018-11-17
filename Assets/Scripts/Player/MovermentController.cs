using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovermentController : MonoBehaviour {
    public GameObject PlayerHead;
    public float Speed = 2.0f;

    private bool SetPerfectAngles = false;
    // Allow 2 deg as a soft spot to reset angles
    private float AngleSoftSpot = 2.0f;
    // Player can't move when hes getting up
    private bool CanMove = true;
    // ((Mathf.Abs(gameObject.transform.localEulerAngles.z) - AngleSoftSpot) < 0) || ((Mathf.Abs(gameObject.transform.localEulerAngles.z) + AngleSoftSpot) > 360)
    void Update() {
        if (CanMove) {
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
                if (!SetPerfectAngles) {
                    var beginFallTransition = false;
                    var gameObjectAngles = gameObject.transform.localEulerAngles;
                    if (gameObjectAngles.x != 0) {
                        if (Mathf.Abs(gameObjectAngles.x) + 1 > 90) {
                            beginFallTransition = true;
                        }
                    } else if (gameObjectAngles.z != 0) {
                        if (Mathf.Abs(gameObjectAngles.z) + 1 > 90) {
                            beginFallTransition = true;
                        }
                    }
                    if (beginFallTransition) {
                        gameObject.GetComponent<Rigidbody>().useGravity = false;
                        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                        SetPerfectAngles = true;
                        CanMove = false;
                    }
                }
            }
        }
        if (SetPerfectAngles) {
            // todo refac this shit code
            var endFallTransition = false;
            var gameObjectAngles = gameObject.transform.localEulerAngles;
            var lerpedRotationX = Mathf.LerpAngle(gameObjectAngles.x, 0, Speed * Time.deltaTime);
            var lerpedRotationZ = Mathf.LerpAngle(gameObjectAngles.z, 0, Speed * Time.deltaTime);
            var lerpedPositionY = Mathf.Lerp(gameObject.transform.localPosition.y, 0, Speed * Time.deltaTime);
            gameObject.transform.localEulerAngles = new Vector3(lerpedRotationX, 0, lerpedRotationZ);
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, lerpedPositionY, gameObject.transform.localPosition.z);
            if (gameObjectAngles.x != 0) {
                if ((Mathf.Abs(gameObject.transform.localRotation.eulerAngles.x) - AngleSoftSpot < AngleSoftSpot)) {
                    endFallTransition = true;
                }
            } else if (gameObjectAngles.z != 0) {
                if ((Mathf.Abs(gameObject.transform.localRotation.eulerAngles.z) - AngleSoftSpot < AngleSoftSpot)) {
                    endFallTransition = true;
                }
            }
            if (endFallTransition) {
                gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0, gameObject.transform.localPosition.z);
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                SetPerfectAngles = false;
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
                CanMove = true;
            }
        }

    }
}
