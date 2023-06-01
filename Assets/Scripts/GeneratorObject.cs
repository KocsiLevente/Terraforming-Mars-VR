using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GeneratorObject : MonoBehaviour
{
    public GameObject TemplateObject;
    public Material SelectedHexagon;
    public int HexagonQuantity = 0;
    public int RingQuantity = 0;
    public float XAngle, YAngle, ZAngle, Radius;
    public float PlanetYAngle = 0.0f;

    //GeneratorObject onStart.
    public void Start()
    {
        GenerateHexagonMap();
    }

    //GeneratorObject onUpdate.
    public void Update()
    {
        //Rotate the planet.
        if (PlanetYAngle > 360.0f)
        {
            PlanetYAngle -= 360.0f;
        }
        transform.Rotate(0.0f, PlanetYAngle + 0.0015f, 0.0f, Space.World);
    }

    //Generate the maps.
    public void GenerateHexagonMap()
    {
        StartCoroutine(GenerateMapForMars());
        //StartCoroutine(GenerateMapForPhobos());
        //StartCoroutine(GenerateMapForDeimos());
    }

    //Generate the map of Mars.
    public IEnumerator GenerateMapForMars()
    {
        for (int r = 0; r < 5; r++)
        {
            RingQuantity++;

            //Set angle for rotation and radius.
            switch (r+1)
            {
                case 1:
                    XAngle = 90.0f;
                    YAngle = 180.0f;
                    ZAngle = 0.0f;
                    Radius = 511.0f;
                    break;
                case 2:
                    XAngle = 98.0f;
                    YAngle = 195.0f;
                    ZAngle = 0.0f;
                    Radius = 505.0f;
                    break;
                case 3:
                    XAngle = 82.0f;
                    YAngle = 195.0f;
                    ZAngle = 0.0f;
                    Radius = 505.0f;
                    break;
                case 4:
                    XAngle = 106.5f;
                    YAngle = 180.0f;
                    ZAngle = 0.0f;
                    Radius = 491.0f;
                    break;
                case 5:
                    XAngle = 73.5f;
                    YAngle = 180.0f;
                    ZAngle = 0.0f;
                    Radius = 491.0f;
                    break;
                default:
                    XAngle = 90.0f;
                    YAngle = 180.0f;
                    ZAngle = 0.0f;
                    Radius = 511.0f;
                    break;
            }

            //Start to generate a Ring.
            for (int i = 0; i < 36; i++)
            {
                HexagonQuantity++;

                //Generate a Hexagon.
                GameObject TerrainObject = Instantiate(TemplateObject, CalculatePoint(YAngle, XAngle, Radius), Quaternion.identity);
                TerrainObject.transform.localScale = new Vector3(90.0f, 0.9f, 90.0f);
                TerrainObject.transform.Rotate(XAngle, YAngle, ZAngle, Space.World);
                TerrainObject.name = "Terrain" + HexagonQuantity;
                TerrainObject.transform.parent = transform;

                //Create a Hexagon and set the fresh object into the Database.
                Hexagon Hexagon = new Hexagon(HexagonQuantity, TerrainObject);
                GameDatabaseService.AddHexagonToGameBoard(Hexagon);
                if (HexagonQuantity == 1)
                {
                    Hexagon.Territory.GetComponent<Renderer>().material = SelectedHexagon;
                    GameDatabaseService.Data.SelectedHexagon = Hexagon;
                }

                //Changing angle for the next object.
                /*Extra information for calculations.
                 *R = 500.0f + 11.0f (because the Hexagon is above the surface).
                 *Planet is at (0.0f, 0.0f, 1000.0f) position.
                 *X = R * Sin(y).
                 *Z = R * Cos(y).
                 *Y = R * Cos(z).
                 */
                YAngle -= 10.0f;
            }
        }
        GameDatabaseService.SetNeighboursOnGameBoard();

        yield return new WaitForSeconds(0.01f);
    }

    //Calc function for onPlanet Points.
    public Vector3 CalculatePoint(float YAngle, float ZAngle, float radius)
    {
        //Calculate the point for the next Hexagon.
        float XsinValue = Convert.ToSingle(Math.Sin(ConvertDegreesToRadians(YAngle)));
        float ZcosValue = Convert.ToSingle(Math.Cos(ConvertDegreesToRadians(YAngle)));
        float YcosValue = Convert.ToSingle(Math.Cos(ConvertDegreesToRadians(ZAngle)));
        return new Vector3(radius * XsinValue, radius * YcosValue, 1000.0f + radius * ZcosValue);
    }

    //Converter function.
    public double ConvertDegreesToRadians(float angle)
    {
        return (angle * Math.PI / 180.0);
    }
}
