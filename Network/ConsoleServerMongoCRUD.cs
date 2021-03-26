// An example of how I used mongo in an earlier project

using Microsoft.Win32.SafeHandles;
using MongoDB.Bson;
using MongoDB.Driver;
using ReldawinServerMaster.DBPackages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace ReldawinServerMaster
{
    public class MongoCRUD : IDisposable
    {
        //TODO Implement conditions params in searches to allow for more more accurate results
        public struct Condition
        {
            public string field { get; set; }
            public string value { get; set; }
        }

        private IMongoDatabase db;

        public MongoCRUD( string database )
        {
            MongoClient client = new MongoClient();
            db = client.GetDatabase( database );
        }

        public void InsertRecord<T>( string table, T record )
        {
            var collection = db.GetCollection<T>( table );
            collection.InsertOne( record );
        }

        public void InsertRecordAsync<T>( string table, T record )
        {
            var collection = db.GetCollection<T>( table );
            collection.InsertOneAsync( record ).Wait();
        }

        public void InsertRecords<T>( string table, T[] records )
        {
            var collection = db.GetCollection<T>( table );
            collection.InsertMany( records );
        }

        public void InsertRecordsAsync<T>( string table, T[] records )
        {
            var collection = db.GetCollection<T>( table );
            collection.InsertManyAsync( records ).Wait();
        }

        public List<T> GetRecords<T>( string table )
        {
            var collection = db.GetCollection<T>( table );
            return collection.Find( new BsonDocument() ).ToList();
        }

        public T GetRecord<T>( string table, Guid id )
        {
            var collection = db.GetCollection<T>( table );
            var filter = Builders<T>.Filter.Eq( "ID", id );

            return collection.Find( filter ).FirstOrDefault();
        }

        public T GetRecord<T>( string table, Condition condition )
        {
            var collection = db.GetCollection<T>( table );
            var filter = Builders<T>.Filter.Eq( condition.field, condition.value );

            return collection.Find( filter ).FirstOrDefault();
        }

        public List<T> GetChunk<T>( int chunkX, int chunkY )
        {
            var collection = db.GetCollection<T>( "WorldData" );
            var filter = 
                   Builders<T>.Filter.Gte( "X", chunkX * 16 )
                 & Builders<T>.Filter.Lt( "X",  (chunkX + 1) * 16 )
                 & Builders<T>.Filter.Gte( "Y", chunkY * 16 )
                 & Builders<T>.Filter.Lt( "Y",  (chunkY + 1) * 16 );
            return collection.Find( filter ).ToList();
        }

        public void UpsertRecord<T>( string table, Guid id, T record )
        {
            var collection = db.GetCollection<T>( table );
            var result = collection.ReplaceOne(
                new BsonDocument( "ID", id ),
                record,
                new ReplaceOptions { IsUpsert = true } );
        }

        public void DeleteRecord<T>( string table, Guid id )
        {
            var collection = db.GetCollection<T>( table );
            var filter = Builders<T>.Filter.Eq( "ID", id );
            collection.DeleteOne( filter );
        }

        private bool disposed = false;

        private SafeHandle disposeHandle = new SafeFileHandle( IntPtr.Zero, true );

        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        protected virtual void Dispose( bool disposing )
        {
            if ( disposed )
                return;

            if ( disposing )
            {
                disposeHandle.Dispose();
            }

            disposed = true;
        }

    }

    public class CommonMongo
    {
        public static Point GetEntityCoordinates( string id )
        {
            using ( MongoCRUD mongo = new MongoCRUD( "Reldawin" ) )
            {
                DBPlayer player = mongo.GetRecord<DBPlayer>( "player", new Guid( id ) );

                Point p = new Point( player.Entity.WorldPositionX, player.Entity.WorldPositionY );

                return p;
            }
        }

        public static bool DoesUserExist( string username )
        {
            using ( MongoCRUD mongo = new MongoCRUD( "Reldawin" ) )
            {
                DBPlayer player = mongo.GetRecord<DBPlayer>( "player", new MongoCRUD.Condition { field = "Username", value = username } );
                bool doesPlayerExist = player != null ? true : false;
                return doesPlayerExist;
            }
        }

        public static string GetEntityID( string username )
        {
            throw new NotImplementedException();

            using ( MongoCRUD mongo = new MongoCRUD( "Reldawin" ) )
            {
                return mongo.GetRecord<DBPlayer>( "player", new MongoCRUD.Condition { field = "ID", value = username } ).ID.ToString();
            }
        }

        public static void CreateAccount( string username, string password )
        {
            Random rand = new Random();
            DBPlayer dBPlayer = new DBPlayer
            {
                Password = password,
                Username = username,
                Entity = new DBEntity
                {
                    WorldPositionX = rand.Next( 0, Program.MapWidth ),
                    WorldPositionY = rand.Next( 0, Program.MapHeight )
                }
            };

            using ( MongoCRUD mongo = new MongoCRUD( "Reldawin" ) )
            {
                mongo.InsertRecord<DBPlayer>( "player", dBPlayer );
            }
        }

        public static string[] GetPlayerPasswordAndID( string username )
        {
            using ( MongoCRUD mongo = new MongoCRUD( "Reldawin" ) )
            {
                DBPlayer player = mongo.GetRecord<DBPlayer>( "player", new MongoCRUD.Condition { field = "Username", value = username } );

                string[] result = new string[2] { player.Password, player.ID.ToString() };

                return result;
            }
        }

        internal static void SetEntityCoodinates( int x, int y, string iD )
        {
            throw new NotImplementedException();
        }

        internal static List<DBTile> GetChunkData( int x, int y )
        {
            int chunkX = Convert.ToInt32( Math.Floor( (double)x / 16 ) );
            int chunkY = Convert.ToInt32( Math.Floor( (double)y / 16 ) );

            using ( MongoCRUD mongo = new MongoCRUD( "Reldawin" ) )
            {
                List<DBTile> tiles = mongo.GetChunk<DBTile>( chunkX, chunkY );

                return tiles;
            }
        }
    }
}
