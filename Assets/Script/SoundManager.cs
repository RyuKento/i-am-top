using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class SoundManager : MonoBehaviour
{
    public float masterVolume = 1.0f;
    public float bgmVolume = 1.0f;
    public float seVolume = 1.0f;

    [SerializeField] Slider masterSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;
    [SerializeField] AudioSource bgmAudio;
    [SerializeField] AudioSource seAudio;
    [SerializeField] AudioMixerGroup masterMixer;
    [SerializeField] AudioMixerGroup bgmMixer;
    [SerializeField] AudioMixerGroup seMixer;

    bool isMaster;
    bool isBGM;
    bool isSE;

    // Start is called before the first frame update
    void Start()
    {
        if (masterSlider)
            masterSlider.value = masterVolume;
        if (bgmSlider)
            bgmSlider.value = bgmVolume;
        if (seSlider)
            seSlider.value = seVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBGM || isMaster)
        {
            bgmAudio.volume = bgmVolume * masterVolume;
            if (isBGM) isBGM = false;
            if (isMaster) isMaster = false;
        }
        else if(isSE || isMaster)
        {
            seAudio.volume = seVolume * masterVolume;
            if (isSE) isSE = false;
            if (isMaster) isMaster = false;
        }
    }

    public void SetMasterVolume()
    {
        masterVolume = masterSlider.value;
        isMaster = true;
    }
    public void SetBgmVolume()
    {
        bgmVolume = bgmSlider.value;
        isBGM = true;
    }
    public void SetSeVolume()
    {
        seVolume = seSlider.value;
        isSE = true;
    }
}
