using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class TextManager :MonoBehaviour
{
    [SerializeField] Text text3;
    [SerializeField] Text text2;
    [SerializeField] Text text1;
    [SerializeField] Text textGo;
    public bool isScale;
    private void OnEnable()
    {
        StartCoroutine(Text3Coroutine(text3));
        Debug.Log("yes");
    }
    // Start is called before the first frame update
    public void ChangeTextScale(Text go,int size)
    {
        go.transform.DOScale(new Vector3(size, size, size), size).SetEase(Ease.OutQuad);
        //Debug.Log("kaku");
    }
    IEnumerator Text3Coroutine(Text go)
    {
        ChangeTextScale(go,1);
        yield return new WaitForSeconds(1f);
        ChangeTextScale(go, 0);
        StartCoroutine(Text2Coroutine(text2));
    }
    IEnumerator Text2Coroutine(Text go)
    {
        ChangeTextScale(go,1);
        yield return new WaitForSeconds(1f);
        ChangeTextScale(go, 0);
        StartCoroutine(Text1Coroutine(text1));
    }
    IEnumerator Text1Coroutine(Text go)
    {
        ChangeTextScale(go,1);
        yield return new WaitForSeconds(1f);
        ChangeTextScale(go, 0);
        StartCoroutine(TextGoCoroutine(textGo));
    }
    IEnumerator TextGoCoroutine(Text go)
    {
        ChangeTextScale(go,1);
        yield return new WaitForSeconds(1f);
        ChangeTextScale(go, 0);
    }
}
