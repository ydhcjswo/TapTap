using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
	public enum State { Idle,Attack,Skiil,Damaged,Dead};
	public float startingHealth;
	protected float health;
	protected bool dead;

	public event System.Action OnDeath;

	protected virtual void Start()
	{
		health = startingHealth;
	}
	//public void TakeHit(float damage, RaycastHit hit)
	//{
	//	// 나중에 
	//	TakeDamage(damage);
	//}
	public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)       // RayCastHit엔 불필요한 정보가많아 바꿈
	{
		// 나중에 
		TakeDamage(damage);
	}

	public virtual void TakeDamage(float damage)
	{
		health -= damage;

		if (health <= 0 && !dead)
		{
			Die();
		}
	}

	[ContextMenu("Self Destruct")]
	protected void Die()
	{
		dead = true;
		if (OnDeath != null)
		{
			OnDeath(); // 온데스 구현부 어디?
		}
		GameObject.Destroy(gameObject);
	}
}
