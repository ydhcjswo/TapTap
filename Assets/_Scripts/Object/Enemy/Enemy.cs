using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

	AnimationController AniCtrl = null;
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
	protected void Start ()
	{
		//base.Start();

		AniCtrl = this.GetComponentInChildren<AnimationController>();
		AniCtrl.Set(EndAnimtion);

		SetAnimtion(AniState.Idle);

		EnemyHpBar = GameObject.Find("EnemyHP").GetComponent<Slider>();
		HP = EnemyMaxHP;
	}
	public bool Attack()
	{
		SetAnimtion(AniState.Attack);



		//if (EnemyScript.GetDamage(Random.Range(AttackMin, AttackMax)) == false)
		//{
		//	Invoke("!!,", 2f);
		//}
		return true;
	}

	public bool GetDamage(float _damage)
	{
		SetAnimtion(AniState.Damage);

		HP = -(_damage);
		// HP-=_damage 일경우 CurrHP=value; 로 해줘야한다.
		EnemyHpBar.value = HP / 100;

		//UIManager.Instance.CreateDamageText(((int)_damage).ToString(), 1f, transform.position);
		Debug.Log(HP);
		if (HP <= 0) return false;

		return true;
	}
	void EndAnimtion()
	{
		switch (AniCtrl.State)
		{
			case AniState.Attack:
				SetAnimtion(AniState.Idle);
				break;
			case AniState.Damage:
				SetAnimtion(AniState.Idle);
				break;
		}
	}
	public void SetAnimtion(AniState state)
	{
		print("Enemy Set Animation : " + state);
		AniCtrl.Play(state);

		//if (CurState == state)
		//	return;

		//CurState = state;
		//Anim.Play(state.ToString());
	}


}
