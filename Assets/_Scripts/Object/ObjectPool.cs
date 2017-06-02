using UnityEngine;
using System.Collections;

public class ObjectPool : MonoBehaviour
{

	private static ObjectPool instance = null;

	public static ObjectPool Instance
	{
		get { return instance; }
	}
	
	const int SIZE = 10;
	public GameObject[] Blocks = null;    //Missile 원본
	GameObject[,] BlockPool = new GameObject[2, SIZE];
	public Transform EnemyBlockSpawnPosition;

	private void Awake()
	{
		instance = this;

		for (int i = 0; i < Blocks.Length; i++)
		{
			for (int j = 0; j < SIZE; j++)
			{

				BlockPool[i, j] = Instantiate(Blocks[i]);
				BlockPool[i, j].transform.SetParent(transform, false);
				BlockPool[i, j].SetActive(false);

				//else if(i==1)
				//{
				//	BlockPool[i, j] = Instantiate (Blocks [i],EnemyBlockSpawnPosition.position,EnemyBlockSpawnPosition.rotation);
				//	BlockPool[i, j].transform.SetParent(transform, false);
				//	BlockPool[i, j].SetActive(false);
				//}
			}
		}
	}
	// Use this for initialization

	void Start()
	{


	}

	public GameObject GetBlock(BlockType _blockType)
	{
		for (int i = 0; i < SIZE; i++)
		{
			GameObject obj = BlockPool[(int)_blockType, i];

			if (obj.activeSelf == true)
				continue;

			obj.SetActive(true);
			return obj;
		}
		return null;
	}
	 
	public void Release(GameObject obj)
	{
		obj.SetActive(false);
		obj.transform.position = EnemyBlockSpawnPosition.position ;
		float randomScale = Random.Range(5f, 7f);
		obj.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
	}

	public void ReleaseByType(BlockType _blockType)
	{
		for (int j = 0; j < SIZE; j++)
		{
			Release(BlockPool[(int)_blockType, j]);
		}
	}
}