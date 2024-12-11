using UnityEngine;

namespace EasyDialogue {
    public class EasyDialogueManager : MonoBehaviour {
        [SerializeField] private Character defaultNullCharacter;

        public delegate void DialogueGraphLineDelegate(EasyDialogueGraph graph, dialogue_line line);
        public delegate void DialogueGraphDelegate(EasyDialogueGraph graph);

        public event DialogueGraphLineDelegate OnDialogueStarted;
        public event DialogueGraphLineDelegate OnDialogueProgressed;
        public event DialogueGraphDelegate OnDialogueEnded;


        public dialogue_line StartDialogueEncounter(ref EasyDialogueGraph graph) {
            Debug.Assert(graph != null, "Sent in a null dialogue graph!");
            graph.InitializeGraph();
            dialogue_line result = graph.GetCurrentDialogueLine();
            result = UpdateDialogueLine(result);
            OnDialogueStarted?.Invoke(graph, result);
            return result;
        }

        public bool GetNextDialogue(ref EasyDialogueGraph _graph, out dialogue_line _outLine,
            ushort dialogueChoice = 0) {
            bool result = false;
            Debug.Assert(_graph != null, "Sent in a null dialogue graph!");
            _outLine = new dialogue_line();
            _outLine.text = "";

            if(_graph.GoToNextNode(dialogueChoice)) {
                _outLine = _graph.GetCurrentDialogueLine();
                _outLine = UpdateDialogueLine(_outLine);
                OnDialogueProgressed?.Invoke(_graph, _outLine);
                result = true;
            } else {
                EndDialogueEncounter(ref _graph);
            }

            return result;
        }
        
        public bool EndDialogueEncounter(ref EasyDialogueGraph _graph) {
            Debug.Assert(_graph != null, "Sent in a null dialogue graph!");
            OnDialogueEnded?.Invoke(_graph);
            return true;
        }

        public dialogue_line UpdateDialogueLine(dialogue_line dialogueLine) {
            dialogue_line result = dialogueLine;
            if(!result.character) {
                result.character = defaultNullCharacter;
            }

            result.text = dialogueLine.text;
            if(result.HasPlayerResponses()) {
                for(int i = 0; i < dialogueLine.playerResponces.Length; ++i) {
                    result.playerResponces[i] = dialogueLine.playerResponces[i];
                }
            }
            return result;
        }
    }
}
