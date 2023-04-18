using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private Transform p1;
    private Transform p2;

    [SerializeField] private CinemachineVirtualCamera _cam;

    public Transform P1 { get => p1; set => p1 = value; }
    public Transform P2 { get => p2; set => p2 = value; }

    private Coroutine cameraRoutine;
    [SerializeField] private CinemachineTargetGroup _targetGroup;
    public CinemachineTargetGroup TargetGroup { get => _targetGroup; set => _targetGroup = value; }

    
    public void StartCameraMove()
    {
        Invoke("StartRoutine", 0.1f);
    }

    private void StartRoutine()
    {
        p1 = PlayerManager.instance.Players[0].transform;
        p2 = PlayerManager.instance.Players[1].transform;
        cameraRoutine = StartCoroutine(CameraRoutine());
    }

    IEnumerator CameraRoutine()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            Vector2 midPoint = new Vector2((p1.position.x + p2.position.x) / 2, (p1.position.y + p2.position.y) / 2);

            float distance = Vector2.Distance(p1.position, p2.position);

            _cam.m_Lens.OrthographicSize = Mathf.Clamp(distance, 4f, 6.3f);

            transform.position = new Vector3(midPoint.x, midPoint.y, -10);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(PlayerManager.instance.Players[0].transform.position, PlayerManager.instance.Players[1].transform.position);
    //}
}
