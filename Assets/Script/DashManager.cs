using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashManager : MonoBehaviour
{
    InfoManager im;
    [SerializeField] Image image;
    [SerializeField] GameObject dashText;
    bool isUsed;
    // Start is called before the first frame update
    void Start()
    {
        im = InfoManager.Instance;
        StartCoroutine(StartWaitCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isUsed == false&& im.isGame && im.isPlay)
        {
            image.fillAmount = 0;
            isUsed = true;
            dashText.SetActive(false);
            StartCoroutine(CooltimeCoroutine());
        }
        if (im.isHeal)
        {
            image.fillAmount = 1;
            im.isHeal = false;
            isUsed = false;
            dashText.SetActive(true);
        }
    }
    private void FixedUpdate()
    {
        if(im.isPlay)
        image.fillAmount += 0.004f;
    }
    IEnumerator CooltimeCoroutine()
    {
        yield return new WaitForSeconds(5);
        isUsed = false;
        dashText.SetActive(true);
    }
    IEnumerator StartWaitCoroutine()
    {
        yield return new WaitForSeconds(5);
    }
}
