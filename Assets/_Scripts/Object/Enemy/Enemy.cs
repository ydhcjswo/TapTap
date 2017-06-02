using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
	float  CurrHP = 0;
	public float EnemyMaxHP = 100;

	public float EnemyDamage = 10;
	Slider EnemyHpBar = null;
	
	public float Damage
	{
		get { return EnemyDamage; }
	}
	public float HP           // getter setter 는 잘사용하면 디버그 포인트를 잡기 훨씬 쉬워진다.
	{
		get
		{
			return CurrHP;
		}
		set
		{
			CurrHP += value;
			if (CurrHP > EnemyMaxHP)
				CurrHP = EnemyMaxHP;
		}
	}

	// Use this for initialization
	void Start ()
	{
		HP = EnemyMaxHP;
		EnemyHpBar = GameObject.Find("EnemyHP").GetComponent<Slider>();

	}
	public bool GetDamage(float _damage)
	{
		HP = -(_damage);
		// HP-=_damage 일경우 CurrHP=value; 로 해줘야한다.
		EnemyHpBar.value = HP / 100;
		Debug.Log(HP);
		if (HP <= 0) return false;

		return true;
	}

}
