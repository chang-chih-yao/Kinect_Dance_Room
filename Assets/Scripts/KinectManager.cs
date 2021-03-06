using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Kinect = Windows.Kinect;
using System.Text;
using UnityEditor;

public class KinectManager : MonoBehaviour
{/*
	public GameObject real_time_skeleton_spinebased;
	public GameObject real_time_skeleton_hipleft;
	public GameObject real_time_skeleton_hipright;
	public GameObject real_time_skeleton_spinemid;
	public GameObject real_time_skeleton_kneeleft;
	public GameObject real_time_skeleton_ankleleft;
	public GameObject real_time_skeleton_kneeright;
	public GameObject real_time_skeleton_ankleright;
	public GameObject real_time_skeleton_spineshoulder;
	public GameObject real_time_skeleton_neck;
	public GameObject real_time_skeleton_shoulderleft;
	public GameObject real_time_skeleton_head;
	public GameObject real_time_skeleton_shoulderright;
	public GameObject real_time_skeleton_elbowleft;
	public GameObject real_time_skeleton_wristleft;
	public GameObject real_time_skeleton_handleft;
	public GameObject real_time_skeleton_elbowright;
	public GameObject real_time_skeleton_wristright;
	public GameObject real_time_skeleton_handright;*/
	private float x,y,z;

	public Camera maincamera;
	private GameObject realtime_relation;
	private Kinect.JointType[] jointtype={
		Kinect.JointType.SpineBase,
		Kinect.JointType.HipLeft,
		Kinect.JointType.HipRight,
		Kinect.JointType.SpineMid,
		Kinect.JointType.KneeLeft,
		Kinect.JointType.AnkleLeft,
		Kinect.JointType.KneeRight,
		Kinect.JointType.AnkleRight,
		Kinect.JointType.SpineShoulder,
		Kinect.JointType.Neck,
		Kinect.JointType.ShoulderLeft,
		Kinect.JointType.Head,
		Kinect.JointType.ShoulderRight,
		Kinect.JointType.ElbowLeft,
		Kinect.JointType.WristLeft,
		Kinect.JointType.HandLeft,
		Kinect.JointType.ElbowRight,
		Kinect.JointType.WristRight,
		Kinect.JointType.HandRight

	};

    public Text GestureTextGameObject;
    public Text ConfidenceTextGameObject;
	public Text GestureText;
	public Text JointText;
	public Text spinbase;

	public Text leftshoulder;
   // private Turning turnScript; 
	public GameObject lefthand;

	public GameObject canvas;
	float canvash,canvasw;
    // Kinect 
	private Kinect.KinectSensor kinectSensor;
    // color frame and data 
	private Kinect.ColorFrameReader colorFrameReader;
    private byte[] colorData;
    private Texture2D colorTexture;
	private Kinect.BodyFrameReader bodyFrameReader;
    private int bodyCount;
	private int thisbody;
	private Kinect.Body[] bodies;

	/*

    private string leanLeftGestureName = "kinect_turn_Left";
	private string leanRightGestureName = "kinect_turn_Right";
	private string handfeetLeftGestureName = "hand_feet_Left";
	private string handfeetRightGestureName = "hand_feet_Right";
	private string roundLeftGestureName = "round_Left";

	private string roundRightGestureName = "round_Right";
	private string jumpturnprogressleftGestureName = "jump_turn_Left";
*/
    // GUI output
    private UnityEngine.Color[] bodyColors;
    //private string[] bodyText;

    /// <summary> List of gesture detectors, there will be one detector created for each potential body (max of 6) </summary>
    private List<GestureDetector> gestureDetectorList = null;




	private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();

	public static Material lineMat;
	public static Mesh cylinderMesh;
	public static float radius;
	public static float sensitive;

	public Material BoneMaterial;
	public GameObject BodySourceManager;
	public static float body_scale;
	public static float bone_scale;
	public static float joint_scale;

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
	public GameObject[] skeletonlist;
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

    // Use this for initialization
    void Start()
    {
		AddTag ("realtime01");
		AddTag ("Body_Replay");

		lineMat =  new Material(Shader.Find("Diffuse"));
		lineMat.color = Color.yellow;
		GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Capsule);
		cylinderMesh = temp.GetComponent<MeshFilter> ().sharedMesh;
		GameObject.Destroy (temp);
		radius = 0.03f;
		sensitive = 1.0f;
		body_scale = 1.0f;
		bone_scale = 0.02f;
		joint_scale = 0.04f;

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
		
		canvash=((RectTransform)canvas.transform).rect.height/2.0f;
		canvasw=((RectTransform)canvas.transform).rect.width/2.0f;
		lefthand.SetActive (false);
		realtime_relation = GameObject.Find ("realtime_relation");
        //turnScript = Player.GetComponent<Turning>();
        // get the sensor object
        
		this.kinectSensor = Kinect.KinectSensor.GetDefault();

        if (this.kinectSensor != null)
        {
            this.bodyCount = this.kinectSensor.BodyFrameSource.BodyCount;

            // color reader
            this.colorFrameReader = this.kinectSensor.ColorFrameSource.OpenReader();

            // create buffer from RGBA frame description
            var desc = this.kinectSensor.ColorFrameSource.CreateFrameDescription(Kinect.ColorImageFormat.Rgba);


            // body data
            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

            // body frame to use
            this.bodies = new Kinect.Body[this.bodyCount];

            // initialize the gesture detection objects for our gestures
            this.gestureDetectorList = new List<GestureDetector>();


			//real_time_skeleton_init ();
            for (int bodyIndex = 0; bodyIndex < this.bodyCount; bodyIndex++)
            {
                //PUT UPDATED UI STUFF HERE FOR NO GESTURE
                //GestureTextGameObject.text = "none";
                //this.bodyText[bodyIndex] = "none";
                this.gestureDetectorList.Add(new GestureDetector(this.kinectSensor));
            }

            // start getting data from runtime
            this.kinectSensor.Open();
        }
        else
        {
            //kinect sensor not connected
        }
    }

    // Update is called once per frame
    void Update()
    {
        
            // process bodies
        bool newBodyData = false;

		using (Kinect.BodyFrame bodyFrame = this.bodyFrameReader.AcquireLatestFrame ()) {
			if (bodyFrame != null) {
				bodyFrame.GetAndRefreshBodyData (this.bodies);
				newBodyData = true;
			} 
		}


		if (newBodyData) {
			
			List<ulong> trackedIds = new List<ulong>();
			foreach(var body in bodies)
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
			// update gesture detectors with the correct tracking id
			for (int bodyIndex = 0; bodyIndex < this.bodyCount; bodyIndex++) {
				var body = this.bodies [bodyIndex];
					
				if (body != null) {
					var trackingId = body.TrackingId;
					if (body.IsTracked == true) {
						thisbody = bodyIndex;
						handasmouse (body);

						if (!_Bodies.ContainsKey (body.TrackingId)) {
							Debug.Log ("Create");
							_Bodies [body.TrackingId] = CreateBodyObject (body.TrackingId);
						}
						Debug.Log ("RE");
						RefreshBodyObject (body, _Bodies [body.TrackingId]);

						//	real_time_real_time_skeleton_detect (body);
					}
					// if the current body TrackingId changed, update the corresponding gesture detector with the new value
					/*
					if (trackingId != this.gestureDetectorList [bodyIndex].TrackingId) {
						//GestureTextGameObject.text = "none";

						//this.bodyText[bodyIndex] = "none";
						this.gestureDetectorList [bodyIndex].TrackingId = trackingId;

						// if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
						// if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
						this.gestureDetectorList [bodyIndex].IsPaused = (trackingId == 0);
						this.gestureDetectorList [bodyIndex].OnGestureDetected += CreateOnGestureHandler (bodyIndex);
					}*/
				} 
			}
		}
    }
	private void handasmouse(Kinect.Body body){

		var jointlh = body.Joints [Kinect.JointType.HandRight].Position;
		var jointls = body.Joints [Kinect.JointType.ShoulderRight].Position;
		var jointsb = body.Joints [Kinect.JointType.SpineBase].Position;
		float hinfloat = (float)(Math.Round(jointls.Y - jointsb.Y,2))*0.8f;
		float winfloat = hinfloat*1.4f;

		if (jointlh.Z < jointsb.Z-0.2) {

			jointlh.X = (jointlh.X - jointls.X )/ winfloat * 1.3f * canvasw;
			jointlh.Y = (jointlh.Y - jointls.Y) / hinfloat*2.0f * canvash;
			lefthand.SetActive (true);
			float lhw = ((RectTransform)lefthand.transform).rect.width/2.0f;
			float lhh = ((RectTransform)lefthand.transform).rect.height/2.0f;

			if (jointlh.X-lhw < -canvasw)
				jointlh.X = -canvasw+lhw/2.0f;
			if (jointlh.X+lhw > canvasw)
				jointlh.X = canvasw-lhw/2.0f;
			if (jointlh.Y +lhh> canvash)
				jointlh.Y = canvash-lhh/2.0f;
			if (jointlh.Y -lhh< -canvash)
				jointlh.Y = -canvash+lhh/2.0f;

			lefthand.transform.localPosition = new Vector2 ((float)Math.Round( jointlh.X ,1), (float)Math.Round( jointlh.Y ,1) );
		} else {
			lefthand.SetActive (false);
		}


	}


	private GameObject CreateBodyObject(ulong id)
	{
		GameObject body = new GameObject("realtime01");
		body.tag = "realtime01";

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
			if (jt == Kinect.JointType.HandTipLeft || jt == Kinect.JointType.HandTipRight || jt == Kinect.JointType.ThumbLeft || jt == Kinect.JointType.ThumbRight ||
			    jt == Kinect.JointType.FootLeft || jt == Kinect.JointType.FootRight) {
				continue;
			}
			Kinect.Joint sourceJoint = body.Joints[jt];
			Kinect.Joint? targetJoint = null;

			if(_BoneMap.ContainsKey(jt))
				targetJoint = body.Joints[_BoneMap[jt]];

			Material zzz_obj = bodyObject.transform.Find (jt.ToString ()).gameObject.GetComponent<Renderer> ().material;
			//zzz_obj.color = MyColorPicker.ShowColor;
			zzz_obj.color = Color.red;

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
				 
				if (cylinderDistance >= sensitive && hoverbutton.compareline) point1.GetChild (0).gameObject.GetComponent<MeshRenderer>().enabled = true;
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

	private Vector3 GetVector3FromJoint(Kinect.Joint joint, float scale) 
	{
		x = (joint.Position.X * scale + realtime_relation.transform.position.x);
		y = (joint.Position.Y* scale + realtime_relation.transform.position.y) ;
		z = (joint.Position.Z* scale + realtime_relation.transform.position.z) -0.5f;
		Vector3 tmp = new Vector3 (x, y, z);
		//tmp += maincamera.transform.position;
		return tmp;
	}

	public void AddTag(string tagname)
	{
		UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
		if ((asset != null) && (asset.Length > 0))
		{
			SerializedObject so = new SerializedObject(asset[0]);
			SerializedProperty tags = so.FindProperty("tags");

			for (int i = 0; i < tags.arraySize; ++i)
			{
				if (tags.GetArrayElementAtIndex(i).stringValue == tag)
				{
					return;     // Tag already present, nothing to do.
				}
			}

			tags.InsertArrayElementAtIndex(0);
			tags.GetArrayElementAtIndex(0).stringValue = tagname;
			so.ApplyModifiedProperties();
			so.Update();
		}
	}

}
