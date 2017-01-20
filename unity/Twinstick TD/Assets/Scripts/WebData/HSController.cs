using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HSController : MonoBehaviour
{
	
    private string secretKey = "mySecretKey"; // Edit this value and make sure it's the same as the one stored on the server
    public string addScoreURL = "https://insyprojects.ewi.tudelft.nl/ewi3620tu3/addscore.php?"; //be sure to add a ? to your url
    public string highscoreURL = "https://insyprojects.ewi.tudelft.nl/ewi3620tu3/HSData.php";
	public string[] items;
	private GameObject m_HighScoreMenu; 

    void Start()
    {
        StartCoroutine(GetScores());


    }
 
    // remember to use StartCoroutine when calling this function!
    IEnumerator PostScores(string Name, int Score)
    {
		print ("recieved");
        //This connects to a server side php script that will add the name and score to a MySQL DB.
        // Supply it with a string representing the players name and the players score.
		var hash=Md5.Md5Sum(Name + Score + secretKey);

		string post_url = addScoreURL + "Name=" + WWW.EscapeURL(Name) + "&Score=" + Score + "&hash=" + hash;
		print (post_url);
        // Post the URL to the site and create a download object to get the result.
		WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done
		string itemsDataString = hs_post.text;
		print (itemsDataString);
		items = itemsDataString.Split(';');
        if (hs_post.error != null)
        {
            print("There was an error posting the high score: " + hs_post.error);
        }
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
        
		for (int i = 0; i < 4; i++) {
			m_HighScoreMenu = gameObject.transform.GetChild(0).gameObject;
			Text scoretext = m_HighScoreMenu.transform.GetChild(i+1).GetComponent<Text>();

			scoretext.text = i + 1 + items [i]; //this is a GUIText that will display the scores in game.
		}
	}

}




















