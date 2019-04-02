using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyRupeeController : MonoBehaviour {
  public GameObject rupee;
  public GameObject parent;

  ArrowKeyMovementDummy amd;

  int currentNum;
  float speed = 10f;
  float radius = 0.6f;

  void Start() {
    amd = parent.GetComponent<ArrowKeyMovementDummy>();
  }

  void Update() {
    transform.position = parent.transform.position;
    // Reorganize
    if (currentNum != amd.number_of_rupees) {

      for (int i = 0; i < currentNum; ++i) {
        Destroy(transform.GetChild(i).gameObject);
      }

      currentNum = amd.number_of_rupees;
      for (int i = 0; i < currentNum; ++i) {
        float theta = i * 2 * Mathf.PI / currentNum;
        Vector3 position = radius * new Vector3(Mathf.Cos(theta), -0.5f, Mathf.Sin(theta));
        GameObject instance = Instantiate(rupee, transform.position + position, Quaternion.identity);
        //instance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        instance.SetActive(true);
        instance.transform.parent = transform;
      }
    }

    transform.Rotate(Vector3.up, 45 * Time.deltaTime * speed / Mathf.Sqrt(currentNum));
  }
}
