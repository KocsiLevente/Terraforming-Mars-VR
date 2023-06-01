using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Comment out OVRTouchSample if working without VR headset.
using OVRTouchSample;
using System;
using System.Text;

//Class for managing the interface in the menus.
public class MainMenuInterface : MonoBehaviour
{
    private bool IsGameStarted = false;
    private bool IsLoadGame = false;
    private bool IsInLoadMenu = false;
    private bool IsInOptions = false;
    private bool IsInMainMenu = true;
    private bool IsInMultiplayerMenu = false;
    private bool IsMultiplayerGame = false;
    private bool RightThumbIsReset = true;

    //Multiplayer related.
    private bool IsUserLoggedIn = false;
    public static bool IsInGameRoom = false;
    private bool IsJoiningGameRoom = false;
    private bool IsInvitingPlayer = false;
    private bool IsKickingPlayer = false;
    public static bool IsGameRoomLeader = false;
    private int SelectedGameRoomIndex = 0;
    private GameRoom SelectedGameRoom = null;
    private int SelectedUserIndex = 0;
    private TerraformingMarsUser SelectedUser = null;

    public int SelectedMenuItemId, SelectedLoadMenuItemId, SelectedOptionsMenuItemId, SelectedMultiplayerMenuItemId = 1;

    private Vector2 menuDirInput, loadMenuDirInput, optionsMenuDirInput, multiplayerMenuDirInput;

    GameObject cameraInShip;
    GameObject spaceShip;

    //Menu objects.
    GameObject menuObjects;
    GameObject loadObjects;
    GameObject optionsObjects;
    GameObject multiplayerObjects;

    //Main menu.
    Text selectorBody1;
    Text selectorBody2;
    Text selectorBody3;
    Text selectorBody4;
    Text selectorBody5;
    Text start;
    Text load;
    Text multi;
    Text multiUserName;
    GameObject multiUserNameInput;
    Text options;
    Text exit;

    //Load menu.
    Text loadSelectorBody1;
    Text loadSelectorBody2;
    Text loadSelectorBody3;
    Text loadSelectorBody4;
    Text slot1;
    Text slot2;
    Text slot3;
    Text backFromLoad;
    Text gameInfo;

    //Multiplayer menu.
    Text multiplayerSelectorBody1;
    Text multiplayerSelectorBody2;
    Text multiplayerSelectorBody3;
    Text multiplayerSelectorBody4;
    Text multiplayerSelectorBody5;
    Text createGameRoom;
    Text joinGameRoom;
    Text leaveGameRoom;
    Text startGame;
    Text sendChatMessage;
    Text sendChatMessageTitle;
    GameObject sendChatMessageInput;
    Text inviteUser;
    Text kickUser;
    Text back;
    GameObject gameRoomListView;
    GameObject chatMessageListView;
    GameObject usersListView;
    GameObject gameRoomListTitle;
    Text chatMessageListTitle;

    //Options menu.
    Text optionsSelectorBody1;
    Text optionsSelectorBody2;
    Text optionsSelectorBody3;
    Text optionsSelectorBody4;
    Text easy;
    Text medium;
    Text hard;
    Text backOptions;

    void Start()
    {
        cameraInShip = GameObject.Find("First Person Controller");
        spaceShip = GameObject.Find("SpaceShip");

        //Menu objects.
        menuObjects = GameObject.Find("MainMenuView");
        loadObjects = GameObject.Find("LoadGameView");
        optionsObjects = GameObject.Find("OptionsView");
        multiplayerObjects = GameObject.Find("MultiplayerMenuView");

        //Main menu.
        selectorBody1 = GameObject.Find("MainMenuSelector1").GetComponent<Text>();
        selectorBody2 = GameObject.Find("MainMenuSelector2").GetComponent<Text>();
        selectorBody3 = GameObject.Find("MainMenuSelector3").GetComponent<Text>();
        selectorBody4 = GameObject.Find("MainMenuSelector4").GetComponent<Text>();
        selectorBody5 = GameObject.Find("MainMenuSelector5").GetComponent<Text>();
        start = GameObject.Find("StartNewGame").GetComponent<Text>();
        load = GameObject.Find("LoadGame").GetComponent<Text>();
        multi = GameObject.Find("Multiplayer").GetComponent<Text>();
        multiUserName = GameObject.Find("MultiplayerUserName").GetComponent<Text>();
        multiUserNameInput = GameObject.Find("MultiplayerUserNameInput");
        options = GameObject.Find("Options").GetComponent<Text>();
        exit = GameObject.Find("Exit").GetComponent<Text>();

        //Load menu.
        loadSelectorBody1 = GameObject.Find("LoadMenuSelector1").GetComponent<Text>();
        loadSelectorBody2 = GameObject.Find("LoadMenuSelector2").GetComponent<Text>();
        loadSelectorBody3 = GameObject.Find("LoadMenuSelector3").GetComponent<Text>();
        loadSelectorBody4 = GameObject.Find("LoadMenuSelector4").GetComponent<Text>();
        slot1 = GameObject.Find("LoadSlot1").GetComponent<Text>();
        slot2 = GameObject.Find("LoadSlot2").GetComponent<Text>();
        slot3 = GameObject.Find("LoadSlot3").GetComponent<Text>();
        backFromLoad = GameObject.Find("BackFromLoad").GetComponent<Text>();
        gameInfo = GameObject.Find("GameInformation").GetComponent<Text>();

        //Multiplayer menu.
        multiplayerSelectorBody1 = GameObject.Find("MultiplayerMenuSelector1").GetComponent<Text>();
        multiplayerSelectorBody2 = GameObject.Find("MultiplayerMenuSelector2").GetComponent<Text>();
        multiplayerSelectorBody3 = GameObject.Find("MultiplayerMenuSelector3").GetComponent<Text>();
        multiplayerSelectorBody4 = GameObject.Find("MultiplayerMenuSelector4").GetComponent<Text>();
        multiplayerSelectorBody5 = GameObject.Find("MultiplayerMenuSelector5").GetComponent<Text>();
        createGameRoom = GameObject.Find("CreateGameRoom").GetComponent<Text>();
        joinGameRoom = GameObject.Find("JoinGameRoom").GetComponent<Text>();
        leaveGameRoom = GameObject.Find("LeaveGameRoom").GetComponent<Text>();
        startGame = GameObject.Find("StartGameMultiplayer").GetComponent<Text>();
        sendChatMessage = GameObject.Find("SendChatMessage").GetComponent<Text>();
        sendChatMessageTitle = GameObject.Find("SendChatMessageTitle").GetComponent<Text>();
        sendChatMessageInput = GameObject.Find("SendChatMessageInput");
        inviteUser = GameObject.Find("InviteUser").GetComponent<Text>();
        kickUser = GameObject.Find("KickUser").GetComponent<Text>();
        back = GameObject.Find("BackFromMultiplayer").GetComponent<Text>();
        gameRoomListView = GameObject.Find("GameRoomListView");
        chatMessageListView = GameObject.Find("ChatMessageListView");
        usersListView = GameObject.Find("TerraformingMarsUserListView");
        gameRoomListTitle = GameObject.Find("GameRoomTitle");
        chatMessageListTitle = GameObject.Find("ChatMessageTitle").GetComponent<Text>();

        //Options menu.
        optionsSelectorBody1 = GameObject.Find("OptionsMenuSelector1").GetComponent<Text>();
        optionsSelectorBody2 = GameObject.Find("OptionsMenuSelector2").GetComponent<Text>();
        optionsSelectorBody3 = GameObject.Find("OptionsMenuSelector3").GetComponent<Text>();
        optionsSelectorBody4 = GameObject.Find("OptionsMenuSelector4").GetComponent<Text>();
        easy = GameObject.Find("Easy").GetComponent<Text>();
        medium = GameObject.Find("Medium").GetComponent<Text>();
        hard = GameObject.Find("Hard").GetComponent<Text>();
        backOptions = GameObject.Find("BackFromOptions").GetComponent<Text>();

        //Hide the unneeded canvases.
        GameObject status = GameObject.Find("TerraformingStatus");
        if (status != null)
        {
            status.GetComponent<Canvas>().enabled = false;
        }
        GameObject res = GameObject.Find("UserResources");
        if (res != null)
        {
            res.GetComponent<Canvas>().enabled = false;
        }
        GameObject shopObj = GameObject.Find("ShopObjects");
        if (shopObj != null)
        {
            shopObj.GetComponent<Canvas>().enabled = false;
        }

        if (loadObjects != null)
        {
            loadObjects.GetComponent<Canvas>().enabled = false;
        }
        if (optionsObjects != null)
        {
            optionsObjects.GetComponent<Canvas>().enabled = false;
        }
        if (multiplayerObjects != null)
        {
            multiplayerObjects.GetComponent<Canvas>().enabled = false;
        }
    }

    void Update()
    {
        //If the game is started hide the menus.
        if (IsGameStarted && spaceShip != null)
        {
            spaceShip.GetComponent<ShipInterface>().enabled = true;
            if (menuObjects != null && loadObjects != null && optionsObjects != null)
            {
                menuObjects.GetComponent<Canvas>().enabled = false;
                loadObjects.GetComponent<Canvas>().enabled = false;
                optionsObjects.GetComponent<Canvas>().enabled = false;
                multiplayerObjects.GetComponent<Canvas>().enabled = false;
            }
        }
        //If the game is not started yet we are in a menu.
        else if (!IsGameStarted)
        {
            //Main menu
            if (IsInMainMenu)
            {
                HandleMainMenu();
            }
            //Load menu
            else if (IsInLoadMenu)
            {
                HandleLoadMenu();
            }
            //Multiplayer menu.
            else if (IsInMultiplayerMenu)
            {
                HandleMultiplayerMenu();
            }
            //Options menu.
            else if (IsInOptions)
            {
                HandleOptionsMenu();
            }
        }
    }

    void HandleMainMenu()
    {
        if (TerraformingMarsClient.isJoined)
        {
            IsUserLoggedIn = true;
            IsInMultiplayerMenu = true;
            IsInMainMenu = false;
        }

        if (cameraInShip != null)
        {
            cameraInShip.transform.localPosition = new Vector3(0.0f, 1.5f, -2400.0f);
        }

        if (selectorBody1 != null && selectorBody2 != null && selectorBody3 != null && selectorBody4 != null && selectorBody5 != null && menuObjects != null && start != null && load != null &&
            multi != null && multiUserName != null && multiUserNameInput != null && options != null && exit != null)
        {
            menuObjects.transform.LookAt(Camera.main.transform);
            menuObjects.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
            //Move the selector if showed.
            switch (SelectedMenuItemId)
            {
                case 1:
                    selectorBody1.GetComponent<Text>().enabled = true;
                    selectorBody2.GetComponent<Text>().enabled = false;
                    selectorBody3.GetComponent<Text>().enabled = false;
                    selectorBody4.GetComponent<Text>().enabled = false;
                    selectorBody5.GetComponent<Text>().enabled = false;
                    start.color = new Color32(255, 142, 0, 255);
                    load.color = new Color32(255, 69, 0, 255);
                    multi.color = new Color32(255, 69, 0, 255);
                    multiUserName.enabled = false;
                    multiUserNameInput.SetActive(false);
                    options.color = new Color32(255, 69, 0, 255);
                    exit.color = new Color32(255, 69, 0, 255);
                    break;
                case 2:
                    selectorBody1.GetComponent<Text>().enabled = false;
                    selectorBody2.GetComponent<Text>().enabled = true;
                    selectorBody3.GetComponent<Text>().enabled = false;
                    selectorBody4.GetComponent<Text>().enabled = false;
                    selectorBody5.GetComponent<Text>().enabled = false;
                    start.color = new Color32(255, 69, 0, 255);
                    load.color = new Color32(255, 142, 0, 255);
                    multi.color = new Color32(255, 69, 0, 255);
                    multiUserName.enabled = false;
                    multiUserNameInput.SetActive(false);
                    options.color = new Color32(255, 69, 0, 255);
                    exit.color = new Color32(255, 69, 0, 255);
                    break;
                case 3:
                    selectorBody1.GetComponent<Text>().enabled = false;
                    selectorBody2.GetComponent<Text>().enabled = false;
                    selectorBody3.GetComponent<Text>().enabled = true;
                    selectorBody4.GetComponent<Text>().enabled = false;
                    selectorBody5.GetComponent<Text>().enabled = false;
                    multiUserName.enabled = true;
                    multiUserNameInput.SetActive(true);
                    start.color = new Color32(255, 69, 0, 255);
                    load.color = new Color32(255, 69, 0, 255);
                    multi.color = new Color32(255, 142, 0, 255);
                    options.color = new Color32(255, 69, 0, 255);
                    exit.color = new Color32(255, 69, 0, 255);
                    break;
                case 4:
                    selectorBody1.GetComponent<Text>().enabled = false;
                    selectorBody2.GetComponent<Text>().enabled = false;
                    selectorBody3.GetComponent<Text>().enabled = false;
                    selectorBody4.GetComponent<Text>().enabled = true;
                    selectorBody5.GetComponent<Text>().enabled = false;
                    start.color = new Color32(255, 69, 0, 255);
                    load.color = new Color32(255, 69, 0, 255);
                    multi.color = new Color32(255, 69, 0, 255);
                    multiUserName.enabled = false;
                    multiUserNameInput.SetActive(false);
                    options.color = new Color32(255, 142, 0, 255);
                    exit.color = new Color32(255, 69, 0, 255);
                    break;
                case 5:
                    selectorBody1.GetComponent<Text>().enabled = false;
                    selectorBody2.GetComponent<Text>().enabled = false;
                    selectorBody3.GetComponent<Text>().enabled = false;
                    selectorBody4.GetComponent<Text>().enabled = false;
                    selectorBody5.GetComponent<Text>().enabled = true;
                    start.color = new Color32(255, 69, 0, 255);
                    load.color = new Color32(255, 69, 0, 255);
                    multi.color = new Color32(255, 69, 0, 255);
                    multiUserName.enabled = false;
                    multiUserNameInput.SetActive(false);
                    options.color = new Color32(255, 69, 0, 255);
                    exit.color = new Color32(255, 142, 0, 255);
                    break;
                default:
                    break;
            }
        }

        //Use the outcommented row and do not use OVRInput if testing without VR headset.
        menuDirInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        //menuDirInput = Input.GetKeyDown(KeyCode.DownArrow) ? new Vector2(0f, -0.5f) : new Vector2(0f, 0f);
        if (RightThumbIsReset)
        {
            if (menuDirInput.y < -0.25f)
            {
                RightThumbIsReset = false;
                if (SelectedMenuItemId == 5)
                {
                    SelectedMenuItemId = 1;
                }
                else
                {
                    SelectedMenuItemId++;
                }
            }
            if (menuDirInput.y > 0.25f)
            {
                RightThumbIsReset = false;
                if (SelectedMenuItemId == 1)
                {
                    SelectedMenuItemId = 5;
                }
                else
                {
                    SelectedMenuItemId--;
                }
            }
        }
        if (menuDirInput.x == 0.0f && menuDirInput.y == 0.0f)
        {
            RightThumbIsReset = true;
        }

        //Select the actual menu item.
        //Use the outcommented row and do not use OVRInput if testing without VR headset.
        if (OVRInput.GetDown(OVRInput.Button.One))
        //if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            switch (SelectedMenuItemId)
            {
                case 1:
                    //Start the game.
                    IsGameStarted = true;
                    break;
                case 2:
                    //Navigate to the load menu.
                    IsInLoadMenu = true;
                    IsInMainMenu = false;
                    break;
                case 3:
                    //Navigate to the multiplayer menu.
                    if (ReadMultiplayerInput.inputUserName != null && ReadMultiplayerInput.inputUserName.Length > 0)
                    {
                        TerraformingMarsClient.StartClient();
                    }
                    break;
                case 4:
                    //Navigate to the options menu.
                    IsInOptions = true;
                    IsInMainMenu = false;
                    break;
                case 5:
                    //Quit.
                    Application.Quit();
                    break;
                default:
                    break;
            }
        }
    }

    void HandleLoadMenu()
    {
        if (cameraInShip != null)
        {
            cameraInShip.transform.localPosition = new Vector3(0.0f, 1.5f, 300.0f);
        }

        if (loadObjects != null)
        {
            loadObjects.GetComponent<Canvas>().enabled = true;
        }

        if (loadSelectorBody1 != null && loadSelectorBody2 != null && loadSelectorBody3 != null && loadSelectorBody4 != null && loadObjects != null && slot1 != null && slot2 != null && slot3 != null && backFromLoad != null && gameInfo != null)
        {
            loadObjects.transform.LookAt(Camera.main.transform);
            loadObjects.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
            //Move the selector if showed.
            switch (SelectedLoadMenuItemId)
            {
                case 1:
                    loadSelectorBody1.GetComponent<Text>().enabled = true;
                    loadSelectorBody2.GetComponent<Text>().enabled = false;
                    loadSelectorBody3.GetComponent<Text>().enabled = false;
                    loadSelectorBody4.GetComponent<Text>().enabled = false;
                    slot1.color = new Color32(255, 142, 0, 255);
                    slot2.color = new Color32(255, 69, 0, 255);
                    slot3.color = new Color32(255, 69, 0, 255);
                    backFromLoad.color = new Color32(255, 69, 0, 255);
                    break;
                case 2:
                    loadSelectorBody1.GetComponent<Text>().enabled = false;
                    loadSelectorBody2.GetComponent<Text>().enabled = true;
                    loadSelectorBody3.GetComponent<Text>().enabled = false;
                    loadSelectorBody4.GetComponent<Text>().enabled = false;
                    slot1.color = new Color32(255, 69, 0, 255);
                    slot2.color = new Color32(255, 142, 0, 255);
                    slot3.color = new Color32(255, 69, 0, 255);
                    backFromLoad.color = new Color32(255, 69, 0, 255);
                    break;
                case 3:
                    loadSelectorBody1.GetComponent<Text>().enabled = false;
                    loadSelectorBody2.GetComponent<Text>().enabled = false;
                    loadSelectorBody3.GetComponent<Text>().enabled = true;
                    loadSelectorBody4.GetComponent<Text>().enabled = false;
                    slot1.color = new Color32(255, 69, 0, 255);
                    slot2.color = new Color32(255, 69, 0, 255);
                    slot3.color = new Color32(255, 142, 0, 255);
                    backFromLoad.color = new Color32(255, 69, 0, 255);
                    break;
                case 4:
                    loadSelectorBody1.GetComponent<Text>().enabled = false;
                    loadSelectorBody2.GetComponent<Text>().enabled = false;
                    loadSelectorBody3.GetComponent<Text>().enabled = false;
                    loadSelectorBody4.GetComponent<Text>().enabled = true;
                    slot1.color = new Color32(255, 69, 0, 255);
                    slot2.color = new Color32(255, 69, 0, 255);
                    slot3.color = new Color32(255, 69, 0, 255);
                    backFromLoad.color = new Color32(255, 142, 0, 255);
                    break;
                default:
                    break;
            }
        }

        //Use the outcommented row and do not use OVRInput if testing without VR headset.
        loadMenuDirInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        //loadMenuDirInput = Input.GetKeyDown(KeyCode.DownArrow) ? new Vector2(0f, -0.5f) : new Vector2(0f, 0f);
        {
            if (loadMenuDirInput.y < -0.25f)
            {
                RightThumbIsReset = false;
                if (SelectedLoadMenuItemId == 4)
                {
                    SelectedLoadMenuItemId = 1;
                }
                else
                {
                    SelectedLoadMenuItemId++;
                }
            }
            if (loadMenuDirInput.y > 0.25f)
            {
                RightThumbIsReset = false;
                if (SelectedLoadMenuItemId == 1)
                {
                    SelectedLoadMenuItemId = 4;
                }
                else
                {
                    SelectedLoadMenuItemId--;
                }
            }
        }
        if (loadMenuDirInput.x == 0.0f && loadMenuDirInput.y == 0.0f)
        {
            RightThumbIsReset = true;
        }

        //Select the menu item.
        //Use the outcommented row and do not use OVRInput if testing without VR headset.
        if (OVRInput.GetDown(OVRInput.Button.One))
        //if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            string resFolderPath = Application.dataPath + "\\Resources\\";

            switch (SelectedLoadMenuItemId)
            {
                case 1:
                    //Load a previous game and start on success.
                    GameDatabaseService.LoadGameData();
                    CreateGameObjects();
                    IsGameStarted = true;
                    break;
                case 2:
                    GameDatabaseService.LoadGameData();
                    CreateGameObjects();
                    IsGameStarted = true;
                    break;
                case 3:
                    GameDatabaseService.LoadGameData();
                    CreateGameObjects();
                    IsGameStarted = true;
                    break;
                case 4:
                    //Go back to main menu.
                    IsInLoadMenu = false;
                    if (loadObjects != null)
                    {
                        loadObjects.GetComponent<Canvas>().enabled = false;
                    }
                    IsInMainMenu = true;
                    break;
                default:
                    break;
            }
        }
    }

    void HandleMultiplayerMenu()
    {
        if (!TerraformingMarsClient.isJoined)
        {
            IsInMultiplayerMenu = false;
            IsUserLoggedIn = false;
            if (multiplayerObjects != null)
            {
                multiplayerObjects.GetComponent<Canvas>().enabled = false;
            }
            IsInMainMenu = true;
        }

        if (cameraInShip != null)
        {
            cameraInShip.transform.localPosition = new Vector3(-900.0f, -40.0f, 975.0f);
        }

        if (multiplayerObjects != null)
        {
            multiplayerObjects.GetComponent<Canvas>().enabled = true;
        }

        multiplayerObjects.transform.LookAt(Camera.main.transform);
        multiplayerObjects.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));

        if (IsUserLoggedIn && TerraformingMarsClient.isJoined)
        {
            if (multiplayerSelectorBody1 != null && multiplayerSelectorBody2 != null && multiplayerSelectorBody3 != null && multiplayerSelectorBody4 != null && multiplayerSelectorBody5 != null &&
                createGameRoom != null && joinGameRoom != null && leaveGameRoom != null && startGame != null && sendChatMessage != null && sendChatMessageTitle && sendChatMessageInput &&
                inviteUser != null && kickUser != null && back != null && gameRoomListView != null && chatMessageListView != null && usersListView != null)
            {
                if (IsInGameRoom)
                {
                    gameRoomListTitle.SetActive(false);
                    gameRoomListView.SetActive(false);
                    chatMessageListTitle.text = "Room chat";
                    if (IsGameRoomLeader)
                    {
                        createGameRoom.GetComponent<Text>().enabled = false;
                        joinGameRoom.GetComponent<Text>().enabled = false;
                        leaveGameRoom.GetComponent<Text>().enabled = true;
                        startGame.GetComponent<Text>().enabled = true;
                        sendChatMessage.GetComponent<Text>().enabled = true;
                        inviteUser.GetComponent<Text>().enabled = true;
                        kickUser.GetComponent<Text>().enabled = true;
                        back.GetComponent<Text>().enabled = false;
                    }
                    else
                    {
                        createGameRoom.GetComponent<Text>().enabled = false;
                        joinGameRoom.GetComponent<Text>().enabled = false;
                        leaveGameRoom.GetComponent<Text>().enabled = true;
                        startGame.GetComponent<Text>().enabled = false;
                        sendChatMessage.GetComponent<Text>().enabled = true;
                        inviteUser.GetComponent<Text>().enabled = true;
                        kickUser.GetComponent<Text>().enabled = false;
                        back.GetComponent<Text>().enabled = false;
                    }
                }
                else
                {
                    gameRoomListTitle.SetActive(true);
                    gameRoomListView.SetActive(true);
                    chatMessageListTitle.text = "Lobby chat";
                    createGameRoom.GetComponent<Text>().enabled = true;
                    joinGameRoom.GetComponent<Text>().enabled = true;
                    leaveGameRoom.GetComponent<Text>().enabled = false;
                    startGame.GetComponent<Text>().enabled = false;
                    sendChatMessage.GetComponent<Text>().enabled = true;
                    inviteUser.GetComponent<Text>().enabled = false;
                    kickUser.GetComponent<Text>().enabled = false;
                    back.GetComponent<Text>().enabled = true;
                }

                //Move the selector if showed.
                switch (SelectedMultiplayerMenuItemId)
                {
                    case 1:
                        multiplayerSelectorBody1.GetComponent<Text>().enabled = true;
                        multiplayerSelectorBody2.GetComponent<Text>().enabled = false;
                        multiplayerSelectorBody3.GetComponent<Text>().enabled = false;
                        multiplayerSelectorBody4.GetComponent<Text>().enabled = false;
                        multiplayerSelectorBody5.GetComponent<Text>().enabled = false;

                        sendChatMessageTitle.GetComponent<Text>().enabled = false;
                        sendChatMessageInput.SetActive(false);

                        if (IsInGameRoom)
                        {
                            if (IsGameRoomLeader)
                            {
                                startGame.color = new Color32(255, 142, 0, 255);
                                inviteUser.color = new Color32(255, 69, 0, 255);
                                sendChatMessage.color = new Color32(255, 69, 0, 255);
                                leaveGameRoom.color = new Color32(255, 69, 0, 255);
                                kickUser.color = new Color32(255, 69, 0, 255);
                            }
                        }
                        else
                        {
                            createGameRoom.color = new Color32(255, 142, 0, 255);
                            joinGameRoom.color = new Color32(255, 69, 0, 255);
                            sendChatMessage.color = new Color32(255, 69, 0, 255);
                            back.color = new Color32(255, 69, 0, 255);
                        }
                        break;
                    case 2:
                        multiplayerSelectorBody1.GetComponent<Text>().enabled = false;
                        multiplayerSelectorBody2.GetComponent<Text>().enabled = true;
                        multiplayerSelectorBody3.GetComponent<Text>().enabled = false;
                        multiplayerSelectorBody4.GetComponent<Text>().enabled = false;
                        multiplayerSelectorBody5.GetComponent<Text>().enabled = false;

                        sendChatMessageTitle.GetComponent<Text>().enabled = false;
                        sendChatMessageInput.SetActive(false);

                        if (IsInGameRoom)
                        {
                            if (IsGameRoomLeader)
                            {
                                startGame.color = new Color32(255, 69, 0, 255);
                                inviteUser.color = new Color32(255, 142, 0, 255);
                                sendChatMessage.color = new Color32(255, 69, 0, 255);
                                leaveGameRoom.color = new Color32(255, 69, 0, 255);
                                kickUser.color = new Color32(255, 69, 0, 255);
                            }
                            else
                            {
                                inviteUser.color = new Color32(255, 142, 0, 255);
                                sendChatMessage.color = new Color32(255, 69, 0, 255);
                                leaveGameRoom.color = new Color32(255, 69, 0, 255);
                            }
                        }
                        else
                        {
                            createGameRoom.color = new Color32(255, 69, 0, 255);
                            joinGameRoom.color = new Color32(255, 142, 0, 255);
                            sendChatMessage.color = new Color32(255, 69, 0, 255);
                            back.color = new Color32(255, 69, 0, 255);
                        }
                        break;
                    case 3:
                        multiplayerSelectorBody1.GetComponent<Text>().enabled = false;
                        multiplayerSelectorBody2.GetComponent<Text>().enabled = false;
                        multiplayerSelectorBody3.GetComponent<Text>().enabled = true;
                        multiplayerSelectorBody4.GetComponent<Text>().enabled = false;
                        multiplayerSelectorBody5.GetComponent<Text>().enabled = false;

                        sendChatMessageTitle.GetComponent<Text>().enabled = true;
                        sendChatMessageInput.SetActive(true);

                        if (IsInGameRoom)
                        {
                            if (IsGameRoomLeader)
                            {
                                startGame.color = new Color32(255, 69, 0, 255);
                                inviteUser.color = new Color32(255, 69, 0, 255);
                                sendChatMessage.color = new Color32(255, 142, 0, 255);
                                leaveGameRoom.color = new Color32(255, 69, 0, 255);
                                kickUser.color = new Color32(255, 69, 0, 255);
                            }
                            else
                            {
                                inviteUser.color = new Color32(255, 69, 0, 255);
                                sendChatMessage.color = new Color32(255, 142, 0, 255);
                                leaveGameRoom.color = new Color32(255, 69, 0, 255);
                            }
                        }
                        else
                        {
                            createGameRoom.color = new Color32(255, 69, 0, 255);
                            joinGameRoom.color = new Color32(255, 69, 0, 255);
                            sendChatMessage.color = new Color32(255, 142, 0, 255);
                            back.color = new Color32(255, 69, 0, 255);
                        }
                        break;
                    case 4:
                        multiplayerSelectorBody1.GetComponent<Text>().enabled = false;
                        multiplayerSelectorBody2.GetComponent<Text>().enabled = false;
                        multiplayerSelectorBody3.GetComponent<Text>().enabled = false;
                        multiplayerSelectorBody4.GetComponent<Text>().enabled = true;
                        multiplayerSelectorBody5.GetComponent<Text>().enabled = false;

                        sendChatMessageTitle.GetComponent<Text>().enabled = false;
                        sendChatMessageInput.SetActive(false);

                        if (IsInGameRoom)
                        {
                            if (IsGameRoomLeader)
                            {
                                startGame.color = new Color32(255, 69, 0, 255);
                                inviteUser.color = new Color32(255, 69, 0, 255);
                                sendChatMessage.color = new Color32(255, 69, 0, 255);
                                leaveGameRoom.color = new Color32(255, 142, 0, 255);
                                kickUser.color = new Color32(255, 69, 0, 255);
                            }
                            else
                            {
                                inviteUser.color = new Color32(255, 69, 0, 255);
                                sendChatMessage.color = new Color32(255, 69, 0, 255);
                                leaveGameRoom.color = new Color32(255, 142, 0, 255);
                            }
                        }
                        else
                        {
                            createGameRoom.color = new Color32(255, 69, 0, 255);
                            joinGameRoom.color = new Color32(255, 69, 0, 255);
                            sendChatMessage.color = new Color32(255, 69, 0, 255);
                            back.color = new Color32(255, 142, 0, 255);
                        }
                        break;
                    case 5:
                        multiplayerSelectorBody1.GetComponent<Text>().enabled = false;
                        multiplayerSelectorBody2.GetComponent<Text>().enabled = false;
                        multiplayerSelectorBody3.GetComponent<Text>().enabled = false;
                        multiplayerSelectorBody4.GetComponent<Text>().enabled = false;
                        multiplayerSelectorBody5.GetComponent<Text>().enabled = true;

                        sendChatMessageTitle.GetComponent<Text>().enabled = false;
                        sendChatMessageInput.SetActive(false);

                        if (IsInGameRoom)
                        {
                            if (IsGameRoomLeader)
                            {
                                startGame.color = new Color32(255, 69, 0, 255);
                                inviteUser.color = new Color32(255, 69, 0, 255);
                                sendChatMessage.color = new Color32(255, 69, 0, 255);
                                leaveGameRoom.color = new Color32(255, 69, 0, 255);
                                kickUser.color = new Color32(255, 142, 0, 255);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            //Use the outcommented row and do not use OVRInput if testing without VR headset.
            multiplayerMenuDirInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
            //multiplayerMenuDirInput = Input.GetKeyDown(KeyCode.DownArrow) ? new Vector2(0f, -0.5f) : new Vector2(0f, 0f);
            if (RightThumbIsReset)
            {
                if (multiplayerMenuDirInput.y < -0.25f)
                {
                    RightThumbIsReset = false;
                    
                    if (IsJoiningGameRoom)
                    {
                        //Deselect
                        if (SelectedGameRoom != null)
                        {
                            GameRoomAdapter.views.Find(grv => grv.room.Id == SelectedGameRoom.Id).OnSelected(true);
                        }
                        //Select
                        if (GameRoomAdapter.views.Count > SelectedGameRoomIndex + 1)
                        {
                            SelectedGameRoomIndex++;
                            SelectedGameRoom = GameRoomAdapter.views[SelectedGameRoomIndex].room;
                            GameRoomAdapter.views[SelectedGameRoomIndex].OnSelected(false);
                        }
                        else if (GameRoomAdapter.views.Count > 0)
                        {
                            SelectedGameRoomIndex = 0;
                            SelectedGameRoom = GameRoomAdapter.views[SelectedGameRoomIndex].room;
                            GameRoomAdapter.views[SelectedGameRoomIndex].OnSelected(false);
                        }
                        else
                        {
                            SelectedGameRoom = null;
                            SelectedGameRoomIndex = 0;
                        }
                    }
                    else if (IsInvitingPlayer)
                    {
                        //Deselect
                        if (SelectedUser != null)
                        {
                            TerraformingMarsUserAdapter.views.Find(tmuv => tmuv.user.OuterId == SelectedUser.OuterId).OnSelected(true);
                        }
                        //Select
                        if (TerraformingMarsUserAdapter.views.Count > SelectedUserIndex + 1)
                        {
                            SelectedUserIndex++;
                            SelectedUser = TerraformingMarsUserAdapter.views[SelectedUserIndex].user;
                            TerraformingMarsUserAdapter.views[SelectedUserIndex].OnSelected(false);
                        }
                        else if (TerraformingMarsUserAdapter.views.Count > 0)
                        {
                            SelectedUserIndex = 0;
                            SelectedUser = TerraformingMarsUserAdapter.views[SelectedUserIndex].user;
                            TerraformingMarsUserAdapter.views[SelectedUserIndex].OnSelected(false);
                        }
                        else
                        {
                            SelectedUser = null;
                            SelectedUserIndex = 0;
                        }
                    }
                    else
                    {
                        if (IsInGameRoom)
                        {
                            if (IsGameRoomLeader)
                            {
                                if (SelectedMultiplayerMenuItemId == 5)
                                {
                                    SelectedMultiplayerMenuItemId = 1;
                                }
                                else
                                {
                                    SelectedMultiplayerMenuItemId++;
                                }
                            }
                            else
                            {
                                if (SelectedMultiplayerMenuItemId == 4)
                                {
                                    SelectedMultiplayerMenuItemId = 2;
                                }
                                else
                                {
                                    SelectedMultiplayerMenuItemId++;
                                }
                            }
                        }
                        else
                        {
                            if (SelectedMultiplayerMenuItemId == 4)
                            {
                                SelectedMultiplayerMenuItemId = 1;
                            }
                            else
                            {
                                SelectedMultiplayerMenuItemId++;
                            }
                        }
                    }
                }
                if (multiplayerMenuDirInput.y > 0.25f)
                {
                    RightThumbIsReset = false;
                    if (IsInGameRoom)
                    {
                        if (IsGameRoomLeader)
                        {
                            if (SelectedMultiplayerMenuItemId == 1)
                            {
                                SelectedMultiplayerMenuItemId = 5;
                            }
                            else
                            {
                                SelectedMultiplayerMenuItemId--;
                            }
                        }
                        else
                        {
                            if (SelectedMultiplayerMenuItemId == 2)
                            {
                                SelectedMultiplayerMenuItemId = 4;
                            }
                            else
                            {
                                SelectedMultiplayerMenuItemId--;
                            }
                        }
                    }
                    else
                    {
                        if (SelectedMultiplayerMenuItemId == 1)
                        {
                            SelectedMultiplayerMenuItemId = 4;
                        }
                        else
                        {
                            SelectedMultiplayerMenuItemId--;
                        }
                    }
                }
            }
            if (multiplayerMenuDirInput.x == 0.0f && multiplayerMenuDirInput.y == 0.0f)
            {
                RightThumbIsReset = true;
            }

            //Select the menu item.
            //Use the outcommented row and do not use OVRInput if testing without VR headset.
            if (OVRInput.GetDown(OVRInput.Button.One))
            //if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (IsInGameRoom)
                {
                    if (IsGameRoomLeader)
                    {
                        switch (SelectedMultiplayerMenuItemId)
                        {
                            case 1:
                                //Start the game.
                                IsGameStarted = true;
                                IsMultiplayerGame = true;
                                TerraformingMarsClient.StartGame(SelectedGameRoom.Id);
                                break;
                            case 2:
                                //Invite player.
                                IsInvitingPlayer = true;
                                SelectedUserIndex = 0;
                                if (TerraformingMarsUserAdapter.views.Count > 0)
                                {
                                    SelectedUser = TerraformingMarsUserAdapter.views[SelectedUserIndex].user;
                                    TerraformingMarsUserAdapter.views[SelectedUserIndex].OnSelected(false);
                                }
                                break;
                            case 3:
                                //Send chat message.
                                TerraformingMarsClient.SendChatMessage(ReadMultiplayerInput.inputChatMessage);
                                break;
                            case 4:
                                //Leave game room.
                                TerraformingMarsClient.LeaveGameRoom(SelectedGameRoom.Id);
                                break;
                            case 5:
                                //Kick player.
                                IsKickingPlayer = true;
                                IsInvitingPlayer = true;
                                SelectedUserIndex = 0;
                                if (TerraformingMarsUserAdapter.views.Count > 0)
                                {
                                    SelectedUser = TerraformingMarsUserAdapter.views[SelectedUserIndex].user;
                                    TerraformingMarsUserAdapter.views[SelectedUserIndex].OnSelected(false);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (SelectedMultiplayerMenuItemId)
                        {
                            case 2:
                                //Invite player.
                                IsInvitingPlayer = true;
                                SelectedUserIndex = 0;
                                if (TerraformingMarsUserAdapter.views.Count > 0)
                                {
                                    SelectedUser = TerraformingMarsUserAdapter.views[SelectedUserIndex].user;
                                    TerraformingMarsUserAdapter.views[SelectedUserIndex].OnSelected(false);
                                }
                                break;
                            case 3:
                                //Send chat message.
                                TerraformingMarsClient.SendChatMessage(ReadMultiplayerInput.inputChatMessage);
                                break;
                            case 4:
                                //Leave game room.
                                TerraformingMarsClient.LeaveGameRoom(SelectedGameRoom.Id);
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    if (IsJoiningGameRoom && SelectedGameRoom != null)
                    {
                        IsJoiningGameRoom = false;
                        TerraformingMarsClient.JoinGameRoom(SelectedGameRoom.Id);
                    }
                    else if (IsInvitingPlayer && SelectedUser != null)
                    {
                        IsInvitingPlayer = false;
                        if (IsKickingPlayer == true)
                        {
                            IsKickingPlayer = false;
                            TerraformingMarsClient.KickPlayer(SelectedGameRoom.Id, SelectedUser.OuterId);
                        }
                        else
                        {
                            TerraformingMarsClient.InvitePlayer(SelectedGameRoom.Id, SelectedUser.OuterId);
                        }
                    }
                    else
                    {
                        switch (SelectedMultiplayerMenuItemId)
                        {
                            case 1:
                                //Create game room.
                                IsInGameRoom = true;
                                IsGameRoomLeader = true;
                                TerraformingMarsClient.CreateGameRoom();
                                break;
                            case 2:
                                //Join game room.
                                IsJoiningGameRoom = true;
                                SelectedGameRoomIndex = 0;
                                if (GameRoomAdapter.views.Count > 0)
                                {
                                    SelectedGameRoom = GameRoomAdapter.views[SelectedGameRoomIndex].room;
                                    GameRoomAdapter.views[SelectedGameRoomIndex].OnSelected(false);
                                }
                                break;
                            case 3:
                                //Send chat message.
                                TerraformingMarsClient.SendChatMessage(ReadMultiplayerInput.inputChatMessage);
                                break;
                            case 4:
                                //Go back to main menu.
                                TerraformingMarsClient.CloseClient();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }

    void HandleOptionsMenu()
    {
        if (optionsObjects != null)
        {
            optionsObjects.GetComponent<Canvas>().enabled = true;
        }

        if (optionsSelectorBody1 != null && optionsSelectorBody2 != null && optionsSelectorBody3 != null && optionsSelectorBody4 != null && menuObjects != null && easy != null && medium != null && hard != null && backOptions != null)
        {
            optionsObjects.transform.LookAt(Camera.main.transform);
            optionsObjects.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
            //Move the selector if showed.
            switch (SelectedOptionsMenuItemId)
            {
                case 1:
                    optionsSelectorBody1.GetComponent<Text>().enabled = true;
                    optionsSelectorBody2.GetComponent<Text>().enabled = false;
                    optionsSelectorBody3.GetComponent<Text>().enabled = false;
                    optionsSelectorBody4.GetComponent<Text>().enabled = false;
                    easy.color = new Color32(255, 142, 0, 255);
                    medium.color = new Color32(255, 69, 0, 255);
                    hard.color = new Color32(255, 69, 0, 255);
                    backOptions.color = new Color32(255, 69, 0, 255);
                    break;
                case 2:
                    optionsSelectorBody1.GetComponent<Text>().enabled = false;
                    optionsSelectorBody2.GetComponent<Text>().enabled = true;
                    optionsSelectorBody3.GetComponent<Text>().enabled = false;
                    optionsSelectorBody4.GetComponent<Text>().enabled = false;
                    easy.color = new Color32(255, 69, 0, 255);
                    medium.color = new Color32(255, 142, 0, 255);
                    hard.color = new Color32(255, 69, 0, 255);
                    backOptions.color = new Color32(255, 69, 0, 255);
                    break;
                case 3:
                    optionsSelectorBody1.GetComponent<Text>().enabled = false;
                    optionsSelectorBody2.GetComponent<Text>().enabled = false;
                    optionsSelectorBody3.GetComponent<Text>().enabled = true;
                    optionsSelectorBody4.GetComponent<Text>().enabled = false;
                    easy.color = new Color32(255, 69, 0, 255);
                    medium.color = new Color32(255, 69, 0, 255);
                    hard.color = new Color32(255, 142, 0, 255);
                    backOptions.color = new Color32(255, 69, 0, 255);
                    break;
                case 4:
                    optionsSelectorBody1.GetComponent<Text>().enabled = false;
                    optionsSelectorBody2.GetComponent<Text>().enabled = false;
                    optionsSelectorBody3.GetComponent<Text>().enabled = false;
                    optionsSelectorBody4.GetComponent<Text>().enabled = true;
                    easy.color = new Color32(255, 69, 0, 255);
                    medium.color = new Color32(255, 69, 0, 255);
                    hard.color = new Color32(255, 69, 0, 255);
                    backOptions.color = new Color32(255, 142, 0, 255);
                    break;
                default:
                    break;
            }
        }

        //Use the outcommented row and do not use OVRInput if testing without VR headset.
        optionsMenuDirInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        //optionsMenuDirInput = Input.GetKeyDown(KeyCode.DownArrow) ? new Vector2(0f, -0.5f) : new Vector2(0f, 0f);
        if (RightThumbIsReset)
        {
            if (optionsMenuDirInput.y < -0.25f)
            {
                RightThumbIsReset = false;
                if (SelectedOptionsMenuItemId == 4)
                {
                    SelectedOptionsMenuItemId = 1;
                }
                else
                {
                    SelectedOptionsMenuItemId++;
                }
            }
            if (optionsMenuDirInput.y > 0.25f)
            {
                RightThumbIsReset = false;
                if (SelectedOptionsMenuItemId == 1)
                {
                    SelectedOptionsMenuItemId = 4;
                }
                else
                {
                    SelectedOptionsMenuItemId--;
                }
            }
        }
        if (optionsMenuDirInput.x == 0.0f && optionsMenuDirInput.y == 0.0f)
        {
            RightThumbIsReset = true;
        }

        //Select the menu item.
        //Use the outcommented row and do not use OVRInput if testing without VR headset.
        if (OVRInput.GetDown(OVRInput.Button.One))
        //if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            switch (SelectedOptionsMenuItemId)
            {
                case 1:
                    //Select game difficulty: easy.
                    GameDatabaseService.Data.User.Bank = new Cost(40, 40, 40, 40, 40, 40);
                    break;
                case 2:
                    //Select game difficulty: normal.
                    GameDatabaseService.Data.User.Bank = new Cost(30, 10, 10, 10, 20, 10);
                    break;
                case 3:
                    //Select game difficulty: hard.
                    GameDatabaseService.Data.User.Bank = new Cost(25, 0, 0, 0, 10, 0);
                    break;
                case 4:
                    //Go back to main menu.
                    IsInOptions = false;
                    if (optionsObjects != null)
                    {
                        optionsObjects.GetComponent<Canvas>().enabled = false;
                    }
                    IsInMainMenu = true;
                    break;
                default:
                    break;
            }
        }
    }

    //Called on load operation to create the Unity GameObjects.
    public void CreateGameObjects()
    {
        //Check for every Hexagon.
        foreach (Hexagon h in GameDatabaseService.Data.GameBoard)
        {
            foreach (GameEvent ev in GameDatabaseService.Data.Events)
            {
                //If there is a saved GameEvent then create it.
                if (ev.HexagonId == h.Id)
                {
                    string NameOfGameObject;

                    switch (ev.Type)
                    {
                        case EventTypes.SpaceMine:
                            NameOfGameObject = "SpaceMine";
                            break;
                        case EventTypes.Satellite:
                            NameOfGameObject = "Satellite";
                            break;
                        case EventTypes.SpaceStation:
                            NameOfGameObject = "SpaceStation";
                            break;
                        default:
                            NameOfGameObject = "SpaceMine";
                            break;
                    }

                    //Search for the actual Hexagon.
                    GameObject eventObject = Instantiate(GameObject.Find(NameOfGameObject), h.Territory.transform.position, Quaternion.identity);

                    //Rotation for perfect angle.
                    float XAngle;
                    float YAngle = 180.0f - (h.Id % 36 * 10.0f);
                    float ZAngle = 0.0f;

                    if (h.Id < 37)
                    {
                        XAngle = 90.0f;
                    }
                    else if (h.Id > 36 && h.Id < 73)
                    {
                        XAngle = 98.0f;
                    }
                    else if (h.Id > 72 && h.Id < 109)
                    {
                        XAngle = 82.0f;
                    }
                    else if (h.Id > 108 && h.Id < 145)
                    {
                        XAngle = 106.5f;
                    }
                    else
                    {
                        XAngle = 73.5f;
                    }

                    eventObject.transform.Rotate(XAngle + 180.0f, YAngle, ZAngle, Space.World);

                    //Setting it into the GameDatabase.
                    GameDatabaseService.SetEventOnLoad(ev.Type, eventObject, h.Id, ev.Id);
                }
            }

            //If there is a saved Building then create it.
            if (h.BuildingModel != null && h.BuildingModel.Id != 0)
            {
                string NameOfGameObject;

                switch (h.BuildingModel.Type)
                {
                    case Buildings.PowerPlant:
                        NameOfGameObject = "PowerPlant";
                        break;
                    case Buildings.Greenery:
                        NameOfGameObject = "Greenery";
                        break;
                    case Buildings.City:
                        NameOfGameObject = "City";
                        break;
                    case Buildings.Ocean:
                        NameOfGameObject = "Ocean";
                        break;
                    default:
                        NameOfGameObject = "PowerPlant";
                        break;
                }

                //Search for the actual Hexagon.
                GameObject Building = Instantiate(GameObject.Find(NameOfGameObject), h.Territory.transform.position, Quaternion.identity);

                //Rotation for perfect angle.
                float XAngle;
                float YAngle = 180.0f - (h.Id % 36 * 10.0f);
                float ZAngle = 0.0f;

                if (h.Id < 37)
                {
                    XAngle = 90.0f;
                }
                else if (h.Id > 36 && h.Id < 73)
                {
                    XAngle = 98.0f;
                }
                else if (h.Id > 72 && h.Id < 109)
                {
                    XAngle = 82.0f;
                }
                else if (h.Id > 108 && h.Id < 145)
                {
                    XAngle = 106.5f;
                }
                else
                {
                    XAngle = 73.5f;
                }

                Building.transform.Rotate(XAngle + 180.0f, YAngle, ZAngle, Space.World);

                //Setting it into the GameDatabase.
                GameDatabaseService.SetBuildingToHexagonOnLoad(h.BuildingModel.Type, Building, h.Id, h.BuildingModel.Id);
            }
        }
    }
}
