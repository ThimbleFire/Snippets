using Bindings;
using ReldawinServerMaster.Server;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ReldawinServerMaster
{
    internal /*internal is intentional*/ class ServerTCP
    {
        public static Client[] clients = new Client[Log.MAX_PLAYERS];
        public static Socket serverSocket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );

        public static void SendDataTo( int index, byte[] data )
        {
            byte[] sizeInfo = new byte[4];
            sizeInfo[0] = (byte)data.Length;
            sizeInfo[1] = (byte)( data.Length >> 08 );
            sizeInfo[2] = (byte)( data.Length >> 16 );
            sizeInfo[2] = (byte)( data.Length >> 24 );

            clients[index].socket.Send( sizeInfo );
            clients[index].socket.Send( data );
        }

        public static void InitializeClient( int index, int x, int y, string id )
        {
            clients[index].Setup( x, y, id );
        }

        public static void SetupServer()
        {
            Console.WriteLine( "ServerTCP.SetupServer" );
            for ( int i = 0; i < Log.MAX_PLAYERS; i++ )
            {
                clients[i] = new Client();
            }

            serverSocket.Bind( new IPEndPoint( IPAddress.Any, 5555 ) );
            serverSocket.Listen( Log.BUFFER_PLAYERS );
            serverSocket.BeginAccept( new AsyncCallback( AcceptCallback ), null );
        }

        private static void AcceptCallback( IAsyncResult ar )
        {
            Socket socket = serverSocket.EndAccept( ar );
            serverSocket.BeginAccept( new AsyncCallback( AcceptCallback ), null );

            for ( int i = 0; i < Log.MAX_PLAYERS; i++ )
            {
                if ( clients[i].socket == null )
                {
                    clients[i].socket = socket;
                    clients[i].index = i;
                    clients[i].ip = socket.RemoteEndPoint.ToString();
                    clients[i].StartClient();
                    Console.WriteLine( "[ServerTCP] " + Log.SERVER_LOBBY_JOIN, i );
                    ServerOutput.Compose( Outbound.ConnectionOK, i );
                    return;
                }
            }
        }

        public static List<Client> FetchOtherClients( int index )
        {
            List<Client> clientList = new List<Client>();

            // Get a list of all the clients

            foreach ( Client client in ServerTCP.clients )
                if ( client.loggedIn != false )
                    if ( client.index != index )
                        clientList.Add( client );

            return clientList;
        }
    }
}
