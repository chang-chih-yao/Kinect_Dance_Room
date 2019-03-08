using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class reset : MonoBehaviour {
	private Animator animator;
	public string music;
	int dancer;
	public Text musicname, dancername;
	//public AudioSource audiosource;
	public GameObject modle;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.A))
			animator.SetBool("again", true);
	}
	void finish(){
		animator.SetBool ("again", false);

	}
	void music_and_dancer(){
		music = musicname.text;
		modle.GetComponent<AudioSource> ().clip = Resources.Load ("music/"+musicname, typeof(AudioClip)) as AudioClip;

	}
	int dnacerindex(string dancername){

		switch(dancername){
		case "Dancer01":
			return 1;
		
		}
		return 0;

	}
}

