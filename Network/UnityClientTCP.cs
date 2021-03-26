using System;
using System.Net.Sockets;
using UnityEngine;

    public class ClientTCP : MonoBehaviour
    {
        public static Socket clientSocket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
        private bool close = false;

        public static void SendData( byte[] input )
        {
            clientSocket.Send( input );
        }

        public void Connect()
        {
            clientSocket.BeginConnect( "192.168.0.43" //"82.16.86.56"
                                     , 5555
                                     , new AsyncCallback( ConnectCallback )
                                     , clientSocket
                                     );
        }

        private static void OnRecieve()
        {
            byte[] sizeInfo = new byte[4];
            byte[] recievedBuffer = new byte[1024];

            int totalRead = 0;
            int currentRead = 0;

            try
            {
                currentRead = totalRead = clientSocket.Receive( sizeInfo );

                if ( totalRead <= 0 )
                {
                    Debug.Log( "[Client] You are not connected to the server" );
                }
                else
                {
                    while ( totalRead < sizeInfo.Length && currentRead > 0 )
                    {
                        currentRead = clientSocket.Receive( sizeInfo, totalRead, sizeInfo.Length - totalRead, SocketFlags.None );
                        totalRead += currentRead;
                    }

                    int messageSize = 0;
                    messageSize |= sizeInfo[0];
                    messageSize |= ( sizeInfo[1] << 08 );
                    messageSize |= ( sizeInfo[2] << 16 );
                    messageSize |= ( sizeInfo[3] << 24 );

                    byte[] data = new byte[messageSize];

                    totalRead = 0;
                    currentRead = totalRead = clientSocket.Receive( data, totalRead, data.Length - totalRead, SocketFlags.None );

                    while ( totalRead < messageSize && currentRead > 0 )
                    {
                        currentRead = clientSocket.Receive( data, totalRead, data.Length - totalRead, SocketFlags.None );
                        totalRead += totalRead;
                    }

                    ClientInput.HandleNetworkInformation( data );
                }
            }
            catch ( Exception e )
            {
                Debug.Log( e.Message );
                clientSocket.Close();
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad( gameObject );
            DontDestroyOnLoad( this );
        }

        private void ConnectCallback( IAsyncResult ar )
        {
            clientSocket.EndConnect( ar );

            while ( close == false )
            {
                OnRecieve();
            }
        }

        private void OnApplicationQuit()
        {
            clientSocket.Close();
            close = true;
        }

        private void Start()
        {
            Connect();
        }
    }
