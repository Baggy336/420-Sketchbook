using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;

enum Location 
{ 
    Home,
    Cliffs,
    Castle
}


[Serializable]
public class SaveData : ISerializable
{
    public float playerHealth;

    public int playerLocation;

    public string playerName;

 
    SaveData(SerializationInfo info, StreamingContext context)
    {
        playerHealth = info.GetSingle("playerHealth");
        playerLocation = info.GetInt32("playerLocation");
        playerName = info.GetString("playerName");

    }
    
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("playerHealth", playerHealth);
        info.AddValue("playerLocation", playerLocation);
        info.AddValue("playerName", playerName);
    }
}
