using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CoreGame.Items
{
    public class ItemCatalogCofig : ScriptableObject
    {
#if UNITY_EDITOR
        public const string AssetPath = "Assets/Project/Data/GameSettings/ItemCatalogCofig.asset";
#endif

        [SerializeField] private List<ItemCatalogData> items = new List<ItemCatalogData>();

        public List<ItemCatalogData> Items => items;

        public void AddItemCatalogEntry(CoreGame.Items.ItemData itemData)
        {
            if(items == null)
            {
                items = new List<ItemCatalogData>();
            }

            ItemCatalogData itemCatalogData = items.Find(arg => arg.CategoryType == itemData.CategoryType);
            if(itemCatalogData == null)
            {
                itemCatalogData = new ItemCatalogData(itemData);
                items.Add(itemCatalogData);
            }
            else
            {
                itemCatalogData.UpdateItemCatalogData(itemData);
            }
        }
    }

    [System.Serializable]
    public class ItemCatalogData
    {
        [SerializeField] private string itemName;
        [SerializeField] private string characterId;
        [SerializeField] private CoreGame.Items.Category.Type categoryType;
        [SerializeField] private CoreGame.Theme.Type themeType;

        #region ADDRESSABLE
        
        [Header("Addressable")]
        public AssetReference assetAddress;
        public string labelName;
        public bool isLocal;

        #endregion ADDRESSABLE

        public string ItemName => itemName;
        public string CharacterId => characterId;
        public CoreGame.Items.Category.Type CategoryType => categoryType;
        public CoreGame.Theme.Type ThemeType => themeType;

        public ItemCatalogData(CoreGame.Items.ItemData itemData)
        {
            this.itemName = itemData.ItemName;
            this.characterId = itemData.CharacterId;
            this.categoryType = itemData.CategoryType;
            this.themeType = itemData.ThemeType;
        }

        public void UpdateItemCatalogData(CoreGame.Items.ItemData itemData)
        {
            this.itemName = itemData.ItemName;
            this.characterId = itemData.CharacterId;
            this.categoryType = itemData.CategoryType;
            this.themeType = itemData.ThemeType;
        }

        public void AddAssetReference(AssetReference address, ItemData asset)
        {
            assetAddress = address;
        }
    }

    [System.Serializable]
    public class ItemCatalogEntry
    {
        [SerializeField] private CoreGame.Items.Category.Type categoryType;
        [SerializeField] private List<ItemCatalogData> itemCatalogDatas;

        public CoreGame.Items.Category.Type CategoryType => categoryType;
        public List<ItemCatalogData> ItemCatalogDatas => itemCatalogDatas;

        public ItemCatalogEntry(CoreGame.Items.Category.Type categoryType, ItemCatalogData itemCatalogData)
        {
            this.categoryType = categoryType;
            itemCatalogDatas = new List<ItemCatalogData>();
            this.itemCatalogDatas.Add(itemCatalogData);
        }

        public void AddItemCatalogData(CoreGame.Items.ItemData itemData)
        {
            ItemCatalogData itemCatalogData = itemCatalogDatas.Find(arg => arg.ItemName == itemData.ItemName);
            if(itemCatalogData == null)
            {
                itemCatalogDatas.Add(new ItemCatalogData(itemData));
            }
            else
            {
                itemCatalogData = new ItemCatalogData(itemData);
            }
        }

        
    }
}
