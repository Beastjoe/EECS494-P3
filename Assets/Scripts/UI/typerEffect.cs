using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class typerEffect : MonoBehaviour {

  Text text;
  string words;

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
      yield return new WaitForSeconds(intervals);
    }
  } 
}
