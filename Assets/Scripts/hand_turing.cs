using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class hand_turing : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void turing_off(){
		gameObject.GetComponent<Image> ().sprite = Resources.Load ("hand/hand01", typeof(Sprite)) as Sprite;
		//gameObject.GetComponent<Animator> ().enabled = false;
	}
}
