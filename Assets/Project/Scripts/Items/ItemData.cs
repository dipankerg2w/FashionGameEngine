using UnityEngine;

namespace CoreGame.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "CoreGame/ScriptableObject/Create ItemData")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private string itemName;
        [SerializeField] private string characterId;
        [SerializeField] private Theme.Type themeType;
        [SerializeField] private Category.Type categoryType;
        [SerializeField] private Sprite item, thumbnail;

        public string ItemName => itemName;
        public string CharacterId => characterId;
        public Theme.Type ThemeType => themeType;
        public Category.Type CategoryType => categoryType;
        public Sprite ItemImage => item;
        public Sprite ItemThumbnail => thumbnail;

        public void UpdateData(string itemName, string characterId, Theme.Type themeType, Category.Type categoryType)
        {
            this.itemName       = itemName;
            this.characterId    = characterId;
            this.themeType      = themeType;
            this.categoryType   = categoryType;
        }

        public void UpdateSprite(Sprite item, Sprite thumbnail)
        {
            if (item != null)
            {
                this.item = item;
            }
            if(thumbnail != null)
            {
                this.thumbnail = thumbnail;
            }
        }
    }
}