using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using HappyFunTimes;
using CSSParse;

namespace HappyFunTimesExample
{
	class ExampleSimplePlayer : MonoBehaviour
	{
		
		
		
		
		public string buttonInputReceived = "";
		/*public string Unlock_Code ;*/
		private System.Random m_rand = new System.Random ();
		
		
		// Classes based on MessageCmdData are automatically registered for deserialization
		// by CmdName.
		[CmdName("color")]
		private class MessageColor : MessageCmdData
		{
			public string color = "";    // in CSS format rgb(r,g,b)
		};
		
		[CmdName("move")]
		private class MessageMove : MessageCmdData
		{
			public float x = 0;
			public float y = 0;
		};


		
		[CmdName("consolePrint")]
		private class MessageConsolePrint : MessageCmdData
		{
			public string thingToPrint = "";
			public int buttonNumber = -1;
			
		};
		
		[CmdName("setName")]
		private class MessageSetName : MessageCmdData
		{
			public MessageSetName ()
			{  // needed for deserialization
			}
			
			public MessageSetName (string _name)
			{
				name = _name;
			}
			
			public string name = "";
		};
		
		[CmdName("busy")]
		private class MessageBusy : MessageCmdData
		{
			public bool busy = false;
		}
		
		// NOTE: This message is only sent, never received
		// therefore it does not need a no parameter constructor.
		// If you do receive one you'll get an error unless you
		// add a no parameter constructor.
		[CmdName("scored")]
		private class MessageScored : MessageCmdData
		{
			public MessageScored (int _points)
			{
				points = _points;
			}
			
			public int points;
		}

		[CmdName("shape")]
		private class MessageShape : MessageCmdData
		{
			public MessageShape (string _shape)//, int _othering)
			{
				shape = _shape;
				//otherthing = otherthing;
			}
			
			public string shape;
			//public int otherthing;
		}

		[CmdName("messageToPhone")]
		private class MessageToPhone : MessageCmdData
		{
			public MessageToPhone (string _message)//, int _othering)
			{
				message = _message;
				//otherthing = otherthing;
			}
			
			public string message;
			//public int otherthing;
		}
		
		void InitializeNetPlayer (SpawnInfo spawnInfo)
		{
			m_netPlayer = spawnInfo.netPlayer;
			m_netPlayer.OnDisconnect += Remove;
			
			// Setup events for the different messages.
			m_netPlayer.RegisterCmdHandler<MessageColor> (OnColor);
			m_netPlayer.RegisterCmdHandler<MessageMove> (OnMove);
			m_netPlayer.RegisterCmdHandler<MessageSetName> (OnSetName);
			m_netPlayer.RegisterCmdHandler<MessageBusy> (OnBusy);
			
			m_netPlayer.RegisterCmdHandler<MessageConsolePrint> (OnConsolePrint);
			
			ExampleSimpleGameSettings settings = ExampleSimpleGameSettings.settings ();
			m_position = new Vector3 (m_rand.Next (settings.areaWidth), 0, m_rand.Next (settings.areaHeight));
			transform.localPosition = m_position;
			
			SetName (spawnInfo.name);
		}
		
		void Start ()
		{
			m_position = gameObject.transform.localPosition;
			m_color = new Color (0.0f, 1.0f, 0.0f);
			
			
			
		}
		
		public void Update ()
		{
		}
		
		private void SetName (string name)
		{
			m_name = name;
		}
		
		public void OnTriggerEnter (Collider other)
		{
			// Because of physics layers we can only collide with the goal
			m_netPlayer.SendCmd (new MessageScored (m_rand.Next (5, 15)));
		}
		
		private void Remove (object sender, EventArgs e)
		{
			Destroy (gameObject);
		}
		
		private void OnColor (MessageColor data)
		{
			m_color = CSSParse.Style.ParseCSSColor (data.color);
			gameObject.renderer.material.color = m_color;
		}
		
		private void OnMove (MessageMove data)
		{
			ExampleSimpleGameSettings settings = ExampleSimpleGameSettings.settings ();
			m_position.x = data.x * settings.areaWidth;
			m_position.z = settings.areaHeight - (data.y * settings.areaHeight) - 1;  // because in 2D down is positive.
			
			gameObject.transform.localPosition = m_position;
		}
		
		private void OnConsolePrint (MessageConsolePrint data)
		{
			
			
			buttonInputReceived = buttonInputReceived + data.buttonNumber;
			//Unlock_Code = Random.Range(1,10)+ Random.Range(1,10)+ Random.Range(1,9)+ Random.Range(1,9);
			
			print (data.buttonNumber);
			
			if (buttonInputReceived.Length >= 4) 
			{
				string codeResult = Codes.receiveCode(buttonInputReceived);
				m_netPlayer.SendCmd(new MessageShape(codeResult));// send the code result to the phone
				buttonInputReceived = "";
			}
			/*
						if (buttonInputReceived == Unlock_Code) {
								print ("unlock");
								buttonInputReceived = "";
								Unlock_Code = string.Format ("{0}{1}{2}{3}", m_rand.Next (1, 10), m_rand.Next (1, 10), m_rand.Next (1, 10), m_rand.Next (1, 10));
								Debug.Log (Unlock_Code);
						} else if (buttonInputReceived.Length >= 4) {
								print ("reset");
								buttonInputReceived = "";
						}*/
			
		}
		
		private void OnSetName (MessageSetName data)
		{
			if (data.name.Length == 0) {
				m_netPlayer.SendCmd (new MessageSetName (m_name));
			} else {
				SetName (data.name);
			}
		}
		
		private void OnBusy (MessageBusy data)
		{
			// not used.
		}
		
		private NetPlayer m_netPlayer;
		private Vector3 m_position;
		private Color m_color;
		private string m_name;
	}
	
}  // namespace HappyFunTimesExample
