using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class MainmenuSettingsScript : MonoBehaviour {
    //Volume
    public AudioSource m_background_source;    //Reference to audio source

    //UI
    private Toggle tgl_master;  //Reference to the toggle of the masater
    private Toggle tgl_bgm;     //Reference to the toggle of background music
    private Toggle tgl_sfx;     //Reference to the toggle of background music
    private Slider slr_master;  //Reference to the slider of the master
    private Slider slr_bgm;     //Reference to the slider of background music
    private Slider slr_sfx;     //Reference to the slider of background music
    private Toggle tgl_help;    //Reference to the toggle of the help canvas

    //Private variables
    private bool initialized = false;   //Boolean if this gameobject is initialized

    // Use this for initialization
    public void Start()
    {
        GameObject go_master = gameObject.transform.GetChild(1).gameObject;
        GameObject go_bgm = gameObject.transform.GetChild(2).gameObject;
        GameObject go_sfx = gameObject.transform.GetChild(3).gameObject;
        GameObject go_help = gameObject.transform.GetChild(4).gameObject;
        //m_mixer = Resources.Load("MainMix") as AudioMixer;

        //Get references
        float vol_master = PlayerPrefs.GetFloat("vol_Master", 0f);
        float vol_sfx = PlayerPrefs.GetFloat("vol_sfx", 0f);
        float vol_bgm = PlayerPrefs.GetFloat("vol_bgm", 0f);
        bool bool_help = PlayerPrefs.GetInt("option_helpscreen", 1) == 1;   // 1 means true : show help, 0 false : do not show help

        //Set references
        tgl_master = go_master.transform.GetChild(1).GetComponent<Toggle>();
        slr_master = go_master.transform.GetChild(2).GetComponent<Slider>();

        tgl_bgm = go_bgm.transform.GetChild(1).GetComponent<Toggle>();
        slr_bgm = go_bgm.transform.GetChild(2).GetComponent<Slider>();

        tgl_sfx = go_sfx.transform.GetChild(1).GetComponent<Toggle>();
        slr_sfx = go_sfx.transform.GetChild(2).GetComponent<Slider>();

        tgl_help = go_help.transform.GetChild(1).GetComponent<Toggle>();

        //Set variables
        initialized = true;
        tgl_master.isOn = (vol_master != -80);
        tgl_bgm.isOn = (vol_bgm != -80);
        tgl_sfx.isOn = (vol_sfx != -80);
        tgl_help.isOn = bool_help;
        slr_master.value = vol_master;
        slr_bgm.value = vol_bgm;
        slr_sfx.value = vol_sfx;

    }

    //On Enable function
    private void Update()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void UpdateUI()
    {
        if (initialized)
        {
            slr_master.interactable = tgl_master.isOn;
            slr_bgm.interactable = tgl_bgm.isOn;
            slr_sfx.interactable = tgl_sfx.isOn;

            //m_background_source.volume = Math.Pow(10.0f, ((float)slr_bgm.value)/2.0f) ;
        }
    }

    //Set variables
    public void OnDisable()
    {
        PlayerPrefs.SetFloat("vol_Master", slr_master.value);
        PlayerPrefs.SetFloat("vol_sfx", slr_sfx.value);
        PlayerPrefs.SetFloat("vol_bgm", slr_bgm.value);
        PlayerPrefs.SetInt("option_helpscreen", tgl_help.isOn?1:0);
    }
}
