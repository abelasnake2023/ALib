namespace ALib.Database.ALibSqlServer;



using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;



public class ALibADO
{
    private static string connectionString;
    private static SqlConnection connection;



    

    static ALibADO() //Default connection and Database
    {
        connectionString = "Data Source=mssql-161163-0.cloudclusters.net, 10009;Initial Catalog=CSharpProject;" +
            "Persist Security Info=True;User ID=abelasnake;Password=AbelAsnakeCSharp2024";
        connection = new SqlConnection(connectionString);
    }
    public ALibADO() // the class will use the default database connection
    { 
        ALibADO.connection = connection;
    }
    public ALibADO(SqlConnection connection) //preferred SqlConnection
        :this()
    {
        ALibADO.connection = connection;
    }
    public ALibADO(string connectionString) //preferred connection string
        :this()
    {
        ALibADO.connectionString = connectionString;
    }





    //the following method Open connection and return true if the connection opened else return false
    protected bool OpenForClosedConnection()
    {
        try
        {
            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                Debug.WriteLine("Connected");
                return true;
            }
            else
            {
                Debug.WriteLine("Not Connected");
                return false;
            }
        }
        catch (SqlException se)
        {
            Debug.WriteLine(se.Message);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }

        return false;
    }
    //the following method Close connection and return true if the connection closed else return false
    protected bool CloseForOpenedConnection()
    {
        try 
        {
            connection.Close();
            if (connection.State == ConnectionState.Closed)
            {
                Debug.WriteLine("Closed");
                return true;
            }
            else
            {
                Debug.WriteLine("Not Closed");
                return false;
            }
        }
        catch (SqlException se)
        {
            Debug.WriteLine(se.Message);
        }
        catch (Exception e) 
        {
            Debug.WriteLine(e.Message);
        }

        return false;
    }
    //the following method creates array of sql parameter with the appropriate properties by taking 
    //the properties from the 2d object array. Returns true if all the sql parameters created 
    //successfully else returns false.
    protected bool CreateSqlParameter(SqlParameter[] sqlParam, object[,] objParam)
    {
        //object[][]:
        //  [i] -> represent SqlParameter object as a whole
        //  [i][0] -> parameter name
        //  [i][1] -> parameter datatype
        //  [i][2] -> parameter value

        string convertionExceptionValue = "";
        bool successCreatingSqlParam = true;

        for (byte i = 0; i < sqlParam.Length; i++) //this for loop assign each sql parameter object
        //properties(sql parameter name, sql parameter data type, and sql parameter value)
        {
            try
            {
                sqlParam[i].ParameterName = objParam[i, 0].ToString(); //Assigning the parameter name
                                                                       //for each `SqlParameter` object
                switch (objParam[i, 1].ToString())
                {
                    case "NVARCHAR":
                    case "NvarChar":
                    case "Nvarchar":
                    case "nvarchar":
                    case "nvarChar":
                        sqlParam[i].SqlDbType = SqlDbType.NVarChar; //Assigning the parameter datatype
                                                                    //for each `SqlParameter` object
                        sqlParam[i].Value = objParam[i, 2].ToString(); //Assigning the parameter value
                                                                       //for each `SqlParameter` object
                        break;
                    case "VARCHAR":
                    case "Varchar":
                    case "VarChar":
                    case "varchar":
                    case "varChar":
                        sqlParam[i].SqlDbType = SqlDbType.VarChar;
                        sqlParam[i].Value = objParam[i, 2].ToString();
                        break;
                    case "INT":
                    case "Int":
                    case "int":
                        int normalInt = 0;
                        sqlParam[i].SqlDbType = SqlDbType.Int;
                        int.TryParse(objParam[i, 2].ToString(), out normalInt);
                        sqlParam[i].Value = normalInt;
                        break;
                    case "TINYINT":
                    case "TinyInt":
                    case "tinyInt":
                        int tinyInt = 0;
                        sqlParam[i].SqlDbType = SqlDbType.TinyInt;
                        int.TryParse(objParam[i, 2].ToString(), out tinyInt);
                        sqlParam[i].Value = tinyInt;
                        break;
                    case "BIGINT":
                    case "BigInt":
                    case "bigInt":
                        long bigInt = 0;
                        sqlParam[i].SqlDbType = SqlDbType.BigInt;
                        long.TryParse(objParam[i, 2].ToString(), out bigInt);
                        sqlParam[i].Value = bigInt;
                        break;
                    case "DECIMAL":
                    case "Decimal":
                    case "decimal":
                        Decimal d = new Decimal();
                        sqlParam[i].SqlDbType = SqlDbType.Decimal;
                        Decimal.TryParse(objParam[i, 2].ToString(), out d);
                        sqlParam[i].Value = d;
                        break;
                    case "BIT":
                    case "Bit":
                    case "bit":
                        sqlParam[i].SqlDbType = SqlDbType.Bit;
                        sqlParam[i].Value = Convert.ToInt32(Convert.ToBoolean(objParam[i, 2].ToString()));
                        break;
                    case "DATE":
                    case "Date":
                    case "date":
                        DateOnly dateOnly = new DateOnly();
                        sqlParam[i].SqlDbType = SqlDbType.Date;
                        DateOnly.TryParse(objParam[i, 2].ToString(), out dateOnly);
                        sqlParam[i].Value = dateOnly;
                        break;
                    case "TIME":
                    case "Time":
                    case "time":
                        TimeSpan tSpan = new TimeSpan();
                        sqlParam[i].SqlDbType = SqlDbType.Time;
                        TimeSpan.TryParse(objParam[i, 2].ToString(), out tSpan);
                        sqlParam[i].Value = tSpan;
                        break;
                    case "DATETIME":
                    case "DateTime":
                    case "datetime":
                        DateTime dt = new DateTime();
                        sqlParam[i].SqlDbType = SqlDbType.DateTime;
                        DateTime.TryParse(objParam[i, 2].ToString(), out dt);
                        sqlParam[i].Value = dt;
                        break;
                    case "DATETIME2":
                    case "DateTime2":
                    case "datetime2":
                        DateTime dt2 = new DateTime();
                        sqlParam[i].SqlDbType = SqlDbType.DateTime2;
                        DateTime.TryParse(objParam[i, 2].ToString(), out dt2);
                        sqlParam[i].Value = dt2;
                        break;
                    case "VARBINARY":
                    case "varbinary":
                    case "varBinary":
                        sqlParam[i].SqlDbType = SqlDbType.VarBinary;
                        sqlParam[i].Value = objParam[i, 2];
                        break;
                    default:
                        Debug.WriteLine("Choose the appropriate sql data type");
                        successCreatingSqlParam = false;
                        break;
                }
            }
            catch (FormatException fe)
            {
                successCreatingSqlParam = false;

                convertionExceptionValue = objParam[i, 2].ToString();
                Debug.WriteLine("Can't convert " + convertionExceptionValue + " to "
                    + objParam[i, 1].ToString());
                break;
            }
            catch (Exception ex)
            {
                successCreatingSqlParam = false;

                Debug.WriteLine("Unknown Error!");
                break;
            }
        }

        return successCreatingSqlParam;
    }
    //the following method Add all sql parameters object in the array, to the sql command object.
    protected void AddSqlParameter(SqlParameter[] sqlParam, SqlCommand sqlCommand)
    {
        foreach (SqlParameter param in sqlParam)
        {
            sqlCommand.Parameters.Add(param);
        }
    }
    //Build the arguments identifier that will be passed to Any Sql Function
    protected string BuildArgumentName(object[,] objParm)
    {
        string argument = "";
        int size = objParm.Length / 3;
        for (int i = 0; i < size; i++)
        {
            argument += objParm[i, 0].ToString() + ", ";
        }

        string modArgument = "";
        for(byte i = 0; i < argument.Length - 2; i++)
        {
            modArgument += argument[i];
        }

        return modArgument;
    }
    protected SqlCommand SqlClientProp(string commandType = "Text", string commandText = "" , int commandTimeOut = 15)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Connection = connection;
        switch(commandType)
        {
            case "Text": case "text":
                cmd.CommandType = CommandType.Text;
                break;
            case "StoredProcedure": case "storedProcedure": case "storedprocedure":
                cmd.CommandType = CommandType.StoredProcedure;
                break;
            case "TableDirect": case "tableDirect": case "tabledirect":
                cmd.CommandType = CommandType.TableDirect;
                break;
            default:
                break;
        }
        cmd.CommandText = commandText;
        cmd.CommandTimeout = commandTimeOut;

        return cmd;
    }





}