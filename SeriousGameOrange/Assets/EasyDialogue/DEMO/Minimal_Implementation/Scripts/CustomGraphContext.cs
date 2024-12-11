using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyDialogue.Samples
{
    public enum ItemType
    {
        Consumable = 1 << 0,
        Throwable = 1 << 1,
        Equippable = 1 << 2,
        Placeable = 1 << 3
    }

    public struct item_data
    {
        public string id;
        public string displayName;
        public int type;

        public item_data(string _id, string _name, params ItemType[] _itemTypes)
        {
            id = _id;
            displayName = _name;
            type = 0;

            for(int i = 0; i < _itemTypes.Length; ++i)
            {
                AddType(_itemTypes[i]);
            }
        }

        #region Methods

        [System.Diagnostics.Contracts.Pure]
        public bool IsType(ItemType _type)
        {
            return (type & (int)_type) != 0;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void AddType(ItemType _type)
        {
            type |= (int)_type;
        }
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void RemoveType(ItemType _type)
        {
            type &= ~(int)_type;
        }

        public string TypeString()
        {
            if (IsType(ItemType.Consumable))
            {
                return "consumable";
            }
            else if (IsType(ItemType.Equippable))
            {
                return "equippable";
            }
            else if (IsType(ItemType.Placeable))
            {
                return "placeable";
            }
            else if (IsType(ItemType.Throwable))
            {
                return "throwable";
            }
            else
            {
                return "nothing";
            }
        }

        #endregion
    }

    public class CustomGraphContext : LocalGraphContext
    {
        public item_data heldItem = new item_data();

        private readonly string ItemNameLookupString = "{ItemName}";
        private readonly string ItemTypeLookupString = "{ItemType}";

        public string Evaluate(ref string _ogDialogue)
        {
            string result = _ogDialogue;
            if (heldItem.displayName != null)
            {
                result = result.Replace(ItemNameLookupString, heldItem.displayName);
                result = result.Replace(ItemTypeLookupString, heldItem.TypeString());
            }
            return result;
        }
    }
}