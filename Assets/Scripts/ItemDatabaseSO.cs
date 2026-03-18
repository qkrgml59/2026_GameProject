using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/DatabaseSO")]
public class ItemDatabaseSO : ScriptableObject
{
    public List<ItemSO> items = new List<ItemSO>();                      //ItemSO를 리스트로 관리

    //캐싱을 위한 DIctrionary

    private Dictionary<int, ItemSO> itemsById;                        //ID로 아이템 찾기 위한 캐싱
    private Dictionary<string, ItemSO> itemsByName;                   //이름으로 아이템 찾기

    public void Initialze()
    {
        itemsById = new Dictionary<int, ItemSO>();                         //위에 선언만 했기 때문에 할당
        itemsByName = new Dictionary<string, ItemSO>();                    

        foreach(var item in items)
        {
            itemsById[item.id] = item;
            itemsByName[item.ItemName] = item;
        }
    }

    //ID로 아이템 찾기

    public ItemSO GetItemById(int id)
    {
        if(itemsById == null)
        {
            Initialze();
        }
        if(itemsById.TryGetValue(id, out ItemSO item))
        {
            return item;
        }

        return null;
    }

    public ItemSO GetItemByName(string name)
    {
        if (itemsByName == null)
        {
            Initialze();
        }
        if (itemsByName.TryGetValue(name, out ItemSO item))
            return item;

        return null;
    }

    //타입으로 아이템 필터링
    public List<ItemSO> GetItemByType(ItemType type)
    {
        return items.FindAll(item => item.itemType == type);
    }

}
