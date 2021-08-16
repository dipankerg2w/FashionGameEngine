using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreGame
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private string characterId;

        [Header("DEFAULT ITEMS Ref:")]
        [SerializeField] private ItemPart defaultSkin;
        [SerializeField] private ItemPart defaultHair, defaultEye, defaultLips;
        [SerializeField] private ItemPart defaultShoes;
        [SerializeField] private ItemPart defaultUndergarmetTop, defaultUndergarmetBottom;

        private List<ItemPart> defaultItems;

        void Awake()
        {
            InitDefaultItems();
        }

        private void InitDefaultItems()
        {
            defaultItems = new List<ItemPart>();

            defaultItems.Add(defaultSkin);
            defaultItems.Add(defaultHair);
            defaultItems.Add(defaultEye);
            defaultItems.Add(defaultLips);
            defaultItems.Add(defaultShoes);
            defaultItems.Add(defaultUndergarmetBottom);
            defaultItems.Add(defaultUndergarmetTop);
        }

        private void ShowDefaultItems()
        {
            int count = defaultItems.Count;
            for (int i = 0; i < count; i++)
            {
                defaultItems[i].EnableItem(true);
            }
        }
    }
}
