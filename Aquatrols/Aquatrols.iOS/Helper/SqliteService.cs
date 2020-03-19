using System;
using System.Data;
using SQLite;


namespace Aquatrols.iOS.Helper
{
    public class SqliteService
    {
        string dbName = AppResources.databaseName;
        private static SqliteService instance = null;
        public static SqliteService GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SqliteService();
                }
                return instance;
            }
        }
        /// <summary>
        /// class constructor
        /// </summary>
        private SqliteService()
        {
        }
        /// <summary>
        /// Gets the DBP ath.
        /// </summary>
        /// <returns>The DBP ath.</returns>
        /// <param name="dataName">Data name.</param>
        public string GetDBPath(string dataName)
        {
            string databasePath = string.Empty;
            try
            {
                databasePath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception", ex.Message.ToString(), ex.TargetSite.ToString(), DateTime.Now.ToString());
            }
            return databasePath;
        }
        /// <summary>
        /// Method to check if table exists in db.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool CheckTableExistorNot(string tableName)
        {
            bool result = false;
            try
            {
                SQLite.TableMapping map = new TableMapping(typeof(SqlDbType));
                object[] ps = new object[0];
                int tableCount = 0;
                using (SQLiteConnection dd = new SQLiteConnection(GetDBPath(dbName)))
                {
                    tableCount = dd.Query(map, "SELECT * FROM sqlite_master WHERE type = 'table' AND name = '" + tableName + "'", ps).Count; // Executes the query from which we can count the results
                }
                if (tableCount == 0)
                {
                    result = false;
                }
                else if (tableCount == 1)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString(), ex.Message.ToString(), this.ToString() + " " + ex.TargetSite.ToString());
            }
            return result;
        }
        //public void CreateTables()
        //{
        //    try
        //    {
        //        using (SQLiteConnection db = new SQLiteConnection(GetDBPath(dbName), SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex, true))
        //        {
        //            db.CreateTable<SummaryDbModel>();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception", ex.Message.ToString(), ex.TargetSite.ToString(), DateTime.Now.ToString());
        //    }
        //}

    }
}
