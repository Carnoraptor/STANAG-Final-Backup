using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileButtonHandler : MonoBehaviour
{
    //[SerializeField] TileBase item;
    Button button;
    
    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
    }

    void ButtonClicked()
    {
        //Debug.Log("Button was clicked: " + item.name);
    }
}
