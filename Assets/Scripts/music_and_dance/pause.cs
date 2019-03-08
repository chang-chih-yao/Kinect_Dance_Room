using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Windows.Kinect;

public class pause : hoverbutton {
	public RawImage body;
	public bool record, replay;

	// Use this for initialization
	void Start () {
		init ();
	}

	// Update is called once per frame
	void Update () {
		if(body.texture!=null)
			body_video_movietexture = (MovieTexture)hoverbutton.body_video_movietexture;

		handposition = hand.transform.localPosition;
		mytimer ();
	}


	public override void turn(){
		if(record==true){
			if (Button_click.joint_replay.Count == 0 || Button_click.record_stop == 1) {
				Debug.Log("Please record first");
			}
			else if (Button_click.record_play == 1){
				Button_click.record_play = 0;
				Button_click.record_pause = 1;
				Debug.Log("Pause (record)");
			}
		}

		if (replay == true) {
			if (Button_click.joint_replay.Count == 0 || Button_click.replay_stop == 1) {
				Debug.Log ("Please play first");
			}
			else if (Button_click.replay_play == 1){
				Button_click.replay_play = 0;
				Button_click.replay_pause = 1;
				Debug.Log("Pause (replay)");
			}
		}

		selectaudio.Pause();
		animator.enabled = false;
		bgaudio.Play ();
		body_video_movietexture.Pause ();


	}
}
