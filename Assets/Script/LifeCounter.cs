using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LifeCounter : MonoBehaviour
{
    InfoManager im;
    [SerializeField] Text lifeText;
    [SerializeField] GameObject curvObject;
    // Start is called before the first frame update

    void Start()
    {
        im = InfoManager.Instance;
        im.lifePoint = Mathf.Clamp(im.lifePoint, 0, 99);
        lifeText.text = "×" + im.lifePoint;
    }


    // Update is called once per frame
    void Update()
    {
        if (im.isOut == true) InputText();
        im.isOut = false;
    }


    public void InputText()
    {
        GameManager.TextExpansion(curvObject);
        if (im.lifePoint == 1) im.lifePoint -= 1;
        StartCoroutine(InputCoroutine());
    }

    IEnumerator InputCoroutine()
    {
        yield return new WaitForSeconds(3);
        im.lifePoint -= 1;
        lifeText.text = "×" + im.lifePoint;
    }
}
