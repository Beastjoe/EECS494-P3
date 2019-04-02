using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObject : MonoBehaviour {

    Vector3 position;

    void Awake() {
        position = transform.position;
    }

    public void shake(float duration) {
        StartCoroutine(shaking(duration, 0.05f, 1.0f));
    }

    IEnumerator shaking(float duration, float amount, float speed) {
        for (float t = 0.0f; t<duration; t+=Time.deltaTime)
        {
            Vector3 pos = new Vector3(Random.Range(-amount, amount) * speed, Random.Range(-amount, amount) * speed, Random.Range(-amount, amount) * speed);
            transform.position = pos + position;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        transform.position = position;
    }
}
