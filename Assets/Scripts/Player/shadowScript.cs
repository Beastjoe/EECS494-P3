using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadowScript : MonoBehaviour {
  // Start is called before the first frame update
  void Start() {
    SpriteRenderer render = GetComponent<SpriteRenderer>();
    render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    render.receiveShadows = true;
  }

  // Update is called once per frame
  void Update() {

  }
}
