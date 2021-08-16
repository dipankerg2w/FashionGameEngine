using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EditorTools.ContentsItem
{
    [System.Serializable]
    public class ItemData
    {
        public string itemName { get; private set; }
        public string filePath { get; private set; }
        public string thumbnailPath { get; private set; }
        public string referencePath { get; private set; }
        public string character { get; private set; }
        public CoreGame.Items.Category.Type categoryType { get; private set; }
        public CoreGame.Theme.Type themeType { get; private set; }
        public string itemSOFilePath { get; private set; }
        
        public string GetAssetPath => filePath.Replace(Path.GetFullPath(Application.dataPath), "Assets");
        public string GetThumbnailAssetPath => thumbnailPath.Replace(Path.GetFullPath(Application.dataPath), "Assets");
        public string GetSoAssetPath => itemSOFilePath.Replace(Path.GetFullPath(Application.dataPath), "Assets");
        public string GetReferenceAssetPath => referencePath.Replace(Path.GetFullPath(Application.dataPath), "Assets");


        public ItemData(string filename, string filePath)
        {
            this.itemName = filename.ToLower();
            this.filePath = filePath;
            this.categoryType = GetCategoryType(itemName);
        }

        public void UpdateData(string thumbnailPath, string characterId, string theme)
        {
            this.thumbnailPath = thumbnailPath;
            this.character = characterId;
            this.themeType = GetThemeType(theme);
        }

        public void UpdateSoFilePath(string folderPath)
        {
            itemSOFilePath = $"{folderPath}/{itemName}.asset";
        }

        public static CoreGame.Items.Category.Type GetCategoryType(string itemName)
        {
            string nameLower = itemName.ToLower();
            if(nameLower.Contains("accessory_"))
            {
                return CoreGame.Items.Category.Type.Accessory;
            }
            else if (nameLower.Contains("bag_"))
            {
                return CoreGame.Items.Category.Type.Bag;
            }
            else if (nameLower.Contains("bg_"))
            {
                return CoreGame.Items.Category.Type.BG;
            }
            else if (nameLower.Contains("bottom_"))
            {
                return CoreGame.Items.Category.Type.Bottom;
            }
            else if (nameLower.Contains("dress_"))
            {
                return CoreGame.Items.Category.Type.Dress;
            }
            else if (nameLower.Contains("eyes_"))
            {
                return CoreGame.Items.Category.Type.Eyes;
            }
            else if (nameLower.Contains("hair_"))
            {
                return CoreGame.Items.Category.Type.Hair;
            }
            else if (nameLower.Contains("jewellery_"))
            {
                return CoreGame.Items.Category.Type.Jewellery;
            }
            else if (nameLower.Contains("lips_"))
            {
                return CoreGame.Items.Category.Type.Lips;
            }
            else if (nameLower.Contains("prop_"))
            {
                return CoreGame.Items.Category.Type.Prop;
            }
            else if (nameLower.Contains("shoes_"))
            {
                return CoreGame.Items.Category.Type.Shoes;
            }
            else if (nameLower.Contains("skintone_"))
            {
                return CoreGame.Items.Category.Type.Skintone;
            }
            else if (nameLower.Contains("swimsuit_"))
            {
                return CoreGame.Items.Category.Type.Swimsuit;
            }
            else if (nameLower.Contains("top_"))
            {
                return CoreGame.Items.Category.Type.Top;
            }
            else
            {
                return CoreGame.Items.Category.Type.None;
            }
        }

        public static CoreGame.Theme.Type GetThemeType(string theme)
        {
            switch (theme.ToLower())
            {
                case "all":         return CoreGame.Theme.Type.All;
                case "fantasy":     return CoreGame.Theme.Type.Fantasy;
                case "bohemian":    return CoreGame.Theme.Type.Bohemian;
                case "vintage":     return CoreGame.Theme.Type.Vintage;
                case "modern":      return CoreGame.Theme.Type.Modern;
                case "prewedding":  return CoreGame.Theme.Type.Prewedding;
                case "sporty":      return CoreGame.Theme.Type.Sporty;
                case "shop":        return CoreGame.Theme.Type.Shop;

                default:            return CoreGame.Theme.Type.None;
            }
        }
    }
}
