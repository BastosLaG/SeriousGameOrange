using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyDialogue {
    /// <summary>
    /// Character can hold any data that you want to know about a character.
    /// By default I am only storing an id, a name, and a sprite.
    /// </summary>
    [CreateAssetMenu(fileName = "EasyDialogueCharacter", menuName = "EasyDialogue/Character")]
    public class Character : ScriptableObject {
        public string id;
        public string displayName;
    }
}