namespace ALib;



using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ALib.BusinessLogic;
using ALib.Database.ALibSqlServer;
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

    public static void Main()
    {
        
    }
}