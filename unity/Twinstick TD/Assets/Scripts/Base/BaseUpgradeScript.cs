using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseUpgradeScript : MonoBehaviour {
    //Temp variables
    string use_button = "f";        //Set to use button of player[i]

    //References
    public GameObject m_baseupgradeUI;  //Reference set by UIManager:CanvasBaseUpgrade
    private GameObject player;      //Reference to player Temp solution

    //Public Variables
    public int m_baserange;             //Range of base 
    public int cost_restore_1;          //Restore base health
    public int cost_restore_2;          //Restore Player Health
    public List<int> cost_upgrade_1;    //Cost of upgrade type_1 : Upgrade basehealth + Turret
    public List<int> cost_upgrade_2;    //Cost of upgrade type_2 : Upgrade Player Health
    //public List<int> cost_upgrade_3;    //Cost of upgrade type_3 
    //public List<int> cost_upgrade_4;    //Cost of upgrade type_4

    //Private Variables
    private List<List<int>> upgrade_list;

    // Use this for initialization
    void Start () {
        //Temp sollution to find one player
        GameObject player = GameObject.FindWithTag("Player");

        //Fill upgrade list
        upgrade_list = new List<List<int>>();
        upgrade_list.Add(cost_upgrade_1);
        upgrade_list.Add(cost_upgrade_2);
    }
	
	// Update is called once per frame
	void Update () {
        //Fix:Should check every player use_button
        if (Input.GetKeyUp(use_button))
        {
            //Temp solution
            
            if(Vector3.Distance(player.transform.position, gameObject.transform.position) < m_baserange)
            {
                m_baseupgradeUI.SetActive(true);
            }
        }
    }

    //remove first entry of list
    public void removeUpgrade(List<int> cost_upgrade_list)
    {
        if(cost_upgrade_list.Count > 0)
        {
            cost_upgrade_list.RemoveAt(0);
        }
    }

    // Returns the price of a base upgrade
    public int upgradeBasePrice()
    {
        return cost_upgrade_1.Count > 0 ? cost_upgrade_1[0] : 0;
    }

    // Returns the price of a Player health upgrade
    public int upgradePlayerHealthPrice()
    {
        return cost_upgrade_2.Count > 0 ? cost_upgrade_2[0] : 0;
    }
}
