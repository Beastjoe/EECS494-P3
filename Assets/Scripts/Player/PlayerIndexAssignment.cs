using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndexAssignment : MonoBehaviour
{
    public static PlayerIndexAssignment instance;
    public int[] indices;
    public bool[] robotSelected;

    void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        indices = new int[4];
        robotSelected = new bool[4];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
