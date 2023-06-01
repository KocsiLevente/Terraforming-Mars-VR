using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class that represents a Hexagon on the planet (a unit on the map).
[Serializable]
public class Hexagon
{
    [Serializable]
    public class HexagonNeighbour
    {
        public int id = 1;
        public int Id { get { return id; } set { id = value; } }

        public HexagonDirections dir;
        public HexagonDirections Dir { get { return dir; } set { dir = value; } }

        public HexagonNeighbour(HexagonDirections dir, int id)
        {
            Id = id;
            Dir = dir;
        }
    }

    public int id = 1;
    public int Id { get { return id; } set { id = value; } }

    //The Building that stands on this Hexagon.
    public Building buildingModel;
    public Building BuildingModel { get { return buildingModel; } set { buildingModel = value; } }

    //Only for serialization.
    public List<HexagonNeighbour> neighboursToSave = new List<HexagonNeighbour>();
    public List<HexagonNeighbour> NeighboursToSave { get { return neighboursToSave; } set { neighboursToSave = value; } }

    [NonSerialized]
    public Dictionary<HexagonDirections, int> neighbours = new Dictionary<HexagonDirections, int>();
    public Dictionary<HexagonDirections, int> Neighbours { get { return neighbours; } set { neighbours = value; } }

    public GameObjectToSave territoryToSave;
    public GameObjectToSave TerritoryToSave { get { return territoryToSave; } set { territoryToSave = value; } }

    public GameObjectToSave buildingToSave;
    public GameObjectToSave BuildingToSave { get { return buildingToSave; } set { buildingToSave = value; } }

    [NonSerialized]
    private GameObject territory;
    public GameObject Territory
    {
        get
        {
            return territory;
        }
        set
        {
            territory = value;
        }
    }
    [NonSerialized]
    private GameObject building;
    public GameObject Building
    {
        get
        {
            return building;
        }
        set
        {
            building = value;
        }
    }

    public Hexagon(int id, GameObject territory, GameObject building = null, Building buildingModel = null)
    {
        Id = id;
        Territory = territory;
        Building = building;
        BuildingModel = buildingModel;

        //Setting up GameObjectToSave from normal GameObject.
        if (territory != null)
        {
            TerritoryToSave = new GameObjectToSave(territory.transform.position.x, territory.transform.position.y, territory.transform.position.z);
        }
        if (building != null)
        {
            BuildingToSave = new GameObjectToSave(building.transform.position.x, building.transform.position.y, building.transform.position.z);
        }

        //Default empty Neighbours;
        Neighbours.Add(HexagonDirections.Left, -1);
        Neighbours.Add(HexagonDirections.Right, -1);
        Neighbours.Add(HexagonDirections.UpLeft, -1);
        Neighbours.Add(HexagonDirections.UpRight, -1);
        Neighbours.Add(HexagonDirections.DownLeft, -1);
        Neighbours.Add(HexagonDirections.DownRight, -1);

        NeighboursToSave.Add(new HexagonNeighbour(HexagonDirections.Left, -1));
        NeighboursToSave.Add(new HexagonNeighbour(HexagonDirections.Right, -1));
        NeighboursToSave.Add(new HexagonNeighbour(HexagonDirections.UpLeft, -1));
        NeighboursToSave.Add(new HexagonNeighbour(HexagonDirections.UpRight, -1));
        NeighboursToSave.Add(new HexagonNeighbour(HexagonDirections.DownLeft, -1));
        NeighboursToSave.Add(new HexagonNeighbour(HexagonDirections.DownRight, -1));
    }

    //Setting the neighbour of a Hexagon.
    public void SetNeighbour(HexagonDirections dir, Hexagon hexagon)
    {
        if (hexagon != null)
        {
            Neighbours[dir] = hexagon.Id;
            HexagonNeighbour toRemove = NeighboursToSave.Find(n => n.Dir == dir);
            NeighboursToSave.Remove(toRemove);
            NeighboursToSave.Add(new HexagonNeighbour(dir, hexagon.id));
        }
    }
}

//Enum for directions of a Hexagon.
[Serializable]
public enum HexagonDirections
{
    Left = 0, Right = 1, UpLeft = 2, UpRight = 3, DownLeft = 4, DownRight = 5
}
