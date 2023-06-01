using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class for storing a resource from six unit in the game.
[Serializable]
public class Cost
{
    public double credit;
    public double Credit { get { return credit; } set { credit = value; } }

    public double metal;
    public double Metal { get { return metal; } set { metal = value; } }

    public double titan;
    public double Titan { get { return titan; } set { titan = value; } }

    public double plant;
    public double Plant { get { return plant; } set { plant = value; } }

    public double energy;
    public double Energy { get { return energy; } set { energy = value; } }

    public double heat;
    public double Heat { get { return heat; } set { heat = value; } }

    public Cost(double credit, double metal, double titan, double plant, double energy, double heat)
    {
        Credit = credit;
        Metal = metal;
        Titan = titan;
        Plant = plant;
        Energy = energy;
        Heat = heat;
    }

    //Adding a Cost object to another.
    public void Add(Cost toAdd)
    {
        Credit += toAdd.Credit;
        Metal += toAdd.Metal;
        Titan += toAdd.Titan;
        Plant += toAdd.Plant;
        Energy += toAdd.Energy;
        Heat += toAdd.Heat;
    }
}
