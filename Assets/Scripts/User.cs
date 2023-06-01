using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class for storing the information of the User of the game.
[Serializable]
public class User
{
    public string name;
    public string Name { get { return name; } set { name = value; } }

    //User's resources in the game.
    public Cost bank;
    public Cost Bank { get { return bank; } set { bank = value; } }

    //User's incomes in the game.
    public Cost incomes;
    public Cost Incomes { get { return incomes; } set { incomes = value; } }

    public User(string name, Cost bank)
    {
        Name = name;
        Incomes = new Cost(20, 0, 0, 0, 0, 0);
        Bank = bank;
    }
}
