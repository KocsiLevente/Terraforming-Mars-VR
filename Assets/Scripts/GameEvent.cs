using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class for storing a GameEvent that the user can buy.
[Serializable]
public class GameEvent
{
    public int id;
    public int Id { get { return id; } set { id = value; } }

    //The GameEvent originates from a Hexagon's position.
    public int hexagonId;
    public int HexagonId { get { return hexagonId; } set { hexagonId = value; } }

    public EventTypes type;
    public EventTypes Type { get { return type; } set { type = value; } }

    public GameObjectToSave eventToSave;
    public GameObjectToSave EventToSave { get { return eventToSave; } set { eventToSave = value; } }

    [NonSerialized]
    private GameObject eventObject;
    public GameObject EventObject
    {
        get
        {
            return eventObject;
        }
        set
        {
            eventObject = value;
        }
    }

    public GameEvent(int id, int hId, GameObject eventObject, EventTypes type)
    {
        Id = id;
        HexagonId = hId;
        EventObject = eventObject;
        Type = type;

        //Setting up GameObjectToSave from normal GameObject.
        if (eventObject != null)
        {
            EventToSave = new GameObjectToSave(eventObject.transform.position.x, eventObject.transform.position.y, eventObject.transform.position.z);
        }
    }

    public virtual int GetScore() { return 0; }
}

//Enum for types of a GameEvent object
[Serializable]
public enum EventTypes
{
    SpaceMine = 0, Satellite = 1, SpaceStation = 2
}
