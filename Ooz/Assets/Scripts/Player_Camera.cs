using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour
{
    
    public Transform Target;                    // 추적할 대상 (Player)
    public float distance = 5.0f;               // Camera와의 거리
    public float height = 9.0f;                 // Camera의 높이
    public float followSpeed = 5.0f;

    private Transform tr;
    Vector3 cameraPosition;

    void Start()
    {
        tr = GetComponent<Transform>();
    }
    void LateUpdate()
    {
        cameraPosition = Target.position - (1 * Vector3.forward * distance) + (Vector3.up * height);

        // 카메라 위치 설정
        tr.position = Vector3.Lerp(tr.position, cameraPosition , followSpeed*Time.deltaTime);
    }
}
