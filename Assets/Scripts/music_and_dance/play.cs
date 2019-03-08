using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Windows.Kinect;

public class play : hoverbutton {
	
	AnimatorStateInfo currentstate;
	string music;
	public Text musicname, dancername;
	int dancer;
	public GameObject discounttime;
	Animator discountanmator;
	public bool replay, record;
	RawImage tmpvideo;
	// Use this for initialization
	void Start () {
		init ();
		discountanmator = discounttime.GetComponent<Animator> ();

		tmpvideo = body_video.GetComponentInChildren<RawImage> ();
		tmpvideo.texture = hoverbutton.body_video_movietexture;
        selectaudio.clip = Resources.Load(hoverbutton.music_plus_dance_name, typeof(AudioClip)) as AudioClip;
    }

	// Update is called once per frame
	void Update () {
		currentstate = animator.GetCurrentAnimatorStateInfo(0);

		handposition = hand.transform.localPosition;

		mytimer ();


		if (!currentstate.IsName("Base Layer.idle") && currentstate.normalizedTime>=1.0f) {

			// stop

			if(record==true){
				Button_click.record_play = 0;
				Button_click.record_pause = 0;
				Button_click.record_stop = 1;
				Debug.Log ("joint_replay : " + Button_click.joint_replay.Count.ToString () + "*" + Button_click.joint_replay [0].Count.ToString ());
				Debug.Log ("joint_replay2 : " + Button_click.joint_replay2.Count.ToString () + "*" + Button_click.joint_replay2 [0].Count.ToString ());
				Debug.Log ("Stop Recording");
			}

			if (replay == true) {
                GameObject.Find("Score").GetComponentInChildren<Text>().text = Button_click.final_score.ToString("F1");
			}

			animator.SetInteger("music", 0);
			animator.Play ("Base Layer.idle");
			selectaudio.Stop ();
			bgaudio.UnPause ();
			//hoverbutton.body_video_movietexture.Stop ();
			//	"gameObject".setactive(true);
			}


	}

	public override void mytimer(){
		//handover = isHandOver();
		//Debug.Log (buttom_name);
		if (hoverbutton.buttom_name == "null" || hoverbutton.buttom_name == gameObject.name) {
			if (isHandOver ()) {

				timecount = timestart - (int)Time.time;
				if (hand_animator.isActiveAndEnabled)
					hand_animator.Play ("Base Layer.hand");
				buttonimgwhenhover ();

				hoverbutton.buttom_name = gameObject.name;
				if (timecount == 0) {
					turn ();
					buttonimgwhenselected ();
					selected = true;
					timestart = (int)Time.time + timelong;
					timecount = timelong;
				}
			} else {
				hand_animator.Play ("Base Layer.none");
				timestart = (int)Time.time + timelong;
				buttonimgwhenoff ();

				hoverbutton.buttom_name="null";

			}
		}
	}
	public override void turn(){
		
		discounttime.SetActive (true);
		discountanmator.Play (0);
		

		//body_video_movietexture=  Resources.Load ("dance/Dance01_demovedio",typeof(MovieTexture)) as MovieTexture;
		//music = "music/" + music_plus_dance_name;

		
		//animator.SetInteger("music", dnacerindex(dancername.text));//dnacerindex(dancername.text)

		StartCoroutine (delayanimator ());

		bgaudio.Pause ();



	}
	int dnacerindex(string dancername){

		switch(dancername){
		case "Dance01":
			return 1;
		case "Dance02":
			return 2;
		}
		return 0;

	}

	IEnumerator delayanimator(){
		yield return new WaitForSeconds(3);

		if(record==true){
			if (Button_click.record_play == 1)
				Debug.Log ("You need to Stop first");
			else if (Button_click.record_pause == 1) {
				Button_click.record_pause = 0;
				Button_click.record_play = 1;
			}
			else {
				Button_click.joint_replay.Clear ();
				Button_click.joint_replay2.Clear ();
				if (GameObject.FindWithTag ("Body_Replay") != null) {
					GameObject temp = GameObject.FindWithTag ("Body_Replay");
					GameObject.Destroy (temp);
					Debug.Log ("Destroy Body_Replay");
				}

				Button_click.record_play = 1;
				Button_click.record_stop = 0;
				Button_click.replay_play = 0;
				Button_click.replay_pause = 0;
				Button_click.frame_cou = 0;

				Debug.Log ("Start Record");
			}
		}

		if (replay == true) {
			if (Button_click.joint_replay.Count == 0) {
				Debug.Log ("Please record first");
			}
			else if (Button_click.replay_pause == 1) {
				Button_click.replay_pause = 0;
				Button_click.replay_play = 1;
			}
			else {
				Button_click.replay_play = 1;
				Button_click.replay_stop = 0;
			}
		}
		animator.enabled = true;
		compareline = compareline_on_off;
		animator.Play ("Base Layer." + dancername.text);
		selectaudio.Play ();
		if(body_video!=null)
			body_video_movietexture.Play ();
		//	"gameObject".setactive(false);
		//record

	}
}
