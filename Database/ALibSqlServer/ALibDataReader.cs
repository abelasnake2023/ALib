namespace ALib.Database.ALibSqlServer;



using System.Collections;
using System.Data.SqlClient;
using System.Diagnostics;



public class ALibDataReader : ALibADO
{
    //the following method Execute stored procedure
    public void ExecuteStoredProcedure(string procName, object[,] objParam = null)
    {
        //the sql command properties  
        SqlCommand cmd = SqlClientProp("StoredProcedure", procName);

        //Create the parameters
        if (objParam != null)
        {
            int innerLength = objParam.GetLength(0);
            SqlParameter[] sqlParam = new SqlParameter[objParam.Length / innerLength];
            for (byte i = 0; i < sqlParam.Length; i++)
            {
                sqlParam[i] = new SqlParameter();
            }
            bool allCreated = CreateSqlParameter(sqlParam, objParam);

            //Add the parameters to the SqlCommand object(cmd)
            if (allCreated)
            {
                AddSqlParameter(sqlParam, cmd);
            }
        }

        //Open Connection
        OpenForClosedConnection();

        //Execute the stored procedure
        int rowsAffected = cmd.ExecuteNonQuery();

        //Close the connection
        CloseForOpenedConnection();

        Debug.WriteLine("Rows Affected: " + rowsAffected); // Rows affected by `ExecuteNonQuery`
    }
    //the following method Execute Scalar function
    public object ExecuteScalarFunction(string funcName, object[,] objParam = null)
    {
        //Build the Argument name
        string argument = "";
        if (objParam != null)
        {
            argument = BuildArgumentName(objParam);
        }

        //the sql command properties  
        string cmdText = $"SELECT {funcName}({argument})";
        SqlCommand cmd = SqlClientProp("Text", cmdText);

        //Create the parameters
        if (objParam != null)
        {
            int innerLength = 3;
            SqlParameter[] sqlParam = new SqlParameter[objParam.Length / innerLength];
            for (byte i = 0; i < sqlParam.Length; i++)
            {
                sqlParam[i] = new SqlParameter();
            }
            bool allCreated = CreateSqlParameter(sqlParam, objParam);

            //Add the parameters to the SqlCommand object(cmd)
            if (allCreated)
            {
                AddSqlParameter(sqlParam, cmd);
            }
        }

        //Open Connection
        OpenForClosedConnection();

        //Execute the Scalar Function
        var result = cmd.ExecuteScalar();

        //Close the connection
        CloseForOpenedConnection();

        return result; // return the scalar value, that is returned from the Scalar function
    }
    //the following method Execute TableValued function
    public object[,] ExecuteTableValuedFunction(string funcName, string columns = "*", object[,] objParam = null)
    {
        //Build the Argument name
        string argument = "";
        if (objParam != null)
        {
            argument = BuildArgumentName(objParam);
        }

        //the sql command properties  
        string cmdText = $"SELECT {columns} FROM {funcName}({argument})";
        SqlCommand cmd = SqlClientProp("Text", cmdText);

        //Create the parameters
        if (objParam != null)
        {
            int innerLength = 3;
            SqlParameter[] sqlParam = new SqlParameter[objParam.Length / innerLength];
            for (byte i = 0; i < sqlParam.Length; i++)
            {
                sqlParam[i] = new SqlParameter();
            }
            bool allCreated = CreateSqlParameter(sqlParam, objParam);

            //Add the parameters to the SqlCommand object(cmd)
            if (allCreated)
            {
                AddSqlParameter(sqlParam, cmd);
            }
        }

        //Open Connection
        OpenForClosedConnection();

        //Execute the Table valued Function
        SqlDataReader reader = cmd.ExecuteReader();

        ArrayList reader2D = new ArrayList();

        //All the result set -> in 2D ArrayList
        while (reader.Read())
        {
            ArrayList rowList = new ArrayList();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                var s = reader[i];
                rowList.Add(s);
            }

            reader2D.Add(rowList);
        }

        reader.Close(); //close reader also
        //Close the connection
        CloseForOpenedConnection();

        //convert the 2d Array list -> 2d object
        object[,] readerArray = new object[reader2D.Count, ((ArrayList)reader2D[0]).Count];
        for (int i = 0; i < reader2D.Count; i++)
        {
            int jSize = ((ArrayList)reader2D[0]).Count;

            for (int j = 0; j < jSize; j++)
            {
                readerArray[i, j] = ((ArrayList)reader2D[i])[j];
            }
        }

        return readerArray; // return the result set as a string
    }
}