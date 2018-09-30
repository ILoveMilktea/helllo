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

    public float step_timer = 0.0f;
    public static float COOLDOWN_TIME_OWL = 5.0f;     // Owl 쿨타임 상수
    public static float COOLDOWN_TIME_ALPACA = 20.0f;      // Alpaca 쿨타임 상수
    public static float COOLDOWN_TIME_TURTLE = 10.0f;      // Turtle 쿨타임 상수
    private float cooldown_timer_owl = 0.0f;           // Owl 쿨타임 타이머
    private float cooldown_timer_alpaca = 0.0f;            // Alpaca 쿨타임 타이머
    private float cooldown_timer_turtle = 0.0f;           // Turtle 쿨타임 타이머

    public void UseOwl()
    {
        GameObject go = GameObject.Instantiate(this.OwlPrefab) as GameObject;          // OwlPos 가져오기 위한 객체 생성
        Vector3 pos = GameObject.Find("OwlPos").transform.position;                // 현재 Owl 위치를 가져와서
        // 스킬 사용시 기능 입력 요망

        go.transform.position = pos;                                                    // 바뀐 위치를 알려줍니다.
    }
    public void UseAlpaca()
    {
        GameObject go = GameObject.Instantiate(this.AlpacaPrefab) as GameObject;          // AlpacaPos 가져오기 위한 객체 생성
        Vector3 pos = GameObject.Find("AlpacaPos").transform.position;                // 현재 Alpaca 위치를 가져와서
        // 스킬 사용시 기능 입력 요망

        go.transform.position = pos;                                                    // 바뀐 위치를 알려줍니다.
    }
    public void UseTurtle()
    {
        GameObject go = GameObject.Instantiate(this.TurtlePrefab) as GameObject;          // TurtlePos 가져오기 위한 객체 생성
        Vector3 pos = GameObject.Find("TurtlePos").transform.position;                // 현재 Turtle 위치를 가져와서
        // 스킬 사용시 기능 입력 요망

        go.transform.position = pos;                                                    // 바뀐 위치를 알려줍니다.
    }
    

    void Start () {
        this.Skill_list = new List<GameObject>();
        this.Skill_list.Add(OwlPrefab);
        this.Skill_list.Add(AlpacaPrefab);
        this.Skill_list.Add(TurtlePrefab);
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
