using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.TDS {
  public class GameManager : MonoBehaviourPunCallbacks {
    public static GameManager Instance;
    public GameObject playerPrefab;

    public override void OnLeftRoom () {
      SceneManager.LoadScene (0);
    }

    void Start () {
      Instance = this;

      if (playerPrefab == null) {
        Debug.Log ("No player prefab");
      } else {
        if (PlayerManager.LocalPlayerInstance == null) {
          Debug.LogFormat ("Instantiating LocalPlayer from {0}", Application.loadedLevelName);
          PhotonNetwork.Instantiate (this.playerPrefab.name, new Vector3 (0f, 5f, 0f), Quaternion.identity, 0);
        } else {
          Debug.LogFormat ("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }
      }
    }

    public void LeaveRoom () {
      PhotonNetwork.LeaveRoom ();
    }
  }

}