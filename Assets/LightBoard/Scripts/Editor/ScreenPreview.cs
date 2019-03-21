using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/*
 * Class used to Generate fake board in editor mode.
 * 
 */
[InitializeOnLoad]
public class ScreenPreview : MonoBehaviour {
	static float			time = 0;
	static float			refreshTime = 0.005f;
	static List<GameObject>	gos = new List<GameObject>();

	static ScreenPreview () { 
		EditorApplication.update += Update;
	}

	static void Update ()
	{
		if (!EditorApplication.isPlaying) {
			time += Time.deltaTime;
			if (time >= refreshTime) {
				gos.Clear ();

				PanelGenerator[] ledPanels = GameObject.FindObjectsOfType<PanelGenerator> ();

				foreach (PanelGenerator pg in ledPanels) {
					foreach(Transform tr in pg.transform) {
						DestroyImmediate (tr.gameObject);
					}
					GameObject go = Instantiate (Resources.Load<GameObject> ("Prefabs/Background"));
					go.transform.SetParent (pg.gameObject.transform);
					go.transform.localPosition = new Vector3 (0, 0, 0);
					go.transform.localScale = new Vector3 (pg.NumberUnitWidth + pg.Spaces * (pg.NumberUnitWidth - 1) + 1, pg.NumberUnitHeight + pg.Spaces * (pg.NumberUnitHeight - 1) + 1, 1);
					go.transform.localRotation = new Quaternion (0, 0, 0, 0);
					gos.Add(go);
				}
				time = 0;
			}
		}
	}

}
