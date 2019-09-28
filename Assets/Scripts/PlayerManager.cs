using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Com.TDS {
  public class PlayerManager : MonoBehaviourPunCallbacks {
    public static GameObject LocalPlayerInstance;

    void Start () {
      if (photonView.IsMine) {
        PlayerManager.LocalPlayerInstance = this.gameObject;
      }
    }
  }
}