using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {
	GameObject cube2;
	public CubeManager manager;
	public int cubeNumber;


	// Use this for initialization
	void Start () {
		cube2 = GameObject.Find("Cube2");
	}



		
		// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		//print("Collision detected with trigger object " + other.name);
		//if (other.gameObject.name== "cube 1")
		if (other.gameObject.GetComponent<Player>() !=null)
		{
			manager.cubeWasHit(cubeNumber);
		}
	}
}