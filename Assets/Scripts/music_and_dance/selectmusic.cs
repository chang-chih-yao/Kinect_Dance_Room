using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Windows.Kinect;

public class selectmusic : hoverbutton {
	
	public Text music_name;
	Text musicname;
	public GameObject musicpanel;
	UnityEngine.AudioSource tmpaudio;
	public Button selectbutton;
	string music;

	// Use this for initialization
	void Start () {
		init ();
		music_init ();
	}
	void Update () {
		

		mytimer ();
	}

	public override void button_init(){

		button_image = GetComponent<Image> ();
		buttonposition = transform.localPosition+musicpanel.transform.localPosition;//
		buttonw = ((RectTransform)transform).rect.width/2.0f;
		buttonh = ((RectTransform)transform).rect.height/2.0f;
		handw = ((RectTransform)hand.transform).rect.width;
		handh = ((RectTransform)hand.transform).rect.height;

	}
	void music_init(){

		musicname = selectbutton.GetComponentInChildren<Text> ();
		tmpaudio = selectbutton.GetComponent<UnityEngine.AudioSource> ();
		tmpaudio.clip = Resources.Load ( "music/" + musicname.text, typeof(AudioClip)) as AudioClip;

	}
	// Update is called once per frame


	public override void mytimer(){
		handposition = hand.transform.localPosition;
		if (hoverbutton.buttom_name == "null" || hoverbutton.buttom_name == gameObject.name) {
			if (isHandOver ()) {
				timecount = timestart - (int)Time.time;
				if (hand_animator.isActiveAndEnabled)
					hand_animator.Play ("Base Layer.hand");
				if (music_name.text != musicname.text) {
					buttonimgwhenhover ();
					//button_image.sprite = button_image_hover;// Resources.Load ("04/04_music_hover", typeof(Sprite)) as Sprite;
					//musicname.color = Color.black;
				
				}
				hoverbutton.buttom_name = gameObject.name;
				//music_init ();
				bgaudio.Pause ();
				if (!tmpaudio.isPlaying )
					tmpaudio.Play ();
				tmpaudio.UnPause ();
				if (timecount == 0) {
					decide_music ();
					timestart = 0;
					timecount = timelong;
				}
			} else {
				if (music_name.text != musicname.text)
					buttonimgwhenoff ();
				
				hand_animator.Play ("Base Layer.none");
				hoverbutton.buttom_name="null";
				tmpaudio.Stop ();
				bgaudio.UnPause ();
				timestart = (int)Time.time + timelong;
			}
		}
	}

	public virtual void decide_music(){
		
		music_name.text = musicname.text;
		buttonimgwhenselected ();

		//text.enabled=false;
	}
}
