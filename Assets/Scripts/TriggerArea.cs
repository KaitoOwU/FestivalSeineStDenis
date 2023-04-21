using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class TriggerArea : MonoBehaviour
{
    [SerializeField] UnityEvent OnAreaEnter;
    [SerializeField] UnityEvent OnAreaExit;

    [SerializeField] Animator _fade;

    [SerializeField] private Vector2 _topRight;
    [SerializeField] private Vector2 _botleft;

    [SerializeField] private GameObject[] _entityToSpawn;
    [SerializeField] private GameObject[] _entityKill;

    private bool _isEntered;
    private bool _isExited;

    [SerializeField] private BoxCollider2D _collider;

    [SerializeField] private LayerMask _layer;
    [SerializeField] private bool _isFinalDialogue;

    [Scene]
    [SerializeField] private int _winScene;


    //private void Start()
    //{
    //    Invoke("Prout", 1f);
    //}

    //private void Prout() => GameManager.instance.OpenMenuScene();
    private void Update()
    {
        if (_isExited)
            return;


        foreach(GameObject go in _entityKill)
        {
            if(go != null)
            {
                return;
            }
        }
        Collider2D[] numPlayer = Physics2D.OverlapAreaAll(new Vector2(transform.position.x, transform.position.y) + _topRight, new Vector2(transform.position.x, transform.position.y) + _botleft, _layer);

        if(!_isEntered && numPlayer.Length >= 2)
        {
            _isEntered = true;
            OnAreaEnter?.Invoke();
            StartCoroutine(WaitEndOfDialogue());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        CustomDebug.DrawRectange(new Vector2(transform.position.x, transform.position.y) + _topRight, new Vector2(transform.position.x, transform.position.y) + _botleft);
    }

    IEnumerator WaitEndOfDialogue()
    {
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            Debug.Log(DialogueManager.instance._dialogueQueue.Count);
            yield return null;
            if(DialogueManager.instance.DialogueCanva.activeSelf == false)
            {
                OnAreaExit?.Invoke();
                _collider.enabled = false;
                _isExited = true;

                if (_isFinalDialogue)
                {
                    yield return new WaitUntil( () => DialogueManager.instance.DialogueCanva.activeSelf == false);
                    _fade.SetTrigger("FadeIn");
                    yield return new WaitForSeconds(1f);
                    GameManager.instance.OpenMenuScene();
                    SceneManager.LoadScene(_winScene);
                }

                if(_entityToSpawn != null)
                {
                    foreach(GameObject go in _entityToSpawn)
                    {
                        Debug.Log(go.name);
                        go.SetActive(true);
                    }
                }
                yield break;
            }

        }
    }

    public void Porut()
    {
        Debug.Log("ss");
    }

    public void Porsut()
    {
        Debug.Log("ssss");
    }
}
