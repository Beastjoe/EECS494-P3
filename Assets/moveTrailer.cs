using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTrailer : MonoBehaviour {

    public AnimationCurve curveY, curveZ, curveAngle;
    public float time = 3.0f;
    public float yStart = 100, yEnd = 13.55f;
    public float zStart = -161.5f, zEnd = -10.1f;
    public float xAngleStart = 82.647f, xAngleEnd = 54.9f;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(move());
    }

    // Update is called once per frame
    void Update() {

    }

    IEnumerator move() {
        for (float t = 0.0f; t<time; t+=Time.deltaTime)
        {
            float yCurr = curveY.Evaluate(t / time) * (yEnd - yStart) + yStart;
            float zCurr = Mathf.Clamp(curveZ.Evaluate(t / time) * (zEnd - zStart) + zStart, zStart, zEnd);
            float angleCurr = curveAngle.Evaluate(t / time) * (xAngleEnd - xAngleStart) + xAngleStart;
            transform.position = new Vector3(transform.position.x, yCurr, zCurr);
            transform.eulerAngles = new Vector3(angleCurr, 0, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
