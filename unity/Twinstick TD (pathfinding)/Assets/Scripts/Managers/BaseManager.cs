using UnityEngine;
using System;

/// <summary>
/// Class BaseManager 
/// </summary>
[Serializable]
public class BaseManager
{
    //public variables
    public Transform m_SpawnPoint;  //Spawn position of base
    [HideInInspector] public GameObject m_Instance; //Reference to instance of base

    //private variables
    private Basehealth m_basehealth;                //Reference to base health script

    //Setup
    public void Setup()
    {
        //Set reference to script
        m_basehealth = m_Instance.GetComponent<Basehealth>();
    }

    //Reset function
    public void Reset()
    {
        //Reset base position and direction
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        //Reset active value
        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
