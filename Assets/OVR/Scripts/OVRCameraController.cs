﻿/************************************************************************************

Filename    :   OVRCameraController.cs
Content     :   Camera controller interface. 
				This script is used to interface the OVR cameras.
Created     :   January 8, 2013
Authors     :   Peter Giokaris

Copyright   :   Copyright 2013 Oculus VR, Inc. All Rights reserved.

Use of this software is subject to the terms of the Oculus LLC license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

//-------------------------------------------------------------------------------------
// ***** OVRCameraController
//
// OVRCameraController is a component that allows for easy handling of the lower level cameras.
// It is the main interface between Unity and the cameras. 
// This is attached to a prefab that makes it easy to add a Rift into a scene.
//
// All camera control should be done through this component.
//
public class OVRCameraController : OVRComponent
{	
	// PRIVATE MEMBERS
	private bool   UpdateCamerasDirtyFlag = false;	
	private Camera CameraLeft, CameraRight = null;
	private float  IPD = 0.064f; 							// in millimeters
	private float  IPDOffsetLeft, IPDOffsetRight = 0.0f; 	// normalized screen space	
	private float  LensOffsetLeft, LensOffsetRight = 0.0f;  // normalized screen space
	private float  VerticalFOV = 90.0f;	 					// in degrees
	private float  AspectRatio = 1.0f;						
	private float  DistK0, DistK1, DistK2, DistK3 = 0.0f; 	// lens distortion parameters
	
	// Initial direction of camera controller
	private Vector3 DirectionOffset = Vector3.zero;	
	// Set Y rotation here; this will influence the y rotation of the cameras
	private float   YRotation       = 0.0f;
	
	// PUBLIC MEMBERS
	// Camera positioning:
	// From root of camera to neck (translation only)
	public Vector3 NeckPosition = new Vector3(0.0f, 0.7f,  0.0f);	
	// From neck to eye (rotation and translation; x will be different for each eye)
	public Vector3 EyeCenterPosition = new Vector3(0.0f, 0.15f, 0.09f);
	// Set to true if we want the rotation of the camera controller to be influenced by the camera
	public bool  CameraRotatesY	= true;
	// Set the background color for both cameras
	public Color BackgroundColor = new Color(0.192f, 0.302f, 0.475f, 1.0f);
	
	
	// * * * * * * * * * * * * *
		
	// Awake
	new void Awake()
	{
		base.Awake();
	}

	// Start
	new void Start()
	{
		base.Start();
		
		// Get the cameras
		Camera[] cameras = gameObject.GetComponentsInChildren<Camera>();
		
		for (int i = 0; i < cameras.Length; i++)
		{
			if(cameras[i].name == "CameraLeft")
				CameraLeft = cameras[i];
			
			if(cameras[i].name == "CameraRight")
				CameraRight = cameras[i];
		}
		
		if((CameraLeft == null) || (CameraRight == null))
			Debug.LogWarning("WARNING: Unity Cameras in OVRCameraController not found!");
		
		// Get the required Rift infromation needed to set cameras
		InitCameraControllerVariables();
		
		// Initialize the cameras
		UpdateCamerasDirtyFlag = true;
		UpdateCameras();
		
		SetMaximumVisualQuality();
		
	}
		
	// Update 
	new void Update()
	{
		base.Update();		
		UpdateCameras();
	}
		
	// InitCameraControllerVariables
	// Made public so that it can be called by classes that require information about the
	// camera to be present when initing variables in 'Start'
	public void InitCameraControllerVariables()
	{
		// Get the IPD value (distance between eyes in meters)
		OVRDevice.GetIPD(ref IPD);

		// Get the values for both IPD and lens distortion correction shift. We don't normally
		// need to set the PhysicalLensOffset once it's been set here.
		OVRDevice.GetPhysicalLensOffsetsFromIPD(IPD, ref IPDOffsetLeft, ref IPDOffsetRight);
		LensOffsetLeft  = IPDOffsetLeft;
		LensOffsetRight = IPDOffsetRight;
		
		// Using the calculated FOV, based on distortion parameters, yeilds the best results.
		// However, public functions will allow to override the FOV if desired
		VerticalFOV = OVRDevice.VerticalFOV();
		
		// Store aspect ratio as well
		AspectRatio = OVRDevice.CalculateAspectRatio();
		
		OVRDevice.GetDistortionCorrectionCoefficients(ref DistK0, ref DistK1, ref DistK2, ref DistK3);
		
		// Get our initial direction of the camera controller from the rotation set in the scene
		DirectionOffset = transform.rotation * Vector3.forward;
	}
	
	// InitCameras
	void UpdateCameras()
	{
		// We can safely set certain global camera members without worrying about performance
		OVRCamera.DirectionOffset = DirectionOffset;
		OVRCamera.YRotation = YRotation;
		OVRCamera.SetParentYRotation = CameraRotatesY;
		
		if(UpdateCamerasDirtyFlag == false)
			return;
		
		float lensOffset = 0.5f + (LensOffsetLeft * 0.5f);
		float eyePositionOffset = -IPD * 0.5f;
		ConfigureCamera(ref CameraLeft, lensOffset, IPDOffsetLeft, eyePositionOffset);
		
		lensOffset = 0.5f + (LensOffsetRight * 0.5f);
		eyePositionOffset = IPD * 0.5f;
		ConfigureCamera(ref CameraRight, lensOffset, IPDOffsetRight, eyePositionOffset);
		
		UpdateCamerasDirtyFlag = false;
	}
	
	// SetCamera
	bool ConfigureCamera(ref Camera camera, float lensOffset, float ipdOffset, float eyePositionOffset)
	{
		Vector3 LensOffset = Vector3.zero;
		Vector3 EyePosition = EyeCenterPosition;
				
		// Vertical FOV
		camera.fov = VerticalFOV;
			
		// Aspect ratio 
		camera.aspect = AspectRatio;
			
		// IPD offset for image
		// NOTE: We are recalculating the prespective matrix, so we must make
		// sure that VerticalFOV in camera is set prior to this
		LensOffset.x = ipdOffset;
		camera.GetComponent<OVRCamera>().SetPerspectiveOffset(ref LensOffset);
		
		// Centre of lens correction
		camera.GetComponent<OVRLensCorrection>()._Center.x = lensOffset;
			
		// Lens correction
		ConfigureCameraLensCorrection(ref camera);
			
		// Set camera variables that pertain to the neck and eye position
		// NOTE: We will want to add a scale vlue here in the event that the player 
		// grows or shrinks in the world. This keeps head modelling behaviour
		// accurate
		camera.GetComponent<OVRCamera>().NeckPosition = NeckPosition;
		EyePosition.x = eyePositionOffset; 
			
		camera.GetComponent<OVRCamera>().EyePosition = EyePosition;		
					
		// Background color
		camera.backgroundColor = BackgroundColor;
			
		return true;
	}
	
	// SetCameraLensCorrection
	void ConfigureCameraLensCorrection(ref Camera camera)
	{
		// Get the distortion scale and aspect ratio to use when calculating distortion shader
		float distortionScale = 1.0f / OVRDevice.DistortionScale();
		float aspectRatio     = OVRDevice.CalculateAspectRatio();
		
		// These values are different in the SDK World Demo; Unity renders each camera to a buffer
		// that is normalized, so we will respect this rule when calculating the distortion inputs
		float NormalizedWidth  = 1.0f;
		float NormalizedHeight = 1.0f;
		
		OVRLensCorrection lc = camera.GetComponent<OVRLensCorrection>();
		
		lc._Scale.x     = (NormalizedWidth  / 2.0f) * distortionScale;
		lc._Scale.y     = (NormalizedHeight / 2.0f) * distortionScale * aspectRatio;
		lc._ScaleIn.x   = (2.0f / NormalizedWidth);
		lc._ScaleIn.y   = (2.0f / NormalizedHeight) / aspectRatio;
		lc._HmdWarpParam.x = DistK0;		
		lc._HmdWarpParam.y = DistK1;
		lc._HmdWarpParam.z = DistK2;
	}
	
	///////////////////////////////////////////////////////////
	// PUBLIC FUNCTIONS
	///////////////////////////////////////////////////////////
	
	// SetCameras - Should we want to re-target the cameras
	public void SetCameras(ref Camera cameraLeft, ref Camera cameraRight)
	{
		CameraLeft = cameraLeft;
		CameraRight = cameraRight;
		UpdateCamerasDirtyFlag = true;
	}
	
	// Get/SetIPD 
	public void GetIPD(ref float ipd)
	{
		ipd = IPD;
	}
	public void SetIPD(float ipd)
	{
		IPD = ipd;
		OVRDevice.GetPhysicalLensOffsetsFromIPD(IPD, ref IPDOffsetLeft, ref IPDOffsetRight);
		UpdateCamerasDirtyFlag = true;
		
	}
	
	// Get/SetIPDOffsets (note: Setting IPD directly isn't something we would 
	// normally do, since it is best set by SetIPD, above
	public void GetIPDOffsets(ref float offsetLeft, ref float offsetRight)
	{
		offsetLeft  = IPDOffsetLeft;
		offsetRight = IPDOffsetRight;
	}
	public void SetIPDOffsets(float offsetLeft, float offsetRight)
	{
		IPDOffsetLeft  = offsetLeft;
		IPDOffsetRight = offsetRight;
		UpdateCamerasDirtyFlag = true;
	}
	
	// Get/SetPhysicalLensOffsets 
	public void GetPhysicalLensOffsets(ref float offsetLeft, ref float offsetRight)
	{
		offsetLeft = LensOffsetLeft;
		offsetRight = LensOffsetRight;
	}
	public void SetPhysicalLensOffsets(float offsetLeft, float offsetRight)
	{
		LensOffsetLeft = offsetLeft;
		LensOffsetRight = offsetRight;
		UpdateCamerasDirtyFlag = true;
	}
	
	//Get/SetVerticalFOV
	public void GetVerticalFOV(ref float verticalFOV)
	{
		verticalFOV = VerticalFOV;
	}
	public void SetVerticalFOV(float verticalFOV)
	{
		VerticalFOV = verticalFOV;
		UpdateCamerasDirtyFlag = true;
	}
	
	//Get/SetAspectRatio
	public void GetAspectRatio(ref float aspecRatio)
	{
		aspecRatio = AspectRatio;
	}
	public void SetAspectRatio(float aspectRatio)
	{
		AspectRatio = aspectRatio;
		UpdateCamerasDirtyFlag = true;
	}
	
	// Get/SetDistortionCoefs
	public void GetDistortionCoefs(ref float distK0, 
								   ref float distK1, 
								   ref float distK2, 
		                           ref float distK3)
	{
		distK0 = DistK0;
		distK1 = DistK1;
		distK2 = DistK2;
		distK3 = DistK3;
	}
	public void SetDistortionCoefs(float distK0, 
								   float distK1, 
								   float distK2, 
								   float distK3)
	{
		DistK0 = distK0;
		DistK1 = distK1;
		DistK2 = distK2;
		DistK3 = distK3;
		UpdateCamerasDirtyFlag = true;
	}
	
	// Get/SetNeckPosition
	public void GetNeckPosition(ref Vector3 neckPosition)
	{
		neckPosition = NeckPosition;
	}
	public void SetNeckPosition(Vector3 neckPosition)
	{
		NeckPosition = neckPosition;
		UpdateCamerasDirtyFlag = true;
	}
	
	// Get/SetEyeCenterPosition
	public void GetEyeCenterPosition(ref Vector3 eyeCenterPosition)
	{
		eyeCenterPosition = EyeCenterPosition;
	}
	public void SetEyeCenterPosition(Vector3 eyeCenterPosition)
	{
		EyeCenterPosition = eyeCenterPosition;
		UpdateCamerasDirtyFlag = true;
	}
	
	// Get/SetDirectionOffset
	public void GetDirectionOffset(ref Vector3 directionOffset)
	{
		directionOffset = DirectionOffset;
	}
	public void SetDirectionOffset(Vector3 directionOffset)
	{
		DirectionOffset = directionOffset;
	}
	
	// Get/SetYRotation
	public void GetYRotation(ref float yRotation)
	{
		yRotation = YRotation;
	}
	public void SetYRotation(float yRotation)
	{
		YRotation = yRotation;
	}
	
	// Get/SetCameraRotatesY
	public void GetCameraRotatesY(ref bool cameraRotatesY)
	{
		cameraRotatesY = CameraRotatesY;
	}
	public void SetCameraRotatesY(bool cameraRotatesY)
	{
		CameraRotatesY = cameraRotatesY;
	}

	
	///////////////////////////////////////////////////////////
	// STATIC PUBLIC FUNCTIONS
	///////////////////////////////////////////////////////////
	
	// SetMaximumVisualQuality
	static public void SetMaximumVisualQuality()
	{
		QualitySettings.softVegetation  = 		true;
		QualitySettings.maxQueuedFrames = 		0;
		QualitySettings.anisotropicFiltering = 	AnisotropicFiltering.ForceEnable;
		QualitySettings.vSyncCount = 			1;
	}
	
}
