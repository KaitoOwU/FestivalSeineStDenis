using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetPlayerFade : MonoBehaviour
{
    void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChange;
        PlayerManager.instance.Players[0].GetComponent<PlayerHealth>().Fade = GetComponent<Animator>();
        PlayerManager.instance.Players[1].GetComponent<PlayerHealth>().Fade = GetComponent<Animator>();

    }
    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }
    private void OnSceneChange(Scene s1, Scene s2)
    {
        if (SceneManager.GetActiveScene().name == "Tom")
        {
            
        }
    }
}
