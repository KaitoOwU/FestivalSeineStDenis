using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cam;
    private float Shake = 1f;
    private float Timee = 2f;

    private float timer;
    private CinemachineBasicMultiChannelPerlin _cbmcp;

    private void Awake()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void Start()
    {
        StopShake();
    }
    private void OnSceneChange(Scene s1, Scene s2)
    {
        if (SceneManager.GetActiveScene().name == "Tom")
        {
            ShakeCamera();
        }
    }

    public void ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin cbmcp = _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = Shake;

        timer = Timee;
    }

    private void StopShake()
    {
        CinemachineBasicMultiChannelPerlin cbmp = _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmp.m_AmplitudeGain = 0;

        timer = 0;
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                StopShake();
            }
        }
    }
}
