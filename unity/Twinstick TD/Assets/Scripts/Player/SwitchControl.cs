using UnityEngine;
using System.Collections;

public class SwitchControl : MonoBehaviour {

	// public variables 
	[HideInInspector]public int m_PlayerNumber = 1;  //Used to identify differnt players. Needs to be updated automatically when multiplayed-mode
	[HideInInspector]public PlayerManager player;
	public PlayerMovement playerMovement; // PlayerMovement script for updating boolean useController 

	// Update is called once per frame
	void Update () {
		// detect mouse input, if so set (boolean) useController to false! 
		// mouseHovering for orientation 
		if (Input.GetAxisRaw ("Mouse X_" + m_PlayerNumber) != 0.0f || 
			Input.GetAxisRaw ("Mouse Y_" + m_PlayerNumber) != 0.0f) {
			playerMovement.useController = false;
		}

		// detect controller input 
		// rightJoystick for orientation 
		if (Input.GetAxisRaw ("RightJoystickHorizontalMac_"+ (m_PlayerNumber)) != 0.0f || 
			Input.GetAxisRaw ("RightJoystickVerticalMac_"+ (m_PlayerNumber)) != 0.0f || 
			Input.GetAxisRaw ("RightJoystickHorizontalWindowsXBOX_"+ (m_PlayerNumber)) != 0.0f || 
			Input.GetAxisRaw ("RightJoystickVerticalWindowsXBOX_"+ (m_PlayerNumber)) != 0.0f ) {
			playerMovement.useController = true;
		}
	}

}
