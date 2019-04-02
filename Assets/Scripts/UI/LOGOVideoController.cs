using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class LOGOVideoController : MonoBehaviour {
    private VideoPlayer vp;

    // Start is called before the first frame update
    void Start() {
        vp = GetComponent<VideoPlayer>();
        vp.loopPointReached += End;
    }

    // Update is called once per frame
    void Update() {

    }

    void End(UnityEngine.Video.VideoPlayer vp) {
        SceneManager.LoadScene("MainMenu");
    }
}
