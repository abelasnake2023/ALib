namespace ALib;



using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ALib.BusinessLogic;
using ALib.Database.ALibSqlServer;
using ALib.Networking;
using ALibWinForms;



internal class Program
{
    private static string connectionString;
    private static SqlConnection connection;

    private Program()
    {
        //scince this class is for testing the ALib classes
        //it will not be accessible by any other class, that's 
        //why private.
    }

    private static void Main()
    {
        /*        object[,] param = new object[1, 3]
                {
                    { "@photo", "int",  1 }
                };

                ALibDataReader reader = new ALibDataReader();
                object o = reader.ExecuteScalarFunction("dbo.GetPhoto", param);

                if (o is byte[])
                {
                    byte[] byteArray = (byte[])o;
                    // Do something with the byte array...
                    Console.WriteLine(BitConverter.ToString(byteArray));
                }*/
    }
}