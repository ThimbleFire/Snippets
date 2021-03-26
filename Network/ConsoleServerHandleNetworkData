using Bindings;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ReldawinServerMaster.Server
{
    internal class ServerHandleNetworkData
    {
        public static void TellOPCsAPlayerIsMoving( int index, Vector2 position )
        {
            List<Client> clientList = ServerTCP.FetchOtherClients( index );

            if ( clientList.Count == 0 )
                return;

            foreach ( Client item in clientList ) {

                ServerOutput.Compose( Outbound.EntityMoveQuery, item.index, position );
            }

        }

        public static void HandlePlayerCharacterMovedPosition( int index, int x, int y )
        {
            ServerTCP.clients[index].MovePosition( x, y );
            CommonMongo.SetEntityCoodinates( x, y, ServerTCP.clients[index].properties.ID );
        }

        public static void HandleLoginPositionRequest( int index, string playerID )
        {
            Point position = CommonMongo.GetEntityCoordinates( playerID );
            ServerTCP.InitializeClient( index, position.X, position.Y, playerID );
            ServerOutput.Compose( Outbound.PCLoginPositionRequest, index, position );
            ServerTCP.clients[index].MovePosition( position.X, position.Y );
            CommonMongo.SetEntityCoodinates( position.X, position.Y, ServerTCP.clients[index].properties.ID );
        }

        public static void HandleOtherPlayerCharacterListRequest( int index )
        {
            ServerOutput.SendOtherPlayerCharacterListRequest( index );
        }

        public static void HandleDoesUserExist( int index, string username )
        {
            var result = CommonMongo.DoesUserExist( username );
            ServerOutput.Compose( Outbound.DoesUserExist, index, result ? false : true );
        }

        public static void HandleAccountCreateQuery( int index, string username, string password )
        {
            bool userExists = CommonMongo.DoesUserExist( username );

            if ( userExists )
            {
                ServerOutput.Compose( Outbound.Account_Create_Fail, index, Log.DatabaseAccountAlreadyExists );
                return;
            }

            CommonMongo.CreateAccount( username, password );
            ServerOutput.Compose( Outbound.Account_Create_Success, index, Log.DatabaseAccountCreated );
        }

        public static void HandleOnUserConnect( int index )
        {
            // nothing
        }

        public static void HandleUserLoginQuery( int index, string username, string password, string[] result )
        {
            if ( result == null )
            {
                ServerOutput.Compose( Outbound.Account_Login_Fail, index, Log.DatabaseUsernameMismatch );
                return;
            }

            string recordedPassword = result[0];
            string recordedID = result[1];

            if ( recordedPassword == password )
            {
                ServerOutput.Compose( Outbound.Account_Login_Success, index, recordedID );
                ServerTCP.clients[index].properties.Username = username;
            }
            else
            {
                ServerOutput.Compose( Outbound.Account_Login_Fail, index, Log.DatabasePasswordMismatch );
            }
        }

        internal static void HandleLoginChunkRequest(int index, int x, int y)
        {
            List<DBPackages.DBTile> tiles = CommonMongo.GetChunkData( x, y );

            ServerOutput.ComposeChunk( Outbound.LoginChunkRequest, index, tiles );
        }
    }
}
