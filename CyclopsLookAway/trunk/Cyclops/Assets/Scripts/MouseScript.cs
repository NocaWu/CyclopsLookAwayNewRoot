using UnityEngine;
using System.Collections;

public class MouseScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//hide the mouse cursor
		Cursor.visible = false;
	}
	
	// Update is called once per frame
//	void Update () {
//		//move the object to the mouse
//
//		//get the mouse position on screen (in pixels)
//		Vector3 mouseScreenPosition = Input.mousePosition;
//
//		//convert that to world coordinates
//		Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
// 		
//		//set the z position (depth) to 0 
//		mouseWorldPosition.z = -15f;
//
// 		//move this object to the mouse
//		transform.position = mouseWorldPosition;
//	}
//

	public Texture2D cursorTex;
	public int cursorSize = 63;
	int sizeX;
	int sizeY;

	void Awake() {
		sizeX = cursorSize;
		sizeY = cursorSize;
	}

//	void Start() 
//	{
//		Screen.showCursor = false;
//	}
//
	void Update() {
		sizeX = cursorSize;
		sizeY = cursorSize;
				//get the mouse position on screen (in pixels)
				Vector3 mouseScreenPosition = Input.mousePosition;
		
				//convert that to world coordinates
				Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
		 		
				//set the z position (depth) to 0 
				mouseWorldPosition.z = -15f;
		
		 		//move this object to the mouse
				transform.position = mouseWorldPosition;
	}

	void OnGUI()
	{
		GUI.DrawTexture(new Rect(Event.current.mousePosition.x - (cursorSize/2), Event.current.mousePosition.y - (cursorSize/2), sizeX, sizeY), cursorTex);
	}
}
