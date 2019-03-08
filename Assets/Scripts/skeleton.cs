using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Kinect = Windows.Kinect;

public class skeleton : MonoBehaviour {
	
	public GameObject[] skeletonlist,modlelist;
	public GameObject skeletonjoint, modeljoint;
	public GameObject skeleton_spinebased;
	public GameObject skeleton_hipleft;
	public GameObject skeleton_hipright;
	public GameObject skeleton_spinemid;
	public GameObject skeleton_kneeleft;
	public GameObject skeleton_ankleleft;
	public GameObject skeleton_footleft;
	public GameObject skeleton_kneeright;
	public GameObject skeleton_ankleright;
	public GameObject skeleton_footright;
	public GameObject skeleton_spineshoulder;
	public GameObject skeleton_neck;
	public GameObject skeleton_shoulderleft;
	public GameObject skeleton_head;
	public GameObject skeleton_shoulderright;
	public GameObject skeleton_elbowleft;
	public GameObject skeleton_wristleft;
	public GameObject skeleton_handleft;
	public GameObject skeleton_handtipleft;
	public GameObject skeleton_thumbleft;
	public GameObject skeleton_elbowright;
	public GameObject skeleton_wristright;
	public GameObject skeleton_handright;
	public GameObject skeleton_handtipright;
	public GameObject skeleton_thumbright;

	public GameObject model_spinebased;
	public GameObject model_hipleft;
	public GameObject model_hipright;
	public GameObject model_spinemid;
	public GameObject model_kneeleft;
	public GameObject model_ankleleft;
	public GameObject model_footleft;
	public GameObject model_kneeright;
	public GameObject model_ankleright;
	public GameObject model_footright;
	public GameObject model_spineshoulder;
	public GameObject model_neck;
	public GameObject model_shoulderleft;
	public GameObject model_head;
	public GameObject model_shoulderright;
	public GameObject model_elbowleft;
	public GameObject model_wristleft;
	public GameObject model_handleft;
	public GameObject model_handtipleft;
	public GameObject model_thumbleft;
	public GameObject model_elbowright;
	public GameObject model_wristright;
	public GameObject model_handright;
	public GameObject model_handtipright;
	public GameObject model_thumbright;

	public float length;


	private Dictionary<int, int> _BoneMap = new Dictionary<int, int>()
	{
		{7, 6},
		{6, 2},
		{2, 0},
		{0, 1},
		{1, 4},
		{4, 5},

		{11, 9},
		{9, 8},
		{8, 3},
		{3, 0},

		{15, 14},
		{14, 13},
		{13, 10},
		{10, 8},

		{18, 17},
		{17, 16},
		{16, 12},
		{12, 8},
	};

	// Use this for initialization
	void Start () {
		skeletonlist = new GameObject[]{
			skeleton_spinebased        // 0
			,skeleton_hipleft          // 1
			,skeleton_hipright         // 2
			,skeleton_spinemid         // 3
			,skeleton_kneeleft         // 4
			,skeleton_ankleleft        // 5
			,skeleton_kneeright        // 6
			,skeleton_ankleright       // 7
			,skeleton_spineshoulder    // 8
			,skeleton_neck             // 9
			,skeleton_shoulderleft     // 10
			,skeleton_head             // 11
			,skeleton_shoulderright    // 12
			,skeleton_elbowleft        // 13
			,skeleton_wristleft        // 14
			,skeleton_handleft         // 15
			,skeleton_elbowright       // 16
			,skeleton_wristright       // 17
			,skeleton_handright        // 18
		};
		modlelist = new GameObject[] { model_spinebased
			, model_hipleft
			, model_hipright
			, model_spinemid
			, model_kneeleft
			, model_ankleleft
			, model_kneeright
			, model_ankleright
			, model_spineshoulder
			, model_neck
			, model_shoulderleft
			, model_head
			, model_shoulderright
			, model_elbowleft
			, model_wristleft
			, model_handleft
			, model_elbowright
			, model_wristright
			, model_handright


		};
		skeleton_init ();

		s_m_transform2 ();
		s_m_initcolor ();

	}

	// Update is called once per frame
	void Update () {
		s_m_transform2 ();
		s_m_initcolor ();

	}

	void skeleton_init(){
		for (int i = 0; i < skeletonlist.Length; i++) {
			GameObject jointObj = skeletonlist [i];
			LineRenderer lr = jointObj.AddComponent<LineRenderer>();
			lr.SetVertexCount(2);
			//lr.material = BoneMaterial;
			lr.material = new Material(Shader.Find("Particles/Additive"));
			lr.SetWidth(KinectManager.bone_scale, KinectManager.bone_scale);
		}
	}

	void s_m_transform2(){
		for (int i = 0; i < modlelist.Length; i++) {
			skeletonlist [i].gameObject.transform.position = modlelist [i].gameObject.transform.position;
		}
		skeletonlist [1].gameObject.transform.position = modlelist [2].gameObject.transform.position;
		skeletonlist [2].gameObject.transform.position = modlelist [1].gameObject.transform.position;
		skeletonlist [4].gameObject.transform.position = modlelist [6].gameObject.transform.position;
		skeletonlist [5].gameObject.transform.position = modlelist [7].gameObject.transform.position;
		skeletonlist [6].gameObject.transform.position = modlelist [4].gameObject.transform.position;
		skeletonlist [7].gameObject.transform.position = modlelist [5].gameObject.transform.position;
		skeletonlist [10].gameObject.transform.position = modlelist [12].gameObject.transform.position;
		skeletonlist [12].gameObject.transform.position = modlelist [10].gameObject.transform.position;
		skeletonlist [13].gameObject.transform.position = modlelist [16].gameObject.transform.position;
		skeletonlist [14].gameObject.transform.position = modlelist [17].gameObject.transform.position;
		skeletonlist [15].gameObject.transform.position = modlelist [18].gameObject.transform.position;
		skeletonlist [16].gameObject.transform.position = modlelist [13].gameObject.transform.position;
		skeletonlist [17].gameObject.transform.position = modlelist [14].gameObject.transform.position;
		skeletonlist [18].gameObject.transform.position = modlelist [15].gameObject.transform.position;

		skeleton_spinebased.gameObject.transform.position =model_spinebased.gameObject.transform.position +new Vector3(length,0,0);

		for (int i = 0; i < skeletonlist.Length; i++) {
			Transform jointObj = skeletonlist [i].transform;
			int next_joint;
			LineRenderer lr = jointObj.GetComponent<LineRenderer>();
			if (_BoneMap.ContainsKey (i)) {
				next_joint = _BoneMap [i];
				Vector3 v = new Vector3 ((skeletonlist[next_joint].transform.position.x ) * KinectManager.body_scale, (skeletonlist[next_joint].transform.position.y) * KinectManager.body_scale, (skeletonlist[next_joint].transform.position.z) * KinectManager.body_scale);
				lr.SetPosition(0, jointObj.position);
				lr.SetPosition(1, v);
				lr.SetColors(Color.white, Color.white);
			}
			else lr.enabled = false;
		}
	}

	void s_m_initcolor(){
		string materialname="Materials/skeleton";

		for (int i = 0; i < skeletonlist.Length; i++) {
			Renderer rend = skeletonlist [i].GetComponent<Renderer> ();
			rend.enabled = true;
			rend.sharedMaterial =  Resources.Load (materialname, typeof(Material)) as Material;
			//rend.material.color = Color.red;

		}

	}

}
