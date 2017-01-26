using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HSController : MonoBehaviour
{
	
    private string secretKey = "mySecretKey"; // Edit this value and make sure it's the same as the one stored on the server
    
    public string highscoreURL = "https://insyprojects.ewi.tudelft.nl/ewi3620tu3/HSData.php";
	public string[] items;
	private GameObject m_HighScoreMenu; 
	private GameObject m_backbutton;

	public void StartInitialization()
    {
		//Set references to children
		m_HighScoreMenu = gameObject.transform.GetChild(0).gameObject;
		m_backbutton = m_HighScoreMenu.transform.GetChild(6).gameObject;
		//Activate game over menu
		m_HighScoreMenu.SetActive(true);
        StartCoroutine(GetScores());
    }
 
	 
    // Get the scores from the MySQL DB to display in a GUIText.
    // remember to use StartCoroutine when calling this function!
    IEnumerator GetScores()
	{
		
	
		WWW itemsData = new WWW ("https://insyprojects.ewi.tudelft.nl/ewi3620tu3/HSData.php");
		yield return itemsData;

		string itemsDataString = itemsData.text;
		print (itemsDataString);
		items = itemsDataString.Split (';');

		if (itemsData.error != null) {
			print ("There was an error getting the high score: " + itemsData.error);
		}
        
		for (int i = 0; i < 5; i++) {
			m_HighScoreMenu = gameObject.transform.GetChild(0).gameObject;
			Text scoretext = m_HighScoreMenu.transform.GetChild(i+1).GetComponent<Text>();
			int j = i + 1;
			scoretext.text = j + ". " + items [i]; //this is a GUIText that will display the scores in game.
		}
	}

}


















