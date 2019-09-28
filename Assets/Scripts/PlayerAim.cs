using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace Com.TDS {
  public class PlayerAim : MonoBehaviourPunCallbacks {
    public Vector3 aimLocation;

    Camera cam;

    void Start() {
      cam = Camera.main;
    }

    // Update is called once per frame
    void Update () {
      if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
        return;
      }

      SetAimLocation ();
    }

    void SetAimLocation () {
      Ray ray = cam.ScreenPointToRay (Input.mousePosition);;
      RaycastHit hit;
      
      if (Physics.Raycast (ray, out hit)) {
        aimLocation = hit.point;
      }
    }
  }
}