using Bindings;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ReldawinServerMaster.Server
{
    internal class ServerOutput
    {
        public static void Compose( Outbound packetIndex, int clientIndex )
        {
            Console.WriteLine( string.Format( "[ServerOutput] {0}", packetIndex ) );

            PacketBuffer buffer = new PacketBuffer();

            try
            {
                buffer.Write( packetIndex );
                ServerTCP.SendDataTo( clientIndex, buffer.ToArray() );
            }
            catch ( Exception e ) { Console.WriteLine( e.Message ); }
            finally { buffer.Dispose(); }
        }

        public static void Compose( Outbound packetIndex, int clientIndex, string a )
        {
            Console.WriteLine( string.Format( "[ServerOutput] {0}", packetIndex ) );

            PacketBuffer buffer = new PacketBuffer();

            try
            {
                buffer.Write( packetIndex );
                buffer.Write( a );
                ServerTCP.SendDataTo( clientIndex, buffer.ToArray() );
            }
            catch ( Exception e ) { Console.WriteLine( e.Message ); }
            finally { buffer.Dispose(); }
        }

        public static void Compose( Outbound packetIndex, int clientIndex, bool a )
        {
            Console.WriteLine( string.Format( "[ServerOutput] {0}", packetIndex ) );

            PacketBuffer buffer = new PacketBuffer();

            try
            {
                buffer.Write( packetIndex );
                buffer.Write( (byte)( a == true ? 1 : 0 ) );
                ServerTCP.SendDataTo( clientIndex, buffer.ToArray() );
            }
            catch ( Exception e ) { Console.WriteLine( e.Message ); }
            finally { buffer.Dispose(); }
        }

        public static void Compose( Outbound packetIndex, int clientIndex, Vector2 position )
        {
            Console.WriteLine( string.Format( "[ServerOutput] {0}", packetIndex ) );

            PacketBuffer buffer = new PacketBuffer();

            try
            {
                buffer.Write( packetIndex );
                buffer.Write( position.X );
                buffer.Write( position.Y );
                ServerTCP.SendDataTo( clientIndex, buffer.ToArray() );
            }
            catch ( Exception e ) { Console.WriteLine( e.Message ); }
            finally { buffer.Dispose(); }
        }

        public static void Compose( Outbound packetIndex, int clientIndex, Point position )
        {
            Console.WriteLine( string.Format( "[ServerOutput] {0}", packetIndex ) );

            PacketBuffer buffer = new PacketBuffer();

            try
            {
                buffer.Write( packetIndex );
                buffer.Write( position.X );
                buffer.Write( position.Y );
                ServerTCP.SendDataTo( clientIndex, buffer.ToArray() );
            }
            catch ( Exception e ) { Console.WriteLine( e.Message ); }
            finally { buffer.Dispose(); }
        }

        public static void Compose( Outbound packetIndex, int clientIndex, int w, int h, int x, int y )
        {
            Console.WriteLine( string.Format( "[ServerOutput] {0}", packetIndex ) );

            PacketBuffer buffer = new PacketBuffer();

            try
            {
                buffer.Write( packetIndex );
                buffer.Write( w );
                buffer.Write( h );
                buffer.Write( x );
                buffer.Write( y );
                ServerTCP.SendDataTo( clientIndex, buffer.ToArray() );
            }
            catch ( Exception e ) { Console.WriteLine( e.Message ); }
            finally { buffer.Dispose(); }
        }

        public static void ComposeChunk(Outbound packetIndex, int clientIndex, List<DBPackages.DBTile> tiles)
        {
            Console.WriteLine( string.Format( "[ServerOutput] {0}", packetIndex ) );

            PacketBuffer buffer = new PacketBuffer();

            try
            {
                buffer.Write( packetIndex );
                buffer.Write( tiles.Count );
                for ( int i = 0; i < tiles.Count; i++ )
                {
                    buffer.Write( tiles[i].ID.ToString() );
                    buffer.Write( tiles[i].TileIndex );
                    buffer.Write( tiles[i].X );
                    buffer.Write( tiles[i].Y );
                    buffer.Write( tiles[i].Z );
                }
            }
            catch ( Exception e ) { Console.WriteLine( e.Message ); }
            finally { buffer.Dispose(); }
        }

        public static void SendOtherPlayerCharacterListRequest( int index )
        {
            PacketBuffer buffer = new PacketBuffer();
            try
            {
                buffer.Write( Outbound.OtherPlayerCharacterListRequest );

                List<Client> clientList = ServerTCP.FetchOtherClients( index );

                buffer.Write( clientList.Count );

                if ( clientList.Count > 0 )
                {
                    foreach ( Client client in clientList )
                    {
                        buffer.Write( client.properties.Username );
                        buffer.Write( client.properties.Position.X );
                        buffer.Write( client.properties.Position.Y );
                        buffer.Write( client.properties.ID );
                    }
                }
                ServerTCP.SendDataTo( index, buffer.ToArray() );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
            finally
            {
                buffer.Dispose();
            }
        }
    }
}
