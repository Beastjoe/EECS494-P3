using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRotation : MonoBehaviour {
  public float speed = 0.7f;
  public GameObject[] players;

  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    if (GameControl.instance.isPaused) {
      return;
    }
    foreach (GameObject player in players) {
      if ((player.transform.position-transform.position).sqrMagnitude>45.0f) {
        //Debug.Log((player.transform.position - transform.position).sqrMagnitude);
        player.GetComponent<ArrowKeyMovement>().onOuterGround = true;
      } else {
        player.GetComponent<ArrowKeyMovement>().onOuterGround = false;
        //player.transform.parent = null;
      }
    }
    transform.Rotate(Vector3.forward, 45 * Time.deltaTime * speed);
  }
}
