using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiWindowHud : MonoBehaviour {

	public Text TextPlayerLocation;
	public Text TextPlayerRotation;
	
	// Update is called once per frame
	void Update () 
	{
		//MyUtils.LogInfo ("UiWindowHud:Update");

		// Player Location
		if (TextPlayerLocation != null) {
			var loc = Labyrinth.Globals.Player.Location;
			TextPlayerLocation.text = string.Format ("Location: X:{0} Y:{1} Z:{2}", (int)loc.x, (int)loc.y, (int)loc.z);
		}

		// Player Forward Dir
		if (TextPlayerRotation != null ) {
			var rot = Labyrinth.Globals.Player.Rotation;
			TextPlayerRotation.text = string.Format ("Rotation: X:{0} Y:{1} Z:{2}", (int)rot.x, (int)rot.y, (int)rot.z);
		}
	}
}
