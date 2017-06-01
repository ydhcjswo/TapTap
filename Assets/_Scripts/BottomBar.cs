using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockState
{
	None,
	PlayerAttack,
	EnemyAttack,

}
public class BottomBar : MonoBehaviour
{
	public AnimationCurve Curve;



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
	int ComboCount = 1;

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
	Transform Slider = null;

	Player PlayerScript = null;
	Enemy EnemyScript = null;

	EnemyBlockMove EnemyBlock = null;

	BlockVanish _BlockVanish = null;
	

	// Use this for initialization
	void Start()
	{
		SliderSpeed = OriginSliderSpeed;
		BlockSpeed = OriginBlockSpeed;
		BlockSpawnTimeMin = OriginBlockSpawnTimeMin;
		BlockSpawnTimeMax = OriginBlockSpawnTimeMax;

		Slider = transform.FindChild("Slider");
		Slider.parent = this.transform;
		Slider.localPosition = this.transform.position;

		PlayerScript = GameObject.Find("Player").GetComponent<Player>();
		EnemyScript = GameObject.Find("Enemy").GetComponent<Enemy>();

		//EnemyAttack = GameObject.Find("EnemyBlock");
		StartCoroutine("BlockSpawn");

		//EnemyBlockSpeed =GameObject.Find("EnemyBlockMove").GetComponent;
	}
	Vector3 vtemp;
	float temp = 0;
	// Update is called once per frame
	void Update()
	{
		MoveSlider();
		CheckMouseClick();

		if (tempHit != null)
		{
			temp += Time.deltaTime;
			
			tempHit.transform.localScale = Vector3.Lerp(vtemp, Vector3.zero, temp / 1f);
			if (temp > 1.0f)
			{
				tempHit = null;
				temp = 0;
			}
		}

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

			if (hit.collider != null && hit.collider.tag.Contains("Attack"))
			{
				if (hit.collider.tag.Contains("Player"))
				{
					ComboCount++;
					ObjectPool.Instance.Release(hit.collider.gameObject);
					PlayerScript.Attack();

					

					//Destroy(hit.collider.gameObject);
					//PlayerAttack();
				}
				else if (hit.collider.tag.Contains("Enemy"))
				{
					ComboCount++;
					ObjectPool.Instance.Release(hit.collider.gameObject);
					//Destroy(hit.collider.gameObject);
					//EnemyAttackDefense();
				}
				//vtemp = hit.transform.localScale;
				//tempHit = hit.collider.gameObject;
				ComboCal();
			}
			else
			{
				print("Miss!!");
				InitializeValues();

				PlayerScript.GetDamage(EnemyScript.Damage);
			}
			//CurrTime += Time.deltaTime;
			//TotalSpeed = ComboCount + Curve.Evaluate(CurrTime);
			
		}
	}



	
	IEnumerator BlcokVanish(RaycastHit hit)
	{
		
		//while (hit.transform.localScale.x >= 1)
		//{
		//	//hit.transform.localScale = Vector3.Lerp(vtemp, Vector3.zero, temp/10f);
		//	print(hit.transform.localScale);
		//}

		//ObjectPool.Instance.Release(hit.collider.gameObject);

		yield return null;
	}


	void ComboCal()
	{
			SliderSpeed += 1 + (ComboCount / 10);
			BlockSpeed += 1 + (ComboCount / 10);
			BlockSpawnTimeMin -= ComboCount / 100;
			BlockSpawnTimeMax -= ComboCount / 100;
	}

	void InitializeValues()
	{
		SliderSpeed = OriginSliderSpeed;
		BlockSpeed = OriginBlockSpeed;
		BlockSpawnTimeMin = OriginBlockSpawnTimeMin;
		BlockSpawnTimeMax = OriginBlockSpawnTimeMax;
		ComboCount = 0;
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

}

