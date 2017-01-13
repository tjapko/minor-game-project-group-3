using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeechBubbleScript : MonoBehaviour {

    //Public variables
    [Header("Global Variables")]
    public float m_lifetime = 1.0f;
    public Vector3 m_offset = new Vector3(0, 5, 0);

    //References
    [Header("References")]
    public Text m_TextBox;              //Reference to the Textbox of the Hitmark canvas
    private GameObject m_MainCamera;    //Reference to the main camera

    //Private variables
    private string bubble_text;

    // Use this for initialization
    void Start()
    {
        m_MainCamera = GameObject.FindWithTag("MainCamera");

        gameObject.transform.position = gameObject.transform.position + m_offset;
        lookToCamera();

        Destroy(gameObject, m_lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        m_TextBox.text = bubble_text;
    }

    //Function to look at camera
    private void lookToCamera()
    {
        Vector3 dir = m_MainCamera.transform.position - gameObject.transform.position;
        gameObject.transform.rotation = Quaternion.LookRotation(dir);
    }

    //Function to set damage
    public void setText(string text)
    {
        bubble_text = text;
    }
}
