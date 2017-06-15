using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlockMove : MonoBehaviour
{


	private float moveSpeed =20f;
	//public float lifeTime = 3f;

	Player Player= null;
	Enemy Enemy = null;
	BottomBar BottomBar = null;

	public float EnemyBlockMoveSpeed
	{
		get { return moveSpeed; }
		set { moveSpeed = value; }
	}

	private void Start()
	{
		Player = GameObject.Find("Player").GetComponent<Player>();
		Enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
		//BottomBar = GameObject.Find("Bar").GetComponent<BottomBar>();
		//EnemyBlockMoveSpeed = BottomBar.OriginBlockSpeed;

	}

	void Update()
	{
		transform.position += Vector3.left * moveSpeed * Time.deltaTime;

		if (transform.localPosition.x <= -20)
		{
			Player.GetDamage(Enemy.Damage);
			ObjectPool.Instance.Release(gameObject);
		}
	}

}

