using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Kinect = Windows.Kinect;
using UnityEngine.UI;
using UnityEditor;

public class Button_click : MonoBehaviour {
	private FileStream fs;
	private StreamWriter sw;
	GameObject realtime01;
	private static int flag_record;
	private static int flag_replay;
	private static int flag_pause;
	private static int flag_stop;
	public static int frame_cou;
	private static int flag_lock;

	public static int record_play;
	public static int record_pause;
	public static int record_stop;
	public static int replay_play;
	public static int replay_pause;
	public static int replay_stop;

    public static float final_score;
    public static float sum_score;

    private int record_time;          // debug
	public static List<List<Vector3>> joint_replay = new List<List<Vector3>>();
	public static List<List<Vector3>> joint_replay2 = new List<List<Vector3>>();

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
		flag_record = 0;
		flag_stop = 0;
		flag_replay = 0;
		flag_pause = 0;
		frame_cou = 0;

		record_play = 0;
		record_pause = 0;
		record_stop = 0;
		replay_play = 0;
		replay_pause = 0;
		replay_stop = 0;

        sum_score = 0.0f;
        final_score = 0.0f;

        record_time = 400;       // debug

	}


	void Update () {
		string s = "record_play : " + record_play.ToString() + "\nrecord_pause : " + record_pause.ToString() + "\nreplay_play : " + replay_play.ToString() + "\nreplay_pause : " + replay_pause.ToString() + "\nframe_cou : " + frame_cou.ToString();
		//this.gameObject.transform.Find ("flag").GetComponent<Text> ().text = s;

		if (record_play == 1) {   //&& record_time>0) {
			realtime01 = GameObject.FindWithTag ("realtime01");
			if (realtime01 == null) {
				//this.gameObject.transform.Find ("Debug_Text").GetComponent<Text> ().text = "Can't detect your body";
			}
			else {
				//this.gameObject.transform.Find ("Debug_Text").GetComponent<Text> ().text = "Recording";
				List<Vector3> temp_list = new List<Vector3> ();
				List<Vector3> temp_list2 = new List<Vector3> ();
				for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++) {
					Vector3 temp;
					temp = realtime01.transform.Find (jt.ToString ()).position;
					temp_list.Add (temp);
					if (jt == Kinect.JointType.HandRight || jt == Kinect.JointType.WristRight || jt == Kinect.JointType.ElbowRight ||
						jt == Kinect.JointType.HandLeft || jt == Kinect.JointType.WristLeft || jt == Kinect.JointType.ElbowLeft ||
						jt == Kinect.JointType.SpineShoulder || jt == Kinect.JointType.SpineMid || jt == Kinect.JointType.Neck || jt == Kinect.JointType.Head ||
						jt == Kinect.JointType.HipRight || jt == Kinect.JointType.KneeRight || jt == Kinect.JointType.AnkleRight ||
						jt == Kinect.JointType.HipLeft || jt == Kinect.JointType.KneeLeft || jt == Kinect.JointType.AnkleLeft) {

						Vector3 temp2;
						temp2 = realtime01.transform.Find (jt.ToString ()).Find ("mainPoint").position;
						temp_list2.Add (temp2);
					}
				}
				joint_replay.Add (temp_list);
				joint_replay2.Add (temp_list2);
				//record_time--;             // debug
				Debug.Log ("In");

				/*if (record_time <= 0) {    // debug
					record_play = 0;
					record_pause = 0;
					record_stop = 1;
					Debug.Log ("joint_replay : " + joint_replay.Count.ToString () + "*" + joint_replay [0].Count.ToString ());
					Debug.Log ("joint_replay2 : " + joint_replay2.Count.ToString () + "*" + joint_replay2 [0].Count.ToString ());
					this.gameObject.transform.Find ("Debug_Text").GetComponent<Text> ().text = "Stop";
				}*/
			}
		}

		if (replay_play == 1) {
			if (GameObject.FindWithTag ("Body_Replay") == null) {
				Debug.Log ("Create Body_Replay");
				//------------------------------ Create "Body_Replay GameObject" ----------------------------------//
				CreateBodyObject();
			} 
			else {
				//this.gameObject.transform.Find ("Debug_Text").GetComponent<Text> ().text = "Replay";
				//------------------------------ RefreshBodyObject -----------------------------------//
				GameObject bodyObject = GameObject.FindWithTag("Body_Replay");
				for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++) {
					Transform jointObj = bodyObject.transform.Find(jt.ToString ());
					jointObj.position = joint_replay[frame_cou][Joint_to_Int[jt]];
					Material zzz_obj = bodyObject.transform.Find (jt.ToString ()).gameObject.GetComponent<Renderer> ().material;
					zzz_obj.color = Color.cyan;

					int next_joint;
					LineRenderer lr = jointObj.GetComponent<LineRenderer>();
					if (_BoneMap.ContainsKey (jt)) {
						next_joint = Joint_to_Int [_BoneMap [jt]];
						Vector3 v = new Vector3 ((joint_replay[frame_cou][next_joint].x ) * KinectManager.body_scale, (joint_replay[frame_cou][next_joint].y) * KinectManager.body_scale, (joint_replay[frame_cou][next_joint].z) * KinectManager.body_scale);
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
						main_trans.position = joint_replay2[frame_cou][Joint_to_Int2[jt]];

						float cylinderDistance = 0.5f * Vector3.Distance (main_trans.position, jointObj.position) / KinectManager.joint_scale;
						Transform point1 = jointObj.transform.Find("1");
						point1.localScale = new Vector3 (point1.localScale.x, cylinderDistance, point1.localScale.z);
						point1.LookAt (main_trans, Vector3.up);
						point1.rotation *= Quaternion.Euler (90f, 0f, 0f);

						if (cylinderDistance >= KinectManager.sensitive) point1.GetChild (0).gameObject.GetComponent<MeshRenderer>().enabled = true;
						else point1.GetChild (0).gameObject.GetComponent<MeshRenderer>().enabled = false;

                        if (jt != Kinect.JointType.SpineShoulder && jt != Kinect.JointType.SpineMid &&
                            jt != Kinect.JointType.Neck && jt != Kinect.JointType.Head)
                        {
                            if (cylinderDistance >= KinectManager.sensitive)
                            {
                                sum_score += (cylinderDistance - KinectManager.sensitive);
                            }
                        }
                    }
				}

                Debug.Log(sum_score.ToString());

                if ((sum_score - 1) <= 0)
                {
                    sum_score = 100;
                }
                else
                {
                    sum_score -= 1;
                    sum_score = (sum_score - 20) * -1;
                    sum_score *= 5;
                }

                final_score += sum_score;
                sum_score = 0.0f;

                frame_cou++;

				if (frame_cou >= joint_replay.Count) {
                    final_score /= frame_cou;
                    frame_cou = 0;
					replay_play = 0;
					replay_pause = 0;
					replay_stop = 1;
                    //this.gameObject.transform.Find ("Debug_Text").GetComponent<Text> ().text = "End of replay";
                    /*GameObject score_temp = GameObject.FindWithTag("score");
                    score_temp.transform.GetChild(0).GetComponent<Text>().text = final_score.ToString("F1");*/
				}
			}
		}
	}


	public void Record_Play(){
		if (record_play == 1)
			this.gameObject.transform.Find ("Debug_Text").GetComponent<Text> ().text = "You need to Stop first";
		else if (record_pause == 1) {
			record_pause = 0;
			record_play = 1;
		}
		else {
			joint_replay.Clear ();
			joint_replay2.Clear ();
			if (GameObject.FindWithTag ("Body_Replay") != null) {
				GameObject temp = GameObject.FindWithTag ("Body_Replay");
				GameObject.Destroy (temp);
				Debug.Log ("Destroy Body_Replay");
			}

			record_play = 1;
			record_stop = 0;
			replay_play = 0;
			replay_pause = 0;
			frame_cou = 0;

			record_time = 400; // debug

			Debug.Log ("Start Record");
		}
	}

	public void Record_Pause(){
		if (joint_replay.Count == 0 || record_stop == 1) {
			this.gameObject.transform.Find ("Debug_Text").GetComponent<Text> ().text = "Please record first";
		}
		else if (record_play == 1){
			record_play = 0;
			record_pause = 1;
			this.gameObject.transform.Find ("Debug_Text").GetComponent<Text> ().text = "Pause";
		}
	}

	public void Record_Stop(){

	}



	//---------------------------------------- Replay -----------------------------------------------------//



	public void Replay_Play(){
		if (joint_replay.Count == 0) {
			this.gameObject.transform.Find ("Debug_Text").GetComponent<Text> ().text = "Please record first";
		}
		else if (replay_pause == 1) {
			replay_pause = 0;
			replay_play = 1;
		}
		else {
			replay_play = 1;
			replay_stop = 0;
		}
	}

	public void Replay_Pause(){
		if (joint_replay.Count == 0 || replay_stop == 1) {
			this.gameObject.transform.Find ("Debug_Text").GetComponent<Text> ().text = "Please play first";
		}
		else if (replay_play == 1){
			replay_play = 0;
			replay_pause = 1;
			this.gameObject.transform.Find ("Debug_Text").GetComponent<Text> ().text = "Pause";
		}
	}

	public void Replay_Stop(){
		/*replay_play = 0;
		replay_pause = 0;
		replay_stop = 1;
		frame_cou = 0;
		this.gameObject.transform.Find ("Debug_Text").GetComponent<Text> ().text = "Stop";*/
	}

	public void Next(){
		GameObject temp = GameObject.FindWithTag ("Body_Replay");
		if (temp == null) {
			Debug.Log ("Create Body_Replay");
			CreateBodyObject ();
		}
		else temp.SetActive (true);

		GameObject bodyObject = GameObject.FindWithTag("Body_Replay");
		for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++) {
			Transform jointObj = bodyObject.transform.Find(jt.ToString ());
			jointObj.position = joint_replay[frame_cou][Joint_to_Int[jt]];
			//Debug.Log ("1 : " + jt.ToString());
			Material zzz_obj = bodyObject.transform.Find (jt.ToString ()).gameObject.GetComponent<Renderer> ().material;
			zzz_obj.color = Color.cyan;
			//Debug.Log ("2 : " + jt.ToString());
			int next_joint;
			LineRenderer lr = jointObj.GetComponent<LineRenderer>();
			if (_BoneMap.ContainsKey (jt)) {
				next_joint = Joint_to_Int [_BoneMap [jt]];
				//Debug.Log ("3 : " + jt.ToString());
				Vector3 v = new Vector3 ((joint_replay[frame_cou][next_joint].x ) * KinectManager.body_scale, (joint_replay[frame_cou][next_joint].y) * KinectManager.body_scale, (joint_replay[frame_cou][next_joint].z) * KinectManager.body_scale);
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
				main_trans.position = joint_replay2[frame_cou][Joint_to_Int2[jt]];

				float cylinderDistance = 0.5f * Vector3.Distance (main_trans.position, jointObj.position) / KinectManager.joint_scale;
				Transform point1 = jointObj.transform.Find("1");
				point1.localScale = new Vector3 (point1.localScale.x, cylinderDistance, point1.localScale.z);
				point1.LookAt (main_trans, Vector3.up);
				point1.rotation *= Quaternion.Euler (90f, 0f, 0f);

				if (cylinderDistance >= KinectManager.sensitive) point1.GetChild (0).gameObject.GetComponent<MeshRenderer>().enabled = true;
				else point1.GetChild (0).gameObject.GetComponent<MeshRenderer>().enabled = false;
			}
		}
		this.gameObject.transform.Find ("Debug_Text").GetComponent<Text> ().text = "Next";
	}

	public void Back(){
		GameObject temp = GameObject.FindWithTag ("Body_Replay");
		if (temp != null) {
			temp.SetActive (false);
		}
		replay_play = 0;
		replay_pause = 1;
		this.gameObject.transform.Find ("Debug_Text").GetComponent<Text> ().text = "Back";
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
