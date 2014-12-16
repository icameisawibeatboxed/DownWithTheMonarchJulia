using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}


	void OnTriggerEnter(Collider other)
	{

		print("Collision detected with trigger object " + other.name);

	
	}


	// Update is called once per frame
	void Update () {
	
	}
}
