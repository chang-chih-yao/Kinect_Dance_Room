using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Kinect = Windows.Kinect;

public class pushbutton : hoverbutton {

	public GameObject close,open;
	public Vector3 model_move,realtime_move;
	protected Camera maincamera;
	protected GameObject realtime_relation;
	public bool hidden_replay;
	public bool next;
	public bool back;

	private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
	{
		{ Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
		{ Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
		{ Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
		{ Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },

		{ Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
		{ Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
		{ Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
		{ Kinect.JointType.HipRight, Kinect.JointType.SpineBase },

		{ Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
		{ Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
		{ Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
		{ Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
		{ Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
		{ Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },

		{ Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
		{ Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
		{ Kinect.JointType.HandRight, Kinect.JointType.WristRight },
		{ Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
		{ Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
		{ Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },

		{ Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
		{ Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
		{ Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
		{ Kinect.JointType.Neck, Kinect.JointType.Head },
	};

	private Dictionary<Kinect.JointType, int> Joint_to_Int = new Dictionary<Kinect.JointType, int>()
	{
		{ Kinect.JointType.SpineBase, 0 },
		{ Kinect.JointType.SpineMid, 1 },
		{ Kinect.JointType.Neck, 2 },
		{ Kinect.JointType.Head, 3 },
		{ Kinect.JointType.ShoulderLeft, 4 },
		{ Kinect.JointType.ElbowLeft, 5 },
		{ Kinect.JointType.WristLeft, 6 },
		{ Kinect.JointType.HandLeft, 7 },
		{ Kinect.JointType.ShoulderRight, 8 },
		{ Kinect.JointType.ElbowRight, 9 },
		{ Kinect.JointType.WristRight, 10 },
		{ Kinect.JointType.HandRight, 11 },
		{ Kinect.JointType.HipLeft, 12 },
		{ Kinect.JointType.KneeLeft, 13 },
		{ Kinect.JointType.AnkleLeft, 14 },
		{ Kinect.JointType.FootLeft, 15 },
		{ Kinect.JointType.HipRight, 16 },
		{ Kinect.JointType.KneeRight, 17 },
		{ Kinect.JointType.AnkleRight, 18 },
		{ Kinect.JointType.FootRight, 19 },
		{ Kinect.JointType.SpineShoulder, 20 },
		{ Kinect.JointType.HandTipLeft, 21 },
		{ Kinect.JointType.ThumbLeft, 22 },
		{ Kinect.JointType.HandTipRight, 23 },
		{ Kinect.JointType.ThumbRight, 24 },
	};

	private Dictionary<Kinect.JointType, int> Joint_to_Int2 = new Dictionary<Kinect.JointType, int>()
	{
		{ Kinect.JointType.SpineMid, 0 },
		{ Kinect.JointType.Neck, 1 },
		{ Kinect.JointType.Head, 2 },
		{ Kinect.JointType.ElbowLeft, 3 },
		{ Kinect.JointType.WristLeft, 4 },
		{ Kinect.JointType.HandLeft, 5 },
		{ Kinect.JointType.ElbowRight, 6 },
		{ Kinect.JointType.WristRight, 7 },
		{ Kinect.JointType.HandRight, 8 },
		{ Kinect.JointType.HipLeft, 9 },
		{ Kinect.JointType.KneeLeft, 10 },
		{ Kinect.JointType.AnkleLeft, 11 },
		{ Kinect.JointType.HipRight, 12 },
		{ Kinect.JointType.KneeRight, 13 },
		{ Kinect.JointType.AnkleRight, 14 },
		{ Kinect.JointType.SpineShoulder, 15 },
	};

	// Use this for initialization
	void Start () {
		realtime_relation = GameObject.Find ("realtime_relation");
		init ();
		button_image_select = Resources.Load("04/none",typeof(Sprite)) as Sprite;
		camera_init ();
	}

	// Update is called once per frame
	void Update () {

		handposition = hand.transform.localPosition;
		mytimer ();
	}
	public void camera_init(){
		maincamera = GameObject.Find ("Main Camera").GetComponent<Camera>();

	}

	void music_and_dancer(){
		selectaudio.Stop ();
		animator.enabled = true;
		animator.Play ("Base Layer.idle");
		animator.SetInteger("music", 0);//dnacerindex(dancername.text)

	}

	public override void turn(){
		button_image.sprite = button_image_select;
		close.SetActive (false);
		open.SetActive (true);
		realtime_relation.transform.position = realtime_move;
		modle.gameObject.transform.position=maincamera.transform.position +model_move;
		compareline = compareline_on_off;
		hoverbutton.buttom_name="null";
		hand_animator.Play ("Base Layer.none");
		music_and_dancer ();

		if (next == true) {  // next page
            Debug.Log("next");
			GameObject temp = GameObject.FindWithTag ("Body_Replay");
			if (temp == null) {
				Debug.Log ("Create Body_Replay");
				CreateBodyObject ();
			}
			else temp.SetActive (true);

			GameObject bodyObject = GameObject.FindWithTag("Body_Replay");
			for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++) {
				Transform jointObj = bodyObject.transform.Find(jt.ToString ());
				jointObj.position = Button_click.joint_replay[Button_click.frame_cou][Joint_to_Int[jt]];
				//Debug.Log ("1 : " + jt.ToString());
				Material zzz_obj = bodyObject.transform.Find (jt.ToString ()).gameObject.GetComponent<Renderer> ().material;
				zzz_obj.color = Color.cyan;
				//Debug.Log ("2 : " + jt.ToString());
				int next_joint;
				LineRenderer lr = jointObj.GetComponent<LineRenderer>();
				if (_BoneMap.ContainsKey (jt)) {
					next_joint = Joint_to_Int [_BoneMap [jt]];
					//Debug.Log ("3 : " + jt.ToString());
					Vector3 v = new Vector3 ((Button_click.joint_replay[Button_click.frame_cou][next_joint].x ) * KinectManager.body_scale, (Button_click.joint_replay[Button_click.frame_cou][next_joint].y) * KinectManager.body_scale, (Button_click.joint_replay[Button_click.frame_cou][next_joint].z) * KinectManager.body_scale);
					lr.SetPosition(0, jointObj.position);
					lr.SetPosition(1, v);
					lr.SetColors(Color.white, Color.white);
				}
				else lr.enabled = false;

				if (jt == Kinect.JointType.FootLeft || jt == Kinect.JointType.FootRight || jt == Kinect.JointType.HandTipLeft || jt == Kinect.JointType.HandTipRight ||
					jt == Kinect.JointType.ThumbLeft || jt == Kinect.JointType.ThumbRight) {
					lr.enabled = false;
				}

				if (jt == Kinect.JointType.HandRight || jt == Kinect.JointType.WristRight || jt == Kinect.JointType.ElbowRight ||
					jt == Kinect.JointType.HandLeft || jt == Kinect.JointType.WristLeft || jt == Kinect.JointType.ElbowLeft ||
					jt == Kinect.JointType.SpineShoulder || jt == Kinect.JointType.SpineMid || jt == Kinect.JointType.Neck || jt == Kinect.JointType.Head ||
					jt == Kinect.JointType.HipRight || jt == Kinect.JointType.KneeRight || jt == Kinect.JointType.AnkleRight ||
					jt == Kinect.JointType.HipLeft || jt == Kinect.JointType.KneeLeft || jt == Kinect.JointType.AnkleLeft) {

					Transform main_trans = jointObj.transform.Find("mainPoint");
					main_trans.position = Button_click.joint_replay2[Button_click.frame_cou][Joint_to_Int2[jt]];

					float cylinderDistance = 0.5f * Vector3.Distance (main_trans.position, jointObj.position) / KinectManager.joint_scale;
					Transform point1 = jointObj.transform.Find("1");
					point1.localScale = new Vector3 (point1.localScale.x, cylinderDistance, point1.localScale.z);
					point1.LookAt (main_trans, Vector3.up);
					point1.rotation *= Quaternion.Euler (90f, 0f, 0f);

					if (cylinderDistance >= KinectManager.sensitive) point1.GetChild (0).gameObject.GetComponent<MeshRenderer>().enabled = true;
					else point1.GetChild (0).gameObject.GetComponent<MeshRenderer>().enabled = false;
				}
			}
            GameObject.Find("Score").GetComponentInChildren<Text>().text = "0";
            Debug.Log ("Next");
		}

		if (back == true) {
			GameObject temp = GameObject.FindWithTag ("Body_Replay");
			if (temp != null) {
				temp.SetActive (false);
			}
			Button_click.replay_play = 0;
			Button_click.replay_pause = 1;
            
            Debug.Log ("Back");
		}
	}


	private void CreateBodyObject(){
		GameObject body = new GameObject ("Body_Replay");
		body.transform.position = new Vector3 (0f, 0f, 0f);
		body.tag = "Body_Replay";

		for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++) {
			GameObject jointObj = GameObject.CreatePrimitive (PrimitiveType.Sphere);

			LineRenderer lr = jointObj.AddComponent<LineRenderer> ();
			lr.SetVertexCount (2);
			//lr.material = BoneMaterial;
			lr.material = new Material (Shader.Find ("Particles/Additive"));
			lr.SetWidth (KinectManager.bone_scale, KinectManager.bone_scale);

			jointObj.transform.localScale = new Vector3 (KinectManager.joint_scale, KinectManager.joint_scale, KinectManager.joint_scale);
			jointObj.name = jt.ToString ();
			jointObj.transform.parent = body.transform;

			//---------------------- Arrow GameObject ------------------------------//
			GameObject mainPoint = new GameObject ();
			mainPoint.transform.parent = jointObj.transform;
			mainPoint.name = "mainPoint";

			GameObject point1 = new GameObject ();
			point1.transform.parent = jointObj.transform;
			point1.name = "1";

			GameObject cylinder = new GameObject ();
			cylinder.name = "cylinder";
			cylinder.transform.parent = point1.transform;
			cylinder.transform.localPosition = new Vector3 (0f, 1f, 0f);
			cylinder.transform.localScale = new Vector3 (KinectManager.radius, 1f, KinectManager.radius);
			MeshFilter line_mesh = cylinder.AddComponent<MeshFilter> ();
			line_mesh.mesh = KinectManager.cylinderMesh;
			MeshRenderer line_Renderer = cylinder.AddComponent<MeshRenderer> ();
			line_Renderer.material = KinectManager.lineMat;

			cylinder.GetComponent<MeshRenderer> ().enabled = false;
		}
	}

}