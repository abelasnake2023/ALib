namespace ALib;



using System;
using ALib.Database.ALibSqlServer;



internal class Program
{
    private Program()
    {
        //scince this class is for testing the ALib classes
        //it will not be accessible by any other class, that's 
        //why private.
    }

    public static void Main()
    {
        object[,] param = new object[2, 3]
{
            { "@username", "varchar", "abelasnake" },
            { "@password", "varchar", "password" }
};

        ALibDataReader reader = new ALibDataReader();
        object[,] result = reader.ExecuteTableValuedFunction("SearchManager", "*", param);

        //string reason = (string)result[0, 1];
    }
}