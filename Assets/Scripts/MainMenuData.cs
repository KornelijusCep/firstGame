using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MainMenuData
{
    public bool isGameCreated;

    public MainMenuData( MainMenu menu)
    {
        isGameCreated = menu.isGameCreated;
    }  
}
