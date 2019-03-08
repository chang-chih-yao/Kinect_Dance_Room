using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class check_move_correlation : MonoBehaviour {
	private GameObject spinbased,next;
	public GameObject buttom_move, right_move, left_move,go;
	public Camera maincamera;
	private Vector3 spinbased_position;
	public float left, forward, right, back;
	bool between=false,middle=false;
	public bool correlation_or_dance;
	// Use this for initialization
	void Start () {
		left +=maincamera.transform.position.x;
		right += maincamera.transform.position.x;
		forward += maincamera.transform.position.z;
		back+= maincamera.transform.position.z;
		spinbased = GameObject.Find ("realtime01/SpineBase");


		if (correlation_or_dance) {
			go = GameObject.Find ("go");
			go.SetActive (false);
			next = GameObject.Find ("Canvas/start-2/Next");
			next.SetActive(false);
		}
		buttom_move.SetActive ( false);
		right_move.SetActive  ( false);
		left_move.SetActive  ( false);
	}
	
	// Update is called once per frame
	void Update () {
		spinbased = GameObject.Find ("realtime01/SpineBase");
		if (spinbased != null) {
			spinbased_position = spinbased.transform.position;
			if (spinbased_position.x < left) {
			
				right_move.SetActive (true);
				between = false;
			} else if (spinbased_position.x > right) {
				between = false;
				left_move.SetActive (true);
			} else {
				right_move.SetActive (false);
				left_move.SetActive (false);
				between = true;
			}
			if (spinbased_position.z < forward) {
				middle = false;
				buttom_move.SetActive (true);
				buttom_move.GetComponent<Image> ().sprite = Resources.Load ("03/03_move_backward", typeof(Sprite)) as Sprite;
			} else if (spinbased_position.z > back) {
				middle = false;
				buttom_move.SetActive (true);
				buttom_move.GetComponent<Image> ().sprite = Resources.Load ("03/03_move_forward", typeof(Sprite)) as Sprite;
			} else {
				middle = true;		
				buttom_move.SetActive (false);
			}
			if (correlation_or_dance) {
				if (between && middle) {
					go.SetActive (true);
					next.SetActive (true);
				} else {
					go.SetActive (false);
					next.SetActive (false);
				}
			}
		} else {
			buttom_move.SetActive ( false);
			right_move.SetActive  ( false);
			left_move.SetActive  ( false);
		}
	}
}
