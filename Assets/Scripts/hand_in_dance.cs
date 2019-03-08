using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hand_in_dance : MonoBehaviour {
	public GameObject hand;
	private Vector2 handposition,buttonposition;
	private float buttonw , buttonh ,handw,handh;

	// Use this for initialization
	void Start () {
		button_init ();
	}
	
	// Update is called once per frame
	void Update () {
		handposition = hand.transform.localPosition;
		set_off_hand ();
	}
	public virtual void button_init(){

		//button_image_select = Resources.Load ("04/none", typeof(Sprite)) as Sprite;
		buttonposition = transform.localPosition;
		buttonw = ((RectTransform)transform).rect.width/2.0f;
		buttonh = ((RectTransform)transform).rect.height/2.0f;
		handw = ((RectTransform)hand.transform).rect.width;
		handh = ((RectTransform)hand.transform).rect.height;
	}
	void set_off_hand(){
		if (isHandOver ())
			hand.GetComponent<Image> ().sprite = Resources.Load("hand/hand01none",typeof(Sprite)) as Sprite;
		else
			hand.GetComponent<Image> ().sprite = Resources.Load("hand/hand01",typeof(Sprite)) as Sprite;
	}
	public bool isHandOver(){
		if ((handposition.x>= buttonposition.x - buttonw) && (handposition.x <= buttonposition.x + buttonw )&&( handposition.y >= buttonposition.y - buttonh )&&( handposition.y <= buttonposition.y + buttonh) ){
			return true;
		}
		return false;
	}
}
