using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Codes : MonoBehaviour
{
		public static SpawnSphere shapeSpawn;
		
		public static List<string> goodCodes = null;// new List<string> ();
		public static List<string> triCodes = null;
		public static List<string> pentCodes = null;
		public static List<string> sqCodes = null;
		public static List<string> badCodes = null;// new List<string> ();
		public static List<string> allCodes = null;
		public static List<string> drawCodes = null;

		public static float theta = 0.0f;
		public static float angularOffset = 0.0f;
		public static int counter = 0;
		public static float timeCounter = 0.0f;
		public static Color codeColor = Color.red;

		

		// Use this for initialization
		void Start ()
		{
				
		shapeSpawn = GameObject.Find ("OculusEverything/SphereMaker").GetComponent<SpawnSphere>();

				if (goodCodes == null) { //haven't set up the codes yet
						goodCodes = new List<string> ();
						triCodes = new List<string> ();
						pentCodes = new List<string> ();
						sqCodes = new List<string> ();
						badCodes = new List<string> ();
						allCodes = new List<string> ();

						for (int i = 0; i < 10; i++) {
			
								triCodes.Add (makeRandomCode ());
								pentCodes.Add (makeRandomCode ());
								sqCodes.Add (makeRandomCode ());
								badCodes.Add (makeRandomCode ());
								goodCodes.Add (triCodes [i]);
								goodCodes.Add (pentCodes [i]);
								goodCodes.Add (sqCodes [i]);
								allCodes.Add (pentCodes [i]);
								allCodes.Add (sqCodes [i]);
								allCodes.Add (badCodes [i]);
								allCodes.Add (triCodes [i]);
				
						}



				}
	
		}

		public string makeRandomCode ()
		{
				string newCode = "";
				for (int j = 0; j < 4; j++) {
						newCode = newCode + Random.Range (1, 9);
				}
				return newCode;
		}
	
		public static string receiveCode (string code)
		{
				bool wasTriCode = triCodes.Contains (code);
				bool wasPentCode = pentCodes.Contains (code);
				bool wasSqCode = sqCodes.Contains (code);
				bool wasGoodCode = goodCodes.Contains (code);
				bool wasBadCode = badCodes.Contains (code);
				
		string result = "";
				if (wasGoodCode) {
					goodCodes.Remove (code);
					
						
						//could add a new code
				} else if (wasBadCode == false) {
			result= ("reset");
				}
				if (wasTriCode) {
						triCodes.Remove (code);
						allCodes.Remove (code);
			result= ("triangle");
						shapeSpawn.spawnSphere(0);
						
						
				}
				if (wasPentCode) {
						pentCodes.Remove (code);
						allCodes.Remove (code);
			result= ("pentagon");
						shapeSpawn.spawnSphere(1);

				}
				if (wasSqCode) {
						sqCodes.Remove (code);
						allCodes.Remove (code);
			result= ("square");
						shapeSpawn.spawnSphere(2);
				}
				if (wasBadCode) {
						badCodes.Remove (code);
						allCodes.Remove (code);
			result= ("dud");

		
				}
				print (result);
				return result;

		}
	
		// Update is called once per frame
		void Update () 


		///////input keys for easy play test//////////
		{
			if (Input.GetKeyDown(KeyCode.A))
		{
			receiveCode(triCodes[0]);
		}	



		if (Input.GetKeyDown(KeyCode.S))
		{
			receiveCode(pentCodes[0]);
		}	



		if (Input.GetKeyDown(KeyCode.D))
		{
			receiveCode(sqCodes[0]);
		}	

		if (Input.GetKeyDown(KeyCode.F))
		{
			receiveCode(badCodes[0]);
		}	
		////////////////////////////////////////////


			if (timeCounter < 90)
			{   
				timeCounter ++;
			}
			else 
			{
				timeCounter = 0;
				if (counter < allCodes.Count) 
				{
					counter += 1;
				} 
				else 
				{
					counter = 0;
				}
			}
			
		}

		void OnGUI ()
		{

				for (int i = 0; i < allCodes.Count; i++) {
						theta = 2*Mathf.PI*i/(allCodes.Count);
						float angleIncrement = 2*Mathf.PI/(allCodes.Count);	
						theta += angleIncrement*counter;
						//codeColor = HSVToRGB (i/40, 1, 1);
						

						
			AlexUtil.DrawText (new Vector2 (((Mathf.Sin(theta)*((Screen.height*allCodes.Count/2-400)/40))+Screen.width/2), (Mathf.Cos(theta)*((Screen.height*allCodes.Count/2-400)/40)+Screen.height/2)-5), "" + allCodes [i], 12, Color.black);
				}










				/*for (int i = 0; i < badCodes.Count; i++) {
						AlexUtil.DrawText (new Vector2 (200, 10 + 20 * i), "code: " + badCodes [i], 24, Color.white);

				}*/




		}
//	public Color HSVToRGB(float H, float S, float V)
//	{
//		if (S == 0f)
//			return new Color(V,V,V);
//		else if (V == 0f)
//			return Color.black;
//		else
//		{
//			Color col = Color.black;
//			float Hval = H * 6f;
//			int sel = Mathf.FloorToInt(Hval);
//			float mod = Hval - sel;
//			float v1 = V * (1f - S);
//			float v2 = V * (1f - S * mod);
//			float v3 = V * (1f - S * (1f - mod));
//			switch (sel + 1)
//			{
//			case 0:
//				col.r = V;
//				col.g = v1;
//				col.b = v2;
//				break;
//			case 1:
//				col.r = V;
//				col.g = v3;
//				col.b = v1;
//				break;
//			case 2:
//				col.r = v2;
//				col.g = V;
//				col.b = v1;
//				break;
//			case 3:
//				col.r = v1;
//				col.g = V;
//				col.b = v3;
//				break;
//			case 4:
//				col.r = v1;
//				col.g = v2;
//				col.b = V;
//				break;
//			case 5:
//				col.r = v3;
//				col.g = v1;
//				col.b = V;
//				break;
//			case 6:
//				col.r = V;
//				col.g = v1;
//				col.b = v2;
//				break;
//			case 7:
//				col.r = V;
//				col.g = v3;
//				col.b = v1;
//				break;
//			}
//			col.r = Mathf.Clamp(col.r, 0f, 1f);
//			col.g = Mathf.Clamp(col.g, 0f, 1f);
//			col.b = Mathf.Clamp(col.b, 0f, 1f);
//			return col;
//		}
//	}
}
