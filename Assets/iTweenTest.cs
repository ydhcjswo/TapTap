using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iTweenTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		iTween.ScaleTo(gameObject, Vector3.one * 3f, 0f);
		iTween.FadeTo(gameObject, 0f, 5f);

		//iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.one * 3f, "time", 5f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
