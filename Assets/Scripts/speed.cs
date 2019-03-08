using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class speed : hoverbutton {
	bool downspeed;
	public Text music_name;

	// Use this for initialization
	void Start () {
		init ();
	}
	
	// Update is called once per frame
	void Update () {
		handposition = hand.transform.localPosition;
		mytimer ();
	}
    public override void mytimer()
    {
        //handover = isHandOver();
        //Debug.Log (buttom_name);
        if (hoverbutton.buttom_name == "null" || hoverbutton.buttom_name == gameObject.name)
        {
            if (isHandOver())
            {

                timecount = timestart - (int)Time.time;
                if (hand_animator.isActiveAndEnabled)
                    hand_animator.Play("Base Layer.hand");
                buttonimgwhenhover();

                hoverbutton.buttom_name = gameObject.name;
                if (timecount == 0)
                {
                    turn();
                    buttonimgwhenselected();
                    selected = !selected;
                    timestart = (int)Time.time + timelong;
                    timecount = timelong;
                }
            }
            else
            {
                hand_animator.Play("Base Layer.none");
                timestart = (int)Time.time + timelong;
                if (!selected)
                    buttonimgwhenoff();

                hoverbutton.buttom_name = "null";

            }
        }


    }

    public override void turn(){
		downspeed = !downspeed;
		if (downspeed) {
            hoverbutton.music_plus_dance_name += "_slow";
            gameObject.GetComponent<Image>().sprite = Resources.Load("09/downspeed_select",typeof(Sprite)) as Sprite;
            selectaudio.clip = Resources.Load ( hoverbutton.music_plus_dance_name, typeof(AudioClip)) as AudioClip;
			animator.speed = 0.5f;
		}
		else {
			animator.speed = 1.0f;
            hoverbutton.music_plus_dance_name = hoverbutton.music_plus_dance_name .Substring(0, hoverbutton.music_plus_dance_name.Length - 5);

            gameObject.GetComponent<Image>().sprite = Resources.Load("09/downspeed", typeof(Sprite)) as Sprite;
            selectaudio.clip = Resources.Load ( hoverbutton.music_plus_dance_name, typeof(AudioClip)) as AudioClip;
		}
	}
}
