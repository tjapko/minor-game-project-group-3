using UnityEngine;
using System;

[Serializable]
public class BaseManager
{
    public Transform m_SpawnPoint;  //Spawn position of base

    [HideInInspector] public GameObject m_Instance; //Reference to instance of base
    private Basehealth m_basehealth;                //Reference to base health script

    public void Setup()
    {
        //Set reference
        m_basehealth = m_Instance.GetComponent<Basehealth>();
    }

    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
