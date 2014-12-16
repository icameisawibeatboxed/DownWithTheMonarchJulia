using UnityEngine;
using System.Collections;

public class CubeManager : MonoBehaviour {

	public GameObject[] cubes;//= new GameObject[4]; 
	int theCubeNumberYoureLookingFor=0;

	// Use this for initialization
	void Start () {

		for(int i = 0; i < cubes.Length; i++)//look up for loops
		{
			cubes[i].renderer.enabled = false;
			cubes[i].GetComponent<Cube>().cubeNumber=i;//this line is setting the cube number for each cube
			cubes[i].GetComponent<Cube>().manager = this;//this line sets the cubes manager variable to not null
		}

		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void cubeWasHit(int cubeNumber)
	{
		if (cubeNumber == theCubeNumberYoureLookingFor)
		{
			cubes[cubeNumber].renderer.enabled = true;

			print ("found " + cubeNumber);
			theCubeNumberYoureLookingFor = theCubeNumberYoureLookingFor + 1;
		}

	}


}
