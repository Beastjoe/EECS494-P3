using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public float slowdownFactor = 0.05f;
    public float slowdownLength = 3f;
    bool inSlowMotion = false;
    Vector3 originalCameraPos;
    Vector3 originalCameraAngle;
    Vector3 cameraFocus;
    Vector3 cameraFocusAngle;

    private void Start() {
        originalCameraPos = transform.position;
        originalCameraAngle = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update() {
        if (Time.timeScale>=1)
        {
            inSlowMotion = false;
        }
        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        if (inSlowMotion)
        {
            if (Time.timeScale > 0.6f)
            {
                transform.position = Vector3.Lerp(cameraFocus, originalCameraPos, Time.timeScale);
            }
            transform.eulerAngles = Vector3.Lerp(cameraFocusAngle, originalCameraAngle, Time.timeScale);
        }
    }

    public void DoSlowMotion(Vector3 focus) {
        inSlowMotion = true;
        cameraFocus = focus;
        transform.position = Vector3.Lerp(focus, originalCameraPos, 0.6f);
        transform.LookAt(focus, Vector3.up);
        cameraFocusAngle = transform.eulerAngles;
        if (cameraFocusAngle.y>180)
        {
            cameraFocusAngle.y -= 360;
        }
        Debug.Log("here" + cameraFocusAngle);
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
}
