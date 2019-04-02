using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RupeeMovement : MonoBehaviour
{
    float total_radius = 7.5f;
    float speed = 0.7f;
    private GameObject outGround;

    private void Start() {
        outGround = GameObject.Find("playground/Outerground");
    }

    void Update()
    {
        if(outGround != null)
        {
            Vector3 pos = transform.position;
            if (pos.x * pos.x + pos.z * pos.z >= total_radius * total_radius && (pos.y >= -1f || pos.y <= 1f))
            {
                transform.parent = outGround.transform;
            }
            else
            {
                transform.parent = null;
            }
        }

    }
}
