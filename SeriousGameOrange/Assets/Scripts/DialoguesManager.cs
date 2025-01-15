using System.Collections;
using System.Collections.Generic;
using EasyDialogue;
using UnityEngine;
using TMPro;

public class DialoguesManager : MonoBehaviour {
    [SerializeField] private TMP_Text textBoxName;
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private TMP_Text[] playerChoices;
    [SerializeField] private EasyDialogueGraph graphToPlay; //Ne pas en mettre qu'un seul
    [SerializeField] private Canvas myCanvas;
    public List<string> memory;

    private EasyDialogueManager easyDialogueManager;
    private EasyDialogueGraph currentGraph;

    private void Start() {
        easyDialogueManager = GetComponent<EasyDialogueManager>();
        memory = new List<string>();

        //SUPRIMER LES 3 LIGNES UNE FOIS LES TESTS FINI
        //easyDialogueManager.OnDialogueStarted += (EasyDialogueGraph graph, DialogueLine dl) => { Debug.Log($"{dl.Text} was said by {dl.Character}"); };
        //easyDialogueManager.OnDialogueProgressed += (EasyDialogueGraph graph, DialogueLine dl) => { Debug.Log($"{dl.Text} was said by {dl.Character}"); };
        //easyDialogueManager.OnDialogueEnded += (EasyDialogueGraph graph) => Debug.Log($"Dialogue ended on graph {graph.name}");

        InitializeDialogue();
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(currentGraph) {
                // CHANGER LA FACON DE PASSER UN DIALOGUE
                GetNextDialogue(-1);
            } else {
                // CHANGER LA FACON DE COMMENCER UN DIALOGUE
                StartDialogueEncounter(ref graphToPlay);
            }
        }
    }

    public void StartDialogueEncounter(ref EasyDialogueGraph dialogueGraph) {
        currentGraph = dialogueGraph;
        DialogueLine dialogue = easyDialogueManager.StartDialogueEncounter(ref dialogueGraph);
        DisplayDialogue(ref dialogue);
    }

    public void GetNextDialogue(int choiceIndex = 0) {
        if(!currentGraph) return;
        
        //On est obligé de choisir une réponse s'il y en a
        if(currentGraph.HasPlayerResponses() && choiceIndex == -1) {
            return;
        }
        
        //DIT SI C'EST UNE BONNE OU UNE MAUVAISE RÉPONSE
        //Renvoie un enum AnswerType (None, Good, Bad) [Cf Assets/EasyDialogue/UserFacingAPI/Structs.cs]
        Debug.Log("GoodAnswer: " + currentGraph.IsGoodAnswer(choiceIndex));
        
        //PERMET DE GARDER EN MEMOIRE LES CHOIX DE L'UTILISATEUR
        //Renvoie une chaine de caractère (Il suffit de rechercher si cette chaine se trouve dans la list memory, cela marche donc comme un boolean)
        string text = currentGraph.HaveIdentifiantMemory(choiceIndex);
        Debug.Log("Souvenir: " + text);
        if(!string.IsNullOrEmpty(text)) {
            memory.Add(text);
        }
        
        if(easyDialogueManager.GetNextDialogue(ref currentGraph, out DialogueLine dialogue, (ushort)choiceIndex)) {
            DisplayDialogue(ref dialogue);
        } else {
            EndDialogue();
        }
    }

    public void EndDialogue() {
        if(easyDialogueManager.EndDialogueEncounter(ref currentGraph)) {
            InitializeDialogue();
        }
    }

    private void InitializeDialogue() {
        currentGraph = null;
        myCanvas.enabled = false;
        HidePlayerResponses();
    }

    private void DisplayDialogue(ref DialogueLine dialogue) {
        ShowCharacterDialogue(dialogue.Character, dialogue.Text);
        HidePlayerResponses();
        if(dialogue.HasPlayerResponses()) {
            ShowPlayerResponses(dialogue.PlayerResponces);
        } else {
            HidePlayerResponses();
        }
    }

    private void ShowCharacterDialogue(Character character, string text) {
        myCanvas.enabled = true;
        textBox.text = text;
        textBoxName.text = character.displayName;
    }

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
