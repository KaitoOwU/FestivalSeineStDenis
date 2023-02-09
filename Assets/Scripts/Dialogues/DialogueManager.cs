using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;
using System;

public class DialogueManager : MonoBehaviour
{
    public event Action OnDialogueStart;
    public event Action OnDialogueEnd;

    [SerializeField] private Dialogue testDialogue;
    [SerializeField] private GameObject _dialogueCanva;
    [SerializeField] private GameObject _dialogueNextButton;
    [SerializeField] private Image _characterSprite;
    [SerializeField] private TextMeshProUGUI _characterLine;
    [SerializeField] private TextMeshProUGUI _characterName;

    private Queue _dialogueQueue = new Queue();
    private Coroutine _dialogueRoutine;

    private void Awake()
    {
        OnDialogueStart += () => _dialogueCanva.SetActive(true);
        OnDialogueEnd += () => _dialogueCanva.SetActive(false);
    }
    private void Start()
    {
        QueueDialogue(testDialogue);
        _dialogueNextButton.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
        QueueDialogue(testDialogue);

        }
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

            case DialogueLine.Emotion.Fear:
                return character._fearSprite;
        }
        return null;
    }

    IEnumerator DisplayDialogue()
    {
        OnDialogueStart?.Invoke();

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
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            }
        }

        _characterName.text = null;
        _characterSprite.sprite = null;
        _characterLine.text = null;
        OnDialogueEnd?.Invoke();
        _dialogueRoutine = null;
    }
}
