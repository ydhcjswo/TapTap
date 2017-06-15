using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
	public ParticleSystem afterEffect;

	private void Update()
	{
		MoveFireBall();
	}

	void MoveFireBall()
	{
		this.transform.Translate(0.2f,0,0);
		//transform.position = new Vector3(transform.localPosition.x + 0.01f, transform.localPosition.y, transform.localPosition.z);
	}

	private void OnTriggerEnter(Collider other)
	{
		//if (other.gameObject.name.Contains("Enemy"))
		//{
		//	Destroy(transform.gameObject);
		//}

		if(other.gameObject.name.Equals( "Enemy"))
		{
			StartCoroutine(AfterEffect());
			Destroy(this.transform.gameObject);
		
		}
	}
	IEnumerator AfterEffect()
	{
		ParticleSystem particle = Instantiate(afterEffect);
		particle.transform.SetParent(transform.parent, false);
		particle.transform.position = transform.position;
		particle.Play();
		Destroy(particle.gameObject, particle.main.duration);
		yield return null;
	}
}
