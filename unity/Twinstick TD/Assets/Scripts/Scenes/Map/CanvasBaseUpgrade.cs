using UnityEngine;
using System.Collections;

public class CanvasBaseUpgrade : MonoBehaviour {

    //Reference
    private GameObject m_base;
    private BaseUpgradeScript m_baseupgradescript;
	// Use this for initialization
	void Start () {
        m_base = GameObject.FindWithTag("Base");

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void purcaseUpgrade(int upgrade_index)
    {

    }
}
