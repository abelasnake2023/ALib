namespace ALib;

using ALib.BusinessLogic;
using ALib.Networking;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

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
        // sending
        /*byte[] a = Encoding.UTF8.GetBytes("hello");

        double e = 22.2;
        byte[] b = ALibDataNetProtocol.ToBeSentDataToALibProtocolType(20.0, "double", "01", "abelasnake", a);

        string s = Encoding.UTF8.GetString(b);

        //Console.WriteLine("go: "+  s);

        byte[] c = new byte[b.Length * 2];

        for(int i = 0; i < b.Length; i++)
        {
            c[i] = b[i];
        }
        for (int j = 0, i = b.Length; j < b.Length; i++, j++)
        {
            c[i] = b[j];
        }

        byte[] d = c.Concat(c).ToArray();
        byte[] f = new byte[2]
        {
            0, 1
        };

        List<byte[]> all = ALibDataNetProtocol.RidOutDelimiter(d, a);
        byte[] first = all[0];
        byte[] second = all[1];
        byte[] thrid = all[2];
        byte[] forth = all[3];

        object[] o = ALibDataNetProtocol.GetAllDataFromNonDelimitedPacket(first);
        Console.WriteLine("Only data: " + (double)o[1]);*/
    }
}