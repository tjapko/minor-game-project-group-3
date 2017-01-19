using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsScript : MonoBehaviour {

    //References
    private GameManager m_gamemanager;  //Reference to game manager
    //Mixer
    public AudioMixer m_mixer;         //Reference to the audio mixer
    //UI
    private Toggle tgl_master;  //Reference to the toggle of the masater
    private Toggle tgl_bgm; //Reference to the toggle of background music
    private Toggle tgl_sfx;             //Reference to the toggle of background music
    private Slider slr_master;      //Reference to the slider of the master
    private Slider slr_bgm; //Reference to the slider of background music
    private Slider slr_sfx;             //Reference to the slider of background music

    //Private variables
    private bool initialized = false;   //Boolean if this gameobject is initialized

	// Use this for initialization
	public void StartInitialzation () {
        GameObject go_master = gameObject.transform.GetChild(1).gameObject;
        GameObject go_bgm = gameObject.transform.GetChild(2).gameObject;
        GameObject go_sfx = gameObject.transform.GetChild(3).gameObject;
        //m_mixer = Resources.Load("MainMix") as AudioMixer;

        //Get references
        float vol_master = PlayerPrefs.GetFloat("vol_Master", 0f);
        float vol_sfx = PlayerPrefs.GetFloat("vol_sfx", 0f);
        float vol_bgm = PlayerPrefs.GetFloat("vol_bgm", 0f);

        //Set referencces
        m_gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<GameManager>();

        tgl_master = go_master.transform.GetChild(1).GetComponent<Toggle>();
        slr_master = go_master.transform.GetChild(2).GetComponent<Slider>();

        tgl_bgm = go_bgm.transform.GetChild(1).GetComponent<Toggle>();
        slr_bgm = go_bgm.transform.GetChild(2).GetComponent<Slider>();

        tgl_sfx = go_sfx.transform.GetChild(1).GetComponent<Toggle>();
        slr_sfx = go_sfx.transform.GetChild(2).GetComponent<Slider>();

        //Set variables
        initialized = true;
        slr_master.value = vol_master;
        slr_bgm.value = vol_bgm; 
        slr_sfx.value = vol_sfx;
        m_mixer.SetFloat("volMaster", vol_master);
        m_mixer.SetFloat("volBGM", vol_bgm);
        m_mixer.SetFloat("volSFX", vol_sfx);
        m_mixer.SetFloat("volWalking", vol_sfx);
    }
	
    //On Enable function
    private void Update()
    {
        UpdateUI();
    }

	// Update is called once per frame
	void UpdateUI () {
        if(initialized)
        {
            slr_master.interactable = tgl_master.isOn;
            slr_bgm.interactable = tgl_bgm.isOn;
            slr_sfx.interactable = tgl_sfx.isOn;

            m_mixer.SetFloat("volMaster", tgl_master.isOn? (slr_master.value == slr_master.minValue? -80f: slr_master.value) : -80f);
            m_mixer.SetFloat("volBGM", tgl_bgm.isOn ? (slr_bgm.value == slr_bgm.minValue ? -80f : slr_bgm.value) : -80f );
            m_mixer.SetFloat("volSFX", tgl_sfx.isOn ? (slr_sfx.value == slr_sfx.minValue ? -80f : slr_sfx.value) : -80f);
            m_mixer.SetFloat("volWalking", tgl_sfx.isOn ? (slr_sfx.value == slr_sfx.minValue ? -80f : slr_sfx.value) : -80f);


        }
	}

    //Set variables
    public void OnDisable()
    {
        PlayerPrefs.SetFloat("vol_Master", slr_master.value);
        PlayerPrefs.SetFloat("vol_sfx", slr_sfx.value);
        PlayerPrefs.SetFloat("vol_bgm", slr_bgm.value);
    }
}
