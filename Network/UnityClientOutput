// a collection of static methods that can be called to quickly send packets to the server

    public static class ClientOutput
    {
        public static void Compose( Outbound packageIndex)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.Write( packageIndex );
            ClientTCP.SendData( buffer.ToArray() );
            buffer.Dispose();
        }
        public static void Compose( Outbound packageIndex, int x, int y )
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.Write( packageIndex );
            buffer.Write( x );
            buffer.Write( y );
            ClientTCP.SendData( buffer.ToArray() );
            buffer.Dispose();
        }
        public static void Compose( Outbound packageIndex, string a, string b )
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.Write( packageIndex );
            buffer.Write( a );
            buffer.Write( b );
            ClientTCP.SendData( buffer.ToArray() );
            buffer.Dispose();
        }
        public static void Compose( Outbound packageIndex, string a )
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.Write( packageIndex );
            buffer.Write( a );
            ClientTCP.SendData( buffer.ToArray() );
            buffer.Dispose();
        }
        public static void Compose( Outbound packageIndex, float x, float y, string a)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.Write( packageIndex );
            buffer.Write( x );
            buffer.Write( y );
            buffer.Write( a );
            ClientTCP.SendData( buffer.ToArray() );
            buffer.Dispose();
        }
    }
