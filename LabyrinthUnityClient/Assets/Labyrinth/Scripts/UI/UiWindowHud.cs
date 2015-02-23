using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiWindowHud : MonoBehaviour {

	public Text TextPlayerLocation;
	public Text TextPlayerRotation;

	void Start()
	{
		// Size the Canvas to the Camera View Port Size.
		// This is done manually in the Editor:  W=703, H=634, Scale = 0.0025, PosX = 0, PosY = 0.4, Pos Z = 0.9
		// Not sure exatly why this makes sense....
		///*
		var cameraObj = GameObject.Find( "CameraLeft" );
		var rectTransform = gameObject.GetComponent<RectTransform> ();
		var width = cameraObj.camera.pixelWidth*0.9f;
		var height = cameraObj.camera.pixelHeight*0.9f;
		var xScale = rectTransform.localScale.x;
		var yScale = rectTransform.localScale.y;
		rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, width);
		rectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, height);
		//gameObject.transform.position = new Vector3 (-width / 2 * xScale, -height / 2 * yScale, 1);

		Labyrinth.Utils.LogInfo ("UiWindowHud.Start(): Width={0}, Height={1}, XScale={2}, YScale={3}", width, height, xScale, yScale);
		//*/
	}
	
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
