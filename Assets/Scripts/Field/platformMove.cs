using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformMove : MonoBehaviour {

  public void triggerMove() {
    StartCoroutine(move());
  }

  IEnumerator move() {
    for (float i=0.0f;i<=2.0f;i+=Time.deltaTime) {
      Debug.Log(i.ToString() + " " + Mathf.Lerp(-1.0f, 1.5f, i / 2.0f).ToString());
      transform.position = new Vector3(transform.position.x, Mathf.Lerp(-1.0f, 1.5f, i / 2.0f), transform.position.z);
      yield return new WaitForSeconds(Time.deltaTime);
    }
    GameObject dummy = transform.GetChild(0).gameObject;
    dummy.transform.parent = null;
    dummy.GetComponent<ArrowKeyMovementDummy>().triggerForward();
    yield return new WaitForSeconds(2.0f);
    for (float i = 0; i <= 2.0f; i += Time.deltaTime) {
      transform.position = new Vector3(transform.position.x, Mathf.Lerp(1.5f, -1.0f, i / 2.0f), transform.position.z);
      yield return new WaitForSeconds(Time.deltaTime);
    }
  }
}
