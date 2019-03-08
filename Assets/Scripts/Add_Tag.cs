using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Kinect = Windows.Kinect;
using UnityEngine.UI;
using UnityEditor;

public class Add_Tag : MonoBehaviour {

	// Use this for initialization
	void Start () {
        /*add_tag("score");
        this.gameObject.transform.Find("score").Find("Score").gameObject.tag = "score";*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void add_tag(string tagname)
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
