using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 10f;
    private Vector3 offset;
    [SerializeField] float distanceUp = 10;
    [SerializeField] float distanceAway = 10;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        Vector3 targetPosition = target.position + Vector3.up * distanceUp - target.forward * distanceAway;
        //摄像机从当前位置移动到目标位置
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
        //摄像机要看向角色物体
        transform.LookAt(target);
    }
}
