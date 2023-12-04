using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTorque : MonoBehaviour
{
    InfoManager im;
    [SerializeField]private float speed = 100;
    // Start is called before the first frame update
    void Start()
    {
        im = InfoManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (im.isGame == true || im.isClear == true)
        this.transform.Rotate(new Vector3(0,speed,0),Space.Self);
    }
}
