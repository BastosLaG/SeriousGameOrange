using EasyDialogue;
using UnityEngine;
using TMPro;

public class DialogesManager : MonoBehaviour {
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private TMP_Text[] playerChoices;
    [SerializeField] EasyDialogue.EasyDialogueGraph graphToPlay;

    private EasyDialogueGraph currentGraph;
    public bool HasDialogueGraph() => currentGraph != null;
    //private EasyDialogueManager easyDialogueManager;
    private Canvas myCanvas;

    private void Start() {
        /*
        myCanvas = GetComponent<Canvas>();
        easyDialogueManager = FindObjectOfType<EasyDialogueManager>();
        easyDialogueManager.OnDialogueStarted += (EasyDialogueGraph _graph, dialogue_line _dl) => { Debug.Log($"{_dl.text} was said by {_dl.character}"); };
        easyDialogueManager.OnDialogueProgressed += OnDialogueProgressedHandler;
        easyDialogueManager.OnDialogueEnded += (EasyDialogueGraph _graph) => Debug.Log($"Dialogue ended on graph {_graph.name}");
        InitializeDialogue();*/
    }
/*
    private void OnDialogueProgressedHandler(EasyDialogueGraph _graph, dialogue_line _line) {
        Debug.Log($"{_line.text} was said by {_line.character}");
    }*/
/*
    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(HasDialogueGraph()) {
                GetNextDialogue();
            } else {
                StartDialogueEncounter(ref graphToPlay);
            }
        }
    }*/
    
    private void HidePlayerResponses() {
        foreach(TMP_Text t in playerChoices) {
            t.transform.parent.gameObject.SetActive(false);
        }
    }

    private void ShowPlayerResponses(string[] responses) {
        for(int i = 0; i < responses.Length; ++i) {
            playerChoices[i].text = responses[i];
            playerChoices[i].transform.parent.gameObject.SetActive(true);
        }
    }
}
