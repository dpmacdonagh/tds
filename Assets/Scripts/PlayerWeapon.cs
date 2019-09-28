using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Com.TDS {
  public class PlayerWeapon : MonoBehaviourPunCallbacks, IPunObservable {
    [SerializeField] Transform weaponContainer;
    [SerializeField] Transform raycastOrigin;
    [SerializeField] GameObject hitParticle;
    [SerializeField] GameObject muzzleFlashParticle;
    private PlayerAim playerAim;
    [SerializeField] private Quaternion weaponContainerRotation = Quaternion.identity;

    public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
      if (stream.IsWriting) {
        stream.SendNext (weaponContainerRotation);
      } else {
        this.weaponContainerRotation = (Quaternion) stream.ReceiveNext ();
      }
    }

    void Start () {
      playerAim = GetComponent<PlayerAim> ();
    }

    void Update () {
      if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
        return;
      }

      AimWeapon ();
      ProcessInputs ();
    }

    void AimWeapon () {
      if (playerAim.aimLocation != null) {
        weaponContainerRotation = Quaternion.LookRotation (playerAim.aimLocation - transform.position);
        weaponContainer.rotation = weaponContainerRotation;
      }
    }

    void ProcessInputs() {
      if (Input.GetButtonDown("Fire1")) {
        RPCShoot(playerAim.aimLocation - raycastOrigin.position);
      }
    }

    [PunRPC]
    void RPCShoot(Vector3 shootDirection) {
      RaycastHit hit;
      Instantiate(muzzleFlashParticle, raycastOrigin.position, Quaternion.identity);
      if (Physics.Raycast(raycastOrigin.position, shootDirection, out hit, 100000)) {
        Debug.DrawRay(raycastOrigin.position, (shootDirection) * hit.distance, Color.red, 2);
        
        if (hit.collider.tag == "Player") {
          hit.collider.gameObject.GetComponent<PlayerHealth>().TakeDamage(10f, hit.point);
        } else {
          Debug.Log("Hit but not player " + hit.collider.tag + " " + hit.collider.gameObject.name);
          GameObject newHit = Instantiate(hitParticle, hit.point, Quaternion.identity);
          Destroy(newHit, 2);
        }

      } else {
        Debug.DrawRay(raycastOrigin.position, (shootDirection) * 1000000, Color.blue, 2);
        Debug.Log("Miss");
      }

      if (photonView.IsMine) {
        photonView.RPC("RPCShoot", RpcTarget.OthersBuffered, shootDirection);
      }
    }
  }
}