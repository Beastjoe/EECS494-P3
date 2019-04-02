using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class typerEffect : MonoBehaviour {

  Text text;
  string words;
  bool jump = false;

  public float intervals = 0.1f;

  // Start is called before the first frame update
  void Start() {
    text = GetComponent<Text>();
    words = text.text;
    text.text = "";
    StartCoroutine(type());
  }

  IEnumerator type() {
    for (int i=0;i<words.Length;i++) {
      text.text = words.Substring(0, i+1);
      if (jump) {
        text.text = words;
        break;
      }
      yield return new WaitForSeconds(intervals);
    }
  } 

  public void jumpText() {
    jump = true;
  }
}
