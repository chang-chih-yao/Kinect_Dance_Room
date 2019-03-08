using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using System;

public class BodySourceView : MonoBehaviour 
{
	public Material BoneMaterial;
	public GameObject BodySourceManager;
	public float body_scale;
	public float bone_scale;
	public float joint_scale;
	/*public Transform L_Elbow;
	public Transform L_UpperArm;
	public Transform L_Hand;
	public Transform Head;
	public Transform Neck;
	public Transform Spineshoulder;
	public Transform SpineMid;
	public Transform SpineMid2;
	public Transform SpineBase;
	public Transform SpineBase2;*/

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

	void Start (){
		/*GameObject colortest = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		colortest.transform.position = new Vector3(-8.47f, 8.02f, 7.42f);
		colortest.name = "colorPicker";

		Material joint_obj = colortest.GetComponent<Renderer> ().material;
		joint_obj.color = MyColorPicker.ShowColor;*/
	}

	void Update () 
	{
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
				continue;
			}

			if(body.IsTracked)
			{
				if(!_Bodies.ContainsKey(body.TrackingId))
				{
					_Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
				}

				RefreshBodyObject(body, _Bodies[body.TrackingId]);
			}
		}
	}

	private GameObject CreateBodyObject(ulong id)
	{
		GameObject body = new GameObject("Body:" + id);

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

		}
		return body;
	}

	private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
	{
		for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
		{
			Kinect.Joint sourceJoint = body.Joints[jt];
			Kinect.JointOrientation source_ori = body.JointOrientations [jt];
			Kinect.Joint? targetJoint = null;

			if(_BoneMap.ContainsKey(jt))
			{
				targetJoint = body.Joints[_BoneMap[jt]];
			}

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
			else
			{
				lr.enabled = false;
			}


			/*if (jt == Kinect.JointType.ElbowLeft) {
				L_Elbow.position = GetVector3FromJoint(sourceJoint, body_scale);
				Quaternion temp;
				temp.w = source_ori.Orientation.W;
				temp.x = source_ori.Orientation.X;
				temp.y = source_ori.Orientation.Y;
				temp.z = source_ori.Orientation.Z;
				L_Elbow.rotation = temp;
			}
			if (jt == Kinect.JointType.ShoulderLeft) {
				L_UpperArm.position = GetVector3FromJoint(sourceJoint, body_scale);
				Quaternion temp;
				temp.w = source_ori.Orientation.W;
				temp.x = source_ori.Orientation.X;
				temp.y = source_ori.Orientation.Y;
				temp.z = source_ori.Orientation.Z;
				L_UpperArm.rotation = temp;
			}
			if (jt == Kinect.JointType.WristLeft) {
				L_Hand.position = GetVector3FromJoint(sourceJoint, body_scale);
				Quaternion temp;
				temp.w = source_ori.Orientation.W;
				temp.x = source_ori.Orientation.X;
				temp.y = source_ori.Orientation.Y;
				temp.z = source_ori.Orientation.Z;
				L_Hand.rotation = temp;
			}
			if (jt == Kinect.JointType.Head) {
				Head.position = GetVector3FromJoint(sourceJoint, body_scale);
				Quaternion temp;
				temp.w = source_ori.Orientation.W;
				temp.x = source_ori.Orientation.X;
				temp.y = source_ori.Orientation.Y;
				temp.z = source_ori.Orientation.Z;
				Head.rotation = temp;
			}
			if (jt == Kinect.JointType.Neck) {
				Neck.position = GetVector3FromJoint(sourceJoint, body_scale);
				Quaternion temp;
				temp.w = source_ori.Orientation.W;
				temp.x = source_ori.Orientation.X;
				temp.y = source_ori.Orientation.Y;
				temp.z = source_ori.Orientation.Z;
				Neck.rotation = temp;
			}
			if (jt == Kinect.JointType.SpineShoulder) {
				Spineshoulder.position = GetVector3FromJoint(sourceJoint, body_scale);
				Quaternion temp;
				temp.w = source_ori.Orientation.W;
				temp.x = source_ori.Orientation.X;
				temp.y = source_ori.Orientation.Y;
				temp.z = source_ori.Orientation.Z;
				Spineshoulder.rotation = temp;
			}
			if (jt == Kinect.JointType.SpineMid) {
				SpineMid.position = GetVector3FromJoint(sourceJoint, body_scale);
				SpineMid2.position = GetVector3FromJoint(sourceJoint, body_scale);
				Quaternion temp;
				temp.w = source_ori.Orientation.W;
				temp.x = source_ori.Orientation.X;
				temp.y = source_ori.Orientation.Y;
				temp.z = source_ori.Orientation.Z;
				SpineMid.rotation = temp;
				SpineMid2.rotation = temp;
			}
			if (jt == Kinect.JointType.SpineBase) {
				SpineBase.position = GetVector3FromJoint(sourceJoint, body_scale);
				SpineBase2.position = GetVector3FromJoint(sourceJoint, body_scale);
				Quaternion temp;
				temp.w = source_ori.Orientation.W;
				temp.x = source_ori.Orientation.X;
				temp.y = source_ori.Orientation.Y;
				temp.z = source_ori.Orientation.Z;
				SpineBase.rotation = temp;
				SpineBase2.rotation = temp;
			}*/
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
		return new Vector3((joint.Position.X - 1.0f) * scale, (joint.Position.Y + 1.0f) * scale, (joint.Position.Z - 1.6f) * scale);
	}
}
