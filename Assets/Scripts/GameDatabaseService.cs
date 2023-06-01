using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;
using System.IO;
using System.Linq;
using System;

//Class for managing the state of the game and user.
public static class GameDatabaseService
{
    private static int guid = 1;

    //Class for storing every data of the game. Some of them will be serialized on save and deserialized on load.
    [Serializable]
    public class GameData
    {
        public int id = 1;
        public int Id { get { return id; } set { id = value; } }

        public int difficulty = 1;
        public int Difficulty { get { return difficulty; } set { difficulty = value; } }

        public User user = new User("User1", new Cost(40, 40, 40, 40, 40, 40));
        public User User { get { return user; } set { user = value; } }

        public List<Hexagon> gameBoard = new List<Hexagon>();
        public List<Hexagon> GameBoard { get { return gameBoard; } set { gameBoard = value; } }

        public List<GameEvent> events = new List<GameEvent>();
        public List<GameEvent> Events { get { return events; } set { events = value; } }

        public int oxygenLevel = 0;
        public int OxygenLevel { get { return oxygenLevel; } set { oxygenLevel = value; } }

        public int temperatureLevel = -30;
        public int TemperatureLevel { get { return temperatureLevel; } set { temperatureLevel = value; } }

        public int oceanLevel = 0;
        public int OceanLevel { get { return oceanLevel; } set { oceanLevel = value; } }

        public int generation = 0;
        public int Generation { get { return generation; } set { generation = value; } }

        public bool isGameEnded = false;
        public bool IsGameEnded { get { return isGameEnded; } set { isGameEnded = value; } }

        //Final score.
        public int score = 0;
        public int Score { get { return score; } set { score = value; } }

        //Timer for Generation counting.
        [NonSerialized]
        private TimeSpan timeRemaining = new TimeSpan(0, 1, 0);
        public TimeSpan TimeRemaining { get { return timeRemaining; } set { timeRemaining = value; } }

        //The selected Hexagon on the map.
        [NonSerialized]
        private Hexagon selectedHexagon = null;
        public Hexagon SelectedHexagon { get { return selectedHexagon; } set { selectedHexagon = value; } }
    }

    public static GameData Data { get; set; } = new GameData();

    public static Timer timer = new Timer(1000);

    //Save the actual GameData object.
    public static bool SaveGameData()
    {
        string json = JsonUtility.ToJson(Data);
        string filePath = Application.dataPath + "\\Resources\\game_" + Data.Id + ".json";

        try
        {
            File.WriteAllText(filePath, json);
            Debug.Log("Game Saved");
            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
            return false;
        }
    }

    //Load a previously saved GameData object.
    public static bool LoadGameData()
    {
        string filePath = Application.dataPath + "\\Resources\\game_1.json";
        string jsonToLoad = "";

        try
        {
            jsonToLoad = File.ReadAllText(filePath);
            GameData toLoad = JsonUtility.FromJson<GameData>(jsonToLoad);

            //Override with the loaded values.
            Data.Id = toLoad.Id;
            Data.Difficulty = toLoad.Difficulty;
            Data.User = toLoad.User;
            Data.OxygenLevel = toLoad.OxygenLevel;
            Data.TemperatureLevel = toLoad.TemperatureLevel;
            Data.OceanLevel = toLoad.OceanLevel;
            Data.Generation = toLoad.Generation;
            Data.IsGameEnded = toLoad.IsGameEnded;
            Data.Score = toLoad.Score;

            //Place the loaded Buildings on the map.
            foreach (Hexagon hl in toLoad.GameBoard)
            {
                if (hl.BuildingModel != null && hl.BuildingModel.Id != 0)
                {
                    Hexagon toFind = Data.GameBoard.Find(h => h.Id == hl.Id);
                    if (toFind != null)
                    {
                        switch (hl.BuildingModel.Type)
                        {
                            case Buildings.PowerPlant:
                                toFind.BuildingModel = new PowerPlant(hl.BuildingModel.Id, hl, Buildings.PowerPlant);
                                break;
                            case Buildings.Greenery:
                                toFind.BuildingModel = new Greenery(hl.BuildingModel.Id, hl, Buildings.Greenery);
                                break;
                            case Buildings.City:
                                toFind.BuildingModel = new City(hl.BuildingModel.Id, hl, Buildings.City);
                                break;
                            case Buildings.Ocean:
                                toFind.BuildingModel = new Ocean(hl.BuildingModel.Id, hl, Buildings.Ocean);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            foreach (GameEvent ev in toLoad.Events)
            {
                Data.Events.Add(ev);
            }

            Debug.Log("Game Loaded");
            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
            return false;
        }
    }

    //End the Game if the Generation counter is 14 or get the incomes for the user.
    public static void GetTheIncomes()
    {
        if (Data.Generation == 14)
        {
            Data.IsGameEnded = true;
            timer.Stop();
            foreach (Hexagon h in Data.GameBoard)
            {
                Data.Score += h.BuildingModel.GetScore();
            }
        }
        else
        {
            Data.Generation++;
            Data.User.Bank.Add(Data.User.Incomes);
        }
    }

    public static Hexagon GetHexagonFromGameBoardById(int id)
    {
        if (id <= 0)
        {
            return null;
        }
        else
        {
            return Data.GameBoard.SingleOrDefault(h => h.Id == id);
        }
    }

    //One second in the Game.
    public static void SecondElapsed()
    {
        Data.TimeRemaining = Data.TimeRemaining - new TimeSpan(0, 0, 1);
        timer.Stop();
        timer.Start();
    }

    //Validate if the user can pay a Cost object or not.
    public static bool CanPay(Cost from, Cost value)
    {
        if ((from.Credit - value.Credit) > -1 &&
            (from.Metal - value.Metal) > -1 &&
            (from.Titan - value.Titan) > -1 &&
            (from.Plant - value.Plant) > -1 &&
            (from.Energy - value.Energy) > -1 &&
            (from.Heat - value.Heat) > -1)
        {
            return true;
        }
        return false;
    }

    //Adding a Hexagon to the GameBoard.
    public static void AddHexagonToGameBoard(Hexagon hexagon)
    {
        Data.GameBoard.Add(hexagon);
    }

    public static void AddGameEventToEvents(GameEvent ev)
    {
        Data.Events.Add(ev);
    }

    //Setting the Neighbours.
    public static void SetNeighboursOnGameBoard()
    {
        for (int r = 0; r < 5; r++)
        {
            for (int i = 1; i < 37; i++)
            {
                //Searching the exact Hexagons.
                Hexagon ActualHexagon = Data.GameBoard.Find(h => h.Id == ((r * 36) + i));
                Hexagon RightHexagon;
                Hexagon UpLeftHexagon = null;
                Hexagon UpRightHexagon = null;
                Hexagon DownLeftHexagon = null;
                Hexagon DownRightHexagon = null;

                //If last item in the ring.
                if (i == 36)
                {
                    RightHexagon = Data.GameBoard.Find(h => h.Id == ((r * 36) + 1));
                }
                else
                {
                    RightHexagon = Data.GameBoard.Find(h => h.Id == ((r * 36) + i + 1));
                }

                switch (r)
                {
                    case 0:
                        if (i == 36)
                        {
                            UpLeftHexagon = Data.GameBoard.Find(h => h.Id == (i + 37));
                        }
                        else
                        {
                            UpLeftHexagon = Data.GameBoard.Find(h => h.Id == (i + 73));
                        }

                        if (i == 35 || i == 36)
                        {
                            UpRightHexagon = Data.GameBoard.Find(h => h.Id == (i + 38));
                        }
                        else
                        {
                            UpRightHexagon = Data.GameBoard.Find(h => h.Id == (i + 74));
                        }

                        if (i == 36)
                        {
                            DownLeftHexagon = Data.GameBoard.Find(h => h.Id == (i + 1));
                        }
                        else
                        {
                            DownLeftHexagon = Data.GameBoard.Find(h => h.Id == (i + 37));
                        }

                        if (i == 35 || i == 36)
                        {
                            DownRightHexagon = Data.GameBoard.Find(h => h.Id == (i + 2));
                        }
                        else
                        {
                            DownRightHexagon = Data.GameBoard.Find(h => h.Id == (i + 38));
                        }
                        break;
                    case 1:
                        if (i == 1 || i == 2)
                        {
                            UpLeftHexagon = Data.GameBoard.Find(h => h.Id == (i + 34));
                        }
                        else
                        {
                            UpLeftHexagon = Data.GameBoard.Find(h => h.Id == (i - 2));
                        }

                        if (i == 1)
                        {
                            UpRightHexagon = Data.GameBoard.Find(h => h.Id == (i + 35));
                        }
                        else
                        {
                            UpRightHexagon = Data.GameBoard.Find(h => h.Id == (i - 1));
                        }

                        if (i == 1 | i == 2)
                        {
                            DownLeftHexagon = Data.GameBoard.Find(h => h.Id == (i + 142));
                        }
                        else
                        {
                            DownLeftHexagon = Data.GameBoard.Find(h => h.Id == (i + 106));
                        }

                        if (i == 1)
                        {
                            DownRightHexagon = Data.GameBoard.Find(h => h.Id == (i + 143));
                        }
                        else
                        {
                            DownRightHexagon = Data.GameBoard.Find(h => h.Id == (i + 107));
                        }
                        break;
                    case 2:
                        if (i == 1 || i == 2)
                        {
                            UpLeftHexagon = Data.GameBoard.Find(h => h.Id == (i + 178));
                        }
                        else
                        {
                            UpLeftHexagon = Data.GameBoard.Find(h => h.Id == (i + 142));
                        }

                        if (i == 1)
                        {
                            UpRightHexagon = Data.GameBoard.Find(h => h.Id == (i + 179));
                        }
                        else
                        {
                            UpRightHexagon = Data.GameBoard.Find(h => h.Id == (i + 143));
                        }

                        if (i == 1 | i == 2)
                        {
                            DownLeftHexagon = Data.GameBoard.Find(h => h.Id == (i + 34));
                        }
                        else
                        {
                            DownLeftHexagon = Data.GameBoard.Find(h => h.Id == (i - 2));
                        }

                        if (i == 1)
                        {
                            DownRightHexagon = Data.GameBoard.Find(h => h.Id == (i + 35));
                        }
                        else
                        {
                            DownRightHexagon = Data.GameBoard.Find(h => h.Id == (i - 1));
                        }
                        break;
                    case 3:
                        //No Downer Hexagons in this ring.
                        if (i == 36)
                        {
                            UpLeftHexagon = Data.GameBoard.Find(h => h.Id == (i + 1));
                        }
                        else
                        {
                            UpLeftHexagon = Data.GameBoard.Find(h => h.Id == (i + 37));
                        }

                        if (i == 36)
                        {
                            UpRightHexagon = Data.GameBoard.Find(h => h.Id == (i + 2));
                        }
                        else if (i == 35)
                        {
                            UpRightHexagon = Data.GameBoard.Find(h => h.Id == (i + 2));
                        }
                        else
                        {
                            UpRightHexagon = Data.GameBoard.Find(h => h.Id == (i + 38));
                        }
                        break;
                    case 4:
                        //No Upper Hexagons in this ring.
                        if (i == 36)
                        {
                            DownLeftHexagon = Data.GameBoard.Find(h => h.Id == (i + 37));
                        }
                        else
                        {
                            DownLeftHexagon = Data.GameBoard.Find(h => h.Id == (i + 73));
                        }

                        if (i == 36)
                        {
                            DownRightHexagon = Data.GameBoard.Find(h => h.Id == (i + 38));
                        }
                        else if (i == 35)
                        {
                            DownRightHexagon = Data.GameBoard.Find(h => h.Id == (i + 38));
                        }
                        else
                        {
                            DownRightHexagon = Data.GameBoard.Find(h => h.Id == (i + 74));
                        }
                        break;
                    default:
                        break;
                }

                //Setting the Hexagon Neighbours.
                ActualHexagon.SetNeighbour(HexagonDirections.Right, RightHexagon);
                ActualHexagon.SetNeighbour(HexagonDirections.UpLeft, UpLeftHexagon);
                ActualHexagon.SetNeighbour(HexagonDirections.UpRight, UpRightHexagon);
                ActualHexagon.SetNeighbour(HexagonDirections.DownLeft, DownLeftHexagon);
                ActualHexagon.SetNeighbour(HexagonDirections.DownRight, DownRightHexagon);
                RightHexagon.SetNeighbour(HexagonDirections.Left, ActualHexagon);
                if (UpLeftHexagon != null)
                {
                    UpLeftHexagon.SetNeighbour(HexagonDirections.DownRight, ActualHexagon);
                }
                if (UpRightHexagon != null)
                {
                    UpRightHexagon.SetNeighbour(HexagonDirections.DownLeft, ActualHexagon);
                }
                if (DownLeftHexagon != null)
                {
                    DownLeftHexagon.SetNeighbour(HexagonDirections.UpRight, ActualHexagon);
                }
                if (DownRightHexagon != null)
                {
                    DownRightHexagon.SetNeighbour(HexagonDirections.UpLeft, ActualHexagon);
                }
            }
        }
    }

    //Moving function on Surface.
    public static int MoveOnSurface(HexagonDirections dir, Material selectedMat, Material normalMat)
    {
        Hexagon Neighbour = GetHexagonFromGameBoardById(Data.SelectedHexagon.Neighbours[dir]);

        if (Neighbour != null)
        {
            Data.SelectedHexagon.Territory.GetComponent<Renderer>().material = normalMat;
            Data.SelectedHexagon = Neighbour;
            Neighbour.Territory.GetComponent<Renderer>().material = selectedMat;

            return Neighbour.Id;
        }
        else
        {
            return -1;
        }
    }

    //Setting a Building to a Hexagon.
    public static void SetBuildingToHexagon(Buildings buildingType, GameObject building, int hexagonId)
    {
        Hexagon SearchedHexagon = Data.GameBoard.Find(h => h.Id == hexagonId);
        GameObject Mars = GameObject.Find("Planet Mars");

        if (SearchedHexagon != null && Mars != null)
        {
            building.transform.parent = Mars.transform;

            switch (buildingType)
            {
                case Buildings.PowerPlant:
                    //Pay the cost of the Building.
                    Data.User.Bank = new Cost(
                            Data.User.Bank.Credit - 10,
                            Data.User.Bank.Metal,
                            Data.User.Bank.Titan,
                            Data.User.Bank.Plant,
                            Data.User.Bank.Energy,
                            Data.User.Bank.Heat
                            );
                    //Adding the incomes of the Building if there are any.
                    Data.User.Incomes = new Cost(
                        Data.User.Incomes.Credit,
                        Data.User.Incomes.Metal,
                        Data.User.Incomes.Titan,
                        Data.User.Incomes.Plant,
                        Data.User.Incomes.Energy + 1,
                        Data.User.Incomes.Heat
                        );
                    //Update the game status.
                    Data.TemperatureLevel += 2;
                    //Refresh the UI with the new values.
                    RefreshUserResourcesOnUI();
                    //Connecting the Building and the Hexagon.
                    SearchedHexagon.Building = building;
                    SearchedHexagon.BuildingModel = new PowerPlant(guid++, SearchedHexagon, Buildings.PowerPlant);
                    break;
                case Buildings.Greenery:
                    Data.User.Bank = new Cost(
                            Data.User.Bank.Credit - 15,
                            Data.User.Bank.Metal,
                            Data.User.Bank.Titan,
                            Data.User.Bank.Plant,
                            Data.User.Bank.Energy,
                            Data.User.Bank.Heat
                            );
                    Data.User.Incomes = new Cost(
                        Data.User.Incomes.Credit,
                        Data.User.Incomes.Metal,
                        Data.User.Incomes.Titan,
                        Data.User.Incomes.Plant + 1,
                        Data.User.Incomes.Energy,
                        Data.User.Incomes.Heat
                        );
                    Data.OxygenLevel++;
                    RefreshUserResourcesOnUI();
                    SearchedHexagon.Building = building;
                    SearchedHexagon.BuildingModel = new Greenery(guid++, SearchedHexagon, Buildings.Greenery);
                    break;
                case Buildings.City:
                    Data.User.Bank = new Cost(
                            Data.User.Bank.Credit - 25,
                            Data.User.Bank.Metal,
                            Data.User.Bank.Titan,
                            Data.User.Bank.Plant,
                            Data.User.Bank.Energy,
                            Data.User.Bank.Heat
                            );
                    Data.User.Incomes = new Cost(
                        Data.User.Incomes.Credit + 5,
                        Data.User.Incomes.Metal,
                        Data.User.Incomes.Titan,
                        Data.User.Incomes.Plant,
                        Data.User.Incomes.Energy - 2,
                        Data.User.Incomes.Heat
                        );
                    RefreshUserResourcesOnUI();
                    SearchedHexagon.Building = building;
                    SearchedHexagon.BuildingModel = new City(guid++, SearchedHexagon, Buildings.City);
                    break;
                case Buildings.Ocean:
                    Data.User.Bank = new Cost(
                            Data.User.Bank.Credit - 20,
                            Data.User.Bank.Metal,
                            Data.User.Bank.Titan,
                            Data.User.Bank.Plant,
                            Data.User.Bank.Energy,
                            Data.User.Bank.Heat
                            );
                    Data.OceanLevel++;
                    RefreshUserResourcesOnUI();
                    SearchedHexagon.Building = building;
                    SearchedHexagon.BuildingModel = new Ocean(guid++, SearchedHexagon, Buildings.Ocean);
                    break;
                default:
                    break;
            }
        }

        //Save the GameData.
        SaveGameData();
    }

    //It is the same as the previous function but it is called on load operation and does not affect the user's bank.
    public static void SetBuildingToHexagonOnLoad(Buildings buildingType, GameObject building, int hexagonId, int bId)
    {
        Hexagon SearchedHexagon = Data.GameBoard.Find(h => h.Id == hexagonId);
        GameObject Mars = GameObject.Find("Planet Mars");

        if (SearchedHexagon != null && Mars != null)
        {
            building.transform.parent = Mars.transform;

            switch (buildingType)
            {
                case Buildings.PowerPlant:
                    RefreshUserResourcesOnUI();
                    SearchedHexagon.Building = building;
                    SearchedHexagon.BuildingModel = new PowerPlant(bId, SearchedHexagon, Buildings.PowerPlant);
                    break;
                case Buildings.Greenery:
                    RefreshUserResourcesOnUI();
                    SearchedHexagon.Building = building;
                    SearchedHexagon.BuildingModel = new Greenery(bId, SearchedHexagon, Buildings.Greenery);
                    break;
                case Buildings.City:
                    RefreshUserResourcesOnUI();
                    SearchedHexagon.Building = building;
                    SearchedHexagon.BuildingModel = new City(bId, SearchedHexagon, Buildings.City);
                    break;
                case Buildings.Ocean:
                    RefreshUserResourcesOnUI();
                    SearchedHexagon.Building = building;
                    SearchedHexagon.BuildingModel = new Ocean(bId, SearchedHexagon, Buildings.Ocean);
                    break;
                default:
                    break;
            }
        }
    }

    //Setting the GameEvent to the GameData after buy.
    public static void SetGameEvent(EventTypes eventType, GameObject eventObject, int hexagonId)
    {
        GameObject Mars = GameObject.Find("Planet Mars");

        if (Mars != null)
        {
            eventObject.transform.parent = Mars.transform;
            GameEvent createdEvent = null;

            switch (eventType)
            {
                case EventTypes.SpaceMine:
                    //Pay the cost of the GameEvent.
                    Data.User.Bank = new Cost(
                            Data.User.Bank.Credit - 10,
                            Data.User.Bank.Metal - 10,
                            Data.User.Bank.Titan - 10,
                            Data.User.Bank.Plant,
                            Data.User.Bank.Energy - 10,
                            Data.User.Bank.Heat
                            );
                    //Update the incomes if there are any.
                    Data.User.Incomes = new Cost(
                        Data.User.Incomes.Credit + 5,
                        Data.User.Incomes.Metal + 2,
                        Data.User.Incomes.Titan + 1,
                        Data.User.Incomes.Plant,
                        Data.User.Incomes.Energy - 2,
                        Data.User.Incomes.Heat
                        );
                    //Refresh the UI.
                    RefreshUserResourcesOnUI();
                    createdEvent = new GameEvent(guid++, hexagonId, eventObject, eventType);
                    break;
                case EventTypes.Satellite:
                    Data.User.Bank = new Cost(
                            Data.User.Bank.Credit - 5,
                            Data.User.Bank.Metal,
                            Data.User.Bank.Titan - 5,
                            Data.User.Bank.Plant,
                            Data.User.Bank.Energy - 5,
                            Data.User.Bank.Heat - 5
                            );
                    Data.User.Incomes = new Cost(
                        Data.User.Incomes.Credit,
                        Data.User.Incomes.Metal,
                        Data.User.Incomes.Titan + 1,
                        Data.User.Incomes.Plant,
                        Data.User.Incomes.Energy,
                        Data.User.Incomes.Heat + 1
                        );
                    Data.OxygenLevel++;
                    RefreshUserResourcesOnUI();
                    createdEvent = new GameEvent(guid++, hexagonId, eventObject, eventType);
                    break;
                case EventTypes.SpaceStation:
                    Data.User.Bank = new Cost(
                            Data.User.Bank.Credit - 25,
                            Data.User.Bank.Metal,
                            Data.User.Bank.Titan - 10,
                            Data.User.Bank.Plant,
                            Data.User.Bank.Energy,
                            Data.User.Bank.Heat - 10
                            );
                    Data.User.Incomes = new Cost(
                        Data.User.Incomes.Credit + 5,
                        Data.User.Incomes.Metal,
                        Data.User.Incomes.Titan + 2,
                        Data.User.Incomes.Plant,
                        Data.User.Incomes.Energy - 2,
                        Data.User.Incomes.Heat
                        );
                    RefreshUserResourcesOnUI();
                    createdEvent = new GameEvent(guid++, hexagonId, eventObject, eventType);
                    break;
                default:
                    break;
            }

            if (createdEvent != null)
            {
                Data.Events.Add(createdEvent);
            }
        }

        //Save the GameData.
        SaveGameData();
    }

    //It is the same as the previous function but it is called on load operation and does not affect the user's bank.
    public static void SetEventOnLoad(EventTypes eventType, GameObject eventObject, int hexagonId, int evId)
    {
        GameObject Mars = GameObject.Find("Planet Mars");

        if (Mars != null)
        {
            eventObject.transform.parent = Mars.transform;
            GameEvent createdEvent;

            switch (eventType)
            {
                case EventTypes.SpaceMine:
                    RefreshUserResourcesOnUI();
                    createdEvent = new GameEvent(evId, hexagonId, eventObject, eventType);
                    break;
                case EventTypes.Satellite:
                    RefreshUserResourcesOnUI();
                    createdEvent = new GameEvent(evId, hexagonId, eventObject, eventType);
                    break;
                case EventTypes.SpaceStation:
                    RefreshUserResourcesOnUI();
                    createdEvent = new GameEvent(evId, hexagonId, eventObject, eventType);
                    break;
                default:
                    break;
            }
        }
    }

    //Refresh UI function.
    public static void RefreshUserResourcesOnUI()
    {
        Text creditText = GameObject.Find("Credit").GetComponent<Text>();
        Text metalText = GameObject.Find("Metal").GetComponent<Text>();
        Text titanText = GameObject.Find("Titan").GetComponent<Text>();
        Text plantText = GameObject.Find("Plant").GetComponent<Text>();
        Text energyText = GameObject.Find("Energy").GetComponent<Text>();
        Text heatText = GameObject.Find("Heat").GetComponent<Text>();
        creditText.text = "Credit: " + Data.User.Bank.Credit + " Inc. " + Data.User.Incomes.Credit;
        metalText.text = "Metal: " + Data.User.Bank.Metal + " Inc. " + Data.User.Incomes.Metal;
        titanText.text = "Titan: " + Data.User.Bank.Titan + " Inc. " + Data.User.Incomes.Titan;
        plantText.text = "Plant: " + Data.User.Bank.Plant + " Inc. " + Data.User.Incomes.Plant;
        energyText.text = "Energy: " + Data.User.Bank.Energy + " Inc. " + Data.User.Incomes.Energy;
        heatText.text = "Heat: " + Data.User.Bank.Heat + " Inc. " + Data.User.Incomes.Heat;

        Text oxygen = GameObject.Find("OxygenLevel").GetComponent<Text>();
        Text temperature = GameObject.Find("TemperatureLevel").GetComponent<Text>();
        Text ocean = GameObject.Find("OceanLevel").GetComponent<Text>();
        Text gen = GameObject.Find("Generation").GetComponent<Text>();
        oxygen.text = "Oxygen: " + Data.OxygenLevel + "%:14%";
        temperature.text = "Temperature: " + Data.TemperatureLevel + "°C:+8°C";
        ocean.text = "Ocean: " + Data.OceanLevel + ":9";
        gen.text = "Generation: " + Data.Generation + ":14, Time: " + Data.TimeRemaining.Seconds;
    }
}
