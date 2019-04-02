using UnityEngine;
using System.Collections;

/*
 * This class is used by led unit to change its material if the led is ligthed in or not 
 * 
*/
public class LedUnit : MonoBehaviour {

	public Material		LightOn;
	public Material 	LightOff;
	public bool			IsLightOn = false;
	public bool			PreviousState = false;

	private Renderer	renderer;

	void Start() {
		renderer = gameObject.GetComponent<Renderer> ();
		if (IsLightOn)
			renderer.material = LightOn;
		else
			renderer.material = LightOff;
	}
	
	void Update () {
		if (PreviousState != IsLightOn) {
			if (IsLightOn)
				renderer.material = LightOn;
			else
				renderer.material = LightOff;
			PreviousState = IsLightOn;
		}
	}
}
