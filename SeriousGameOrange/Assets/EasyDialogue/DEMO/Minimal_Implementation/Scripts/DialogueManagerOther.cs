﻿using UnityEngine;

namespace EasyDialogue {
    /// <summary>
    /// The only script in the Minimal_Implementation Scene, this houses all interactions with the user and EasyDialogue System
    /// </summary>
    public class DialogueManagerOther : MonoBehaviour {

        #region Attributes

        [SerializeField] private string textBox;
        [SerializeField] private string characterName;
        [SerializeField] private string[] playerChoices;
        [SerializeField] EasyDialogue.EasyDialogueGraph graphToPlay;

        [SerializeField] bool startWithOverrideCharacter;
        [SerializeField, Tooltip("This will only be used if \"startWithOverrideCharacter\" is ticked on")]
        Character overrideCharacter;


        private EasyDialogueGraph currentGraph;
        public bool HasDialogueGraph() => currentGraph != null;
        private EasyDialogueManager easyDialogueManager;
        private Canvas myCanvas;

        #endregion

        #region Setup And Input

        private void Start() {
            myCanvas = GetComponent<Canvas>();
            easyDialogueManager = FindObjectOfType<EasyDialogueManager>();
            easyDialogueManager.OnDialogueStarted += (EasyDialogueGraph _graph, dialogue_line _dl) => { Debug.Log($"{_dl.text} was said by {_dl.character}"); };
            easyDialogueManager.OnDialogueProgressed += OnDialogueProgressedHandler;
            easyDialogueManager.OnDialogueEnded += (EasyDialogueGraph _graph) => Debug.Log($"Dialogue ended on graph {_graph.name}");
            InitializeDialogue();
        }

        private void OnDialogueProgressedHandler(EasyDialogueGraph _graph, dialogue_line _line) {
            Debug.Log($"{_line.text} was said by {_line.character}");
        }

        /// <summary>
        /// Handle user input
        /// </summary>
        void Update() {
            //Next Dialogue (Also starts an encounter)
            if(Input.GetKeyDown(KeyCode.Space)) {
                if(HasDialogueGraph()) {
                    //Progress Dialogue if no player response, or select response 1.
                    GetNextDialogue();
                } else {
                    StartDialogueEncounter(ref graphToPlay);
                }
            }
        }

        #endregion

        #region Main Functionality

        /// <summary>
        /// Called to start the dialogue with the given graph.
        /// </summary>
        /// <param name="_dialogueGraph"></param>
        public void StartDialogueEncounter(ref EasyDialogueGraph _dialogueGraph) {
            currentGraph = _dialogueGraph;
            if(startWithOverrideCharacter) {
                currentGraph.AddOverrideCharacter(ref overrideCharacter);
            }
            dialogue_line dialogue = easyDialogueManager.StartDialogueEncounter(ref _dialogueGraph);
            DisplayDialogue(ref dialogue);
        }

        /// <summary>
        /// Get's the next dialogue, from the 1,2,3 inputs as well as button clicks.
        /// </summary>
        /// <param name="_choiceIndex"></param>
        public void GetNextDialogue(int _choiceIndex = 0) {
            if(!HasDialogueGraph()) return;
            dialogue_line dialogue;
            if(easyDialogueManager.GetNextDialogue(ref currentGraph, out dialogue, (ushort)_choiceIndex)) {
                DisplayDialogue(ref dialogue);
            } else {
                InitializeDialogue();
            }
        }

        /// <summary>
        /// Abruptly ends or kills the current dialogue session.
        /// </summary>
        public void EndDialogue() {
            if(easyDialogueManager.EndDialogueEncounter(ref currentGraph)) {
                InitializeDialogue();
            }
        }

        #endregion

        //All of the following functions are for the presentation and setting of UI.

        #region Helper Functions

        private void InitializeDialogue() {
            currentGraph = null;
            textBox = "Initialized text box, please start a dialogue";
            characterName = "The mystical asset creator";
            myCanvas.enabled = false;
            for(int i = 0; i < playerChoices.Length; ++i) {
                playerChoices[i] = "Tmp player choice";
            }
            HidePlayerResponses();
        }

        private void DisplayDialogue(ref dialogue_line _dialogue) {
            ShowCharacterDialogue(_dialogue.character, _dialogue.text);
            if(_dialogue.HasPlayerResponses()) {
                ShowPlayerResponses(_dialogue.playerResponces);
            } else {
                HidePlayerResponses();
            }
        }

        private void ShowCharacterDialogue(Character _character, string _text) {
            myCanvas.enabled = true;
            textBox = _text;
            characterName = _character.displayName;
        }

        private void HidePlayerResponses() {
            for(int i = 0; i < playerChoices.Length; ++i) { ;
                //playerChoices[i].transform.parent.parent.gameObject.SetActive(false);
            }
        }

        private void ShowPlayerResponses(string[] responses) {
            for(int i = 0; i < responses.Length; ++i) {
                playerChoices[i] = responses[i];
                //playerChoices[i].transform.parent.parent.gameObject.SetActive(true);
            }
        }

        #endregion

    }
}