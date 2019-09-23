using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Com.TDS {
  public class LauncherManager : MonoBehaviourPunCallbacks {
    [SerializeField]
    private byte maxPlayersPerRoom = 4;

    [SerializeField]
    private GameObject controlPanel;

    [SerializeField]
    private GameObject connectionStatus;

    string gameVersion = "1";
    bool isConnecting;

    void Awake() {
      PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start() {
      ShowControlPanelUI();
    }

    public void Connect() {
      isConnecting = true;
      ShowConnectionStatusUI();

      if (PhotonNetwork.IsConnected) {
        PhotonNetwork.JoinRandomRoom();
      } else {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
      }
    }

    public override void OnConnectedToMaster() {
      if (isConnecting) {
        PhotonNetwork.JoinRandomRoom();
      }
    }

    public override void OnDisconnected(DisconnectCause cause) {
      ShowControlPanelUI();
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
      PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom() {
      if (PhotonNetwork.CurrentRoom.PlayerCount == 1) {
        PhotonNetwork.LoadLevel("Test Arena");
      }
    }

    void ShowControlPanelUI() {
      controlPanel.SetActive(true);
      connectionStatus.SetActive(false);
    }

    void ShowConnectionStatusUI() {
      controlPanel.SetActive(false);
      connectionStatus.SetActive(true);
    }
  }
}