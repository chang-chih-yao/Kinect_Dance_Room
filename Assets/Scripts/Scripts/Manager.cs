using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Threading;
using Kinect = Windows.Kinect;

public class Manager : MonoBehaviour {

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

	public Vector3 length;

	/*
	private GameObject[] modellist={
	  	 model_spinebased
		,model_hipleft
		,model_hipright
		,model_spinemid
		,model_kneeleft
		,model_ankleleft
		,model_footleft
		,model_kneeright
		,model_ankleright
		,model_footright
		,model_spineshoulder
		,model_neck
		,model_shoulderleft
		,model_head
		,model_shoulderright
		,model_elbowleft
		,model_wristleft
		,model_handleft
		,model_handtipleft
		,model_thumbleft
		,model_elbowright
		,model_wristright
		,model_handright
		,model_handtipright
		,model_thumbright


	}, skeletonlist={
		skeleton_spinebased
		,skeleton_hipleft
		,skeleton_hipright
		,skeleton_spinemid
		,skeleton_kneeleft
		,skeleton_ankleleft
		,skeleton_footleft
		,skeleton_kneeright
		,skeleton_ankleright
		,skeleton_footright
		,skeleton_spineshoulder
		,skeleton_neck
		,skeleton_shoulderleft
		,skeleton_head
		,skeleton_shoulderright
		,skeleton_elbowleft
		,skeleton_wristleft
		,skeleton_handleft
		,skeleton_handtipleft
		,skeleton_thumbleft
		,skeleton_elbowright
		,skeleton_wristright
		,skeleton_handright
		,skeleton_handtipright
		,skeleton_thumbright


	};
	*/

	public Material lineMat;
	public Mesh cylinderMesh;
	public float radius = 0.03f;
	public float sensitive = 1.0f;

	public Material BoneMaterial;
	public GameObject BodySourceManager;
	public float body_scale;
	public float bone_scale;
	public float joint_scale;

	private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
	private BodySourceManager _BodyManager;

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

	private Dictionary<Kinect.JointType, Kinect.JointType> myBoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
	{
		{ Kinect.JointType.HandRight, Kinect.JointType.WristRight },
		{ Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
		{ Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },

		{ Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
		{ Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
		{ Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },

		{ Kinect.JointType.Head, Kinect.JointType.Neck },
		{ Kinect.JointType.Neck, Kinect.JointType.SpineShoulder },
		{ Kinect.JointType.SpineShoulder, Kinect.JointType.SpineMid },
		{ Kinect.JointType.SpineMid, Kinect.JointType.SpineBase },

		{ Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
		{ Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
		{ Kinect.JointType.HipRight, Kinect.JointType.SpineBase },

		{ Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
		{ Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
		{ Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },
	};

	private Dictionary<Kinect.JointType, int> myBoneMap2 = new Dictionary<Kinect.JointType, int>()
	{
		{ Kinect.JointType.SpineBase, 0 },
		{ Kinect.JointType.HipLeft, 1 },
		{ Kinect.JointType.HipRight, 2 },
		{ Kinect.JointType.SpineMid, 3 },
		{ Kinect.JointType.KneeLeft, 4 },
		{ Kinect.JointType.AnkleLeft, 5 },
		{ Kinect.JointType.KneeRight, 6 },
		{ Kinect.JointType.AnkleRight, 7 },
		{ Kinect.JointType.SpineShoulder, 8 },
		{ Kinect.JointType.Neck, 9 },
		{ Kinect.JointType.ShoulderLeft, 10 },
		{ Kinect.JointType.Head, 11 },
		{ Kinect.JointType.ShoulderRight, 12 },
		{ Kinect.JointType.ElbowLeft, 13 },
		{ Kinect.JointType.WristLeft, 14 },
		{ Kinect.JointType.HandLeft, 15 },
		{ Kinect.JointType.ElbowRight, 16 },
		{ Kinect.JointType.WristRight, 17 },
		{ Kinect.JointType.HandRight, 18 },
	};





	void Start () {
		skeletonlist = new GameObject[]{
			skeleton_spinebased
			,skeleton_hipleft
			,skeleton_hipright
			,skeleton_spinemid
			,skeleton_kneeleft
			,skeleton_ankleleft
			,skeleton_kneeright
			,skeleton_ankleright
			,skeleton_spineshoulder
			,skeleton_neck
			,skeleton_shoulderleft
			,skeleton_head
			,skeleton_shoulderright
			,skeleton_elbowleft
			,skeleton_wristleft
			,skeleton_handleft
			,skeleton_elbowright
			,skeleton_wristright
			,skeleton_handright };

		modlelist = new GameObject[] {
			model_spinebased       // 0
			, model_hipleft        // 1
			, model_hipright       // 2
			, model_spinemid       // 3
			, model_kneeleft       // 4
			, model_ankleleft      // 5
			, model_kneeright      // 6
			, model_ankleright     // 7
			, model_spineshoulder  // 8
			, model_neck           // 9
			, model_shoulderleft   // 10
			, model_head           // 11
			, model_shoulderright  // 12
			, model_elbowleft      // 13
			, model_wristleft      // 14
			, model_handleft       // 15
			, model_elbowright     // 16
			, model_wristright     // 17
			, model_handright      // 18
		};
		s_m_transform3 ();
		//s_m_initcolor ();
	}

	// Update is called once per frame
	void Update () {
		

		//s_m_initcolor ();


		if (BodySourceManager == null)
		{
			return;
		}

		_BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
		if (_BodyManager == null)
		{
			return;
		}

		Kinect.Body[] data = _BodyManager.GetData();
		if (data == null)
		{
			return;
		}

		List<ulong> trackedIds = new List<ulong>();
		foreach(var body in data)
		{
			if (body == null)
			{
				continue;
			}

			if(body.IsTracked)
			{
				trackedIds.Add (body.TrackingId);
			}
		}

		List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

		// First delete untracked bodies
		foreach(ulong trackingId in knownIds)
		{
			if(!trackedIds.Contains(trackingId))
			{
				Destroy(_Bodies[trackingId]);
				_Bodies.Remove(trackingId);
			}
		}

		foreach(var body in data)
		{
			if (body == null)
			{
				//s_m_transform3 ();
				continue;
			}

			if (body.IsTracked) {
				if (!_Bodies.ContainsKey (body.TrackingId)) {
					_Bodies [body.TrackingId] = CreateBodyObject (body.TrackingId);
				}

				RefreshBodyObject (body, _Bodies [body.TrackingId]);
				//s_m_transform2 (_Bodies [body.TrackingId]);
			}
			//else s_m_transform3 ();
		}
		s_m_transform3 ();
	}

	void s_m_transform3(){
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
		skeleton_spinebased.gameObject.transform.position += length;
	}
		

	void s_m_initcolor(){
		string materialname="Materials/skeleton";

		for (int i = 0; i < skeletonlist.Length; i++) {
			Renderer rend = skeletonlist [i].GetComponent<Renderer> ();
			rend.enabled = true;
			rend.sharedMaterial =  Resources.Load (materialname, typeof(Material)) as Material;
			rend.material.color = Color.white;

		}
	}


	private GameObject CreateBodyObject(ulong id)
	{
		GameObject body = new GameObject("realtime01");

		for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
		{ 
			
			GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);

			LineRenderer lr = jointObj.AddComponent<LineRenderer>();
			lr.SetVertexCount(2);
			//lr.material = BoneMaterial;
			lr.material = new Material(Shader.Find("Particles/Additive"));
			lr.SetWidth(bone_scale, bone_scale);

			jointObj.transform.localScale = new Vector3(joint_scale, joint_scale, joint_scale);
			jointObj.name = jt.ToString();
			jointObj.transform.parent = body.transform;

			/*---------------------- Arrow GameObject ------------------------------*/
			GameObject mainPoint = new GameObject();
			mainPoint.transform.parent = jointObj.transform;
			mainPoint.name = "mainPoint";

			GameObject point1 = new GameObject ();
			point1.transform.parent = jointObj.transform;
			point1.name = "1";

			GameObject cylinder = new GameObject ();
			cylinder.name = "cylinder";
			cylinder.transform.parent = point1.transform;
			cylinder.transform.localPosition = new Vector3 (0f, 1f, 0f);
			cylinder.transform.localScale = new Vector3 (radius, 1f, radius);
			MeshFilter line_mesh = cylinder.AddComponent<MeshFilter> ();
			line_mesh.mesh = cylinderMesh;
			MeshRenderer line_Renderer = cylinder.AddComponent<MeshRenderer> ();
			line_Renderer.material = lineMat;

			cylinder.GetComponent<MeshRenderer> ().enabled = false;
		}
		return body;
	}

	private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
	{
		for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
		{
			Kinect.Joint sourceJoint = body.Joints[jt];
			Kinect.Joint? targetJoint = null;

			if(_BoneMap.ContainsKey(jt))
				targetJoint = body.Joints[_BoneMap[jt]];

			Material zzz_obj = bodyObject.transform.Find (jt.ToString ()).gameObject.GetComponent<Renderer> ().material;
			zzz_obj.color = MyColorPicker.ShowColor;

			Transform jointObj = bodyObject.transform.Find(jt.ToString ());
			jointObj.localPosition = GetVector3FromJoint(sourceJoint, body_scale);

			LineRenderer lr = jointObj.GetComponent<LineRenderer>();
			if(targetJoint.HasValue)
			{
				lr.SetPosition(0, jointObj.localPosition);
				lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value, body_scale));
				lr.SetColors(GetColorForState (sourceJoint.TrackingState), GetColorForState(targetJoint.Value.TrackingState));
			}
			else lr.enabled = false;

			/*if(jt == Kinect.JointType.ShoulderRight){
				Vector3 model_vector = skeletonlist [16].gameObject.transform.position - skeletonlist [12].gameObject.transform.position;

				lr.SetVertexCount (4);
				lr.SetPosition(2, jointObj.localPosition);
				lr.SetPosition(3, jointObj.localPosition + model_vector);
				lr.SetColors(Color.white, Color.green);

				Transform jointObj1 = bodyObject.transform.Find(Kinect.JointType.ElbowRight.ToString ());
				LineRenderer move_line = jointObj1.GetComponent<LineRenderer>();
				move_line.SetVertexCount (4);
				move_line.SetPosition(2, jointObj1.localPosition);
				move_line.SetPosition(3, jointObj.localPosition + model_vector);
				move_line.SetColors (Color.blue, Color.blue);
				move_line.SetWidth (bone_scale, bone_scale * 2.0f);
			}*/

			if (jt == Kinect.JointType.HandRight || jt == Kinect.JointType.WristRight || jt == Kinect.JointType.ElbowRight ||
				jt == Kinect.JointType.HandLeft || jt == Kinect.JointType.WristLeft || jt == Kinect.JointType.ElbowLeft ||
				jt == Kinect.JointType.SpineShoulder || jt == Kinect.JointType.SpineMid || jt == Kinect.JointType.Neck || jt == Kinect.JointType.Head ||
				jt == Kinect.JointType.HipRight || jt == Kinect.JointType.KneeRight || jt == Kinect.JointType.AnkleRight ||
				jt == Kinect.JointType.HipLeft || jt == Kinect.JointType.KneeLeft || jt == Kinect.JointType.AnkleLeft)
			{
				Kinect.JointType k = myBoneMap [jt];    // bone map
				Transform relative_joint = bodyObject.transform.Find(k.ToString ());
				Vector3 model_vector = skeletonlist [myBoneMap2[jt]].transform.position - skeletonlist [myBoneMap2[k]].transform.position;
				Transform main_trans = jointObj.transform.Find("mainPoint");
				main_trans.position = relative_joint.localPosition + model_vector;

				float cylinderDistance = 0.5f * Vector3.Distance (main_trans.position, jointObj.position) / joint_scale;
				Transform point1 = jointObj.transform.Find("1");
				point1.localScale = new Vector3 (point1.localScale.x, cylinderDistance, point1.localScale.z);
				point1.LookAt (main_trans, Vector3.up);
				point1.rotation *= Quaternion.Euler (90f, 0f, 0f);

				if (cylinderDistance >= sensitive) point1.GetChild (0).gameObject.GetComponent<MeshRenderer>().enabled = true;
				else point1.GetChild (0).gameObject.GetComponent<MeshRenderer>().enabled = false;

				//Debug.DrawLine (relative_joint.localPosition, relative_joint.localPosition + model_vector, Color.green);
			}
		}
	}

	private static Color GetColorForState(Kinect.TrackingState state)
	{
		switch (state)
		{
		case Kinect.TrackingState.Tracked:
			return Color.white;

		case Kinect.TrackingState.Inferred:
			return Color.red;

		default:
			return Color.black;
		}
	}

	private static Vector3 GetVector3FromJoint(Kinect.Joint joint, float scale)
	{
		return new Vector3((joint.Position.X - 1.0f) * scale, (joint.Position.Y + 0.8f) * scale, (joint.Position.Z - 1.8f) * scale);
	}
}
