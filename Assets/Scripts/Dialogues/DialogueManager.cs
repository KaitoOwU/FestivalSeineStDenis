using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public event Action OnDialogueStart;
    public event Action OnDialogueEnd;

    [SerializeField] private Dialogue testDialogue;
    [SerializeField] private GameObject _dialogueCanva;
    [SerializeField] private GameObject _dialogueNextButton;
    [SerializeField] private Image _characterSprite;
    [SerializeField] private TextMeshProUGUI _characterLine;
    [SerializeField] private TextMeshProUGUI _characterName;

    public Queue _dialogueQueue = new Queue();
    private Coroutine _dialogueRoutine;

    private bool _skip;

    public bool Skip { get => _skip; set => _skip = value; }
    public GameObject DialogueCanva { get => _dialogueCanva; set => _dialogueCanva = value; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        OnDialogueStart += () => DialogueCanva.SetActive(true);
        OnDialogueEnd += () => DialogueCanva.SetActive(false);
    }
    private void Start()
    {
        //QueueDialogue(testDialogue);
        _dialogueNextButton.SetActive(false);
        PlayerManager.instance.DialogueManager = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            QueueDialogue(testDialogue);

        }
    }

    public void NextDialogue()
    {
        Skip = true;
    }

    private bool CanNextDialogue()
    {
        if (Skip)
        {
            Skip = false;
            return true;
        }
        return false;
    }

    public void QueueDialogue(Dialogue dialogue)
    {
        _dialogueQueue.Enqueue(dialogue);

        if(_dialogueRoutine == null)
        {
            _dialogueRoutine = StartCoroutine(DisplayDialogue());
        }
    }

    private Sprite GetEmotion(CharacterData character, DialogueLine.Emotion emotion)
    {
        switch (emotion)
        {
            case DialogueLine.Emotion.Sad:
                return character._sadSprite;

            case DialogueLine.Emotion.Happy:
                return character._happySprite;

            case DialogueLine.Emotion.Angry:
                return character._angrySprite;

            case DialogueLine.Emotion.Surprised:
                return character._surprisedSprite;

            case DialogueLine.Emotion.Neutral:
                return character._neutralSprite;

            case DialogueLine.Emotion.Flushed:
                return character._flushedSprite;
        }
        return null;
    }

    IEnumerator DisplayDialogue()
    {
        OnDialogueStart?.Invoke();
        GameManager.instance.GameState = GameManager.GAMESTATE.Dialogue;
        GameManager.instance.OnDialogue?.Invoke();

        while(_dialogueQueue.Count >= 1)
        {
            Dialogue currentDialogue = (Dialogue)_dialogueQueue.Dequeue();

            foreach(DialogueLine dl in currentDialogue.Lines)
            {
                _dialogueNextButton.SetActive(false);
                _characterSprite.sprite = GetEmotion(dl._character, dl._emotion);
                _characterName.text = dl._character._name;
                _characterLine.text = null;

                foreach(char c in dl._line)
                {
                    _characterLine.text += c;
                    yield return null;

                }

                yield return new WaitForSeconds(0.1f);
                _dialogueNextButton.SetActive(true);
                yield return new WaitUntil(() => CanNextDialogue());
            }
        }

        GameManager.instance.GameState = GameManager.GAMESTATE.Playing;
        GameManager.instance.OnStopDialogue?.Invoke();
        _characterName.text = null;
        _characterSprite.sprite = null;
        _characterLine.text = null;
        OnDialogueEnd?.Invoke();
        _dialogueRoutine = null;
    }
}
