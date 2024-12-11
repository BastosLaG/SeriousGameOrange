using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyDialogue
{
    public interface LocalGraphContext
    {
        string Evaluate(ref string _ogDialogue);
    }
}