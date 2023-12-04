using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;
public class GameOverManager : MonoBehaviour
{
    InfoManager im;
    [SerializeField] GameObject quitButton;
    [SerializeField] GameObject restartButton;
    [SerializeField] Image panel;
    [SerializeField] Text gameOverText;
    [SerializeField] Text gameClearText;
    [SerializeField] Text resultText;
    [SerializeField] CinemachineVirtualCamera vcam;
    [SerializeField] CinemachineVirtualCamera vcamClear;
    // Start is called before the first frame update
    void OnEnable()
    {
        resultText.gameObject.SetActive(false);
        quitButton.SetActive(false);
        restartButton.SetActive(false);
        im = InfoManager.Instance;
        gameOverText.rectTransform.DOScale(new Vector3(0, 0, 0), 0f);
        gameClearText.rectTransform.DOScale(new Vector3(0, 0, 0), 0f);
        if (im.isClear == false)
        {
            GameEnd(panel, gameOverText, 0);
            resultText.text = "Last Wave : Wave " + im.waveNumber;
            im.cameraPriority++;
            Debug.Log("gameover");
            vcam.Priority = im.cameraPriority;
        }
        else if(im.isClear)
        {
            GameEnd(panel, gameClearText, 255);
            im.cameraPriority++;
            Debug.Log("gameclear");
            vcamClear.Priority = im.cameraPriority;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameEnd(Image image,Text te,int co)
    {
        image.DOColor(new Color(co, co, co, 255), 0f).SetEase(Ease.InQuart).
            OnComplete(() => {   image.DOColor(new Color(co, co, co, 0), 2f).SetEase(Ease.OutQuad).
                OnComplete(()=> { te.transform.DOScale(new Vector3(1, 1, 1), 2f).SetEase(Ease.OutQuad).
                    OnComplete(()=> { ButtonAwake(); }); }); });
    }
    public void ButtonAwake()
    {
        resultText.gameObject.SetActive(true);
        quitButton.SetActive(true);
        if(im.isClear == false)
        restartButton.SetActive(true);
    }
}
