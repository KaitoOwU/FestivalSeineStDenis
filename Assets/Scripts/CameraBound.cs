using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBound : MonoBehaviour
{
    void Start()
    {
        PlayerManager.instance.Camera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = GetComponent<Collider2D>();
    }

}
