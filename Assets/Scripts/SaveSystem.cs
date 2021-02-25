using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(PlayerController player)
    {
        BinaryFormatter formatter = new  BinaryFormatter();
        string path = Application.persistentDataPath + "/player.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.fun";

        if(File.Exists(path))
        {
            BinaryFormatter formatter = new  BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else {
            Debug.Log("Not found!");
            return null;
        }
    }


    public static void SaveMainMenu(MainMenu mainMenu)
    {
        BinaryFormatter formatter = new  BinaryFormatter();
        string path = Application.persistentDataPath + "/menu.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        MainMenuData menu = new MainMenuData(mainMenu);

        formatter.Serialize(stream, menu);
        stream.Close();
    }

    public static MainMenuData LoadMainMenu()
    {
        string path = Application.persistentDataPath + "/menu.fun";

        if(File.Exists(path))
        {
            BinaryFormatter formatter = new  BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            MainMenuData menu = formatter.Deserialize(stream) as MainMenuData;
            stream.Close();

            return menu;
        }
        else {
            Debug.Log("Not found!");
            return null;
        }
    }
}
