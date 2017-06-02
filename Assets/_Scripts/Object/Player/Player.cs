using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : LivingEntity
{
    public float moveSpeed = 5;


	Slider PlayerHpBar = null;
	float  PlayerHpState = 0;

	Enemy EnemyScript = null;

	int PlayerLv = 1;
    int PlayerExp = 0;
    int PlayerExpMax = 100;

	float CurrHP = 0;
	public float PlayerMaxHp = 100;

    float AttackMin = 1f;
    float AttackMax = 3f;

    int Monney = 0;

    bool IsAttack = false;

    Material skinMaterial;
    Color originalColor;

	public float HP           // getter setter 는 잘사용하면 디버그 포인트를 잡기 훨씬 쉬워진다.
	{
		get
		{
			return CurrHP;
		}
		set
		{
			CurrHP += value;
			if (CurrHP > PlayerMaxHp)
				CurrHP = PlayerMaxHp;
		}
	}


	private void Start()
    {
		//base.Start();
		//Sword = transform.FindChild("Sword");
		//Sword.parent = this.transform;
		//Sword.localPosition = this.transform.position;

		HP = PlayerMaxHp;

		EnemyScript =  GameObject.Find("Enemy").GetComponent<Enemy>();
		PlayerHpBar = GameObject.Find("PlayerHP").GetComponent<Slider>();
	}
    private void Update()
    {
		
        float translation = Input.GetAxisRaw("Horizontal") * moveSpeed;
        translation *= Time.deltaTime;
        transform.Translate(translation, 0, 0);
		//Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
		//Vector3 moveVelocity = moveInput.normalized * moveSpeed;

		//PlayerHpState= HP / 100;
	
    }

	public bool Attack()
	{
		if(EnemyScript.GetDamage(Random.Range(AttackMin,AttackMax))==false)
		{
			Invoke("!!,", 2f);
		}
		return true;
	}

	public bool GetDamage(float _damage)
	{
		HP = -(_damage);
		// HP-=_damage 일경우 CurrHP=value; 로 해줘야한다.
		Debug.Log(HP);
		PlayerHpBar.value = HP / 100;
		if (HP <= 0) return false;

		return true;
	}


	//IEnumerator Attack()
	//{

	//    Vector3 originalPosition = transform.position;
	//    //Vector3 dirToTarget = (target.position - transform.position).normalized;
	//    //Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
	//    Vector3 attackPosition = transform.position + Vector3.right;

	//    float attackSpeed = 3f;
	//    float percent = 0f;

	//    skinMaterial.color = Color.red;     // 원하는 RGB값은 어떻게 사용하지??

	//    bool hasAppliedDamage = false;

	//    while (percent <= 1)
	//    {
	//        if (percent >= .5f && !hasAppliedDamage)
	//        {
	//            hasAppliedDamage = true;
	//            //targxetEntity.TakeDamage(damage);
	//        }

	//        percent += Time.deltaTime * attackSpeed;
	//        float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;                       // 보간값 : 알려진 점들의 위치를 참조하여, 집합의 일정 범위의 점들을 새롭게 그리는 방법. 여기서는 원지점 -> 공격지점으로 이동할 때 참조할 위 그래프의 대칭 곡선을 만드는 참조점을 의미합니다.
	//        transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);     // Lerp 메소드는 두벡터 사이에 비례값(0~1) 으로 내분점 지점을 반환, 보간값(interpolation) 이 0일때 원지점 originalPosition, 1일때 attackPosition 그리고 다시 0일떄 originalPosition
	//        IsAttack = false;
	//        yield return null;                                                                  // 이 지점에서 작업이 뭄추고 , Update 메소드의 작업이 완전 수행된 후 다음 프레임으로 넘어갔을때 yield 키워드 아래에 있는 코드나 다음번 루프가 실행된다.
	//    }
	//    skinMaterial.color = originalColor;
	//}
}
