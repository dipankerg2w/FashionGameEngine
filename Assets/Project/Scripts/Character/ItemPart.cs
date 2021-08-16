using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreGame
{
    public class ItemPart : MonoBehaviour
    {
        [SerializeField] private Part[] parts;

        /// <summary>
        /// Enable or Disable Part SpriteRenderer Image to show or hide
        /// </summary>
        /// <param name="value"></param>
        public void EnableItem(bool value)
        {
            int length = parts.Length;
            for (int i = 0; i < length; i++)
            {
                parts[i].enabledSpriteRenderer = value;
            }
        }
    }
}
