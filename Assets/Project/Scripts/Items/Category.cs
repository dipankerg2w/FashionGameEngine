using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreGame.Items
{
    [System.Serializable]
    public class Category
    {
        public enum Type
        {
            None,
            Eyes, Lips, Hair, Skintone,
            Dress, Top, Bottom, Swimsuit, Shoes,
            Bag, Accessory, Jewellery, Prop,
            BG
        }

        private List<ItemData> items = new List<ItemData>();
    }
}
