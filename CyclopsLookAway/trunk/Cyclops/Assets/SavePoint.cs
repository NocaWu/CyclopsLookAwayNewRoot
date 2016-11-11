using UnityEngine;
using System.Collections;

public class SavePoint : MonoBehaviour {


	public enum DayOfWeek {Tuesday, Wednesday}
	public enum Month {November}
	[Header("When")]
	public Month SPDate_Month;
	public int SPDate_DayOfMonth = 1;
	public DayOfWeek SPDate_Day;
	public string SPDate_Time;
	[Header("Where")]
	public string bar_name = "Dining Room";
	public Vector3 from_school_vector;
	[Range(1f,10f)] public float beer_price;
	[Header("Who")]
	public GameObject[] cool_MFA_folks;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
