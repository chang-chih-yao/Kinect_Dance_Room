using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Kinect = Windows.Kinect;

public class hoverbutton : MonoBehaviour {

	public Image hand;
	public static bool compareline;
	public bool compareline_on_off,selected=false;
	public static MovieTexture body_video_movietexture;
	public GameObject body_video; 
	//public Button selectbutton;
	protected static string music_plus_dance_name;
	protected int timecount,timestart,timelong=2;
	protected Vector2 buttonposition, handposition;
	protected float buttonw , buttonh ,handw,handh;
	public Sprite button_image_none,button_image_select,button_image_hover;
	protected Image button_image;
	protected GameObject modle,modelbased;
	protected GameObject canvas;
	protected UnityEngine.AudioSource bgaudio,selectaudio;
	//protected static int ;
	protected Animator animator,hand_animator;
	public static string buttom_name= "null";


	// Use this for initialization
	void Start () {
		init ();
	}
	public void init(){
		button_init ();
		modle_init ();	
	}
	public virtual void button_init(){
		 
		button_image = GetComponent<Image> ();
		//button_image_select = Resources.Load ("04/none", typeof(Sprite)) as Sprite;
		buttonposition = transform.localPosition;
		buttonw = ((RectTransform)transform).rect.width/2.0f;
		buttonh = ((RectTransform)transform).rect.height/2.0f;
		handw = ((RectTransform)hand.transform).rect.width;
		handh = ((RectTransform)hand.transform).rect.height;
	}
	public void modle_init(){
		canvas = GameObject.Find ("Canvas");
		DontDestroyOnLoad (canvas);
		bgaudio = canvas.GetComponent<UnityEngine.AudioSource>();

		//bgaudio.Play ();
		modle = GameObject.Find ("modle");
		selectaudio = modle.GetComponent<UnityEngine.AudioSource> ();
		animator = modle.GetComponent<Animator> ();

		modelbased = GameObject.Find ("modle/SpineBase");

		hand_animator = hand.GetComponent<Animator> ();

		//

	}


	// Update is called once per frame
	void Update () {
		handposition = hand.transform.localPosition;
		mytimer ();

	}

	public virtual void mytimer(){
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
        else {
            selected = false;
        }


	}

	public virtual void turn(){
		//button_image.sprite = button_image_select;
		//text.enabled=false;
	}

	public bool isHandOver(){
		if ((handposition.x>= buttonposition.x - buttonw) && (handposition.x <= buttonposition.x + buttonw )&&( handposition.y >= buttonposition.y - buttonh )&&( handposition.y <= buttonposition.y + buttonh) ){
			return true;
		}
		return false;
	}
	public void buttonimgwhenhover(){
		button_image.sprite = button_image_hover;
	}
	public void buttonimgwhenselected(){
		button_image.sprite = button_image_select;
		gameObject.GetComponentInChildren<Text> ().color = Color.gray;
	}
	public void buttonimgwhenoff(){
		button_image.sprite = button_image_none;

		gameObject.GetComponentInChildren<Text> ().color = Color.white;
	}


}
