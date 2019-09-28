using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Com.TDS {
  public class PlayerMovement : MonoBehaviourPunCallbacks {
    public float jumpSpeed = 8.0F;
    public float runSpeed = 4.0F;
    public float sprintSpeed = 6.0F;
    public float backwardSpeed = 3.0f;
    public float strafeSpeed = 3.0f;
    public float gravity = 20.0F;
    public float rotationDamping = 10f;
    public float speedMultiplier;
    public float speedMultiplierDampening = 5f;

    private Vector3 moveDirection = Vector3.zero;
    public CharacterController controller;

    private PlayerAim playerAim;

    private Vector3 previousPosition;
    [SerializeField] private float currentHeading;

    void Start () {
      controller = GetComponent<CharacterController> ();
      playerAim = GetComponent<PlayerAim> ();
      previousPosition = transform.position;
      speedMultiplier = runSpeed;
    }

    void Update () {
      if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
        return;
      }

      RotateCharacter ();
      SetHeading();
      SetSpeedMultiplier();
      MoveCharacter ();
    }

    void MoveCharacter () {
      if (controller.isGrounded) {
        moveDirection = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical")).normalized;
        moveDirection = transform.TransformDirection (moveDirection);
        moveDirection *= speedMultiplier;

        if (Input.GetButton ("Jump")) {
          moveDirection.y = jumpSpeed;
        }

      }
      moveDirection.y -= gravity * Time.deltaTime;
      controller.Move (moveDirection * Time.deltaTime);
    }

    void RotateCharacter () {
      if (playerAim.aimLocation != null) {
        Quaternion rotation = Quaternion.LookRotation (playerAim.aimLocation - transform.position);
        rotation.x = 0;
        rotation.z = 0;
        transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationDamping);
      }
    }

    void SetHeading () {
      currentHeading = Vector3.Angle(transform.forward, (transform.position - previousPosition).normalized);
      previousPosition = transform.position;
    }

    void SetSpeedMultiplier() {
      if (currentHeading < 45) {
        speedMultiplier = Mathf.Lerp(speedMultiplier, Input.GetButton("Sprint") ? sprintSpeed : runSpeed, Time.deltaTime * speedMultiplierDampening);
      } else if (currentHeading < 100) {
        speedMultiplier = Mathf.Lerp(speedMultiplier, strafeSpeed, Time.deltaTime * speedMultiplierDampening);
      } else {
        speedMultiplier = backwardSpeed;
      }
    }
  }
}