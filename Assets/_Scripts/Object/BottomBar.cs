﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BlockState
{
	None,
	PlayerAttack,
	EnemyAttack,

}
public class BottomBar : MonoBehaviour
{
	public AnimationCurve Curve;

	bool IsGameOver = false;

	private const float BAR_SIZE = 40f;
	private const float ERROR_MARGIN = 0.5f;
	private const float SLIDER_START = -20f;
	private const float SLIDER_END = 20f;

	private bool IsDone = false;

	private Vector3 SliderStart = new Vector3(-20, 0, -1f);
	private Vector3 SliderEnd = new Vector3(20, 0, -1f);

	float SliderTransition = 0f;
	float BlockTransition = 0;

	float CurrTime = 0f;
	int ComboCount = 0;
	int MaxCombo = 0;

	int SkillStackCount = 0;

	public float OriginBlockSpeed = 4f;
	private float BlockSpeed = 0f;

	public float OriginSliderSpeed = 5f;
	private float SliderSpeed = 0f;

	public float OriginBlockSpawnTimeMin = 0.5f;
	private float BlockSpawnTimeMin = 0f;
	public float OriginBlockSpawnTimeMax = 1.0f;
	private float BlockSpawnTimeMax = 0f;

	public RaycastHit hit;

	float TotalSpeed = 0f;

	bool IsPlayer = true;

	GameObject PlayerAttackBlock = null;
	GameObject EnemyAttackBlock = null;
	GameObject tempHit = null;
	GameObject ComboText = null;

	Transform Slider = null;

	Player PlayerScript = null;
	Enemy EnemyScript = null;

	EnemyBlockMove EnemyBlock = null;

	public GameObject EnemyBlockPrefab = null;

	BlockVanish blockVanish = null;

	// Use this for initialization
	void Start()
	{
		//EnemyAttackBlock = Resources.Load("Prefabs/Enemy") as GameObject;
		//_BlockVanish = EnemyAttackBlock.GetComponent<BlockVanish>();

		SliderSpeed = OriginSliderSpeed;
		BlockSpeed = OriginBlockSpeed;
		BlockSpawnTimeMin = OriginBlockSpawnTimeMin;
		BlockSpawnTimeMax = OriginBlockSpawnTimeMax;

		Slider = transform.FindChild("Slider");
		//Slider.parent = this.transform;
		//Slider.localPosition = this.transform.position;

		PlayerScript = GameObject.Find("Player").GetComponent<Player>();
		EnemyScript = GameObject.Find("Enemy").GetComponent<Enemy>();

		// 프리팸에서 GetComponenet X;
		// blockVanish = EnemyBlockPrefab.GetComponent<BlockVanish>();

		//EnemyAttack = GameObject.Find("EnemyBlock");
		StartCoroutine("BlockSpawn");

		//EnemyBlockSpeed =GameObject.Find("EnemyBlockMove").GetComponent;
	}
	//Vector3 vtemp;
	//float temp = 0;

	void Update()
	{
		if (IsGameOver == true)
			return;

		MoveSlider();
		CheckMouseClick();

		//if (tempHit != null)
		//{
		//	temp += Time.deltaTime;
			
		//	tempHit.transform.localScale = Vector3.Lerp(vtemp, Vector3.zero, temp / 1f);
		//	if (temp > 1.0f)
		//	{
		//		tempHit = null;
		//		temp = 0;
		//	}
		//}

	}
	void MoveSlider()
	{
		SliderTransition += Time.deltaTime * SliderSpeed;
		float movePosition = Mathf.PingPong(SliderTransition, BAR_SIZE) - BAR_SIZE / 2;

		Slider.localPosition = new Vector3(movePosition, 0, -0.1f);
	}


	void CheckMouseClick()
	{
		if (Input.GetMouseButtonDown(0))
		{
			//RaycastHit hit;
			//hit = Physics.Raycast(Slider, Vector3.forward);
			Ray ray = new Ray(Slider.position, Vector3.forward);
			//RaycastHit hit;
			Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Block"));

			if (hit.collider != null)
			{
				BlockVanish block = hit.collider.GetComponent<BlockVanish>();

				if(block != null)
				{
					if (block.type == BlockVanish.BlockType.Player)  //플레이어 공격
					{
						block.OnTouchBlock();
						PlayerScript.Attack();

						// blockVanish.enabled = true;
					}
					else if(block.type == BlockVanish.BlockType.Mosnter)	//몬스터
					{
						//ObjectPool.Instance.Release(hit.collider.gameObject);
						block.OnTouchBlock();
						EnemyScript.Attack();
						PlayerScript.Defend();
						// blockVanish.enabled = true;
					}

					ComboCheck();
				}
				else
				{
					if(SkillStackCount>0)
					{
						PlayerScript.Skill(SkillStackCount);
						UIManager.Instance.SetScore(0);
						EnemyScript.SetAnimtion(AniState.Damage);
						InitializeValues();
					}
					else
					{ 
						print("Miss!!");
						InitializeValues();
						UIManager.Instance.SetScore(0);
						PlayerScript.GetDamage(EnemyScript.Damage);
						EnemyScript.Attack(); 
					}
				}
			}
			//else
			//{
			//	print("Miss!!");
			//	InitializeValues();

			//	PlayerScript.GetDamage(EnemyScript.Damage);
			//}


			//if (hit.collider != null && hit.collider.tag.Contains("Attack"))
			//{
			//	if (hit.collider.CompareTag("PlayerAttack"))
			//	{
			//		ComboCount++;
			//		BlockVanish block = hit.collider.gameObject.GetComponent<BlockVanish>();

			//		if(block != null)
			//		{
			//			//ObjectPool.Instance.Release(hit.collider.gameObject);
			//			block.OnTouchBlock();
			//			PlayerScript.Attack();
			//			blockVanish.enabled = true;
			//		}



			//		//Destroy(hit.collider.gameObject);
			//		//PlayerAttack();
			//	}
			//	else if (hit.collider.tag.Contains("Enemy"))
			//	{
			//		ComboCount++;
			//		ObjectPool.Instance.Release(hit.collider.gameObject);
			//		blockVanish.enabled = true;

			//		//Destroy(hit.collider.gameObject);
			//		//EnemyAttackDefense();
			//	}
			//	//vtemp = hit.transform.localScale;
			//	//tempHit = hit.collider.gameObject;
			//	ComboCal();
			//}
			//else
			//{
			//	print("Miss!!");
			//	InitializeValues();

			//	PlayerScript.GetDamage(EnemyScript.Damage);
			//}




			////CurrTime += Time.deltaTime;
			////TotalSpeed = ComboCount + Curve.Evaluate(CurrTime);

		}
		if(PlayerScript.HP<=0 || EnemyScript.HP<=0)
		{
			IsGameOver = true;
			UIManager.Instance.SetMaxScore(MaxCombo);
			Debug.Log("GameOver");
		}
	}



	void ComboCheck()
	{
		ComboCount++;
		if (MaxCombo < ComboCount)
			MaxCombo = ComboCount;

		ComboCalcurate();

		UIManager.Instance.SetScore(ComboCount);
		if ((ComboCount % 10) == 0)
		{
			SkillStackCount++;

		}
	}


	void ComboCalcurate()
	{
			SliderSpeed += 1 + (ComboCount / 10);
			BlockSpeed += 1 + (ComboCount / 10);
			BlockSpawnTimeMin -= ComboCount / 100;
			BlockSpawnTimeMax -= ComboCount / 100;
			
	}

	void InitializeValues()
	{
		ComboCount = 0;
		SkillStackCount = 0;
		SliderSpeed = OriginSliderSpeed;
		BlockSpeed = OriginBlockSpeed;
		BlockSpawnTimeMin = OriginBlockSpawnTimeMin;
		BlockSpawnTimeMax = OriginBlockSpawnTimeMax;
	}

	IEnumerator BlockSpawn() // 블록 랜덤 생성
	{
		while (true)
		{
			float refreshRate = Random.Range(BlockSpawnTimeMin, BlockSpawnTimeMax);        // 블록 생성 주기

			int blockRatio = Random.Range(1, 5);    // 플레이어 적 생성 비율
													//int BLCOKRATIO =(int)Mathf.Round(BlockRatio);
													//IsPlayer = (BLCOKRATIO == 1);
			if (blockRatio > 2) IsPlayer = true;
			else IsPlayer = false;

			if (IsPlayer)
			{
				float randomPos = Random.Range(-19, 19);
				float randomScale = Random.Range(5f, 7f);

				PlayerAttackBlock = ObjectPool.Instance.GetBlock(BlockType.PLAYER_ATTACK);
				PlayerAttackBlock.transform.localPosition = new Vector3(randomPos, 0, -0.01f);
				PlayerAttackBlock.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
			}
			if (!IsPlayer)
			{
				float randomScale = Random.Range(5f, 7f);
				EnemyAttackBlock = ObjectPool.Instance.GetBlock(BlockType.ENEMY_ATTACK);
				EnemyAttackBlock.transform.localPosition = new Vector3(19, 0, -0.02f);
				EnemyAttackBlock.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
			}
			yield return new WaitForSeconds(refreshRate);
		}
	}
	public void Restart()
	{
		int childCount = this.transform.childCount;

		for (int i = 0; i < childCount; i++)
		{
			Destroy(this.transform.GetChild(i).gameObject);     //	getchild 는 transform 을 반환하기 때문에 destriy가 바로 안된다.
		}

		IsGameOver = false;

		ComboCount = 0;

	}

}

