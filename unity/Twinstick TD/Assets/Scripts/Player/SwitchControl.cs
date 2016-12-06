using UnityEngine;
using System.Collections;

public class SwitchControl : MonoBehaviour {

	public PlayerMovement playerMovement; // PlayerMovement script for updating boolean useController 

	// Update is called once per frame
	void Update () {
	
		// detect mouse input, if so set boolean useController to false! 
		// mouseHovering for orientation 
		if (Input.GetAxisRaw ("Mouse X") != 0.0f || Input.GetAxisRaw ("Mouse Y") != 0.0f) {
			playerMovement.useController = false;
		}

		// detect controller input 
		// rightJoystick for orientation 
		if (Input.GetAxisRaw ("RightJoystickHorizontalMacXBOX") != 0.0f || Input.GetAxisRaw ("RightJoystickVerticalMacXBOX") != 0.0f || 
			Input.GetAxisRaw ("RightJoystickHorizontalWindowsXBOX") != 0.0f || Input.GetAxisRaw ("RightJoystickVerticalWindowsXBOX") != 0.0f ) {
			playerMovement.useController = true;
		}

	}
}
