using UnityEngine;
using System.Collections;

public class Sizer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Screen.SetResolution(3200, 1080, true);
        Screen.fullScreen = false;
	}
}
