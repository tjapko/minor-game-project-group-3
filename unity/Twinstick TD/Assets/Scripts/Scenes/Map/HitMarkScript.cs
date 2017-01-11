using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/// <summary>
/// Hitmark Script
/// </summary>
public class HitMarkScript : MonoBehaviour {

    //Public variables
    [Header("Global Variables")]
    public float m_turnrate = 100f;
    public float m_lifetime = 1.0f;
    public Vector3 m_offset = new Vector3(0,5,0);

    //References
    [Header("References")]
    public Text m_TextBox;    //Reference to the Textbox of the Hitmark canvas
    [HideInInspector] private GameObject m_MainCamera; //Reference to the main camera (set by gameobject that places the speechbubble)

    //Private variables
    private float m_damage = 0;          //

    // Use this for initialization
    void Start () {
        //Reposition the game object
        gameObject.transform.position = gameObject.transform.position + m_offset;
        //Destroy the game object
        Destroy(gameObject, m_lifetime);
    }

    //Function to look at camera
    public void lookToCamera()
    {
        Vector3 dir = m_MainCamera.transform.position - gameObject.transform.position;
        Quaternion dir_to_face = Quaternion.LookRotation(dir);
        //Vector3 rotation = Quaternion.Lerp(m_MainCamera.transform.rotation, dir_to_face, Time.deltaTime * m_turnrate).eulerAngles;
        gameObject.transform.rotation = dir_to_face;
    }

    //Setter for damage
    public void setDamage(float dmg)
    {
        m_damage = dmg;
        m_TextBox.text = m_damage.ToString();
    }

    //Setter for camera
    public void setCamera(GameObject camera)
    {
        m_MainCamera = camera;
    }
}
