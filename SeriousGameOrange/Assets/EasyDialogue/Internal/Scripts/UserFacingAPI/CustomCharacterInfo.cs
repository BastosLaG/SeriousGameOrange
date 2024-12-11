

namespace EasyDialogue
{
    /// <summary>
    /// You want characters to have customized information, but you don't want to write it directly into the Character class.
    /// This is because, if I update the class in the future, your stuff will be overwritten!
    /// Here is a safe space to place any custom information that you care about, and you can put it right into your characters. And I won't update it!
    /// </summary>
    [System.Serializable]
    public class CustomCharacterInfo
    {
    }
}