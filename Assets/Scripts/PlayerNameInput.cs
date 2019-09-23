using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Com.TDS {
  public class PlayerNameInput : MonoBehaviour {
    [SerializeField]
    private InputField inputField;

    const string playerNamePrefKey = "PlayerName";

    void Start() {
      string defaultName = string.Empty;

      if (PlayerPrefs.HasKey(playerNamePrefKey)) {
        defaultName = PlayerPrefs.GetString(playerNamePrefKey);
        inputField.text = defaultName;
      }

      PhotonNetwork.NickName = defaultName;
    }

    public void SetPlayerName(string value) {
      if (string.IsNullOrEmpty(value)) {
        return;
      }

      PhotonNetwork.NickName = value;
      PlayerPrefs.SetString(playerNamePrefKey, value);
    }
  }
}