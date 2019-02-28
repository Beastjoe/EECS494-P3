using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRotation : MonoBehaviour
{
    float speed = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, 45 * Time.deltaTime * speed);
    }
}
