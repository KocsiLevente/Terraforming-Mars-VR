using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameControl: MonoBehaviour
{
    public void Start()
    {
        //Disable the final score canvas at the beginning.
        Text score = GameObject.Find("Score").GetComponent<Text>();
        score.GetComponent<Text>().enabled = false;

        //Refresh the UI with the starting values.
        Text creditText = GameObject.Find("Credit").GetComponent<Text>();
        Text metalText = GameObject.Find("Metal").GetComponent<Text>();
        Text titanText = GameObject.Find("Titan").GetComponent<Text>();
        Text plantText = GameObject.Find("Plant").GetComponent<Text>();
        Text energyText = GameObject.Find("Energy").GetComponent<Text>();
        Text heatText = GameObject.Find("Heat").GetComponent<Text>();
        creditText.text = "Credit: " + GameDatabaseService.Data.User.Bank.Credit + " Inc. " + GameDatabaseService.Data.User.Incomes.Credit;
        metalText.text = "Metal: " + GameDatabaseService.Data.User.Bank.Metal + " Inc. " + GameDatabaseService.Data.User.Incomes.Metal;
        titanText.text = "Titan: " + GameDatabaseService.Data.User.Bank.Titan + " Inc. " + GameDatabaseService.Data.User.Incomes.Titan;
        plantText.text = "Plant: " + GameDatabaseService.Data.User.Bank.Plant + " Inc. " + GameDatabaseService.Data.User.Incomes.Plant;
        energyText.text = "Energy: " + GameDatabaseService.Data.User.Bank.Energy + " Inc. " + GameDatabaseService.Data.User.Incomes.Energy;
        heatText.text = "Heat: " + GameDatabaseService.Data.User.Bank.Heat + " Inc. " + GameDatabaseService.Data.User.Incomes.Heat;

        Text oxygen = GameObject.Find("OxygenLevel").GetComponent<Text>();
        Text temperature = GameObject.Find("TemperatureLevel").GetComponent<Text>();
        Text ocean = GameObject.Find("OceanLevel").GetComponent<Text>();
        Text gen = GameObject.Find("Generation").GetComponent<Text>();
        oxygen.text = "Oxygen: " + GameDatabaseService.Data.OxygenLevel + "%:14%";
        temperature.text = "Temperature: " + GameDatabaseService.Data.TemperatureLevel + "°C:+8°C";
        ocean.text = "Ocean: " + GameDatabaseService.Data.OceanLevel + ":9";
        gen.text = "Generation: " + GameDatabaseService.Data.Generation + ":14, Time: " + GameDatabaseService.Data.TimeRemaining.Seconds;

        //GameDatabaseService.timer.Elapsed += async (sender, e) => GameDatabaseService.SecondElapsed();
        //GameDatabaseService.timer.Start();
    }

    public void Update()
    {
        //StartCoroutine(StartRoundCounting());
    }

    //Start counting the Generations.
    private IEnumerator StartRoundCounting()
    {
        if (!GameDatabaseService.Data.IsGameEnded)
        {
            if (GameDatabaseService.Data.TimeRemaining == new TimeSpan(0,0,0))
            {
                GameDatabaseService.Data.TimeRemaining = new TimeSpan(0, 1, 0);
                RoundTimeElapsed();
            }

            GameDatabaseService.RefreshUserResourcesOnUI();
            yield return new WaitForSeconds(1.0f);
        }
    }

    //Function to be called when a Generation is past in the game.
    private void RoundTimeElapsed()
    {
        //Get the incomes for the user.
        GameDatabaseService.GetTheIncomes();

        //Refreshing the UI with new resource values.
        Text creditText = GameObject.Find("Credit").GetComponent<Text>();
        Text metalText = GameObject.Find("Metal").GetComponent<Text>();
        Text titanText = GameObject.Find("Titan").GetComponent<Text>();
        Text plantText = GameObject.Find("Plant").GetComponent<Text>();
        Text energyText = GameObject.Find("Energy").GetComponent<Text>();
        Text heatText = GameObject.Find("Heat").GetComponent<Text>();
        creditText.text = "Credit: " + GameDatabaseService.Data.User.Bank.Credit + " Inc. " + GameDatabaseService.Data.User.Incomes.Credit;
        metalText.text = "Metal: " + GameDatabaseService.Data.User.Bank.Metal + " Inc. " + GameDatabaseService.Data.User.Incomes.Metal;
        titanText.text = "Titan: " + GameDatabaseService.Data.User.Bank.Titan + " Inc. " + GameDatabaseService.Data.User.Incomes.Titan;
        plantText.text = "Plant: " + GameDatabaseService.Data.User.Bank.Plant + " Inc. " + GameDatabaseService.Data.User.Incomes.Plant;
        energyText.text = "Energy: " + GameDatabaseService.Data.User.Bank.Energy + " Inc. " + GameDatabaseService.Data.User.Incomes.Energy;
        heatText.text = "Heat: " + GameDatabaseService.Data.User.Bank.Heat + " Inc. " + GameDatabaseService.Data.User.Incomes.Heat;

        Text oxygen = GameObject.Find("OxygenLevel").GetComponent<Text>();
        Text temperature = GameObject.Find("TemperatureLevel").GetComponent<Text>();
        Text ocean = GameObject.Find("OceanLevel").GetComponent<Text>();
        Text gen = GameObject.Find("Generation").GetComponent<Text>();
        oxygen.text = "Oxygen: " + GameDatabaseService.Data.OxygenLevel + "%:14%";
        temperature.text = "Temperature: " + GameDatabaseService.Data.TemperatureLevel + "°C:+8°C";
        ocean.text = "Ocean: " + GameDatabaseService.Data.OceanLevel + ":9";
        gen.text = "Generation: " + GameDatabaseService.Data.Generation + ":14, Time: " + GameDatabaseService.Data.TimeRemaining.Seconds;
    }
}
