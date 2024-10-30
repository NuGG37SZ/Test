using System.Data.SQLite;

namespace Airoport
{
    public class ConnectionDataBaseClass
    {
        public static string ConnectionString { get; private set; } =
            "Data Source=Airoport.db;Version=3;Cache=Shared;JournalMode=WAL;Synchronous=OFF;";
        public static SQLiteConnection Connection { get; private set; }

        public static void Connect()
        {
            Connection = new SQLiteConnection(ConnectionString);
            Connection.Open();
        }

        public static void Disconnect()
        {
            if (Connection != null && Connection.State == System.Data.ConnectionState.Open)
            {
                Connection.Close();
                Connection.Dispose();
                Connection = null;
            }
        }
    }
}
