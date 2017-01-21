using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class CanvasGameOverScript : MonoBehaviour {

    //References to managers
    private GameManager m_gamemanager;      //Reference to game manager (used to invoke next wave)
    private UserManager m_usermanager;      //Reference to the usermanager in the game manager
    private WaveManager m_wavemanager;
    private List<PlayerManager> m_players;  //Reference to the players in the game 

    //References to GameObject
    private GameObject m_gameovermenu;  //Reference to the game over menu
    private GameObject m_restartbutton; //Reference to the restart button
	private GameObject m_HighScoreCanvas;
	private string addScoreURL = "https://insyprojects.ewi.tudelft.nl/ewi3620tu3/addscore.php?"; //be sure to add a ? to your url
	private string secretKey = "mySecretKey"; // Edit this value and make sure it's the same as the one stored on the server
	private int scorevalue; //Score to be used for twitter and database
	private InputField inputfield;
	private HSController m_HSController;      //Reference to CanvasGameOverScript

    public void StartInitialization()
    {
        //Find gamemanager
        m_gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<GameManager>();
        m_usermanager = m_gamemanager.getUserManager();
        m_players = m_usermanager.m_playerlist;
        m_wavemanager = m_gamemanager.getWaveManager();

        //Set references to children
        m_gameovermenu = gameObject.transform.GetChild(0).gameObject;
		m_HighScoreCanvas = gameObject.transform.GetChild(1).gameObject;
        m_restartbutton = m_gameovermenu.transform.GetChild(2).gameObject;
        //Activate game over menu
        m_gameovermenu.SetActive(true);

    }

    // Show/hide the gameover menu
    public void showGameoverMenu(bool status)
    {
        m_gameovermenu.SetActive(status);
    }

	// Back menu
	public void GameoverMenu()
	{
		m_gameovermenu = gameObject.transform.GetChild(0).gameObject;
		m_HighScoreCanvas = gameObject.transform.GetChild(1).gameObject;
		m_gameovermenu.SetActive(true);
		m_HighScoreCanvas.SetActive (false);
	}

	// Show/hide the score menu
	public void showHighScore()
	{
		m_gameovermenu = gameObject.transform.GetChild(0).gameObject;
		m_HighScoreCanvas = gameObject.transform.GetChild(1).gameObject;
		m_HSController = m_HighScoreCanvas.GetComponent<HSController> ();
		m_gameovermenu.SetActive(false);
		m_HighScoreCanvas.SetActive (true);
		m_HSController.StartInitialization();


	}

    // Get the score of the player
    public void setScore()
    {
        Text scoretext = m_gameovermenu.transform.GetChild(1).GetComponent<Text>();
        scoretext.text = "";

        List<PlayerManager> playerlist = m_gamemanager.getUserManager().m_playerlist;
        int amountofplayers = playerlist.Count;
        int[] score = new int[amountofplayers];

        for (int i = 0; i < amountofplayers; i++)
        {
            PlayerManager player = playerlist[i];
            score[i] = player.m_stats.getkills() * 10 + player.m_stats.getCurrency();
			scoretext.text += "   Score : " + score[i];
			scorevalue = score [0];
        }
    }

    // Restart Scee
    public void RestartScene(int sceneindex)
    {
        m_restartbutton.GetComponent<Button>().interactable = false;
        m_restartbutton.transform.GetChild(0).GetComponent<Text>().text = "Restarting Game";
        NextScene(sceneindex);
    }


    // Load next scene
    public void NextScene(int sceneindex)
    {
        SceneManager.LoadScene(sceneindex);
    }
	//recives input from the field
	public void input(){
		m_gameovermenu = gameObject.transform.GetChild(0).gameObject;
		inputfield = m_gameovermenu.transform.GetChild (2).GetComponent<InputField> ();
		string name = inputfield.text;
		inputfield.interactable = false;
		StartCoroutine(PostScores(name, scorevalue));

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
		if (hs_post.error != null)
		{
			print("There was an error posting the high score: " + hs_post.error);
		}
	}

    public void ShareToTwitter()
    {
        string url = "http://twitter.com/intent/tweet?text=I+just+scored+";
        url += scorevalue;
        url += "+in+this+amazing+game";
        Application.OpenURL(url);
    }
}
