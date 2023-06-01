using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class for storing a PowerPlant type object in the game.
[Serializable]
public class PowerPlant : Building
{
    public PowerPlant(int id, Hexagon hexagon, Buildings type) : base(id, hexagon, type)
    {
        
    }

    //Returning the endscore value of this object.
    public override int GetScore()
    {
        return 1;
    }
}
