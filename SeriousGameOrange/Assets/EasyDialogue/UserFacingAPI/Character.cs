using UnityEngine;

namespace EasyDialogue {
    [CreateAssetMenu(fileName = "EasyDialogueCharacter", menuName = "EasyDialogue/Character")]
    public class Character : ScriptableObject {
        public string displayName;
    }
}