using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeePath : MonoBehaviour {

	public GameObject beePathHolder;
	public Path beeroute;

	// Use this for initialization
	void Awake () {

		beeroute = new Path ();
//		float a = GetComponent<BeeMaker> ().swarmBounds.x/2f;
//		float b = GetComponent<BeeMaker> ().swarmBounds.y/2f;
//		float x;
//		float y;
//
//
//		int sectNum = 12;
//		for (int i = sectNum; i >= 0; i--) {
//			x = (a / sectNum) * i;
//			y = Mathf.Sqrt ((1f - ((Mathf.Pow (x, 2)) / (Mathf.Pow (a, 2)))) * (Mathf.Pow (b, 2)));
//			beeroute.addNode(new Vector3(x, y, 0));
//		}
//		for (int i = -1; i >= -sectNum; i--) {
//			x = (a / sectNum) * i;
//			y = Mathf.Sqrt ((1f - ((Mathf.Pow (x, 2)) / (Mathf.Pow (a, 2)))) * (Mathf.Pow (b, 2)));
//			beeroute.addNode(new Vector3(x, y, 0));
//		}
//		for (int i = -(sectNum-1); i <= 0; i++) {
//			x = (a / sectNum) * i;
//			y = Mathf.Sqrt ((1f - ((Mathf.Pow (x, 2)) / (Mathf.Pow (a, 2)))) * (Mathf.Pow (b, 2)));
//			beeroute.addNode(new Vector3(x, -y, 0));
//		}
//		for (int i = 1; i <= (sectNum-1); i++) {
//			x = (a / sectNum) * i;
//			y = Mathf.Sqrt ((1f - ((Mathf.Pow (x, 2)) / (Mathf.Pow (a, 2)))) * (Mathf.Pow (b, 2)));
//			beeroute.addNode(new Vector3(x, -y, 0));
//		}

		foreach (Transform point in beePathHolder.transform) {
			beeroute.addNode (new Vector3 (point.position.x, point.position.y, 0));
		}

//		beeroute.addNode (new Vector3(14f, 0, 0));




	}
	
	// Update is called once per frame
	void Update () {
	
	}


}


public class Path{
	
	private List<Vector3> nodes;

	public Path(){
		this.nodes = new List<Vector3> ();
	}

	public void addNode(Vector3 node){
		nodes.Add (node);
	}

	public Vector3[] getNodes(){
		return nodes.ToArray ();
	}



}