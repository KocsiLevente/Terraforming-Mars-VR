using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using OVRTouchSample;
using System;

//Class for managing the interface of the user.
public class ShipInterface : MonoBehaviour
{
    public int ActualHexagonId = 1;
    public Material SelectedHexagon;
    public Material NormalHexagon;
    public int SelectedBuildingId = 1;

    public float forwardSpeed = 30.0f, sidewaySpeed = 15.0f, hoverSpeed = 10.0f;
    private float activeFWS, activeSWS, activeHS;
    private float accelerationFW = 2.5f, accelerationSW = 2.0f, accelerationH = 1.5f;
    private float lookRateSpeed = 10.0f;
    private Vector2 lookInput, screenCenter, mouseDistance, hexagonDirInput, shopDirInput;
    private float rollInputLeft, rollInputRight;
    private float rollScale;
    public float rollSpeed = 30.0f, accelerationRoll = 1.5f;

    private bool IsShopControlTurnedOn = false;
    private bool IsResourcesPoppedUp = false;
    private bool IsTerraformingPoppedUp = false;
    private bool IsShopPoppedUp = false;
    private bool LeftThumbIsReset = true;
    private bool RightThumbIsReset = true;
    private bool PrimaryHandTriggerIsReset = true;
    private bool SecondaryHandTriggerIsReset = true;
    private bool IsShipMoving = false;
    private int CameraNumber = 1;

    //ShipInterface onStart.
    public void Start()
    {
        IsShopControlTurnedOn = false;
        IsResourcesPoppedUp = false;
        IsTerraformingPoppedUp = false;
        IsShopPoppedUp = false;
        GameObject shopObj = GameObject.Find("ShopObjects");
        if (shopObj != null)
        {
            shopObj.GetComponent<Canvas>().enabled = false;
        }

        screenCenter.x = Screen.width / 2.0f;
        screenCenter.y = Screen.height / 2.0f;
    }

    //ShipInterface onUpdate.
    public void Update()
    {
        if (!GameDatabaseService.Data.IsGameEnded)
        {
            //Ship Combustion emission.
            GameObject LCEH = GameObject.Find("LeftCombustionEmissionHard");
            GameObject LCES = GameObject.Find("LeftCombustionEmissionSoft");
            GameObject MCEH = GameObject.Find("MiddleCombustionEmissionHard");
            GameObject MCES = GameObject.Find("MiddleCombustionEmissionSoft");
            GameObject RCEH = GameObject.Find("RightCombustionEmissionHard");
            GameObject RCES = GameObject.Find("RightCombustionEmissionSoft");
            if (LCEH != null && LCES != null && MCEH != null && MCES != null && RCEH != null && RCES != null)
            {
                LCEH.GetComponent<ParticleSystem>().enableEmission = IsShipMoving;
                LCES.GetComponent<ParticleSystem>().enableEmission = IsShipMoving;
                MCEH.GetComponent<ParticleSystem>().enableEmission = IsShipMoving;
                MCES.GetComponent<ParticleSystem>().enableEmission = IsShipMoving;
                RCEH.GetComponent<ParticleSystem>().enableEmission = IsShipMoving;
                RCES.GetComponent<ParticleSystem>().enableEmission = IsShipMoving;
            }

            //Switching between cameras.
            GameObject cameraInShip = GameObject.Find("First Person Controller");
            switch (CameraNumber)
            {
                case 1:
                    GameObject frontShipWindow = GameObject.Find("FrontWindow");
                    if (frontShipWindow != null)
                    {
                        cameraInShip.transform.localPosition = new Vector3(0.0f, 1.5f, 0.0f);
                        cameraInShip.transform.LookAt(frontShipWindow.transform);
                    }
                    cameraInShip.transform.LookAt(null);
                    break;
                case 2:
                    frontShipWindow = GameObject.Find("FrontWindow");
                    if (frontShipWindow != null)
                    {
                        cameraInShip.transform.localPosition = new Vector3(0.0f, 18.5f, -40.0f);
                        cameraInShip.transform.LookAt(frontShipWindow.transform);
                    }
                    cameraInShip.transform.LookAt(null);
                    break;
                case 3:
                    GameObject phobos = GameObject.Find("Phobos");
                    GameObject mars = GameObject.Find("Planet Mars");
                    if (phobos != null && mars != null)
                    {
                        cameraInShip.transform.position = new Vector3(
                            phobos.transform.position.x - 180.0f,
                            phobos.transform.position.y + 120.0f,
                            phobos.transform.position.z + 0.0f
                            );
                        cameraInShip.transform.LookAt(mars.transform);
                    }
                    break;
                case 4:
                    GameObject deimos = GameObject.Find("Deimos");
                    mars = GameObject.Find("Planet Mars");
                    if (deimos != null && mars != null)
                    {
                        cameraInShip.transform.position = new Vector3(
                            deimos.transform.position.x - 0.0f,
                            deimos.transform.position.y + 120.0f,
                            deimos.transform.position.z + 180.0f
                            );
                        cameraInShip.transform.LookAt(mars.transform);
                    }
                    break;
                case 5:
                    mars = GameObject.Find("Planet Mars");
                    if (mars != null)
                    {
                        cameraInShip.transform.position = new Vector3(
                            mars.transform.position.x - 0.0f,
                            mars.transform.position.y + 120.0f,
                            mars.transform.position.z - 1800.0f
                            );
                        cameraInShip.transform.LookAt(mars.transform);
                    }
                    break;
                case 6:
                    mars = GameObject.Find("Planet Mars");
                    if (mars != null)
                    {
                        cameraInShip.transform.position = new Vector3(
                            mars.transform.position.x - 1800.0f,
                            mars.transform.position.y + 120.0f,
                            mars.transform.position.z + 0.0f
                            );
                        cameraInShip.transform.LookAt(mars.transform);
                    }
                    break;
                case 7:
                    mars = GameObject.Find("Planet Mars");
                    if (mars != null)
                    {
                        cameraInShip.transform.position = new Vector3(
                            mars.transform.position.x - 0.0f,
                            mars.transform.position.y + 120.0f,
                            mars.transform.position.z + 1800.0f
                            );
                        cameraInShip.transform.LookAt(mars.transform);
                    }
                    break;
                case 8:
                    mars = GameObject.Find("Planet Mars");
                    if (mars != null)
                    {
                        cameraInShip.transform.position = new Vector3(
                            mars.transform.position.x + 1800.0f,
                            mars.transform.position.y + 120.0f,
                            mars.transform.position.z + 0.0f
                            );
                        cameraInShip.transform.LookAt(mars.transform);
                    }
                    break;
                default:
                    break;
            }

            //Switching between cameras.
            if (Input.GetKey(KeyCode.K)/*OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.1f*/)
            {
                if (CameraNumber != 8 && PrimaryHandTriggerIsReset)
                {
                    PrimaryHandTriggerIsReset = false;
                    CameraNumber++;
                }
                else if (PrimaryHandTriggerIsReset)
                {
                    PrimaryHandTriggerIsReset = false;
                    CameraNumber = 1;
                }
            }
            else if (Input.GetKey(KeyCode.L)/*OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.1f*/)
            {
                if (CameraNumber != 1 && SecondaryHandTriggerIsReset)
                {
                    SecondaryHandTriggerIsReset = false;
                    CameraNumber--;
                }
                else if (SecondaryHandTriggerIsReset)
                {
                    SecondaryHandTriggerIsReset = false;
                    CameraNumber = 8;
                }
            }

            if (true/*OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) == 0.0f*/)
            {
                PrimaryHandTriggerIsReset = true;
            }
            if (true/*OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) == 0.0f*/)
            {
                SecondaryHandTriggerIsReset = true;
            }

            if (Input.GetKey(KeyCode.Q)/*OVRInput.GetDown(OVRInput.Button.One)*/) //B
            {
                //Switching between controls.
                IsShopControlTurnedOn = !IsShopControlTurnedOn;
            }

            //If ShopControlTurnedOn then we don't navigate the ship we are navigating on the surface and the shop.
            if (IsShopControlTurnedOn)
            {

                //Moving logic on surface Hexagons.
                hexagonDirInput = new Vector2(); //OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
                if (LeftThumbIsReset)
                {
                    if (hexagonDirInput.x < -0.35f && hexagonDirInput.y > 0.35f)
                    {
                        LeftThumbIsReset = false;
                        ActualHexagonId = GameDatabaseService.MoveOnSurface(HexagonDirections.UpLeft, SelectedHexagon, NormalHexagon);
                    }
                    if (hexagonDirInput.x < -0.35f && hexagonDirInput.y < -0.35f)
                    {
                        LeftThumbIsReset = false;
                        ActualHexagonId = GameDatabaseService.MoveOnSurface(HexagonDirections.DownLeft, SelectedHexagon, NormalHexagon);
                    }
                    if (hexagonDirInput.x > 0.35f && hexagonDirInput.y > 0.35f)
                    {
                        LeftThumbIsReset = false;
                        ActualHexagonId = GameDatabaseService.MoveOnSurface(HexagonDirections.UpRight, SelectedHexagon, NormalHexagon);
                    }
                    if (hexagonDirInput.x > 0.35f && hexagonDirInput.y < -0.35f)
                    {
                        LeftThumbIsReset = false;
                        ActualHexagonId = GameDatabaseService.MoveOnSurface(HexagonDirections.DownRight, SelectedHexagon, NormalHexagon);
                    }
                    if (hexagonDirInput.x < -0.75f && hexagonDirInput.y > -0.1f && hexagonDirInput.y < 0.1f)
                    {
                        LeftThumbIsReset = false;
                        ActualHexagonId = GameDatabaseService.MoveOnSurface(HexagonDirections.Left, SelectedHexagon, NormalHexagon);
                    }
                    if (hexagonDirInput.x > 0.75f && hexagonDirInput.y > -0.1f && hexagonDirInput.y < 0.1f)
                    {
                        LeftThumbIsReset = false;
                        ActualHexagonId = GameDatabaseService.MoveOnSurface(HexagonDirections.Right, SelectedHexagon, NormalHexagon);
                    }
                }

                if (hexagonDirInput.x == 0.0f && hexagonDirInput.y == 0.0f)
                {
                    LeftThumbIsReset = true;
                }

                if (Input.GetKey(KeyCode.B)/*OVRInput.GetDown(OVRInput.Button.Two)*/) //A
                {
                    //Pop up the shop.
                    IsShopPoppedUp = !IsShopPoppedUp;

                    GameObject shopObj = GameObject.Find("ShopObjects");
                    if (shopObj != null)
                    {
                        shopObj.GetComponent<Canvas>().enabled = IsShopPoppedUp;
                    }
                }

                //ShopControl.
                if (IsShopPoppedUp)
                {
                    Text costsText = GameObject.Find("ShopCosts").GetComponent<Text>();
                    Text incomesText = GameObject.Find("ShopIncomes").GetComponent<Text>();
                    Text selectorBody1 = GameObject.Find("ShopSelector1").GetComponent<Text>();
                    Text selectorBody2 = GameObject.Find("ShopSelector2").GetComponent<Text>();
                    Text selectorBody3 = GameObject.Find("ShopSelector3").GetComponent<Text>();
                    Text selectorBody4 = GameObject.Find("ShopSelector4").GetComponent<Text>();
                    Text selectorBody5 = GameObject.Find("ShopSelector5").GetComponent<Text>();
                    Text selectorBody6 = GameObject.Find("ShopSelector6").GetComponent<Text>();
                    Text selectorBody7 = GameObject.Find("ShopSelector7").GetComponent<Text>();
                    GameObject shopObjects = GameObject.Find("ShopObjects");
                    if (selectorBody1 != null && selectorBody2 != null && selectorBody3 != null && selectorBody4 != null && selectorBody5 && selectorBody6 != null && selectorBody7 && costsText != null && incomesText != null)
                    {
                        shopObjects.transform.LookAt(Camera.main.transform);
                        shopObjects.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
                        //Move the shop selector if showed.
                        switch (SelectedBuildingId)
                        {
                            case 1:
                                selectorBody1.GetComponent<Text>().enabled = true;
                                selectorBody2.GetComponent<Text>().enabled = false;
                                selectorBody3.GetComponent<Text>().enabled = false;
                                selectorBody4.GetComponent<Text>().enabled = false;
                                selectorBody5.GetComponent<Text>().enabled = false;
                                selectorBody6.GetComponent<Text>().enabled = false;
                                selectorBody7.GetComponent<Text>().enabled = false;
                                costsText.text = "Costs: 10 Credit";
                                incomesText.text = "Incomes: +1 Energy +2 Temperature";
                                break;
                            case 2:
                                selectorBody1.GetComponent<Text>().enabled = false;
                                selectorBody2.GetComponent<Text>().enabled = true;
                                selectorBody3.GetComponent<Text>().enabled = false;
                                selectorBody4.GetComponent<Text>().enabled = false;
                                selectorBody5.GetComponent<Text>().enabled = false;
                                selectorBody6.GetComponent<Text>().enabled = false;
                                selectorBody7.GetComponent<Text>().enabled = false;
                                costsText.text = "Costs: 25 Credit";
                                incomesText.text = "Incomes: +5 Credit, -2 Energy";
                                break;
                            case 3:
                                selectorBody1.GetComponent<Text>().enabled = false;
                                selectorBody2.GetComponent<Text>().enabled = false;
                                selectorBody3.GetComponent<Text>().enabled = true;
                                selectorBody4.GetComponent<Text>().enabled = false;
                                selectorBody5.GetComponent<Text>().enabled = false;
                                selectorBody6.GetComponent<Text>().enabled = false;
                                selectorBody7.GetComponent<Text>().enabled = false;
                                costsText.text = "Costs: 15 Credit";
                                incomesText.text = "Incomes: +1 Plant, +1 Oxygen Level";
                                break;
                            case 4:
                                selectorBody1.GetComponent<Text>().enabled = false;
                                selectorBody2.GetComponent<Text>().enabled = false;
                                selectorBody3.GetComponent<Text>().enabled = false;
                                selectorBody4.GetComponent<Text>().enabled = true;
                                selectorBody5.GetComponent<Text>().enabled = false;
                                selectorBody6.GetComponent<Text>().enabled = false;
                                selectorBody7.GetComponent<Text>().enabled = false;
                                costsText.text = "Costs: 20 Credit";
                                incomesText.text = "Incomes: +1 Ocean Level";
                                break;
                            case 5:
                                selectorBody1.GetComponent<Text>().enabled = false;
                                selectorBody2.GetComponent<Text>().enabled = false;
                                selectorBody3.GetComponent<Text>().enabled = false;
                                selectorBody4.GetComponent<Text>().enabled = false;
                                selectorBody5.GetComponent<Text>().enabled = true;
                                selectorBody6.GetComponent<Text>().enabled = false;
                                selectorBody7.GetComponent<Text>().enabled = false;
                                costsText.text = "Costs: 10 Credit, 10 Metal, 10 Titan, 10 Energy";
                                incomesText.text = "Incomes: +5 Credit, +2 Metal, +1 Titan -2 Energy";
                                break;
                            case 6:
                                selectorBody1.GetComponent<Text>().enabled = false;
                                selectorBody2.GetComponent<Text>().enabled = false;
                                selectorBody3.GetComponent<Text>().enabled = false;
                                selectorBody4.GetComponent<Text>().enabled = false;
                                selectorBody5.GetComponent<Text>().enabled = false;
                                selectorBody6.GetComponent<Text>().enabled = true;
                                selectorBody7.GetComponent<Text>().enabled = false;
                                costsText.text = "Costs: 5 Credit, 5 Titan, 5 Energy, 5 Heat";
                                incomesText.text = "Incomes: +1 Titan, +1 Heat";
                                break;
                            case 7:
                                selectorBody1.GetComponent<Text>().enabled = false;
                                selectorBody2.GetComponent<Text>().enabled = false;
                                selectorBody3.GetComponent<Text>().enabled = false;
                                selectorBody4.GetComponent<Text>().enabled = false;
                                selectorBody5.GetComponent<Text>().enabled = false;
                                selectorBody6.GetComponent<Text>().enabled = false;
                                selectorBody7.GetComponent<Text>().enabled = true;
                                costsText.text = "Costs: 25 Credit, 10 Titan, 10 Heat";
                                incomesText.text = "Incomes: +5 Credit, +2 Titan, -2 Energy";
                                break;
                            default:
                                break;
                        }
                    }

                    shopDirInput = Input.GetKey(KeyCode.DownArrow)/*OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick)*/ ?
                    new Vector2(0f, -0.5f) : new Vector2(0f, 0f);
                    if (RightThumbIsReset)
                    {
                        if (shopDirInput.y < -0.25f)
                        {
                            RightThumbIsReset = false;
                            if (SelectedBuildingId == 7)
                            {
                                SelectedBuildingId = 1;
                            }
                            else
                            {
                                SelectedBuildingId++;
                            }
                        }
                        if (shopDirInput.y > 0.25f)
                        {
                            RightThumbIsReset = false;
                            if (SelectedBuildingId == 1)
                            {
                                SelectedBuildingId = 7;
                            }
                            else
                            {
                                SelectedBuildingId--;
                            }
                        }
                    }
                    if (shopDirInput.x == 0.0f && shopDirInput.y == 0.0f)
                    {
                        RightThumbIsReset = true;
                    }

                    if (Input.GetKey(KeyCode.KeypadEnter)/*OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick)*/) //Thumbstick left
                    {
                        switch (SelectedBuildingId)
                        {
                            case 1:
                                BuyBuilding(Buildings.PowerPlant);
                                break;
                            case 2:
                                BuyBuilding(Buildings.City);
                                break;
                            case 3:
                                BuyBuilding(Buildings.Greenery);
                                break;
                            case 4:
                                BuyBuilding(Buildings.Ocean);
                                break;
                            case 5:
                                BuyEvent(EventTypes.SpaceMine);
                                break;
                            case 6:
                                BuyEvent(EventTypes.Satellite);
                                break;
                            case 7:
                                BuyEvent(EventTypes.SpaceStation);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            //If Shop control is turned off we can navigate the SpaceShip;
            else
            {
                //Controls of the SpaceShip.
                //lookInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
                lookInput.x = Input.mousePosition.x;
                lookInput.y = Input.mousePosition.y;

                //mouseDistance = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
                mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
                mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;
                mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1.0f);

                rollInputLeft = 0.0f; //OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
                rollInputRight = -1 * 0.0f; //OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);

                rollScale = Mathf.Lerp(rollScale, (rollInputLeft + rollInputRight), accelerationRoll * Time.deltaTime);
                transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, rollScale * rollSpeed * Time.deltaTime, Space.Self);

                activeFWS = Mathf.Lerp(activeFWS, Input.GetAxisRaw("Vertical") * forwardSpeed, accelerationFW * Time.deltaTime);
                activeSWS = Mathf.Lerp(activeSWS, Input.GetAxisRaw("Horizontal") * sidewaySpeed, accelerationSW * Time.deltaTime);
                activeHS = Mathf.Lerp(activeHS, Input.GetAxisRaw("Hover") * hoverSpeed, accelerationH * Time.deltaTime);

                transform.position += transform.forward * activeFWS * Time.deltaTime;

                if (lookInput.y > 0.0f)
                {
                    IsShipMoving = true;
                }
                else
                {
                    IsShipMoving = false;
                }

                transform.position += transform.right * activeSWS * Time.deltaTime;
                transform.position += transform.up * activeHS * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.X)/*OVRInput.GetDown(OVRInput.Button.Three)*/) //X
            {
                IsResourcesPoppedUp = !IsResourcesPoppedUp;
            }
            if (Input.GetKey(KeyCode.Y)/*OVRInput.GetDown(OVRInput.Button.Four)*/) //Y
            {
                IsTerraformingPoppedUp = !IsTerraformingPoppedUp;
            }

            Text credit = GameObject.Find("Credit").GetComponent<Text>();
            Text metal = GameObject.Find("Metal").GetComponent<Text>();
            Text titan = GameObject.Find("Titan").GetComponent<Text>();
            Text plant = GameObject.Find("Plant").GetComponent<Text>();
            Text energy = GameObject.Find("Energy").GetComponent<Text>();
            Text heat = GameObject.Find("Heat").GetComponent<Text>();
            GameObject res = GameObject.Find("UserResources");
            if (credit != null && metal != null && titan != null && plant != null && energy != null && heat != null && res != null)
            {
                res.GetComponent<Canvas>().enabled = IsResourcesPoppedUp;
                credit.GetComponent<Text>().enabled = IsResourcesPoppedUp;
                metal.GetComponent<Text>().enabled = IsResourcesPoppedUp;
                titan.GetComponent<Text>().enabled = IsResourcesPoppedUp;
                plant.GetComponent<Text>().enabled = IsResourcesPoppedUp;
                energy.GetComponent<Text>().enabled = IsResourcesPoppedUp;
                heat.GetComponent<Text>().enabled = IsResourcesPoppedUp;
                res.transform.LookAt(Camera.main.transform);
                res.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
            }

            Text oxygen = GameObject.Find("OxygenLevel").GetComponent<Text>();
            Text temperature = GameObject.Find("TemperatureLevel").GetComponent<Text>();
            Text oceanLevel = GameObject.Find("OceanLevel").GetComponent<Text>();
            Text gen = GameObject.Find("Generation").GetComponent<Text>();
            GameObject terraStatus = GameObject.Find("TerraformingStatus");
            if (oxygen != null && temperature != null && oceanLevel != null && terraStatus != null)
            {
                terraStatus.GetComponent<Canvas>().enabled = IsTerraformingPoppedUp;
                oxygen.GetComponent<Text>().enabled = IsTerraformingPoppedUp;
                temperature.GetComponent<Text>().enabled = IsTerraformingPoppedUp;
                oceanLevel.GetComponent<Text>().enabled = IsTerraformingPoppedUp;
                gen.GetComponent<Text>().enabled = IsTerraformingPoppedUp;
                terraStatus.transform.LookAt(Camera.main.transform);
                terraStatus.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
            }
        }
        else
        {
            Text oxygen = GameObject.Find("OxygenLevel").GetComponent<Text>();
            Text temperature = GameObject.Find("TemperatureLevel").GetComponent<Text>();
            Text oceanLevel = GameObject.Find("OceanLevel").GetComponent<Text>();
            Text gen = GameObject.Find("Generation").GetComponent<Text>();
            Text score = GameObject.Find("Score").GetComponent<Text>();
            GameObject terraStatus = GameObject.Find("TerraformingStatus");
            if (oxygen != null && temperature != null && oceanLevel != null && terraStatus != null)
            {
                oxygen.GetComponent<Text>().enabled = true;
                temperature.GetComponent<Text>().enabled = true;
                oceanLevel.GetComponent<Text>().enabled = true;
                gen.GetComponent<Text>().enabled = true;
                score.GetComponent<Text>().enabled = true;
                score.text = "Score: " + GameDatabaseService.Data.Score;
                terraStatus.transform.LookAt(Camera.main.transform);
                terraStatus.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
            }
        }
    }

    //Function that occurs when user buys something in the shop. (Building)
    public void BuyBuilding(Buildings buildingType)
    {
        string NameOfGameObject;

        switch (buildingType)
        {
            case Buildings.PowerPlant:
                NameOfGameObject = "PowerPlant";
                if (!GameDatabaseService.CanPay(GameDatabaseService.Data.User.Bank, new Cost(10, 0, 0, 0, 0, 0)))
                {
                    return;
                }
                break;
            case Buildings.Greenery:
                NameOfGameObject = "Greenery";
                if (!GameDatabaseService.CanPay(GameDatabaseService.Data.User.Bank, new Cost(15, 0, 0, 0, 0, 0)))
                {
                    return;
                }
                break;
            case Buildings.City:
                NameOfGameObject = "City";
                if (!GameDatabaseService.CanPay(GameDatabaseService.Data.User.Bank, new Cost(25, 0, 0, 0, 0, 0)))
                {
                    return;
                }
                break;
            case Buildings.Ocean:
                NameOfGameObject = "Ocean";
                if (!GameDatabaseService.CanPay(GameDatabaseService.Data.User.Bank, new Cost(20, 0, 0, 0, 0, 0)))
                {
                    return;
                }
                break;
            default:
                NameOfGameObject = "PowerPlant";
                if (!GameDatabaseService.CanPay(GameDatabaseService.Data.User.Bank, new Cost(10, 0, 0, 0, 0, 0)))
                {
                    return;
                }
                break;
        }

        //Search for the actual Hexagon.
        Hexagon SearchedHexagon = GameDatabaseService.Data.GameBoard.Find(h => h.Id == ActualHexagonId);
        if (SearchedHexagon.BuildingModel != null)
        {
            return;
        }
        GameObject Building = Instantiate(GameObject.Find(NameOfGameObject), SearchedHexagon.Territory.transform.position, Quaternion.identity);

        //Rotation for perfect angle.
        float XAngle;
        float YAngle = 180.0f - (SearchedHexagon.Id % 36 * 10.0f);
        float ZAngle = 0.0f;

        if (SearchedHexagon.Id < 37)
        {
            XAngle = 90.0f;
        }
        else if (SearchedHexagon.Id > 36 && SearchedHexagon.Id < 73)
        {
            XAngle = 98.0f;
        }
        else if (SearchedHexagon.Id > 72 && SearchedHexagon.Id < 109)
        {
            XAngle = 82.0f;
        }
        else if (SearchedHexagon.Id > 108 && SearchedHexagon.Id < 145)
        {
            XAngle = 106.5f;
        }
        else
        {
            XAngle = 73.5f;
        }

        Building.transform.Rotate(XAngle + 180.0f, YAngle, ZAngle, Space.World);

        //Setting it into the GameDatabase.
        GameDatabaseService.SetBuildingToHexagon(buildingType, Building, SearchedHexagon.Id);
    }

    //Function that occurs when user buys something in the shop. (GameEvent)
    public void BuyEvent(EventTypes eventType)
    {
        string NameOfGameObject;

        switch (eventType)
        {
            case EventTypes.SpaceMine:
                NameOfGameObject = "SpaceMine";
                if (!GameDatabaseService.CanPay(GameDatabaseService.Data.User.Bank, new Cost(10, 10, 10, 0, 10, 0)))
                {
                    return;
                }
                break;
            case EventTypes.Satellite:
                NameOfGameObject = "Satellite";
                if (!GameDatabaseService.CanPay(GameDatabaseService.Data.User.Bank, new Cost(5, 0, 5, 0, 5, 5)))
                {
                    return;
                }
                break;
            case EventTypes.SpaceStation:
                NameOfGameObject = "SpaceStation";
                if (!GameDatabaseService.CanPay(GameDatabaseService.Data.User.Bank, new Cost(25, 0, 10, 0, 0, 10)))
                {
                    return;
                }
                break;
            default:
                NameOfGameObject = "SpaceMine";
                if (!GameDatabaseService.CanPay(GameDatabaseService.Data.User.Bank, new Cost(10, 10, 0, 0, 10, 0)))
                {
                    return;
                }
                break;
        }

        //Search for the actual Hexagon.
        Hexagon SearchedHexagon = GameDatabaseService.Data.GameBoard.Find(h => h.Id == ActualHexagonId);
        foreach (GameEvent ge in GameDatabaseService.Data.Events)
        {
            if (ge.HexagonId == ActualHexagonId)
            {
                return;
            }
        }
        GameObject eventObject = Instantiate(GameObject.Find(NameOfGameObject), SearchedHexagon.Territory.transform.position, Quaternion.identity);

        //Rotation for perfect angle.
        float XAngle;
        float YAngle = 180.0f - (SearchedHexagon.Id % 36 * 10.0f);
        float ZAngle = 0.0f;

        if (SearchedHexagon.Id < 37)
        {
            XAngle = 90.0f;
        }
        else if (SearchedHexagon.Id > 36 && SearchedHexagon.Id < 73)
        {
            XAngle = 98.0f;
        }
        else if (SearchedHexagon.Id > 72 && SearchedHexagon.Id < 109)
        {
            XAngle = 82.0f;
        }
        else if (SearchedHexagon.Id > 108 && SearchedHexagon.Id < 145)
        {
            XAngle = 106.5f;
        }
        else
        {
            XAngle = 73.5f;
        }

        eventObject.transform.Rotate(XAngle + 180.0f, YAngle, ZAngle, Space.World);

        //Setting it into the GameDatabase.
        GameDatabaseService.SetGameEvent(eventType, eventObject, SearchedHexagon.Id);
    }
}
