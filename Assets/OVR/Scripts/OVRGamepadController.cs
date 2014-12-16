/************************************************************************************

Filename    :   OVRGamepadController.cs
Content     :   Interface to XBox360 controller
Created     :   January 8, 2013
Authors     :   Peter Giokaris

Copyright   :   Copyright 2013 Oculus VR, Inc. All Rights reserved.

Use of this software is subject to the terms of the Oculus LLC license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

************************************************************************************/

using UnityEngine;
#if UNITY_STANDALONE_WIN	
using XInputDotNetPure;
#else
// Different input device interface for other platforms goes here
#endif

//-------------------------------------------------------------------------------------
// ***** OVRGamepadController
//
// OVRGamepadController is an interface class to a gamepad controller.
//
// On Windows machines, the gamepad must be XInput-compliant.
//
public class OVRGamepadController : MonoBehaviour
{
// Only Windows supports XInput-compliant controllers
#if UNITY_STANDALONE_WIN	

    private bool 				playerIndexSet = false;
    private PlayerIndex 		playerIndex;
    
	public static GamePadState 	state;
 	
	private GamePadState 		testState;
	
	// * * * * * * * * * * * * *
	
 	// Start
	void Start()
    {
    }
	
	// Update
    void Update()
    {
        // Find a PlayerIndex, for a single player game
        if (!playerIndexSet)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex pidx = (PlayerIndex)i;
                testState = GamePad.GetState(pidx);
               
				if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad {0} found", pidx));
                    playerIndex = pidx;
                    playerIndexSet = true;
                }
            }
        }

        state = GamePad.GetState(playerIndex);  
    }
	
	// * * * * * * * * * * * * *
	// Analog
	public static float GetAxisLeftX()
	{
		return state.ThumbSticks.Left.X;
	}
	public static float GetAxisLeftY()
	{
		return state.ThumbSticks.Left.Y;
	}
	public static float GetAxisRightX()
	{
		return state.ThumbSticks.Right.X;
	}
	public static float GetAxisRightY()
	{
		return state.ThumbSticks.Right.Y;
	}
	public static float GetTriggerLeft()
	{
		return state.Triggers.Left;
	}
	public static float GetTriggerRight()
	{
		return state.Triggers.Right;
	}
	// * * * * * * * * * * * * *
	// DPad
	public static bool GetDPadUp()
	{	
		if(state.DPad.Up == ButtonState.Pressed) return true;
		return false;
	}
	public static bool GetDPadDown()
	{	
		if(state.DPad.Down == ButtonState.Pressed) return true;
		return false;
	}
	public static bool GetDPadLeft()
	{	
		if(state.DPad.Left == ButtonState.Pressed) return true;
		return false;
	}
	public static bool GetDPadRight()
	{	
		if(state.DPad.Right == ButtonState.Pressed) return true;
		return false;
	}
	// * * * * * * * * * * * * *
	// Buttons
	public static bool GetButtonStart()
	{
		if(state.Buttons.Start == ButtonState.Pressed) return true;
		return false;
	}
	public static bool GetButtonBack()
	{
		if(state.Buttons.Back == ButtonState.Pressed) return true;
		return false;
	}
	public static bool GetButtonA()
	{
		if(state.Buttons.A == ButtonState.Pressed) return true;
		return false;
	}
	public static bool GetButtonB()
	{
		if(state.Buttons.B == ButtonState.Pressed) return true;
		return false;
	}
	public static bool GetButtonX()
	{
		if(state.Buttons.X == ButtonState.Pressed) return true;
		return false;
	}
	public static bool GetButtonY()
	{
		if(state.Buttons.Y == ButtonState.Pressed) return true;
		return false;
	}
	public static bool GetButtonLShoulder()
	{
		if(state.Buttons.LeftShoulder == ButtonState.Pressed) return true;
		return false;
	}
	public static bool GetButtonRShoulder()
	{
		if(state.Buttons.RightShoulder == ButtonState.Pressed) return true;
		return false;
	}
	public static bool GetButtonLStick()
	{
		if(state.Buttons.LeftStick == ButtonState.Pressed) return true;
		return false;
	}
	public static bool GetButtonRStick()
	{
		if(state.Buttons.RightStick == ButtonState.Pressed) return true;
		return false;
	}
#else
	public static float GetAxisLeftX()
	{
		return 0;
	}
	public static float GetAxisLeftY()
	{
		return 0;
	}
	public static float GetAxisRightX()
	{
		return 0;
	}
	public static float GetAxisRightY()
	{
		return 0;
	}
	public static float GetTriggerLeft()
	{
		return 0;
	}
	public static float GetTriggerRight()
	{
		return 0;
	}
	// DPad
	public static bool GetDPadUp()
	{	
		return false;
	}
	public static bool GetDPadDown()
	{	
		return false;	
	}
	public static bool GetDPadLeft()
	{	
		return false;
	}
	public static bool GetDPadRight()
	{
		return false;
	}
	// Buttons
	public static bool GetButtonStart()
	{
		return false;
	}
	public static bool GetButtonBack()
	{
		return false;
	}
	public static bool GetButtonA()
	{
		return false;
	}
	public static bool GetButtonB()
	{
		return false;
	}
	public static bool GetButtonX()
	{
		return false;
	}
	public static bool GetButtonY()
	{
		return false;
	}
	public static bool GetButtonLShoulder()
	{
		return false;
	}
	public static bool GetButtonRShoulder()
	{
		return false;
	}
	public static bool GetButtonLStick()
	{
		return false;
	}
	public static bool GetButtonRStick()
	{
		return false;
	}
#endif
}
