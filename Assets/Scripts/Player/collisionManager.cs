using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionManager : MonoBehaviour {
  // Start is called before the first frame update
  playerStatus ps;
  ArrowKeyMovement am;
  ArrowKeyMovementDummy amd;
  Inventory inventory;
  Rigidbody rb;

  public AudioClip collectClip;

  void Start() {
    ps = GetComponent<playerStatus>();
    am = GetComponent<ArrowKeyMovement>();
    inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
  }

  private void OnCollisionEnter(Collision collision) {
    if (collision.gameObject.CompareTag("cuboid")) {
      Destroy(collision.gameObject);
      inventory.addRupee(am.playerIndex);
      Camera.main.GetComponent<AudioSource>().PlayOneShot(collectClip, 2.0f);
    }
    if (collision.gameObject.CompareTag("airwall")) {
      if (ps.currStatus == playerStatus.status.DASH) {
        am.stopDash = true;
      }
      if (ps.currStatus==playerStatus.status.FLYING) {
       ReflectProjectile(collision.contacts[0].normal);
      } 
    }
    if (collision.gameObject.CompareTag("Player")) {
      GameObject player = collision.gameObject;
      playerStatus player_ps = player.GetComponent<playerStatus>();
      ArrowKeyMovement player_am = player.GetComponent<ArrowKeyMovement>();

      // I am a bullet
      if (ps.currStatus == playerStatus.status.FLYING) {
        // same team
        if (player_ps.teamIdx == ps.teamIdx) {
          ReflectProjectile(collision.contacts[0].normal);
        } else {
          // different team
          if (player_ps.currStatus == playerStatus.status.FLYING) {
            // hit another bullet
            ReflectProjectile(collision.contacts[0].normal);
          } else {
            am.hitEnemy = true;
            if(player_am == null) {
              player.GetComponent<ArrowKeyMovementDummy>().hurt(am.flyingDir);
            }
            else {
              player_am.hurt(am.flyingDir);
            }
          }
        }
      } else if (player_ps.teamIdx == ps.teamIdx && player_ps.currStatus == playerStatus.status.DEFENSE && ps.currStatus==playerStatus.status.NORMAL) {
          ps.currStatus = playerStatus.status.HOLDING;
          player_ps.currStatus = playerStatus.status.HELD;
          player.GetComponent<Animator>().SetBool("moving", false);
          player.GetComponent<Animator>().SetTrigger("IdelTrigger");
          player.transform.position = transform.position + new Vector3(0, 0.8f, 0);
      }
    }
  }

  private void OnCollisionStay(Collision collision) {
    if (collision.gameObject.CompareTag("Player")) {
      GameObject player = collision.gameObject;
      playerStatus player_ps = player.GetComponent<playerStatus>();
      if (player_ps.teamIdx == ps.teamIdx && player_ps.currStatus == playerStatus.status.DEFENSE && ps.currStatus == playerStatus.status.NORMAL) {
        ps.currStatus = playerStatus.status.HOLDING;
        player_ps.currStatus = playerStatus.status.HELD;
        player.GetComponent<Animator>().SetBool("moving", false);
        player.GetComponent<Animator>().SetTrigger("IdelTrigger");
        player.transform.position = transform.position + new Vector3(0, 0.8f, 0);
      }
    }
    if (collision.gameObject.CompareTag("airwall")) {
      if (ps.currStatus == playerStatus.status.DASH) {
        am.stopDash = true;
      }
    }
  }

  private void OnTriggerEnter(Collider other) {
    // get cuboid
    if (other.gameObject.CompareTag("cuboid")) {
      Destroy(other.gameObject);
      inventory.addRupee(am.playerIndex);
    }
  }

  private void ReflectProjectile(Vector3 reflectVector) {
    am.flyingDir = Vector3.Reflect(am.flyingDir, reflectVector);
    am.flyingSpeed *= 0.75f;
  }

}
