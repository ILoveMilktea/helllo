using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum TYPE                                    // 아이템의 종류 열거체
    {
        NONE = -1,                                      // 없음
        IRON = 0,                                       // 철광석
        APPLE,                                          // 사과
        PLANT,                                          // 식물
        NUM,                                            // 아이템의 종류 수를 나타낸다.(= 3)
    };
};

public class ItemRoot : MonoBehaviour
{
    public GameObject ApplePrefab = null;               // Apple 프리팹
    public GameObject IronPrefab = null;                // Iron 프리팹
    public GameObject PlantPrefab = null;               // Plant 프리팹
    protected List<Vector3> Respawn_points;             // Respawn 지점 리스트

    public float step_timer = 0.0f;
    public static float RESPAWN_TIME_APPLE = 20.0f;     // Apple 출현 시간 상수
    public static float RESPAWN_TIME_IRON = 12.0f;      // Iron 출현 시간 상수
    public static float RESPAWN_TIME_PLANT = 6.0f;      // Plant 출현 시간 상수
    private float respawn_timer_apple = 0.0f;           // Apple 출현 시간
    private float respawn_timer_iron = 0.0f;            // Iron 출현 시간
    private float respawn_timer_plant = 0.0f;           // Plant 출현 시간

    // respawn 위치를 조정하는 method 들입니다.
    public void RespawnApple()
    {
        GameObject go = GameObject.Instantiate(this.ApplePrefab) as GameObject;         // Apple Respawn 위치 생성
        Vector3 pos = GameObject.Find("AppleRespawn").transform.position;               // 출현지점을 가져와서
        pos.y = 1.0f;
        pos.x += Random.Range(-1.0f, 1.0f);
        pos.z += Random.Range(-1.0f, 1.0f);                                             // 약간 랜덤하게 바꾼 다음에
        go.transform.position = pos;                                                    // 바뀐 위치를 알려줍니다.
    }

    public void RespawnIron()
    {
        GameObject go = GameObject.Instantiate(this.IronPrefab) as GameObject;          // Iron Respawn 위치 생성
        Vector3 pos = GameObject.Find("IronRespawn").transform.position;                // 출현지점을 가져와서
        pos.y = 1.0f;
        pos.x += Random.Range(-1.0f, 1.0f);
        pos.z += Random.Range(-1.0f, 1.0f);                                             // 약간 랜덤하게 바꾼 다음에
        go.transform.position = pos;                                                    // 바뀐 위치를 알려줍니다.


    }
    public void RespawnPlant()
    {
        if (this.Respawn_points.Count > 0)                                               // 출현지점 List에 자리가 있다면?
        {
            GameObject go = GameObject.Instantiate(this.PlantPrefab) as GameObject;     // Plant Respawn 위치 생성
            int n = Random.Range(0, this.Respawn_points.Count);
            Vector3 pos = this.Respawn_points[n];                                       // 출현지점을 랜덤하게 가져와서
            pos.y = 1.0f;
            pos.x += Random.Range(-1.0f, 1.0f);
            pos.z += Random.Range(-1.0f, 1.0f);                                         // 또 약간 랜덤하게 바꾼 다음에
            go.transform.position = pos;                                                // 바뀐 위치를 알려줍니다.
        }
    }

    // 아이템의 종류를 Item.TYPE으로 반환하는 method 입니다.
    public Item.TYPE GetItemType(GameObject item_go)
    {
        Item.TYPE type = Item.TYPE.NONE;

        if (item_go != null)
        {
            switch (item_go.tag)                        // tag가 비어있지 않으면 tag에 따라 나뉩니다.
            {
                case "Iron":
                    type = Item.TYPE.IRON;
                    break;
                case "Apple":
                    type = Item.TYPE.APPLE;
                    break;
                case "Plant":
                    type = Item.TYPE.PLANT;
                    break;
            }
        }
        return type;
    }

    void Start()
    {
        // ----- 각 아이템의 출현 지점을 보이지 않게 합니다. -----
        // ----- 현재 Respawn 지점에 Cilynder로 지정해 놓은 것을 안 보이게 합니다. -----
        // Plant의 Mesh Renderer를 보이지 않게 합니다.

        this.Respawn_points = new List<Vector3>();
        GameObject[] respawns = GameObject.FindGameObjectsWithTag("PlantRespawn"); // "PlantRespawn" 태그가 붙은 모든 오브젝트를 배열에 저장합니다.
        foreach (GameObject go in respawns)                                        // 배열 respawns 내의 각 GameObject를 순서대로 처리합니다.
        {
            MeshRenderer renderer = go.GetComponentInChildren<MeshRenderer>();
            if (renderer != null)
            {
                renderer.enabled = false;
            }
            this.Respawn_points.Add(go.transform.position);
        }

        // Apple의 Mesh Renderer를 보이지 않게 합니다.
        GameObject applerespawn = GameObject.Find("AppleRespawn");
        applerespawn.GetComponent<MeshRenderer>().enabled = false;
        // Iron의 Mesh Renderer를 보이지 않게 합니다.
        GameObject ironrespawn = GameObject.Find("IronRespawn");
        ironrespawn.GetComponent<MeshRenderer>().enabled = false;

        // 시작과 함께 Iron, Plant 하나씩 생성.
        this.RespawnIron();
        this.RespawnPlant();

    }

    void Update()
    {
        // 각각의 Object들이 일정 시간을 초과했으면 자동 생성되도록 합니다.
        respawn_timer_apple += Time.deltaTime;
        respawn_timer_iron += Time.deltaTime;
        respawn_timer_plant += Time.deltaTime;

        if(respawn_timer_apple > RESPAWN_TIME_APPLE)
        {
            respawn_timer_apple = 0.0f;
            this.RespawnApple();
        }
        if (respawn_timer_iron > RESPAWN_TIME_IRON)
        {
            respawn_timer_iron = 0.0f;
            this.RespawnIron();
        }
        if (respawn_timer_plant > RESPAWN_TIME_PLANT)
        {
            respawn_timer_plant = 0.0f;
            this.RespawnPlant();
        }
    }
}
