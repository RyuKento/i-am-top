using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatManager : MonoBehaviour
{
    InfoManager im;
    // Start is called before the first frame update
    void Start()
    {
        im = InfoManager.Instance;
        im.isGame = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
