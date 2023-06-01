using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public static class TerraformingMarsClient
{
    public static ClientWebSocket clientWebSocket = null;

    public static bool IsGameStarted = false;
    public static int GameId = -1;

    public static bool isInMultiplayerLobby = true;
    public static bool isJoined = false;
    public static bool isInGameRoom = false;
    public static bool isInGame = false;
    public static bool isDataChanged = false;

    public static string userOuterId;
    public static string userName;
    public static string lobbyChoice;
    public static List<TerraformingMarsUser> users = new List<TerraformingMarsUser>();
    public static List<ChatMessage> messages = new List<ChatMessage>();
    public static List<GameRoom> gameRooms = new List<GameRoom>();

    public static void UpdateGameDataInService(MultiplayerGame Game)
    {
        Debug.Log(GameDatabaseService.Data.TimeRemaining);
        Debug.Log(GameDatabaseService.Data.Generation);

        if (GameDatabaseService.Data != null)
        {
            GameDatabaseService.Data.Id = Game.Id;
            GameDatabaseService.Data.Difficulty = Game.Difficulty;
            GameDatabaseService.Data.TimeRemaining = new TimeSpan(0, 0, Game.TimeRemaining);
            GameDatabaseService.Data.Generation = Game.Generation;
            GameDatabaseService.Data.IsGameEnded = Game.IsGameEnded;
            GameDatabaseService.Data.Score = Game.Score;
            GameDatabaseService.Data.OxygenLevel = Game.OxygenLevel;
            GameDatabaseService.Data.TemperatureLevel = Game.TemperatureLevel;
            GameDatabaseService.Data.OceanLevel = Game.OceanLevel;
        }
        else
        {
            GameDatabaseService.Data = new GameDatabaseService.GameData();
        }
    }

    public static async void StartClient()
    {
        using (clientWebSocket = new ClientWebSocket())
        {
            //Configure connection.
            Uri serviceUri = new Uri("ws://localhost:5000/terraforming_mars/game");
            var token = new CancellationTokenSource();
            token.CancelAfter(TimeSpan.FromSeconds(120));

            try
            {
                //Connect.
                await clientWebSocket.ConnectAsync(serviceUri, token.Token);

                while (clientWebSocket.State == WebSocketState.Open)
                {
                    if (IsGameStarted)
                    {
                        var responseBuffer = new byte[1024 * 4];
                        var offset = 0;
                        var packet = 1024;

                        //We receive the result from the backend.
                        ArraySegment<byte> byteToReceive = new ArraySegment<byte>(responseBuffer, offset, packet);
                        WebSocketReceiveResult respone = await clientWebSocket.ReceiveAsync(byteToReceive, token.Token);
                        var responseMessage = Encoding.UTF8.GetString(responseBuffer, offset, respone.Count);

                        //For debugging.
                        Debug.Log($"Server: {responseMessage}");

                        TerraformingMarsMessage messageToHandle = (TerraformingMarsMessage)JsonUtility.FromJson(responseMessage, typeof(TerraformingMarsMessage));
                        
                        switch (messageToHandle.Type)
                        {
                            case CommunicationType.GetGameStateResult:
                                GetGameStateResultMessage getGameState = (GetGameStateResultMessage)JsonUtility.FromJson(messageToHandle.Data, typeof(GetGameStateResultMessage));
                                MultiplayerGame toUpdate = new MultiplayerGame();
                                toUpdate.Id = getGameState.Id;
                                toUpdate.Difficulty = getGameState.Difficulty;
                                toUpdate.TimeRemaining = getGameState.TimeRemaining;
                                toUpdate.Generation = getGameState.Generation;
                                toUpdate.IsGameEnded = getGameState.IsGameEnded;
                                toUpdate.Score = getGameState.Score;
                                toUpdate.OxygenLevel = getGameState.OxygenLevel;
                                toUpdate.TemperatureLevel = getGameState.TemperatureLevel;
                                toUpdate.OceanLevel = getGameState.OceanLevel;
                                UpdateGameDataInService(toUpdate);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (isInMultiplayerLobby)
                        {
                            if (!isJoined)
                            {
                                if (ReadMultiplayerInput.inputUserName != null)
                                {
                                    JoinMultiplayerLobby joinMessage = new JoinMultiplayerLobby(ReadMultiplayerInput.inputUserName, "userId");
                                    TerraformingMarsMessage terraformingMarsMessage = new TerraformingMarsMessage(CommunicationType.JoinMultiplayerLobby, JsonUtility.ToJson(joinMessage));
                                    string messageToSend = JsonUtility.ToJson(terraformingMarsMessage);
                                    //We send the message to the backend.
                                    ArraySegment<byte> byteToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(messageToSend));
                                    await clientWebSocket.SendAsync(byteToSend, WebSocketMessageType.Text, true, token.Token);
                                    isJoined = true;
                                }
                            }
                            else
                            {
                                var responseBuffer = new byte[4048 * 4];
                                var offset = 0;
                                var packet = 4048;
                                //We receive the result from the backend.
                                ArraySegment<byte> byteToReceive = new ArraySegment<byte>(responseBuffer, offset, packet);
                                WebSocketReceiveResult respone = await clientWebSocket.ReceiveAsync(byteToReceive, token.Token);
                                var responseMessage = Encoding.UTF8.GetString(responseBuffer, offset, respone.Count);
                                TerraformingMarsMessage messageToHandle = UnityJsonParser.FromJson<TerraformingMarsMessage>(responseMessage);
                                switch (messageToHandle.Type)
                                {
                                    case CommunicationType.JoinMultiplayerLobbyResult:
                                        //Reading message.
                                        JoinMultiplayerLobbyResult joinLobbyResult = UnityJsonParser.FromJson<JoinMultiplayerLobbyResult>(messageToHandle.Data);
                                        //We joined the lobby and received the results.
                                        userOuterId = joinLobbyResult.OuterId;
                                        List<TerraformingMarsUser> updatedUser = UnityJsonParser.FromJson<List<TerraformingMarsUser>>(joinLobbyResult.AvailableUserData);
                                        List<ChatMessage> updatedChat = UnityJsonParser.FromJson<List<ChatMessage>>(joinLobbyResult.AvailableChatData);
                                        List<GameRoom> updatedGameRoom = UnityJsonParser.FromJson<List<GameRoom>>(joinLobbyResult.AvailableGameRoomData);
                                        if (users.Count != updatedUser.Count || messages.Count != updatedChat.Count || gameRooms.Count != updatedGameRoom.Count)
                                        {
                                            isDataChanged = true;
                                        }
                                        users.Clear();
                                        messages.Clear();
                                        gameRooms.Clear();
                                        updatedUser.ForEach(users.Add);
                                        updatedChat.ForEach(messages.Add);
                                        updatedGameRoom.ForEach(gameRooms.Add);
                                        break;
                                    case CommunicationType.JoinGameRoomResult:
                                        //Reading message.
                                        JoinGameRoomResult joinGameRoomResult = UnityJsonParser.FromJson<JoinGameRoomResult>(messageToHandle.Data);
                                        if (joinGameRoomResult.IsSuccess)
                                        {
                                            isInGameRoom = true;
                                            MainMenuInterface.IsInGameRoom = true;
                                            if (joinGameRoomResult.IsLeader)
                                            {
                                                MainMenuInterface.IsGameRoomLeader = true;
                                            }
                                        }
                                        break;
                                    case CommunicationType.StartGameResult:
                                        StartGameResultMessage startGame = (StartGameResultMessage)JsonUtility.FromJson(messageToHandle.Data, typeof(StartGameResultMessage));
                                        GameId = startGame.GameId;
                                        IsGameStarted = true;
                                        break;
                                    default:
                                        break;
                                }
                                if (isDataChanged)
                                {
                                    if (isInMultiplayerLobby)
                                    {
                                        if (isJoined)
                                        {
                                            TerraformingMarsUserAdapter.OnReceivedNewModels(users.ToArray());
                                            ChatMessageAdapter.OnReceivedNewModels(messages.ToArray());
                                            GameRoomAdapter.OnReceivedNewModels(gameRooms.ToArray());
                                        }
                                    }
                                    isDataChanged = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (WebSocketException wex)
            {
                Console.WriteLine(wex.Message);
            }
        }
    }

    public static async void SendChatMessage(string message)
    {
        if (clientWebSocket != null && clientWebSocket.State == WebSocketState.Open)
        {
            SendChatMessage sendChatMessage = new SendChatMessage(userOuterId, message);
            TerraformingMarsMessage terraformingMarsMessage = new TerraformingMarsMessage(CommunicationType.SendChatMessage, JsonUtility.ToJson(sendChatMessage));
            string messageToSend = JsonUtility.ToJson(terraformingMarsMessage);

            //We send the message to the backend.
            ArraySegment<byte> byteToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(messageToSend));
            var token = new CancellationTokenSource();
            await clientWebSocket.SendAsync(byteToSend, WebSocketMessageType.Text, true, token.Token);
        }
    }

    public static async void CreateGameRoom()
    {
        if (clientWebSocket != null && clientWebSocket.State == WebSocketState.Open)
        {
            CreateGameRoom createGameRoom = new CreateGameRoom(userOuterId);
            TerraformingMarsMessage terraformingMarsMessage = new TerraformingMarsMessage(CommunicationType.CreateGameRoom, JsonUtility.ToJson(createGameRoom));
            string messageToSend = JsonUtility.ToJson(terraformingMarsMessage);

            //We send the message to the backend.
            ArraySegment<byte> byteToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(messageToSend));
            var token = new CancellationTokenSource();
            await clientWebSocket.SendAsync(byteToSend, WebSocketMessageType.Text, true, token.Token);
        }
    }

    public static async void JoinGameRoom(int id)
    {
        if (clientWebSocket != null && clientWebSocket.State == WebSocketState.Open)
        {
            JoinGameRoom joinGameRoom = new JoinGameRoom(userOuterId, id);
            TerraformingMarsMessage terraformingMarsMessage = new TerraformingMarsMessage(CommunicationType.JoinGameRoom, JsonUtility.ToJson(joinGameRoom));
            string messageToSend = JsonUtility.ToJson(terraformingMarsMessage);

            //We send the message to the backend.
            ArraySegment<byte> byteToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(messageToSend));
            var token = new CancellationTokenSource();
            await clientWebSocket.SendAsync(byteToSend, WebSocketMessageType.Text, true, token.Token);
        }
    }

    public static async void LeaveGameRoom(int id)
    {
        if (clientWebSocket != null && clientWebSocket.State == WebSocketState.Open)
        {
            LeaveGameRoom leaveGameRoom = new LeaveGameRoom(userOuterId, id);
            TerraformingMarsMessage terraformingMarsMessage = new TerraformingMarsMessage(CommunicationType.LeaveGameRoom, JsonUtility.ToJson(leaveGameRoom));
            string messageToSend = JsonUtility.ToJson(terraformingMarsMessage);

            //We send the message to the backend.
            ArraySegment<byte> byteToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(messageToSend));
            var token = new CancellationTokenSource();
            isInGameRoom = false;
            MainMenuInterface.IsInGameRoom = false;
            MainMenuInterface.IsGameRoomLeader = false;
            await clientWebSocket.SendAsync(byteToSend, WebSocketMessageType.Text, true, token.Token);
        }
    }

    public static async void InvitePlayer(int id, string userToInvite)
    {
        if (clientWebSocket != null && clientWebSocket.State == WebSocketState.Open)
        {
            InvitePlayer invitePlayer = new InvitePlayer(userOuterId, userToInvite, id);
            TerraformingMarsMessage terraformingMarsMessage = new TerraformingMarsMessage(CommunicationType.InvitePlayer, JsonUtility.ToJson(invitePlayer));
            string messageToSend = JsonUtility.ToJson(terraformingMarsMessage);

            //We send the message to the backend.
            ArraySegment<byte> byteToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(messageToSend));
            var token = new CancellationTokenSource();
            await clientWebSocket.SendAsync(byteToSend, WebSocketMessageType.Text, true, token.Token);
        }
    }

    public static async void KickPlayer(int id, string userToKick)
    {
        if (clientWebSocket != null && clientWebSocket.State == WebSocketState.Open)
        {
            KickPlayer kickPlayer = new KickPlayer(userOuterId, userToKick, id);
            TerraformingMarsMessage terraformingMarsMessage = new TerraformingMarsMessage(CommunicationType.KickPlayer, JsonUtility.ToJson(kickPlayer));
            string messageToSend = JsonUtility.ToJson(terraformingMarsMessage);

            //We send the message to the backend.
            ArraySegment<byte> byteToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(messageToSend));
            var token = new CancellationTokenSource();
            await clientWebSocket.SendAsync(byteToSend, WebSocketMessageType.Text, true, token.Token);
        }
    }

    public static async void StartGame(int gameRoomId)
    {
        if (clientWebSocket != null && clientWebSocket.State == WebSocketState.Open)
        {
            StartGameMessage startGameMessage = new StartGameMessage(userOuterId, 1);
            TerraformingMarsMessage terraformingMarsMessage = new TerraformingMarsMessage(CommunicationType.StartGame, JsonUtility.ToJson(startGameMessage));
            string messageToSend = JsonUtility.ToJson(terraformingMarsMessage);

            //We send the message to the backend.
            ArraySegment<byte> byteToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(messageToSend));
            var token = new CancellationTokenSource();
            await clientWebSocket.SendAsync(byteToSend, WebSocketMessageType.Text, true, token.Token);
        }
    }

    public static async void BuyBuilding(Buildings type, int hexagonId, int gameId)
    {
        if (clientWebSocket != null && clientWebSocket.State == WebSocketState.Open)
        {
            BuyBuildingMessage buyBuildingMessage = new BuyBuildingMessage(userOuterId, type, hexagonId, gameId);
            TerraformingMarsMessage terraformingMarsMessage = new TerraformingMarsMessage(CommunicationType.BuyBuilding, JsonUtility.ToJson(buyBuildingMessage));
            string messageToSend = JsonUtility.ToJson(terraformingMarsMessage);

            //We send the message to the backend.
            ArraySegment<byte> byteToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(messageToSend));
            var token = new CancellationTokenSource();
            await clientWebSocket.SendAsync(byteToSend, WebSocketMessageType.Text, true, token.Token);
        }
    }

    public static async void CloseClient()
    {
        try
        {
            if (clientWebSocket != null && clientWebSocket.State == WebSocketState.Open)
            {
                isJoined = false;
                isInMultiplayerLobby = true;
                isDataChanged = false;
                isInGame = false;
                isInGameRoom = false;
                IsGameStarted = false;
                await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Leaving multiplayer lobby.", CancellationToken.None);
            }
        }
        catch (WebSocketException wex)
        {
            Console.WriteLine(wex.Message);
        }
    }
}
