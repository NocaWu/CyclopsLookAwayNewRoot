using UnityEngine;
using System.Collections;

public class AudioBouncer : MonoBehaviour {



	public int detail = 500;
	public float minValue = 1.0f;
	public float amplitude = 0.1f;

	private float randomAmplitude = 1.0f;
	private Vector3 startScale;
	private Vector3 startPos;

	void Start(){
		startScale = transform.localScale;
		startPos = transform.position;

		randomAmplitude = Random.Range(1.0f, 3.0f);
	}

	void Update() {
		float[] info = new float[detail];
		AudioListener.GetOutputData(info, 0); 
		float packagedData = 0;

		for(int x = 0; x < info.Length; x++)
		{
			packagedData += System.Math.Abs(info[x]);   
		}

		transform.localScale = new Vector3((packagedData * amplitude) + startScale.y, (packagedData * amplitude) + startScale.y, (packagedData * amplitude) + startScale.z);

//		Vector3 curpos = transform.position;
//
//		transform.position = new Vector3 (startPos.x, (packagedData * amplitude *3f) + startPos.y, startPos.z);

	}
}
