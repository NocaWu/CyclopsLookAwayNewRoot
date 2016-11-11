using UnityEngine;
using System.Collections;
using System.IO;

public class PlayerCount : MonoBehaviour {

	StreamWriter swPlayerCounter;
	StreamReader srPlayerCounter;
	private string PATH;
	public int i;

	// Use this for initialization
	void Start () {
		PATH = Application.dataPath;

		if (File.Exists (PATH + "/PlayerCounter.txt") == false) {
			i = 1;
			swPlayerCounter = new StreamWriter (PATH + "/PlayerCounter.txt",false);
			swPlayerCounter.Write (i.ToString());
			swPlayerCounter.Close ();
		} 

		else {
			srPlayerCounter = new StreamReader (PATH + "/PlayerCounter.txt");
			i = int.Parse(srPlayerCounter.ReadToEnd());
			srPlayerCounter.Close();
		}

	//	i = srPlayerCounter.Read.parse;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			i++;

			StreamWriter sw = new StreamWriter (PATH + "/PlayerCounter.txt",false);
			sw.Write (i.ToString());
			sw.Close ();
		}
	
	}
}
