using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour
{
	public Transform Target;




	private void Update()
	{
		Camera.main.transform.RotateAround(Target.position, Vector3.up, 20*Time.deltaTime);

	}



}
