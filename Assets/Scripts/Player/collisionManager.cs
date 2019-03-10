using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionManager : MonoBehaviour {
  // Start is called before the first frame update
  playerStatus ps;
  ArrowKeyMovement am;
  Inventory inventory;
  Rigidbody rb;

  void Start() {
    ps = GetComponent<playerStatus>();
    am = GetComponent<ArrowKeyMovement>();
    inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
  }

  // Update is called once per frame
  void Update() {

  }

  private void OnCollisionEnter(Collision collision) {
    if (collision.gameObject.CompareTag("airwall")) {
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
            player_am.hurt(am.flyingDir);
          }
        }
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
    am.flyingSpeed *= 0.5f;
  }

}
