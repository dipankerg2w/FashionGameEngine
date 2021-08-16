using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreGame
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "CoreGame/ScriptableObject/Create CharacterData")]
    public class CharacterData : ScriptableObject
    {
        [SerializeField] private string characterId;
        [SerializeField] private string characterName;
        [SerializeField] private Character characterPrefab;
    }
}
