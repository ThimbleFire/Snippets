// a list of inbound and outbound packets.

public enum Inbound
    {
        Crash,
        ConnectionOK,
        Account_Login_Query,
        Account_Login_Success,
        Account_Login_Fail,
        Account_Create_Query,
        Account_Create_Fail,
        Account_Create_Success,
        LoginPositionRequest,
        MovedTile,
        EntityMoveQuery,
        PingTest,
        DoesUserExist,
        OtherPlayerCharacterLoggedIn,
        OtherPlayerCharacterListRequest,
        LoginChunkRequest,
    };

    public enum Outbound
    {
        Crash,
        ConnectionOK,
        Account_Login_Query,
        Account_Login_Success,
        Account_Login_Fail,
        Account_Create_Query,
        Account_Create_Fail,
        Account_Create_Success,
        LoginPositionRequest,
        MovedTile,
        EntityMoveQuery,
        PingTest,
        DoesUserExist,
        OtherPlayerCharacterLoggedIn,
        OtherPlayerCharacterListRequest,
        LoginChunkRequest,
    };
