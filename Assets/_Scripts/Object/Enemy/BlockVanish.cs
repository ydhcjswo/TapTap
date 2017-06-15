using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockVanish : MonoBehaviour
{
	public ParticleSystem VanishEffect;

	public enum BlockType { None, Player, Mosnter }

	public BlockType type = BlockType.None;
	//float CurrTime = 0f;
	//BottomBar Bar = null;
	//Vector3 OriginScale= Vector3.zero;

	//private void Awake()
	//{
	//	this.enabled = false;
	//	Bar = GameObject.Find("BottomBar").GetComponent<BottomBar>();
	//	OriginScale = Bar.transform.localScale;
	
	//}

	//// Update is called once per frame
	//void Update()
	//{

	//	CurrTime += Time.deltaTime;


	//	this.transform.localScale = Vector3.Lerp(OriginScale, Vector3.zero, CurrTime / 10f);
	//	if(this.transform.localScale.x<1f)
	//	{
	//		//this.gameObject.SetActive(false);
	//		this.enabled = false;
	//	}
	//	if (CurrTime > 1.0f)
	//	{
	//		OriginScale= Vector3.zero;
	//		CurrTime = 0;
	//	}
	//}

	public void OnTouchBlock()
	{
		//iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.zero, "time", 1.5f, "oncomplete", "OnComplete"));		
		StartCoroutine(IEScaleTo(Vector3.zero, 0.8f));

		

	}

	IEnumerator IEScaleTo(Vector3 scale, float time)
	{
		float delta = 0;
		bool EffectStarted = false;
		Vector3 originalScale = transform.localScale;
		
		while(delta <= time)
		{
			delta += Time.deltaTime;
			transform.localScale -= (originalScale - scale) * delta / time;
			yield return new WaitForEndOfFrame();

			if(transform.localScale.x <= Vector3.one.x * 0.1f && !EffectStarted)
			{
				EffectStarted = true;

				if (VanishEffect != null)
				{
					ParticleSystem particle = Instantiate(VanishEffect);
					particle.transform.SetParent(transform.parent, false);
					particle.transform.position = transform.position; //new Vector3(transform.position.x, transform.position.y, particle.transform.position.z);
					particle.Play();

					Destroy(particle.gameObject, particle.main.duration);//VanishEffect.startLifetime);
				}
			}
		}

		OnComplete();
	}

	public void OnComplete()
	{
		//print("OnComplete : " + gameObject.name);
		

		ObjectPool.Instance.Release(gameObject);
	}


}
