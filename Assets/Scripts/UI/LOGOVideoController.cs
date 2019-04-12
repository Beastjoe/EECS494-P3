using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LOGOVideoController : MonoBehaviour {
    private VideoPlayer vp;

    public GameObject BlackShader;

    // Start is called before the first frame update
    void Start() {
        vp = GetComponent<VideoPlayer>();
        vp.loopPointReached += End;
    }

    void End(UnityEngine.Video.VideoPlayer vp) {
        StartCoroutine(Fading());
    }

    IEnumerator Fading() {
        for (int i = 0; i < 100; ++i)
        {
            Color c = BlackShader.GetComponent<Image>().color;
            c.a += 0.01f;
            BlackShader.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.003f);
        }
        SceneManager.LoadScene("MainMenu");
    }
}
