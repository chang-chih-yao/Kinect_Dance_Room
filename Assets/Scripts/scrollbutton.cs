using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Windows.Kinect;
using UnityEngine.Sprites;

public class scrollbutton : MonoBehaviour {
	public Text timertext;
	public Image hand;
	public Button selectbutton;
	public Scrollbar scroll;
	public GameObject list,panel;
	public bool updown,turnon;//true=dwon,false=up
	int timecount,timestart;
	Vector2 buttonposition, handposition;
	float buttonw , buttonh ,handw,handh,listt,listb,sensitivity;
	private Sprite sprite;
	// Use this for initialization
	void Start () {
		buttonposition = selectbutton.transform.localPosition;
		buttonw = ((RectTransform)selectbutton.transform).rect.width/2.0f;
		buttonh = ((RectTransform)selectbutton.transform).rect.height/2.0f;
		handw = ((RectTransform)hand.transform).rect.width;
		handh = ((RectTransform)hand.transform).rect.height;
		ScrollRect scrollrect = panel.GetComponent<ScrollRect> ();
		sensitivity = scrollrect.scrollSensitivity;
		listt = ((RectTransform)list.transform).rect.yMin;
		listb = ((RectTransform)list.transform).rect.yMax;
		sprite = selectbutton.GetComponent<Image> ().sprite;


	}

	// Update is called once per frame
	void Update () {
		handposition = hand.transform.localPosition;

		mytimer ();
	}

	public virtual int mytimer(){
		if (isHandOver ()) {
			if (turnon) {
				turn ();
				return 0;
			}
			timecount = timestart - (int)Time.time;
			//timertext.text = "time" + timecount.ToString ();
			if (timecount == 0) {
				turnon = true;
				timestart = (int)Time.time + 1;
			}
		} else {
			selectbutton.GetComponent<Image> ().sprite = sprite;
			turnon = false;
			timestart = (int)Time.time + 1;
		}
		return 0;
	}

	public virtual void turn(){
		if (scroll.value > 0 && updown) {
			scroll.value -= (sensitivity / Math.Abs(listt - listb));
			selectbutton.GetComponent<Image> ().sprite = Resources.Load ("04/04_down_c",typeof(Sprite)) as Sprite;
		}
		if(scroll.value<1&&(!updown)){
			scroll.value+=(sensitivity/Math.Abs(listt - listb));
			selectbutton.GetComponent<Image> ().sprite = Resources.Load ("04/up_c",typeof(Sprite)) as Sprite;

			//((RectTransform)list.transform).rect.yMin += 50;
		}
		//text.enabled=false;
	}

	public bool isHandOver(){



		if ((handposition.x>= buttonposition.x - buttonw) && (handposition.x <= buttonposition.x + buttonw )&&( handposition.y >= buttonposition.y - buttonh )&&( handposition.y <= buttonposition.y + buttonh) ){

			return true;
		}
		return false;


	}
}
