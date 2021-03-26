// packet buffer was not originally written by me.

using System;
using System.Collections.Generic;
using System.Text;

    public class PacketBuffer : IDisposable
    {
        private List<byte> bufferList;
        private bool bufferUpdate = false;

        // IDisposable
        private bool disposedValue = false;

        private byte[] readBuffer;
        private int readPosition;

        public PacketBuffer()
        {
            bufferList = new List<byte>();
            readPosition = 0;
        }

        public void Clear()
        {
            bufferList.Clear();
            readPosition = 0;
        }

        public int Count()
        {
            return bufferList.Count;
        }

        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        public int GetReadPosition()
        {
            return readPosition;
        }

        public int Length()
        {
            return Count() - readPosition;
        }

        public byte ReadByte( bool peek = true )
        {
            if ( bufferList.Count > readPosition )
            {
                if ( bufferUpdate )
                {
                    readBuffer = bufferList.ToArray();
                    bufferUpdate = false;
                }

                byte value = readBuffer[readPosition];

                bool IsThereAdditionalData = peek && bufferList.Count > readPosition;

                if ( IsThereAdditionalData )
                {
                    readPosition += 1;
                }

                return value;
            }
            else
            {
                throw new Exception( "[Client] Buffer is past its limit" );
            }
        }

        public byte[] ReadBytes( int length, bool peek = true )
        {
            if ( bufferUpdate )
            {
                readBuffer = bufferList.ToArray();
                bufferUpdate = false;
            }

            byte[] value = bufferList.GetRange( readPosition, length ).ToArray();

            bool IsThereAdditionalData = peek && bufferList.Count > readPosition;

            if ( IsThereAdditionalData )
            {
                readPosition += length;
            }

            return value;
        }

        public float ReadFloat( bool peek = true )
        {
            if ( bufferList.Count > readPosition )
            {
                if ( bufferUpdate )
                {
                    readBuffer = bufferList.ToArray();
                    bufferUpdate = false;
                }

                float value = BitConverter.ToSingle( readBuffer, readPosition );

                bool IsThereAdditionalData = peek && bufferList.Count > readPosition;

                if ( IsThereAdditionalData )
                {
                    readPosition += 4;
                }

                return value;
            }
            else
            {
                throw new Exception( "[Client] Buffer is past its limit" );
            }
        }

        // Read Data
        public int ReadInteger( bool peek = true )
        {
            if ( bufferList.Count > readPosition )
            {
                if ( bufferUpdate )
                {
                    readBuffer = bufferList.ToArray();
                    bufferUpdate = false;
                }

                int value = BitConverter.ToInt32( readBuffer, readPosition );

                bool IsThereAdditionalData = peek && bufferList.Count > readPosition;

                if ( IsThereAdditionalData )
                {
                    readPosition += 4;
                }

                return value;
            }
            else
            {
                throw new Exception( "[Client] Buffer is past its limit" );
            }
        }

        public string ReadString( bool peek = true )
        {
            int length = ReadInteger( true );

            if ( bufferUpdate )
            {
                readBuffer = bufferList.ToArray();
                bufferUpdate = false;
            }

            string value = Encoding.ASCII.GetString( readBuffer, readPosition, length );

            bool IsThereAdditionalData = peek && bufferList.Count > readPosition;

            if ( IsThereAdditionalData )
            {
                readPosition += length;
            }

            return value;
        }

        public byte[] ToArray()
        {
            return bufferList.ToArray();
        }

        public void Write( byte input )
        {
            bufferList.Add( input );
            bufferUpdate = true;
        }

        public void Write( Outbound input )
        {
            bufferList.AddRange( BitConverter.GetBytes( (int)input ) );
            bufferUpdate = true;
        }

        public void Write( byte[] input )
        {
            bufferList.AddRange( input );
            bufferUpdate = true;
        }

        public void Write( float input )
        {
            bufferList.AddRange( BitConverter.GetBytes( input ) );
            bufferUpdate = true;
        }

        public void Write( int input )
        {
            bufferList.AddRange( BitConverter.GetBytes( input ) );
            bufferUpdate = true;
        }

        public void Write( string input )
        {
            bufferList.AddRange( BitConverter.GetBytes( input.Length ) );
            bufferList.AddRange( Encoding.ASCII.GetBytes( input ) );
            bufferUpdate = true;
        }

        protected virtual void Dispose( bool disposing )
        {
            if ( !disposedValue )
            {
                if ( disposing )
                {
                    bufferList.Clear();
                }
            }
            readPosition = 0;
            disposedValue = true;
        }
    }
