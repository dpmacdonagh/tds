using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Com.TDS {
  public class PlayerHealth : MonoBehaviourPunCallbacks, IPunObservable {
    [SerializeField] private float health = 100f;
    [SerializeField] private GameObject hitParticle;

    public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
      if (stream.IsWriting) {
        stream.SendNext (health);
      } else {
        this.health = (float) stream.ReceiveNext ();
      }
    }

    void Start () {
      if (photonView.IsMine) {
        UiManager.Instance.SetHealth (health);
      }
    }

    public void TakeDamage (float damage, Vector3 hitPoint) {
      Debug.Log ("Take damage");
      GameObject newHit = Instantiate (hitParticle, hitPoint, Quaternion.identity);
      Destroy (newHit, 2);

      if (photonView.IsMine) {
        health -= damage;
        UiManager.Instance.SetHealth (health);

        if  (health <= 0) {
          GameManager.Instance.LeaveRoom();
        }
      }
    }
  }
}