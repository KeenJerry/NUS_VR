using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		List<string> btnName = new List<string> ();

		btnName .Add ("startButton");
		btnName.Add ("sourceButton");
		btnName.Add ("volumnButton");

		foreach(string Name in btnName )
		{
			GameObject btnObj= GameObject .Find(Name);
			Button btn = btnObj.GetComponent<Button>();
			btn.onClick.AddListener (delegate() { 
				this.OnClick(btnObj);
				
			});
		}
		
	}
	public void OnClick(GameObject sender)
	{
		switch (sender.name) 
		{
		case "startButton":
			Debug.Log ("startButton");
			break;//!!!!!
		case "sourceButton":
			Debug.Log ("sourceButton");
			break;
		case "volumnButton":
			Debug.Log ("volumnButton");
			break;

		default :
			Debug.Log ("none");
			break;

		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
