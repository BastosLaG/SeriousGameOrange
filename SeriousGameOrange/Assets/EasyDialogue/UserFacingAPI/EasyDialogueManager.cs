using UnityEngine;
//NE SURTOUT PAS TOUCHER A CE CODE UNE FOIS LES DIALOGUES FAIT, CELA LES SUPPRIMERA TOUS !!!
//S'il y a des erreurs, on fera avec ou VOUS vous amusez à recréer tous les dialogues

namespace EasyDialogue {
    public class EasyDialogueManager : MonoBehaviour {
        [SerializeField] private Character defaultNullCharacter;

        public delegate void DialogueGraphLineDelegate(EasyDialogueGraph graph, DialogueLine line);
        public delegate void DialogueGraphDelegate(EasyDialogueGraph graph);

        public event DialogueGraphLineDelegate OnDialogueStarted;
        public event DialogueGraphLineDelegate OnDialogueProgressed;
        public event DialogueGraphDelegate OnDialogueEnded;


        public DialogueLine StartDialogueEncounter(ref EasyDialogueGraph graph) {
            Debug.Assert(graph, "Sent in a null dialogue graph!");
            graph.InitializeGraph();
            DialogueLine result = graph.GetCurrentDialogueLine();
            result = UpdateDialogueLine(result);
            OnDialogueStarted?.Invoke(graph, result);
            return result;
        }

        public bool GetNextDialogue(ref EasyDialogueGraph graph, out DialogueLine outLine, ushort dialogueChoice = 0) {
            bool result = false;
            Debug.Assert(graph, "Sent in a null dialogue graph!");
            outLine = new DialogueLine();
            outLine.Text = "";

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

        public DialogueLine UpdateDialogueLine(DialogueLine dialogueLine) {
            DialogueLine result = dialogueLine;
            if (!result.Character) {
                result.Character = defaultNullCharacter;
            }

            result.Text = dialogueLine.Text;
            if (result.HasPlayerResponses()) {
                for (int i = 0; i < dialogueLine.PlayerResponces.Length; ++i) {
                    result.PlayerResponces[i] = dialogueLine.PlayerResponces[i];
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