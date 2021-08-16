using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreGame
{
    public class Part : MonoBehaviour
    {
        public enum Type
        {
            MAIN,
            LEFT,
            RIGHT
        }

        [SerializeField] private SpriteRenderer spriteHolder;
        [SerializeField] private Type type;

        /// <summary>
        /// Makes the SpriteRenderer object visible if enabled.
        /// </summary>
        /// <value>Pass True to visible else false </value>
        public bool enabledSpriteRenderer
        {
            get => spriteHolder.enabled;
            set => spriteHolder.enabled = value;
        }
    }
}
