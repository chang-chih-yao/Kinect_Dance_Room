using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class set_difficulty : hoverbutton {
	
	public float difficulty;
	// Use this for initialization
	void Start () {
		init ();

	}

	// Update is called once per frame
	void Update () {
		handposition = hand.transform.localPosition;
		mytimer ();
	}
	public override void turn(){
		KinectManager.sensitive =  difficulty;
	}
}
