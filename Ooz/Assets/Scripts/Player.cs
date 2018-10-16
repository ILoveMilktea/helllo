﻿using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class Player : MonoBehaviour
{

    // ----------------------- 키 입력 관련 부분 ------------------------------
    public float MOVE_SPEED = 5.0f;          // 이동 속도

    private struct Key                              // 키 조작 정보
    {
        public bool up;
        public bool down;
        public bool right;
        public bool left;

        public bool pick;                           // 줍는다/버린다 Z키
        public bool action;                         // 먹는다/수리한다 X키
        public bool lit_Fire;                        // 불피우기 Spacebar

        public bool skill_change_left;              // 스킬교체 A키
        public bool skill_ON;                       // 스킬사용 S키
        public bool skill_OFF;                      // 스킬사용 S키 해제
        public bool skill_change_right;             // 스킬교체 D키
    }

    private Key key;                                // 키 조작 보관 변수 선언

    public enum STEP                                // 플레이어 상태 열거체
    {
        NONE = -1,                                  // 상태 없음
        MOVE = 0,                                   // 이동
        REPAIRING,                                  // 수리
        EATING,                                     // 식사
        NUM,                                        // 현재 상태가 몇 종류인지 나타냄
    };

    public STEP step = STEP.NONE;                   // 현재 상태
    public STEP next_step = STEP.NONE;              // 다음 상태
    public float step_timer = 0.0f;                 // 타이머

    private void Get_Input()                        // key값 갱신 method
    {
        this.key.up = false;
        this.key.down = false;
        this.key.right = false;
        this.key.left = false;

        this.key.pick = false;
        this.key.action = false;
        this.key.lit_Fire = false;

        this.key.skill_change_left = false;
        this.key.skill_ON = false;
        this.key.skill_OFF = false;
        this.key.skill_change_right = false;
        

        // |= 연산은 A |= B; 라면 A = A|B; 의 역할을 합니다.
        // A, B 둘 다 false일 때만 false를 반환합니다.
        // up키가 눌리면 true 대입
        this.key.up |= Input.GetKey(KeyCode.UpArrow);
        this.key.up |= Input.GetKey(KeyCode.Keypad8);

        // down키가 눌리면 true 대입
        this.key.down |= Input.GetKey(KeyCode.DownArrow);
        this.key.down |= Input.GetKey(KeyCode.Keypad2);

        // right키가 눌리면 true 대입
        this.key.right |= Input.GetKey(KeyCode.RightArrow);
        this.key.right |= Input.GetKey(KeyCode.Keypad6);

        // left키가 눌리면 true 대입
        this.key.left |= Input.GetKey(KeyCode.LeftArrow);
        this.key.left |= Input.GetKey(KeyCode.Keypad4);

        // 아래는 연속입력을 방지하기 위해 GetKeyDown을 사용합니다.
        // Z키가 눌리면 true 대입 (줍는다/버린다)
        this.key.pick |= Input.GetKeyDown(KeyCode.Z);
        // X키가 눌리면 true 대입 (먹는다/수리한다)
        this.key.action |= Input.GetKeyDown(KeyCode.X);
        // Spacebar키가 눌리면 true 대입, 횃불을 지핀다.
        this.key.lit_Fire |= Input.GetKeyDown(KeyCode.Space);

        // S키가 눌리면 true 대입 (스킬사용), A,D로 스킬 교체
        this.key.skill_change_left |= Input.GetKeyDown(KeyCode.A);
        this.key.skill_ON |= Input.GetKeyDown(KeyCode.S);
        this.key.skill_OFF |= Input.GetKeyUp(KeyCode.S);
        this.key.skill_change_right |= Input.GetKeyDown(KeyCode.D);



    }

    public SkeletonAnimation Now = null;
    public GameObject Front = null;
    public GameObject Left = null;
    public GameObject Right = null;
    public GameObject Back = null;
    private string cur_Animation = "";              // 현재 실행중인 애니메이션

    private void Move_Control()                     //실제로 Player 를 이동시키는 method 입니다.
    {
        Vector3 move_vector = Vector3.zero;         // 플레이어 이동용 Vector
        Vector3 position = this.transform.position; // 현재 위치를 보관하는 Vector

        // ↑,↓,→,←키가 눌리면
        do
        {
            if (this.key.up)
            {
                move_vector += Vector3.forward;         // move_vector(이동용 Vector)를 위쪽으로 변경
                break;
            }

            if (this.key.down)
            {
                move_vector += Vector3.back;
            }
        } while (false);

        do
        {
            if (this.key.right)
            {
                move_vector += Vector3.right;
                break;
            }

            if (this.key.left)
            {
                move_vector += Vector3.left;
            }
        } while (false);

        do
        {

            if (move_vector.x > 0.0f) // 오른
            {
                Front.SetActive(false);
                Left.SetActive(false);
                Right.SetActive(true);
                Back.SetActive(false);

                Now = Right.GetComponent<SkeletonAnimation>();
                Spine_SetAnimation("run", true, 1.0f);
                break;
            }

            if (move_vector.x < 0.0f) // 왼
            {
                Front.SetActive(false);
                Left.SetActive(true);
                Right.SetActive(false);
                Back.SetActive(false);

                Now = Left.GetComponent<SkeletonAnimation>();
                Spine_SetAnimation("run", true, 1.0f);
                break;
            }

            if (move_vector.z > 0.0f) // 위
            {
                Front.SetActive(false);
                Left.SetActive(false);
                Right.SetActive(false);
                Back.SetActive(true);

                Now = Back.GetComponent<SkeletonAnimation>();
                Spine_SetAnimation("run", true, 1.0f);
                break;
            }

            if (move_vector.z <= 0.0f) //아래
            {

                Front.SetActive(true);
                Left.SetActive(false);
                Right.SetActive(false);
                Back.SetActive(false);

                Now = Front.GetComponent<SkeletonAnimation>();
                Spine_SetAnimation("run", true, 1.0f);
                break;
            }
        } while (false);


        move_vector.Normalize();                    // Vector 길이를 1로 정규화
        move_vector *= MOVE_SPEED * Time.deltaTime; // 거리 = 속도 x 시간
        position += move_vector;                    // 플레이어 위치를 이동                       
        position.y = 0.0f;                          // 플레이어 높이를 0으로 한다



        position.y = this.transform.position.y;     // 새로 구한 위치의 높이를 현재 높이로
        this.transform.position = position;         // 새로 구한 위치를 현재 위치로

        // 이동을 했다면?
        if (move_vector.magnitude > 0.01f)
        {// 캐릭터의 방향을 바꾼다.
            Quaternion q = Quaternion.LookRotation(move_vector, Vector3.up);
            this.transform.rotation = q;            // Lerp 빼버렸다.
        }

    }
    private void Spine_SetAnimation(string name, bool loop, float speed)
    {
        if (name == cur_Animation)
        {
            return;
        }
        else
        {
            Now.state.SetAnimation(0, name, loop).timeScale = speed;
            cur_Animation = name;
        }
    }
    // ----------------------- 수집 관련 부분 ------------------------------

    private GameObject closest_item = null;         // 플레이어의 정면에 있는 게임 오브젝트
    private GameObject carried_item = null;         // 플레이어가 들어 올린 게임 오브젝트
    private ItemRoot item_root = null;              // ItemRoot 스크립트를 가져온다
    public GUIStyle guistyle;                       // Font 스타일

    private void Pick_Or_Drop_Control()             // 물건을 줍거나 떨어뜨리기 위한 method 입니다.
    {
        do
        {
            if (!this.key.pick)                                                             // '줍기/버리기' Z키가 눌리지 않았으면 종료
            {
                break;
            }

            if (this.carried_item == null)                                                  // 들고있는 아이템이 없으면?
            {
                if (this.closest_item == null)                                              // 주목중인 아이템이 없으면? -> 종료
                {
                    break;
                }

                // 주목중인 아이템이 있다면?
                this.carried_item = this.closest_item;                                      // 주목중인 아이템을 들어올림
                this.carried_item.transform.parent = this.transform;                        // 들어 올린 아이템을 Player 자식으로 설정
                this.carried_item.transform.localPosition = Vector3.up * 2.0f;              // 머리 위로 이동
                this.closest_item = null;                                                   // 주목 중인 아이템을 삭제   
            }
            else
            {
                // 들고 있는 아이템이 있으면?
                this.carried_item.transform.localPosition = Vector3.forward * 1.0f;         // 들고 있는 아이템을 앞으로 이동시킴
                this.carried_item.transform.parent = null;                                  // 자식 설정을 해제함
                this.carried_item = null;                                                   // 들고 있는 아이템 삭제
            }
        } while (false);
    }
    private bool Is_Other_In_View(GameObject other) // 접촉한 물건이 자신의 정면에 있는지 판정하는 method 입니다.
    {
        bool ret = false;
        do
        {
            Vector3 heading = this.transform.TransformDirection(Vector3.forward);
            // 자신이 현재 향하고 있는 방향
            Vector3 to_other = other.transform.position - this.transform.position;
            // 자신 쪽에서 본 아이템의 방향

            heading.y = 0.0f;                               // Player 의 Item 의 높이를 같은 선상으로 한 후
            heading.Normalize();                            // Player 의 방향벡터의 길이를 1로 정규화 시킵니다.
            to_other.y = 0.0f;                              // Item 과 Player 의 높이를 같은 선상으로 한 후
            to_other.Normalize();                           // Item 의 방향벡터의 길이를 1로 정규화 시킵니다.

            float dp = Vector3.Dot(heading, to_other);      // 양쪽 벡터의 내적을 얻습니다.
            if (dp < Mathf.Cos(45.0f))                      // 내적이 45도 cosine 값 미만이라면? -> 루프를 빠져나갑니다.
            {
                break;
            }
            ret = true;                                     // 내적이 45도 cosine 값 이상이면 정면에 있다고 판정합니다.
        } while (false);
        return ret;

        /*
         * 벡터의 내적이 기억이 잘 안나서 정리합니다.
         *  vector A 와 vector B 의 내적은 A·B 로 나타내며,
         *  A·B = |A| * |B| * cosθ 입니다.
         *  
         * 여기서 내적이 사용된 이유는 Player의 인지 범위(45도) 내에 Item이 있는지 판별하기 위함입니다.
         * 그렇다면 각도를 비교해야 하지만 그런 프로그래밍적 명령은 없나 봅니다.
         * 
         * 각도를 비교할 수 없기 때문에 내적을 사용해 비교하기 위해 다음과 같은 과정을 거쳤습니다.
         *  1. Mathf.Cos(45.0f)는 각 벡터의 길이가 1이고 각도가 45도인 두 벡터의 내적을 의미합니다.
         *  2. 때문에 Player 의 방향벡터, Item 의 방향벡터의 길이를 1로 정규화 합니다.
         *  3. 두 방향벡터의 내적을 구합니다 = dp
         *  4. dp와 Mathf.Cos(45.0f)를 비교합니다.
         * 
         * 내적이 여기서 나오네
         */
    }
    private bool Is_Carried_Food()                       // 들고 있는 물건이 먹을것인지 판별하는 method입니다.
    {
        if (this.carried_item != null)
        {
            Item.TYPE carried_item_type = this.item_root.GetItemType(this.carried_item);

            switch (carried_item_type)              // 가지고 있는 아이템을 판별합니다.
            {
                case Item.TYPE.APPLE:               // 사과라면
                case Item.TYPE.PLANT:               // 혹은 식물이라면
                    return true;
            }
        }
        return false;
    }

    // 주목한다는 것은 Player가 떨어져 있는 Item을 보고 있음을 의미합니다.
    void OnTriggerStay(Collider other)              // Trigger에 걸린 GameObject를 주목합니다.
    {
        GameObject other_go = other.gameObject;

        // Trigger의 GameObject Layer 설정이 Item이면~
        if (other_go.layer == LayerMask.NameToLayer("Item"))
        {
            if (this.closest_item == null)                  // 아무것도 주목하고 있지 않다면?
            {
                if (this.Is_Other_In_View(other_go))        // 정면에 아이템이 있으면?
                {
                    this.closest_item = other_go;           // 주목한다.
                }
            }
            else if (this.closest_item == other_go)         // 지금 뭔가 주목하고 있다면?
            {
                if (!this.Is_Other_In_View(other_go))       // 정면에 아이템이 없으면?
                {
                    this.closest_item = null;               // 주목을 그만둔다.
                }
            }
        }
    }
    void OnTriggerExit(Collider other)              // 주목을 그만둡니다.
    {
        if (this.closest_item == other.gameObject)
        {
            this.closest_item = null;
        }
    }

    // 워프를 타보자
    public Transform warpTarget = null;
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Warp")
        {
            warpTarget = col.transform.GetChild(0).transform;
            this.transform.position = warpTarget.position;
        }
    }


    // ----------------------- 스킬 사용 부분 ------------------------------
    private GameObject carried_skill = null;
    private SkillRoot skill_root = null;
    private int Skill_Pointer = 0;

    private void Skill_Use_Control()             // 스킬 사용하는 method 입니다.
    {
        switch (this.carried_skill.tag)
        {
            case "Owl":
                this.skill_root.UseOwl();
                break;
            case "Alpaca":
                this.skill_root.UseAlpaca();
                break;
            case "Turtle":
                this.skill_root.UseTurtle();
                break;
        }
    }

    private void Skill_Reset_Control()             // 스킬을 취소하는 method 입니다.
    {
        switch (this.carried_skill.tag)
        {
            case "Owl":
                this.skill_root.resetOwl();
                break;
            case "Alpaca":
                this.skill_root.resetAlpaca();
                break;
            case "Turtle":
                this.skill_root.resetTurtle();
                break;
        }
    }

    private void Skill_Change_Control()
    {
        if (this.key.skill_change_left)
        {
            this.Skill_Pointer--;
            if (this.Skill_Pointer == -1)
                this.Skill_Pointer = this.skill_root.Skill_list.Count - 1;
        }
        else
        {
            this.Skill_Pointer++;
            if (this.Skill_Pointer == this.skill_root.Skill_list.Count)
                this.Skill_Pointer = 0;
        }

        this.carried_skill = this.skill_root.Skill_list[Skill_Pointer];
    }

    // --------- 화면 UI ----------
    void OnGUI()
    {
        // x 는 값을 더하면 오른쪽으로, 빼면 왼쪽으로
        float x = 20.0f;
        // y 는 값을 더하면 아래로, 빼면 위로
        float y = Screen.height - 40.0f;

        if (this.carried_item != null)              // 들고 있는 Item이 있다면
        {
            GUI.Label(new Rect(x, y, 200.0f, 20.0f), "Z : 버린다", guistyle);
            GUI.Label(new Rect(x + 100.0f, y, 200.0f, 20.0f), "X : 먹는다", guistyle);
        }
        else
        {
            if (this.closest_item != null)           // 들고있지 않은데 주목하고 있다면
            {
                GUI.Label(new Rect(x, y, 200.0f, 20.0f), "Z : 줍는다", guistyle);
            }
        }

        switch (this.step)
        {
            case STEP.EATING:
                GUI.Label(new Rect(x, y, 200.0f, 20.0f), "암냠냠", guistyle);
                break;
        }

        GUI.Label(new Rect(x, y - 20.0f, 200.0f, 20.0f), "◀", guistyle);
        GUI.Label(new Rect(x + 100.0f, y - 20.0f, 200.0f, 20.0f), "▶", guistyle);
        switch (this.carried_skill.tag)
        {
            case "Owl":
                GUI.Label(new Rect(x + 30.0f, y - 20.0f, 200.0f, 20.0f), "부엉이", guistyle);
                break;
            case "Alpaca":
                GUI.Label(new Rect(x + 30.0f, y - 20.0f, 200.0f, 20.0f), "알파카", guistyle);
                break;
            case "Turtle":
                GUI.Label(new Rect(x + 30.0f, y - 20.0f, 200.0f, 20.0f), "거북이", guistyle);
                break;
        }
    }



    // ---------- 게임 플레이 -----------
    public GameObject basic_light = null;
    public GameObject torch_light = null;
    public float go_dark = 0.1f;
    public float go_bright = 0.1f;

    // basic_light range = 8 ~ 12 
    // torch_light range = 8 ~ 12

    public float light_timer = 30.0f;

    bool is_carried_Stick()
    {
        return true;
    }

    IEnumerator goDarkness()                            // 횃불 꺼짐.
    {
        Light tBasic = basic_light.GetComponent<Light>();
        Light tTorch = torch_light.GetComponent<Light>();
        float range_instance = tBasic.range + tTorch.range - 2.0f;

        while (tBasic.range + tTorch.range > range_instance)           
        {
            if (tBasic.range + tTorch.range < 16.0f)     // 밝기 최소시 탈출
                break;

            tBasic.range -= go_dark;
            tTorch.range -= go_dark;

            yield return new WaitForSeconds(0.01f);
        }
        
    }

    IEnumerator litFire()                               // 횃불 지피기. 코루틴
    {
        Light tBasic = basic_light.GetComponent<Light>();
        Light tTorch = torch_light.GetComponent<Light>();
        float range_instance = tBasic.range + tTorch.range + 2.0f;

        while (tBasic.range + tTorch.range < range_instance)            
        {
            if (tBasic.range + tTorch.range > 24.0f)    // 밝기 최대시 탈출
                break;

            if (is_carried_Stick())                                // Stick을 가지고 있다면
            {
                tBasic.range += go_bright;
                tTorch.range += go_bright;

            }
            yield return new WaitForSeconds(0.01f);
        }

    }
    

    void Start()
    {
        // 이동, 수리, 식사 등 상태에 관한 초기화입니다
        this.step = STEP.NONE;                      // 현재 상태 초기화
        this.next_step = STEP.NONE;                 // 다음 상태 초기화

        // 수집 행동을 위해 ItemRoot 스크립트를 가져옵니다.
        this.item_root = GameObject.Find("GameRoot").GetComponent<ItemRoot>();
        this.guistyle.fontSize = 16;

        this.skill_root = GameObject.Find("GameRoot").GetComponent<SkillRoot>();
        this.carried_skill = skill_root.Skill_list[0];    // 스킬 설정
    }

    
    void Update()
    {
        this.Get_Input();                           // 키 입력정보 취득
        this.step_timer += Time.deltaTime;
        float eat_time = 2.0f;

        // 상태를 변화시키는 처리
        // 시작시 step = NONE, next_step = NONE
        switch (this.step)                      // 현재 상태가...
        {
            case STEP.NONE:
                if (this.key.up || this.key.down || this.key.left || this.key.right)
                {
                    this.next_step = STEP.MOVE;
                    break;
                }

                if (Is_Carried_Food())
                {
                    this.next_step = STEP.EATING;   // 상태를 '식사'로 바꿉니다.
                    break; // 탈출
                }
                break;
            case STEP.MOVE:                     // 이동중일 때
                do
                {
                    if (!this.key.action)       // 액션(X)키가 눌려 있지 않다 -> 루프 탈출
                    {
                        if (!this.key.up && !this.key.down && !this.key.left && !this.key.right)
                        {
                            this.next_step = STEP.NONE;
                        }
                        break;  // 탈출
                    }

                    if (Is_Carried_Food())
                    {
                        this.next_step = STEP.EATING;   // 상태를 '식사'로 바꿉니다.
                        break; // 탈출
                    }

                } while (false);
                break;
            case STEP.EATING:
                if (this.step_timer > eat_time)         // 아래서 상태 변할때 step_timer를 0으로 만들어 2초간 돌게된다.
                {
                    this.next_step = STEP.NONE;
                }
                break;
        }

        // 상태가 변화할 때의 처리
        if (this.step != this.next_step)
        {
            this.step = this.next_step;

            switch (this.step)
            {
                case STEP.NONE:                     // '대기' 상태 처리
                    break;
                case STEP.MOVE:                     // '이동' 상태 처리
                    break;
                case STEP.EATING:                   // '식사' 상태 처리
                    if (this.carried_item != null)
                    {
                        GameObject.Destroy(this.carried_item);  // 먹은 아이템을 없앱니다.
                    }
                    break;
            }
            this.step_timer = 0.0f;
        }

        // 상태 유지시 처리
        switch (this.step)
        {
            case STEP.NONE:                     // '대기' 상태 처리
                this.Pick_Or_Drop_Control();
                Spine_SetAnimation("idle", true, 1.0f);
                break;
            case STEP.MOVE:                     // '이동' 상태 처리
                this.Move_Control();                // 이동 조작
                this.Pick_Or_Drop_Control();        // 수집 조작
                Spine_SetAnimation("run", true, 1.0f);
                break;
        }


        // 여러가지 키 입력시 조작
        
          //스킬 조작
        do
        {
            if (this.key.skill_change_left || this.key.skill_change_right)
            {
                Skill_Change_Control();
                break;
            }

            if (this.key.skill_ON)              // 스킬을 가지고 있다면?
            {
                Skill_Use_Control();
            }

            if (this.key.skill_OFF)
            {
                Skill_Reset_Control();
            }
        } while (false); 

          //횃불 조작
        if (this.key.lit_Fire)              // 불 지피기
        {
            StartCoroutine("litFire");
        }

        this.light_timer -= Time.deltaTime;
        if (this.light_timer <= 0.0f || Input.GetKeyDown(KeyCode.Alpha0))       // 불 끄기, 테스트용으로 0번을 끄는모드
        {
            StartCoroutine("goDarkness");
            this.light_timer = 30.0f;       // 30초마다 시야 감소
        }

    }

}
