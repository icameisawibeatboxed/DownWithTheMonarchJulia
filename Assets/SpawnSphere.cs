using UnityEngine;
using System.Collections;

public class SpawnSphere : MonoBehaviour {

	public Codes codes;

	int nSpheres = 0;
	//int nCubes = 0;


	public GameObject trianglePrefab;
	public GameObject rectanglePrefab;
	public GameObject pentagonPrefab;
	public GameObject startPosition;


	
	// Use this for initialization
	void Start () {


	}

	public void spawnSphere(int whichShape)
	{
		GameObject[] shapePrefabs = {trianglePrefab, rectanglePrefab, pentagonPrefab};

		GameObject newObject = (GameObject) Instantiate(shapePrefabs[whichShape]);

		newObject.transform.position = startPosition.transform.position + new Vector3(Random.Range(-10,10), Random.Range(-10,10),0);
		
			nSpheres++;
			//nCubes = nCubes+1;//same as nCubes++; and same as nCubes+=1;

		}

	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetKeyDown("z"))
		{
			GameObject newSphere = (GameObject) Instantiate(spherePrefab);
			newSphere.transform.position = startPosition.transform.position + new Vector3(0, nSpheres * .5f, 0);

				nSpheres++;
		}

		if (Input.GetKeyDown("x"))
		{
			GameObject newCube = (GameObject) Instantiate(cubePrefab);
			newCube.transform.position = startPosition.transform.position + new Vector3(0, nSpheres * .5f, 0);

			nSpheres++;
			//nCubes = nCubes+1;//same as nCubes++; and same as nCubes+=1;
		}*/

		GameObject newObject = null;
		if (Input.GetKeyDown("z"))
		{
			newObject = (GameObject) Instantiate(trianglePrefab);

		}

		if (Input.GetKeyDown("y"))
		{
			newObject = (GameObject) Instantiate(rectanglePrefab);
		}

		/*if (Input.GetKeyDown("c"))
		{
			newObject = (GameObject) Instantiate(cylinderPrefab);
		}
		*/

		
		if (newObject !=null)
		{
			newObject.transform.position = startPosition.transform.position + new Vector3(Random.Range(-5,5), Random.Range(-5,5),0);
			
			nSpheres++;
			//nCubes = nCubes+1;//same as nCubes++; and same as nCubes+=1;
		}

	}




}
