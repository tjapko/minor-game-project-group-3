using UnityEngine;
using System.Collections;

public class MudScript : MonoBehaviour {
    //Public variables
	public float mudTime; //time mud is in the scene

    //References
    private PlayerConstruction m_constructionScript;    //Reference to construction script
    private UserObjectStatistics stats;

	// Use this for initialization
	void Start () {
		stats = gameObject.GetComponent<UserObjectStatistics> ();
		Invoke ("OnDeath", mudTime);
	}

    //Ondeath function
	public void OnDeath(){
        m_constructionScript.removeObject(gameObject);
        stats.onDeath();
        CancelInvoke ("OnDeath");
	}

    //Set player construction script
    public void setPlayerConstruction(PlayerConstruction script)
    {
        m_constructionScript = script;
    }
}
