using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class tmpTest : MonoBehaviour
{

    private float cd = 5.0f;
    private float cdCount;
    // Start is called before the first frame update
    void Start()
    {
        cdCount = cd;
        StorageController.instance.LightRed();
    }

    // Update is called once per frame
    void Update()
    {
        cdCount -= Time.deltaTime;
        if (cdCount < 0)
        {
            StorageController.instance.UnlightRed();
            cdCount = cd;
        }
    }
}
