using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public enum TYPE
    {
        NONE = -1,
        OWL,
        ALPACA,
        TURTLE,
        NUM,
    }
}
public class SkillRoot : MonoBehaviour {

    public GameObject OwlPrefab = null;
    public GameObject AlpacaPrefab = null;
    public GameObject TurtlePrefab = null;
    public List<GameObject> Skill_list;

    public GameObject Owl_Light_Directional = null;
    private Player_Camera Camera_Control = null;
    private Player Movespeed_Control = null;

    public float step_timer = 0.0f;
    public static float COOLDOWN_TIME_OWL = 5.0f;     // Owl 쿨타임 상수
    public static float COOLDOWN_TIME_ALPACA = 20.0f;      // Alpaca 쿨타임 상수
    public static float COOLDOWN_TIME_TURTLE = 10.0f;      // Turtle 쿨타임 상수
    private float cooldown_timer_owl = 0.0f;           // Owl 쿨타임 타이머
    private float cooldown_timer_alpaca = 0.0f;            // Alpaca 쿨타임 타이머
    private float cooldown_timer_turtle = 0.0f;           // Turtle 쿨타임 타이머

    public void UseOwl()
    {
        Owl_Light_Directional.GetComponent<Light>().enabled = true;

        Camera_Control.distance += 5.0f;
        Camera_Control.height += 5.0f;
    }
    public void UseAlpaca()
    {
        Movespeed_Control.MOVE_SPEED = 10.0f;
    }
    public void UseTurtle()
    {
        GameObject go = GameObject.Instantiate(this.TurtlePrefab) as GameObject;          // TurtlePos 가져오기 위한 객체 생성
        Vector3 pos = GameObject.Find("TurtlePos").transform.position;                // 현재 Turtle 위치를 가져와서
        // 스킬 사용시 기능 입력 요망

        go.transform.position = pos;                                                    // 바뀐 위치를 알려줍니다.
    }

    public void resetOwl()
    {
        Owl_Light_Directional.GetComponent<Light>().enabled = false;

        Camera_Control.distance -= 5.0f;
        Camera_Control.height -= 5.0f;
    }
    public void resetAlpaca()
    {
        Movespeed_Control.MOVE_SPEED = 5.0f;
    }
    public void resetTurtle()
    {
    }


    void Start () {
        this.Skill_list = new List<GameObject>();
        this.Skill_list.Add(OwlPrefab);
        this.Skill_list.Add(AlpacaPrefab);
        this.Skill_list.Add(TurtlePrefab);

        this.Camera_Control = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Player_Camera>();
        this.Movespeed_Control = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update () {
        // 각 스킬 쿨타임 돌리기
        if(cooldown_timer_owl < COOLDOWN_TIME_OWL)
            cooldown_timer_owl += Time.deltaTime;
        if(cooldown_timer_alpaca < COOLDOWN_TIME_ALPACA)
            cooldown_timer_alpaca += Time.deltaTime;
        if(cooldown_timer_turtle < COOLDOWN_TIME_TURTLE)
            cooldown_timer_turtle += Time.deltaTime;
        
	}
}
