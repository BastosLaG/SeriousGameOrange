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
            Debug.Assert(graph, "Sent in a null dialogue graph!");
            graph.InitializeGraph();
            dialogue_line result = graph.GetCurrentDialogueLine();
            result = UpdateDialogueLine(result);
            OnDialogueStarted?.Invoke(graph, result);
            return result;
        }

        public bool GetNextDialogue(ref EasyDialogueGraph graph, out dialogue_line outLine, ushort dialogueChoice = 0) {
            bool result = false;
            Debug.Assert(graph, "Sent in a null dialogue graph!");
            outLine = new dialogue_line();
            outLine.text = "";

            if (graph.GoToNextNode(dialogueChoice)) {
                outLine = graph.GetCurrentDialogueLine();
                outLine = UpdateDialogueLine(outLine);
                OnDialogueProgressed?.Invoke(graph, outLine);
                result = true;
            } else {
                EndDialogueEncounter(ref graph);
            }

            return result;
        }

        public dialogue_line UpdateDialogueLine(dialogue_line dialogueLine) {
            dialogue_line result = dialogueLine;
            if (!result.character) {
                result.character = defaultNullCharacter;
            }

            result.text = dialogueLine.text;
            if (result.HasPlayerResponses()) {
                for (int i = 0; i < dialogueLine.playerResponces.Length; ++i) {
                    result.playerResponces[i] = dialogueLine.playerResponces[i];
                }
            }

            return result;
        }

        public bool EndDialogueEncounter(ref EasyDialogueGraph graph) {
            Debug.Assert(graph, "Sent in a null dialogue graph!");
            OnDialogueEnded?.Invoke(graph);
            return true;
        }
    }
}