using UnityEngine;
using System.Collections;

public class DestroyableShape : MonoBehaviour {

	public int hp = 2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other)
	{
		hp --;
		if (hp<= 0)
		{
			Destroy(this.gameObject);
		}
		else
		{
			StartCoroutine(blink ());
		}
	}

	void OnDestroy() {
		Debug.Log("You killed me!");
	}



	IEnumerator blink()
	{
		this.renderer.enabled = false;
		yield return new WaitForSeconds(.25f);
		this.renderer.enabled = true;
	}

}
