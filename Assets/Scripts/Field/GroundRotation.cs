using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRotation : MonoBehaviour {
  float speed = 0.7f;
  public GameObject[] players;

  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    foreach (GameObject player in players) {
      if ((player.transform.position-transform.position).sqrMagnitude>55.0f) {
        //Debug.Log((player.transform.position - transform.position).sqrMagnitude);
        player.transform.parent = transform;
      } else {
        player.transform.parent = null;
      }
    }
    transform.Rotate(Vector3.forward, 45 * Time.deltaTime * speed);
  }
}
