using System;
using UnityEngine;

public class TerraformingMarsMessage
{
    public CommunicationType Type;
    public string Data;

    public TerraformingMarsMessage(CommunicationType type, string data)
    {
        Type = type;
        Data = data;
    }
}

public abstract class MessageData
{
    public MessageData() { }
}

//Client sends, Server receives.
public class JoinMultiplayerLobby : MessageData
{
    public string Name;
    public string OuterId;
    public JoinMultiplayerLobby(string name, string outerId)
    {
        Name = name;
        OuterId = outerId;
    }
}

//Server sends, Client receives.
public class JoinMultiplayerLobbyResult : MessageData
{
    public string OuterId;
    public string AvailableUserData;
    public string AvailableChatData;
    public string AvailableGameRoomData;
    public JoinMultiplayerLobbyResult(string outerId, string availableUserData, string availableChatData, string availableGameRoomData)
    {
        OuterId = outerId;
        AvailableUserData = availableUserData;
        AvailableChatData = availableChatData;
        AvailableGameRoomData = availableGameRoomData;
    }
}

//Client sends, Server receives.
public class CreateGameRoom : MessageData
{
    public string UserId;
    public CreateGameRoom(string userId)
    {
        UserId = userId;
    }
}

//Client sends, Server receives.
public class JoinGameRoom : MessageData
{
    public string UserId;
    public int GameRoomId;
    public JoinGameRoom(string userId, int gameRoomId)
    {
        UserId = userId;
        GameRoomId = gameRoomId;
    }
}

//Server sends, client receives.
public class JoinGameRoomResult : MessageData
{
    public bool IsSuccess;
    public bool IsLeader;

    public JoinGameRoomResult(bool isSuccess, bool isLeader)
    {
        IsSuccess = isSuccess;
        IsLeader = isLeader;
    }
}

//Client sends, Server receives.
public class LeaveGameRoom : MessageData
{
    public string UserId;
    public int GameRoomId;

    public LeaveGameRoom(string userId, int gameRoomId)
    {
        UserId = userId;
        GameRoomId = gameRoomId;
    }
}

//Client sends, Server receives.
public class SendChatMessage : MessageData
{
    public string User;
    public string Message;
    public SendChatMessage(string user, string message)
    {
        User = user;
        Message = message;
    }
}

//Client sends, Server receives.
public class InvitePlayer : MessageData
{
    public string User;
    public string UserToInvite;
    public int GameRoom;
    public InvitePlayer(string user, string userToInvite, int gameRoom)
    {
        User = user;
        UserToInvite = userToInvite;
        GameRoom = gameRoom;
    }
}

//Server sends, Client receives.
public class InvitePlayerResult : MessageData
{
    public int BuildingId { get; set; }
    public InvitePlayerResult(int buildingId)
    {
        BuildingId = buildingId;
    }
}

//Client sends, Server receives.
public class KickPlayer : MessageData
{
    public string User;
    public string UserToKick;
    public int GameRoom;

    public KickPlayer(string user, string userToKick, int gameRoom)
    {
        User = user;
        UserToKick = userToKick;
        GameRoom = gameRoom;
    }
}

//Client sends, Server receives.
public class StartGameMessage : MessageData
{
    public string FirstUser;
    public int Difficulty = 1;

    public StartGameMessage(string firstUser, int difficulty = 1)
    {
        FirstUser = firstUser;
        Difficulty = difficulty;
    }
}

//Server sends, Client receives.
public class StartGameResultMessage : MessageData
{
    public int GameId;

    public StartGameResultMessage(int gameId)
    {
        GameId = gameId;
    }
}

//Client sends, Server receives.
public class GetGameStateMessage : MessageData
{
    public string User;
    public int GameId;

    public GetGameStateMessage(string user, int gameId)
    {
        User = user;
        GameId = gameId;
    }
}

//Server sends, Client receives.
public class GetGameStateResultMessage : MessageData
{
    public int Id;
    public int Difficulty;
    public int Generation;
    public int TimeRemaining;
    public bool IsGameEnded;
    public int Score;

    public int OxygenLevel;
    public int TemperatureLevel;
    public int OceanLevel;

    public GetGameStateResultMessage(int id, int difficulty, int generation, int timeRemaining, bool isGameEnded, int score, int oxygenLevel, int temperatureLevel, int oceanLevel)
    {
        Id = id;
        Difficulty = difficulty;
        Generation = generation;
        TimeRemaining = timeRemaining;
        IsGameEnded = isGameEnded;
        Score = score;
        OxygenLevel = oxygenLevel;
        TemperatureLevel = temperatureLevel;
        OceanLevel = oceanLevel;
    }
}

//Client sends, Server receives.
public class BuyBuildingMessage : MessageData
{
    public string User;
    public Buildings Type;
    public int HexagonId;
    public int GameId;

    public BuyBuildingMessage(string user, Buildings type, int hexagonId, int gameId)
    {
        User = user;
        Type = type;
        HexagonId = hexagonId;
        GameId = gameId;
    }
}

//Server sends, Client receives.
public class BuyBuildingResultMessage : MessageData
{
    public int BuildingId;

    public BuyBuildingResultMessage(int buildingId)
    {
        BuildingId = buildingId;
    }
}

//Enum for storing the type of Message between cliend and server.
public enum CommunicationType
{
    JoinMultiplayerLobby, JoinMultiplayerLobbyResult,
    SendChatMessage, CreateGameRoom,
    JoinGameRoom, JoinGameRoomResult,
    LeaveGameRoom,
    InvitePlayer, InvitePlayerResult, KickPlayer,
    StartGame, StartGameResult,
    GetGameState, GetGameStateResult,
    BuyBuilding, BuyBuildingResult
}
