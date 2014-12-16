using UnityEngine;
using System.Collections;

public class test1 : MonoBehaviour {

	/*public float speed = 1f, maxDist = 2;
	private Vector3 finger;
	private Transform myTrans, camTrans;


		void Start ()
		{
			myTrans = this.transform;
			camTrans = Camera.main.transform;
		}

	void LookAtFinger()
	{
		//z of ScreenToWorldPoint is distance from camera into the world, so we need to find this object's distance from the camera to make it stay on the same plane
		Vector3 tempTouch = new Vector3(Input.mousePosition.position.x, Input.GetTouch(touch2Watch).position.y,camTrans.position.y - myTrans.position.y);
		//Convert screen to world space
		finger = Camera.main.ScreenToWorldPoint(tempTouch);
		
		//look at finger position
		myTrans.LookAt(finger);
		
		//move towards finger if not too close
		if(Vector3.Distance(finger, myTrans.position) > maxDist)
			myTrans.Translate (Vector3.forward * speed * Time.deltaTime);
	}
	public override void OnTouchMovedAnywhere()
	{
		LookAtFinger();
	}
	public override void OnTouchStayedAnywhere()
	{
		LookAtFinger();
	}
	public override void OnTouchBeganAnywhere()
	{
		touch2Watch = TouchLogicV2.currTouch;
	}
*/
}




