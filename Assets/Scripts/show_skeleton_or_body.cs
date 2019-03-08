using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class show_skeleton_or_body : hoverbutton {
	public Button body, skelton;
	public bool on_off;

	// Use this for initialization
	void Start () {
		body_video.SetActive (false);
		init ();
		off ();
	}
	
	// Update is called once per frame
	void Update () {
		handposition = hand.transform.localPosition;

		mytimer ();
	}
	public override void turn(){
		body_video.SetActive (on_off);
		if (on_off) {
			on ();
		} else {
			off ();
		}
	}
	private void on(){
		body.GetComponent<Image> ().sprite = button_image_select;
		skelton.GetComponent<Image>().sprite = button_image_none;
	}
	private void off(){
		body.GetComponent<Image>().sprite = button_image_none;
		skelton.GetComponent<Image>().sprite = button_image_select;
	}
}
