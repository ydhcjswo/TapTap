using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Singleton
    static GameManager m_Instance;

    public static GameManager Instance
    {
        get
        {
            return m_Instance;
        }
    }
    //-------------------------------


    //private const zone

    private const int ROW_MAX = 10;
    public int RowMax { get { return ROW_MAX; } }
    private const int COLUMN_MAX = 6;
    public int ColumnMax { get { return COLUMN_MAX; } }
    private const float SPRITE_SIZE = 0.64f;
    private const float BOTTOM_WIDTH = 3.84f;
    private const float WALL_HEIGHT = 7.0f;
    private const float DESTROY_DELAY_TIME = 0.4f;      //죽으면 남아있는 시간

    //--------------------------------

    public GameObject m_OriginBlock = null;

    public ArrayList[] m_Blocks = new ArrayList[COLUMN_MAX];
    public GameObject m_TouchedBlock = null;
    public bool m_IsMoving = false;

   



    private void Awake()
    {
        m_Instance = this;
    }

    // Use this for initialization
    void Start()
    {

        //각 Column 생성
        for (int i = 0; i < COLUMN_MAX; i++)
        {
            m_Blocks[i] = new ArrayList();
        }

        for (int x = 0; x < COLUMN_MAX; x++)
        {
            for(int y = 0; y < ROW_MAX; y++)
            {
                m_Blocks[x].Add(CreateRandomBlock(x, new Vector2(SPRITE_SIZE * (x - (COLUMN_MAX - 1) / 2f), WALL_HEIGHT + y * SPRITE_SIZE)));
            }
        }
        

    }

    void Update()
    {
        //if (Input.GetMouseButton(0))
        //{

        //    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        //    if (hit.collider != null)
        //    {
        //        m_TouchedBlock = hit.collider.gameObject;   //터치한 오브젝트 저장
        //        //Debug.Log(m_TouchedBlock.GetComponent<Rigidbody2D>().velocity);
                
        //        //블락일때만 작용
        //        if (hit.collider.gameObject.name.Contains("Block"))
        //        {
        //            //Test Code;
        //            //DeleteAndRebornBlock(hit.collider.gameObject);
        //        }
        //    }
        //}
        //else if (m_TouchedBlock != null) // 마우스를 뗐을 때 터치한 오브젝트가 있다면
        //{

        //}



        ////Test Code 
        //if (Input.GetMouseButtonDown(0))
        //{
        //    DeleteAndRebornBlock(transform.GetChild(0).gameObject);
        //}




    }

    void CheckBlocks()
    {

    }

    public GameObject CreateRandomBlock(int idx, Vector2 pos)
    {
        GameObject temp = Instantiate(m_OriginBlock);
        temp.transform.parent = transform;              //GameManager의 자식으로 넣는다.

        //랜덤으로 하나의 이미지를 고른다.
        int type = Random.Range(0, 10);
        Block block = temp.GetComponent<Block>();
        block.m_Sprite = temp.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.GetAnimalSprite(type);
        block.m_ColumnIndex = idx;
        block.BlockType = type;
        block.Init(this);

        temp.transform.localPosition = pos;
        temp.name = "Block" + type.ToString();

        return temp.gameObject;
    }

    public void DeleteAndRebornBlock(GameObject block)
    {
        // (Delete Part) Begin
        int x = block.GetComponent<Block>().m_ColumnIndex;
        m_Blocks[x].Remove(block.transform);                    //ArrayList 에서 삭제
        Destroy(block);                     // 현재 객체 제거
        // (Delete Part) End


        // (Reborn Part) Begin
        //5만큼 위에 만들거나 원래 만들어진 것 위에 만들기 위함.
        float y = Mathf.Max(5.0f, ((Transform)m_Blocks[x][m_Blocks[x].Count - 1]).transform.position.y + SPRITE_SIZE);
        

        //Create New Block
        m_Blocks[x].Add(CreateRandomBlock(x, new Vector2(SPRITE_SIZE * (x - (COLUMN_MAX - 1) / 2f), y)));
        // (Reborn Part) End
        
    }
    
    
    
        
}
