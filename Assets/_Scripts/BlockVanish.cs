using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockVanish : MonoBehaviour
{

	float CurrTime = 0f;
	BottomBar Bar = null;
	Transform OriginScale = null;

	private void Awake()
	{
		Bar = GameObject.Find("BottomBar").GetComponent<BottomBar>();
		OriginScale.localScale = Bar.hit.transform.localScale;
		this.gameObject.SetActive(false);

	}


	


	// Update is called once per frame
	void Update()
	{

		CurrTime += Time.deltaTime;


		Bar.hit.transform.localScale = Vector3.Lerp(OriginScale.localScale, Vector3.zero, CurrTime / 1f);
		if (CurrTime > 1.0f)
		{
			OriginScale.localScale = Vector3.zero;
			CurrTime = 0;
		}

	}
}
