using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class check_move_dance : MonoBehaviour {
	private GameObject spinbased;
	public GameObject buttom_move, right_move, left_move;
	public Camera maincamera;
	private Vector3 spinbased_position;
	private float left, forward, right, back;
	public bool correlation_or_dance;

	// Use this for initialization
	void Start () {
		spinbased = GameObject.Find ("realtime01/SpineBase");
		left = -2.72f+maincamera.transform.position.x;
		right = -1.81f+maincamera.transform.position.x;
		forward =0.2f;
		back = 1.5f;

		buttom_move.SetActive ( false);
		right_move.SetActive  ( false);
		left_move.SetActive  ( false);
	}

	// Update is called once per frame
	void Update () {
		if (spinbased != null) {
			spinbased_position = spinbased.transform.position;
			if (spinbased_position.x < left) {

				right_move.SetActive (true);

			} else if (spinbased_position.x > right) {
			
				left_move.SetActive (true);
			} else {
				right_move.SetActive (false);
				left_move.SetActive (false);

			}
			if (spinbased_position.z < forward) {
			
				buttom_move.SetActive (true);
				buttom_move.GetComponent<Image> ().sprite = Resources.Load ("03/03_move_backward", typeof(Sprite)) as Sprite;
			} else if (spinbased_position.z > back) {
			
				buttom_move.SetActive (true);
				buttom_move.GetComponent<Image> ().sprite = Resources.Load ("03/03_move_forward", typeof(Sprite)) as Sprite;
			} else {	
				buttom_move.SetActive (false);
			}

		}
	}
}
