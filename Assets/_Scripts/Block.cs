using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ANI_TYPE
{
    TYPE0,
    TYPE1,
    TYPE2,
    TYPE3,
    TYPE4,
    TYPE5,
    TYPE6,
    TYPE7,
    TYPE8,
    TYPE9,
    TYPE_MAX
}

public class Block : MonoBehaviour
{

    //private const zone
    private const float HIDE_DELAY_TIME = 0.2f;      //죽으면 남아있는 시간
    private const float MOVING_TIME = 0.2f;          //이동하는 시간
    private const float MOVING_THRESHOLD = 0.5f;     //어느정도 이상 움직여야 움직인걸로 확인
    
    //--------------------------------


    bool m_IsDead = false;
    public int m_ColumnIndex = 0;
    Vector2 m_OldPos = Vector2.zero;
    Vector2 m_TargetPos = Vector2.zero;
    bool m_IsSwapping = false;
    ANI_TYPE m_BlockType = ANI_TYPE.TYPE0;
    Block m_Target = null;
    float m_Currtime = 0f;

    public Sprite m_Sprite = null;

    GameManager m_GameManager = null;


    public int BlockType
    {
        set
        {
            m_BlockType = (ANI_TYPE)value;
        }
        get
        {
            return (int)m_BlockType;
        }
    }


    

    public void Init(GameManager gameMgr)
    {
        m_GameManager = gameMgr;
        if(m_GameManager == null)
        {
            Debug.Log("Block says : GameManager is null");
        }
    }


    private void Awake()
    {
        //velocity로 이동중인지 확인하자.
        GetComponent<Rigidbody2D>().velocity = Vector2.down * 5;

    }


    // Use this for initialization
    void Start()
    {
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if(m_GameManager == null)
        {
            Debug.Log("GameManager is not Found");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsSwapping)
        {
            m_Currtime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(m_OldPos, m_TargetPos, m_Currtime / MOVING_TIME);


            //거의다 도착한걸로 인정
            if((transform.localPosition.x - m_TargetPos.x) < 0.001f && (transform.localPosition.y - m_TargetPos.y) < 0.001f)
            {
                m_Currtime = 0f;
                m_OldPos = transform.localPosition;
                m_IsSwapping = false;
                //m_GameManager.m_IsMoving = false;
                ChangeType();
            }
        }

        if (!m_IsDead)
        {
            if (Input.GetMouseButton(0))
            {
                if (m_GameManager.m_TouchedBlock == null)
                {
                    if (GetComponent<Rigidbody2D>().velocity.magnitude <= 0.3f) //움직이지 않는 블럭만 체크하기 위함
                    {
                        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                        if (hit.collider != null && hit.collider.gameObject == gameObject)
                        {
                            //m_GameManager.m_TouchedBlock = hit.collider.gameObject;   
                            //터치한 블락을 저장
                            m_GameManager.m_TouchedBlock = gameObject; //터치한 오브젝트 저장
                        }
                    }
                }
            }
            //마우스를 뗏을 때 터치한 블락이 자신이면
            else if (m_GameManager.m_TouchedBlock == gameObject)
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // 터치한 캐릭터와 마우스를 뗀 위치의 거리 구하기
                float xDis = Mathf.Abs(pos.x - transform.position.x);
                float yDis = Mathf.Abs(pos.y - transform.position.y);

                if (xDis > yDis) //가로로 움직인게 세로로 움직인거보다 더 길면
                {
                    if (xDis >= MOVING_THRESHOLD)
                    {
                        int x = m_ColumnIndex;                                  //현재 캐릭터의 ColumnIndex
                        int y = m_GameManager.m_Blocks[x].IndexOf(gameObject);  //Row Index

                        //터치를 뗀 x좌표가 캐릭터보다 좌측에 있고
                        //열 번호가 1 이상인 경우만 체크
                        //열 번호가 0이면 왼쪽으로 이동 불가
                        if(pos.x < transform.position.x && x >= 1) //왼쪽으로 이동
                        {
                            m_GameManager.m_IsMoving = true;        //여기서 모든 캐릭터의 중력을 금지시켜서 못움직이게 함.

                            //상대방의 Block 컴포넌트를 얻어옴
                            m_Target = ((GameObject)m_GameManager.m_Blocks[x - 1][y]).GetComponent<Block>();

                            //현재 캐릭터를 왼쪽으로 이동
                            //이동하는 함수 넣어줘야 함
                            Move(m_Target.transform.localPosition, (ANI_TYPE)m_Target.BlockType);


                            //현재 캐릭터의 왼쪽에 있는 캐릭터를 오른쪽으로 이동
                            m_Target.Move(transform.localPosition, m_BlockType);

                        }

                        if (pos.x > transform.position.x && x <= m_GameManager.ColumnMax - 2) //오른쪽으로 이동
                        {
                            m_GameManager.m_IsMoving = true;        //여기서 모든 캐릭터의 중력을 금지시켜서 못움직이게 함.

                            //상대방의 Block 컴포넌트를 얻어옴
                            m_Target = ((GameObject)m_GameManager.m_Blocks[x + 1][y]).GetComponent<Block>();

                            //현재 캐릭터를 오른쪽으로 이동
                            //이동하는 함수 넣어줘야 함
                            // TO DO

                            //현재 캐릭터의 왼쪽에 있는 캐릭터를 왼쪽으로 이동
                            //여기도 이동 함수..
                        }
                    }
                }
                else        //세로가 더 길게 움직였을 때
                { 
                    if(yDis >= MOVING_THRESHOLD)
                    {
                        int x = m_ColumnIndex;                                  //현재 캐릭터의 ColumnIndex
                        int y = m_GameManager.m_Blocks[x].IndexOf(gameObject);  //Row Index

                        // 터치를 뗀 y좌표가 블락보다 위에 있고
                        // 행 번호가 맥스보다 2 작은 경우만 체크
                        if(pos.y > transform.position.y && y <= m_GameManager.RowMax - 2)
                        {
                            m_GameManager.m_IsMoving = true;        //여기서 모든 캐릭터의 중력을 금지시켜서 못움직이게 함.

                            //상대방의 Block 컴포넌트를 얻어옴
                            m_Target = ((GameObject)m_GameManager.m_Blocks[x][y + 1]).GetComponent<Block>();

                            //현재 캐릭터를 위쪽으로 이동
                            //이동하는 함수 넣어줘야 함
                            // TO DO

                            //현재 캐릭터의 왼쪽에 있는 캐릭터를 아래쪽으로 이동
                            //여기도 이동 함수..
                        }
                        else if (pos.y < transform.position.y && y >= 1)
                        {
                            m_GameManager.m_IsMoving = true;        //여기서 모든 캐릭터의 중력을 금지시켜서 못움직이게 함.

                            //상대방의 Block 컴포넌트를 얻어옴
                            m_Target = ((GameObject)m_GameManager.m_Blocks[x][y - 1]).GetComponent<Block>();

                            //현재 캐릭터를 아래쪽으로 이동
                            //이동하는 함수 넣어줘야 함
                            // TO DO

                            //현재 캐릭터의 왼쪽에 있는 캐릭터를 위쪽으로 이동
                            //여기도 이동 함수..
                        }
                    }
                }
                m_GameManager.m_TouchedBlock = null;
            }
        }
        if (m_GameManager.m_IsMoving == false) {
            GetComponent<Rigidbody2D>().gravityScale = 1;
        }
        else
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        

    }

    public void DeadBlock()
    {
        if (!m_IsDead)
        {
            m_IsDead = true;
        }
    }

    public bool GetDead()
    {
        return m_IsDead;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

    }

    void ChangeType()
    {
        m_Sprite = SpriteManager.Instance.GetAnimalSprite((int)m_BlockType);
    }

    public void Move(Vector2 dest, ANI_TYPE type)
    {
        m_BlockType = type;
        m_OldPos = transform.localPosition;
        m_TargetPos = dest;
        m_IsSwapping = true;
    }

   

}
