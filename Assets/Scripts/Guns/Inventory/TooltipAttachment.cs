using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipAttachment : MonoBehaviour
{
    InventoryHandler inventoryHandler;

    [SerializeField] string slot;
    //[SerializeField] GameObject tooltip;

    void Start()
    {
        inventoryHandler = GameObject.Find("Inventory").GetComponent<InventoryHandler>();
    }

    public void OpenThis()
    {
        
    }

    public void CloseThis()
    {
        /*if (onCooldown == false)
        {
            StopAllCoroutines();
            Debug.Log("Closing " + tooltip.name);
            tooltip.SetActive(false);
        }*/
    }

    //Fix tooltips another day
}
