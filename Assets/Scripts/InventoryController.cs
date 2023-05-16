using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Item _item;
    [SerializeField] private Button _button;

    public void RemoveAndAddItem()
    {
        InventoryManager.instance.Add(_item);

        Destroy(_button.gameObject);
    }
}
