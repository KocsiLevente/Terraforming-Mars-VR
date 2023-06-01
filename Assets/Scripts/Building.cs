using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class for storing a Building in the game. It is a base class and should be extended in other classes.
[Serializable]
public class Building
{
    public int id;
    public int Id { get { return id; } set { id = value; } }

    public Buildings type;
    public Buildings Type { get { return type; } set { type = value; } }

    //The Hexagon under the Building.
    [NonSerialized]
    private Hexagon hexagon;
    public Hexagon Hexagon
    {
        get
        {
            return hexagon;
        }
        set
        {
            hexagon = value;
        }
    }

    public Building(int id, Hexagon hexagon, Buildings type)
    {
        Id = id;
        Hexagon = hexagon;
        Type = type;
    }

    //Virtual endscore funcion that will be overridden by the extended classes.
    public virtual int GetScore() { return 0; }
}

//Enum for storing the type of Building.
[Serializable]
public enum Buildings
{
    PowerPlant = 0, Greenery = 1, City = 2, Ocean = 3
}
