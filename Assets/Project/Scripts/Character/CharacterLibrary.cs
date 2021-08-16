using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreGame
{
    [CreateAssetMenu(fileName = "CharacterLibrary", menuName = "CoreGame/ScriptableObject/Create Character Library")]
    public class CharacterLibrary : ScriptableObject
    {
        [SerializeField] private List<CharacterData> characters;
    }
}
