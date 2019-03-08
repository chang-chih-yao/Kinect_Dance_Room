using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ScrollbarControl : MonoBehaviour {
	private Scrollbar scrollbar;
	void Start(){
		scrollbar = GetComponent<Scrollbar> ();
	}
	public void ListScroll(RectTransform list){
		list.localPosition = new Vector3 (list.localPosition.x, scrollbar.value * 380.0f, list.localPosition.z);////(1)
	}
}