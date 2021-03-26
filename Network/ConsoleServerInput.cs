using Bindings;
using ReldawinServerMaster.Server;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ReldawinServerMaster
{
    internal class ServerInput
    {
        private static Dictionary<int, Packet_> packets;

        private delegate void Packet_( int index, byte[] data );

        public static void InitializeNetworkPackages()
        {
            Console.WriteLine( "ServerHandleNetworkData.InitializeNetworkPackages" );
            packets = new Dictionary<int, Packet_>
            {
                { (int)Inbound.ConnectionOK, HandleOnUserConnect },
                { (int)Inbound.AccountLoginQuery, HandleUserLoginQuery },
                { (int)Inbound.PCLoginPositionRequest, HandleLoginPositionRequest },
                { (int)Inbound.PCMoved, HandlePlayerCharacterMovedPosition },
                { (int)Inbound.AccountCreateQuery, HandleAccountCreateQuery },
                { (int)Inbound.PingTest, HandlePingTest  },
                { (int)Inbound.AccountDoesAccountExist, HandleDoesUserExist },
                { (int)Inbound.PCOtherListRequest, HandleOtherPlayerCharacterListRequest },
                { (int)Inbound.PCMoveQuery, HandleMoveQuery },
                { (int)Inbound.LoginChunkRequest, HandleLoginChunkRequest },
            };
        }

        public static void HandleNetworkInformation( int index, byte[] data )
        {
            PacketBuffer buffer = new PacketBuffer();
            int packetNum = -1;
            
            try {
                buffer.Write( data );
                packetNum = buffer.ReadInteger();
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
                
                throw;
            }
            finally
            {
                buffer.Dispose();
            }

            if ( packets.TryGetValue( packetNum, out Packet_ packet ) )
            {
                packet.Invoke( index, data );
            }
        }

        private static void HandleMoveQuery( int index, byte[] data )
        {
            PacketBuffer buffer = new PacketBuffer();

            try
            {
                buffer.Write( data );
                var packetNum = buffer.ReadInteger();
                var pointX = buffer.ReadFloat();
                var pointY = buffer.ReadFloat();
                var ID = buffer.ReadInteger();
                ServerHandleNetworkData.TellOPCsAPlayerIsMoving( index, new Vector2(pointX, pointY) );
            }
            catch ( Exception e ) { Console.WriteLine( e.Message ); }
            finally { buffer.Dispose(); }
        }

        public static void HandlePingTest( int index, byte[] data )
        {
            PacketBuffer buffer = new PacketBuffer();

            try
            {
                buffer.Write( data );
                var packetNum = buffer.ReadInteger();
                ServerOutput.Compose( Outbound.PingTest, index );
            }
            catch ( Exception e ) { Console.WriteLine( e.Message ); }
            finally { buffer.Dispose(); }
        }

        public static void HandlePlayerCharacterMovedPosition( int index, byte[] data )
        {
            PacketBuffer buffer = new PacketBuffer();

            try
            {
                buffer.Write( data );
                var packetNum = buffer.ReadInteger();
                var newPosX = buffer.ReadInteger();
                var newPosY = buffer.ReadInteger();
                ServerHandleNetworkData.HandlePlayerCharacterMovedPosition(index, newPosX, newPosY );
            }
            catch ( Exception e ) { Console.WriteLine( e.Message ); }
            finally { buffer.Dispose(); }
        }

        public static void HandleLoginPositionRequest( int index, byte[] data )
        {
            PacketBuffer buffer = new PacketBuffer();

            try
            {
                buffer.Write( data );
                var packetNum = buffer.ReadInteger();
                var playerID = buffer.ReadString();
                ServerHandleNetworkData.HandleLoginPositionRequest( index, playerID );
            }
            catch ( Exception e ) { Console.WriteLine( e.Message ); }
            finally { buffer.Dispose(); }
        }

        public static void HandleOtherPlayerCharacterListRequest( int index, byte[] data )
        {
            PacketBuffer buffer = new PacketBuffer();

            try
            {
                buffer.Write( data );
                var packetNum = buffer.ReadInteger();
                ServerHandleNetworkData.HandleOtherPlayerCharacterListRequest( index );
            }
            catch ( Exception e ) { Console.WriteLine( e.Message ); }
            finally { buffer.Dispose(); }
        }

        private static void HandleDoesUserExist( int index, byte[] data )
        {
            PacketBuffer buffer = new PacketBuffer();

            try
            {
                buffer.Write( data );
                var packetNum = buffer.ReadInteger();
                var username = buffer.ReadString();
                ServerHandleNetworkData.HandleDoesUserExist( index, username );
            }
            catch ( Exception e ) { Console.WriteLine( e.Message ); }
            finally { buffer.Dispose(); }
        }

        private static void HandleAccountCreateQuery( int index, byte[] data )
        {
            PacketBuffer buffer = new PacketBuffer();

            try
            {
                buffer.Write( data );
                var packetNum = buffer.ReadInteger();
                var username = buffer.ReadString();
                var password = buffer.ReadString();
                ServerHandleNetworkData.HandleAccountCreateQuery( index, username, password );
            }
            catch ( Exception e ) { Console.WriteLine( e.Message ); }
            finally { buffer.Dispose(); }
        }

        private static void HandleOnUserConnect( int index, byte[] data )
        {
            PacketBuffer buffer = new PacketBuffer();

            try
            {
                buffer.Write( data );
                var packetNum = buffer.ReadInteger();
                ServerHandleNetworkData.HandleOnUserConnect( index );
            }
            catch ( Exception e ) { Console.WriteLine( e.Message ); }
            finally { buffer.Dispose(); }
        }

        private static void HandleUserLoginQuery( int index, byte[] data )
        {
            PacketBuffer buffer = new PacketBuffer();

            try
            {
                buffer.Write( data );
                var packetNum = buffer.ReadInteger();
                var username = buffer.ReadString();
                var password = buffer.ReadString();
                var result = CommonMongo.GetPlayerPasswordAndID( username );
                ServerHandleNetworkData.HandleUserLoginQuery( index, username, password, result );
            }
            catch ( Exception e ) { Console.WriteLine( e.Message ); }
            finally { buffer.Dispose(); }
        }

        private static void HandleLoginChunkRequest( int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();

            try
            {
                buffer.Write( data );
                var packetNum = buffer.ReadInteger();
                var x = buffer.ReadInteger();
                var y = buffer.ReadInteger();
                ServerHandleNetworkData.HandleLoginChunkRequest( index, x, y );
            }
            catch ( Exception e ) { Console.WriteLine( e.Message ); }
            finally { buffer.Dispose(); }
        }
    }
}
