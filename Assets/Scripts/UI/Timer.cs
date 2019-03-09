using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
  float timer;
  Text text;
  void Start() {
    text = GetComponent<Text>();
    timer = float.Parse(text.text);
  }

  void Update()
  {
    timer -= Time.deltaTime;
    if(timer >= 0.0f) {
      text.text = timer.ToString("F2");
    }
    else{
      //TODO: GameEnd
    }
  }
}
