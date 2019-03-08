using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simulateunitychan : MonoBehaviour {
	private Animator unityanimator,animator;
	public GameObject unitychan;


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		unityanimator = unitychan.GetComponent<Animator>();
		animator.runtimeAnimatorController = unityanimator.runtimeAnimatorController;
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.B))
			animator.SetBool("isdance", true);
	}
	void finish(){
		animator.SetBool ("isdance", false);

	}
}

