using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeManager : MonoBehaviour {
    public static ScreenShakeManager instance;

    Transform cam;
    Vector3 originalPos;

    public float k = 0.2f;
    public float dampening_factor = 0.85f;
    public float debug_displacement = 1.0f;

    // Start is called before the first frame update
    void Start() {
        Application.targetFrameRate = 60;
        cam = Camera.main.transform;
        instance = this;
        originalPos = cam.position;
    }

    Vector3 velocity = Vector3.zero;

    public static void Bump(float amount) {
        //instance.velocity += new Vector3(force.x, force.y, 0) * 10;
        instance.velocity += UnityEngine.Random.onUnitSphere * amount*10;
    }

    // Update is called once per frame
    void Update() {
        // f = kd
        // f = ma
        // a = kd
        Vector3 d = (cam.transform.position - originalPos);
        Vector3 acceleration = -k * d;

        Vector3 vel_delta = acceleration - velocity * (1.0f - dampening_factor);
        velocity += vel_delta * Time.deltaTime * Application.targetFrameRate;

        cam.localPosition += velocity;
    }
}