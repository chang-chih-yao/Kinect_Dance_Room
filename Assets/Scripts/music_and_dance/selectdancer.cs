using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Windows.Kinect;
using UnityEngine.Video;

public class selectdancer : hoverbutton {
	
	public Text dancer_name,music_name;


    Text dancername;
	string hovername;
	public GameObject movieplayer;
	RawImage tmpvideo;
	MovieTexture movie;
	public Button selectbutton;

	void Start () {
		init ();
		timelong = 2;
		//movieplayer = GameObject.Find ("movieplayer");
		tmpvideo = movieplayer.GetComponentInChildren<RawImage> ();
		dancername = GetComponentInChildren<Text> ();

		hovername = "dance/" + dancername.text;
		movie=  Resources.Load (hovername,typeof(MovieTexture)) as MovieTexture;
		tmpvideo.texture = movie;
		movieplayer.SetActive(false);

	}
	// Update is called once per frame
	void Update () {
		

		handposition = hand.transform.localPosition;
		mytimer ();
	}

	public override void mytimer(){
		if (hoverbutton.buttom_name == "null" || hoverbutton.buttom_name == gameObject.name) {
			if (isHandOver ()) {
				timecount = timestart - (int)Time.time;
				preview ();
				if (hand_animator.isActiveAndEnabled)
					hand_animator.Play ("Base Layer.hand");
				if (dancer_name.text != dancername.text)
					button_image.sprite = button_image_hover;//Resources.Load ("05/05_dancer01_hover",typeof(Sprite)) as Sprite;
				hoverbutton.buttom_name = gameObject.name;
				//preview ();
				if (timecount == 0) {
					decide_dancer ();
					timestart = 0;
					timecount = timelong;
				}
			} else {
				if (dancer_name.text != dancername.text)
					buttonimgwhenoff ();
				timestart = (int)Time.time + timelong;
				hand_animator.Play ("Base Layer.none");
				stoppreview ();
				hoverbutton.buttom_name = "null";
			}
		} else {
			if (dancer_name.text != dancername.text)
				buttonimgwhenoff ();
		}
	}
	public void preview(){
		movie=  Resources.Load ("dance/"+dancername.text,typeof(MovieTexture)) as MovieTexture;
		//tmpvideo.enabled = true;
		movieplayer.SetActive (true);
		movie.Play ();
	}
	public void stoppreview(){
		//tmpvideo.enabled = false;
		movieplayer.SetActive (false);
		movie.Stop ();
	}
	public String decide_dancer(){		
		dancer_name.text = dancername.text;
		buttonimgwhenselected ();
		hoverbutton.body_video_movietexture = Resources.Load ("dance/"+dancer_name.text+"_demovedio",typeof(MovieTexture)) as MovieTexture;


        hoverbutton.music_plus_dance_name = "music/" + music_name.text + "_" + dancername.text;
        return dancername.text;
		//text.enabled=false;
	}
	private void buttonselect(){
		button_image.sprite = button_image_select;// Resources.Load ("05/05_dancer01_select",typeof(Sprite)) as Sprite;
		dancername.color = Color.gray;
	}
	private void buttonunselect(){
		button_image.sprite = button_image_none;//Resources.Load ("05/05_dancer01", typeof(Sprite)) as Sprite;
		dancername.color = Color.white;
	}

}