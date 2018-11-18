using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovermentController : MonoBehaviour {
    public GameObject PlayerHead;
    public GameObject PlayerLegs;
    public float Speed = 2.0f;

    private float PlayerHalfHeight = 2f;
    private bool CanMove = true;
    private bool CanJump = true;
    private bool PlayerIsJumping = false;
    private float MaxPlayerJump;
    private float JumpTimer;
    private void Start() {
        MaxPlayerJump = 2 * gameObject.transform.localPosition.y;
        JumpTimer = 0.0f;
    }
    void Update() {
        if (PlayerIsJumping) {
            var lerpJump = Mathf.Lerp(gameObject.transform.localPosition.y, MaxPlayerJump, Speed * Time.deltaTime);
            var playerJumpPosition = new Vector3(gameObject.transform.localPosition.x, lerpJump, gameObject.transform.localPosition.z);
            gameObject.transform.localPosition = playerJumpPosition;
        }
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
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (CanJump) {
                    gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                    CanJump = false;
                    gameObject.GetComponent<Rigidbody>().useGravity = false;
                    PlayerIsJumping = true;
                }
            }
        }
        JumpTimer += Time.deltaTime;
        if (!PlayerIsJumping) {
            CheckIfPlayerOnTheGround();
        }
        CheckIfPlayerIsFinishedJumping();
    }
    private bool CheckIfPlayerHasFallen() {
        return false;
    }
    private void CheckIfPlayerIsFinishedJumping() {
        if (JumpTimer > 1.5f) {
            JumpTimer = 0;
            PlayerIsJumping = false;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }
    private void CheckIfPlayerOnTheGround() {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
        var playerLegPosition = gameObject.transform.localPosition.y + PlayerLegs.transform.localPosition.y;
        var rayStart = new Vector3(gameObject.transform.localPosition.x, playerLegPosition, gameObject.transform.localPosition.z);
        //var vec_end = new Vector3(gameObject.transform.localPosition.x, playerLegPosition - 0.2f, gameObject.transform.localPosition.z);
        //Debug.DrawLine(vec, vec_end, Color.red);
        var legsRay = new Ray(rayStart, Vector3.down);
        if (Physics.Raycast(legsRay, 0.2f)) {
            CanJump = true;
        }
    }
}
