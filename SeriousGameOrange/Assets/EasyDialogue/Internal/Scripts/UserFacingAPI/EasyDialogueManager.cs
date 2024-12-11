using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyDialogue {
    /// <summary>
    /// The main interface between the user and the EasyDialogue system
    /// </summary>
    public class EasyDialogueManager : MonoBehaviour {
        [SerializeField] private Character playerCharacter;
        [SerializeField] private Character defaultNullCharacter;
        [SerializeField] private StringStringDictionary dynamicTextReplacers;

        public delegate void DialogueGraphLineDelegate(EasyDialogueGraph _graph, dialogue_line _line);

        public delegate void DialogueGraphDelegate(EasyDialogueGraph _graph);

        public event DialogueGraphLineDelegate OnDialogueStarted;
        public event DialogueGraphLineDelegate OnDialogueProgressed;
        public event DialogueGraphDelegate OnDialogueEnded;


        /// <summary>
        /// Called to start a dialogue encounter with a given graph.
        /// </summary>
        /// <param name="_graph">Graph to start dialogue in.</param>
        /// <returns></returns>
        public dialogue_line StartDialogueEncounter(ref EasyDialogueGraph _graph) {
            dialogue_line result = new dialogue_line();
            Debug.Assert(_graph != null, "Sent in a null dialogue graph!");
            _graph.InitializeGraph();
            result = _graph.GetCurrentDialogueLine();
            result = UpdateDialogueLine(result);
            OnDialogueStarted?.Invoke(_graph, result);
            return result;
        }

        /// <summary>
        /// Gets the next line of dialogue given a player choise index, when no choices are avalible, 0 is the next choice.
        /// </summary>
        /// <param name="_graph">Graph to get the next dialogue in</param>
        /// <param name="_outLine">struct to fill with dialogue info</param>
        /// <param name="dialogueChoice">choice to get text about</param>
        /// <returns></returns>
        public bool GetNextDialogue(ref EasyDialogueGraph _graph, out dialogue_line _outLine,
            ushort dialogueChoice = 0) {
            bool result = false;
            Debug.Assert(_graph != null, "Sent in a null dialogue graph!");
            _outLine = new dialogue_line();
            _outLine.text = "";

            if (_graph.GoToNextNode(dialogueChoice)) {
                _outLine = _graph.GetCurrentDialogueLine();
                _outLine = UpdateDialogueLine(_outLine);
                OnDialogueProgressed?.Invoke(_graph, _outLine);
                result = true;
            } else {
                EndDialogueEncounter(ref _graph);
            }

            return result;
        }

        /// <summary>
        /// Called to end the current dialogue encounter.
        /// </summary>
        /// <param name="_graph"></param>
        /// <returns></returns>
        public bool EndDialogueEncounter(ref EasyDialogueGraph _graph) {
            Debug.Assert(_graph != null, "Sent in a null dialogue graph!");
            OnDialogueEnded?.Invoke(_graph);
            return true;
        }

        /// <summary>
        /// Used to replace any loop-up text, and make sure that characters are set properly.
        /// </summary>
        /// <param name="_dialogueLine"></param>
        /// <returns></returns>
        public dialogue_line UpdateDialogueLine(dialogue_line _dialogueLine) {
            dialogue_line result = _dialogueLine;
            if (result.character == null) {
                result.character = defaultNullCharacter;
            }

            result.text = ReplaceDyanmicText(_dialogueLine.text, _dialogueLine.character);
            if (result.HasPlayerResponses()) {
                for (int playerResponseIndex = 0;
                     playerResponseIndex < _dialogueLine.playerResponces.Length;
                     ++playerResponseIndex) {
                    result.playerResponces[playerResponseIndex] =
                        ReplaceDyanmicText(_dialogueLine.playerResponces[playerResponseIndex], _dialogueLine.character);
                }
            }

            return result;
        }

        private string ReplaceDyanmicText(string _text, Character _character) {
            foreach (string key in dynamicTextReplacers.Keys) {
                _text = _text.Replace(key, dynamicTextReplacers[key]);
            }

            return _text;
        }
    }
}