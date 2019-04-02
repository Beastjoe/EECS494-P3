using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lightBoardTimer : MonoBehaviour {

  PanelGenerator pg;

  public Text text;
  

  // Start is called before the first frame update
  void Start() {
    pg = GetComponent<PanelGenerator>();
  }

  // Update is called once per frame
  void Update() {
    int currTime = System.Int32.Parse(text.text);
    if (currTime<10) {
      pg.StringToDraw = "0" + currTime.ToString();
    } else {
      pg.StringToDraw = currTime.ToString();
    }
    if (currTime<15) {
      GetComponent<ShakeObject>().shake(Time.deltaTime);
      pg.TextMotionStyle = PanelGenerator.MotionStyle.Blink;
      pg.RefreshTime = 0.2f;
    }
  }
}
