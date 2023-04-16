using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private Transform p1;
    private Transform p2;

    public Transform P1 { get => p1; set => p1 = value; }
    public Transform P2 { get => p2; set => p2 = value; }

    private Coroutine cameraRoutine;

    private void StartCameraMove()
    {
        cameraRoutine = StartCoroutine(CameraRoutine());
    }

    IEnumerator CameraRoutine()
    {
        while (true)
        {
            Vector2 midPoint = new Vector2((p1.position.x + p2.position.x) / 2, (p1.position.y + p2.position.y) / 2);

            transform.position = new Vector3(midPoint.x, midPoint.y, -10);
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(PlayerManager.instance.Players[0].transform.position, PlayerManager.instance.Players[1].transform.position);
    }
}
