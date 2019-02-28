using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionManager : MonoBehaviour {
  // Start is called before the first frame update
  playerStatus ps;
  ArrowKeyMovement am;
  Rigidbody rb;

  void Start() {
    ps = GetComponent<playerStatus>();
    am = GetComponent<ArrowKeyMovement>();
  }

  // Update is called once per frame
  void Update() {

  }

  private void OnCollisionEnter(Collision collision) {
    if (collision.gameObject.CompareTag("cuboid")) {
      Destroy(collision.gameObject);
    }
    if (collision.gameObject.CompareTag("airwall")) {
      if (ps.currStatus==playerStatus.status.FLYING) {
       ReflectProjectile(collision.contacts[0].normal);
      }
    }
  }

  private void ReflectProjectile(Vector3 reflectVector) {
    am.flyingDir = Vector3.Reflect(am.flyingDir, reflectVector);
    am.flyingSpeed *= 0.5f;
  }

}
