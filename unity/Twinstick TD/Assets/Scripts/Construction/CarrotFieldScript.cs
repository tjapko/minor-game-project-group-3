using UnityEngine;
using System.Collections;

/// <summary>
/// Script CarrotFieldScript
/// Contains the basic functions for the carrot field
/// </summary>
public class CarrotFieldScript : MonoBehaviour {

    //Public Variables
    public float m_startyield;                //Yield of carrot field at start
    public float m_inc;                       //Yield(n+1) = inc * Yield(n)

    //Private variables
    private float m_currentYield;           //Current yield Y(n)

    void Start()
    {
        m_currentYield = m_startyield;
    }

    //Initialize variables
    public int waveYield()
    {
        
        m_currentYield = m_currentYield * m_inc;
        return (int)m_currentYield;
    }

}
