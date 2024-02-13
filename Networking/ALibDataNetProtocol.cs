namespace ALib.Networking;



using System.Diagnostics;
using System.Net.Sockets;
using System.Text;



public class ALibDataNetProtocol
{
    private static SortedList<string, string> categories;



    static ALibDataNetProtocol()
    {
        categories = new SortedList<string, string>();

        // super manager always start with 0
        // normal manager always start with 1
        // users always start with 2
        categories.Add("S.M.C.D", "00");
        categories.Add("M.S.V.A", "10");
        categories.Add("M.S.V.D", "11");
        categories.Add("M.S.V.I", "12");
        categories.Add("M.S.V.S", "13");
        categories.Add("M.S.T.A", "14");
        categories.Add("M.S.T.D", "15");
        categories.Add("M.S.T.I", "16");
        categories.Add("M.S.T.S", "17");
        categories.Add("M.R.D", "18");
        categories.Add("U.R.D", "20");
        categories.Add("U.S.T.M", "21");
    }



    public static string GetCategoriesByKey(string key)
    {
        key = key.Trim();
        return categories[key];
    }



    public static bool WriteToBinaryFile(string filePath, byte[] write)
    {
        filePath = filePath.Trim();

        try
        {
            using (FileStream fileStream = new FileStream(filePath,
              FileMode.Truncate, FileAccess.Write))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    binaryWriter.Write(write, 0, write.Length);
                }
            }

            return true;
        }
        catch(Exception e)
        {
            Debug.WriteLine("Unable to write to the binary file.");
            return false;
        }
    }
    public static byte[] ReadBinaryFile(string filepath)
    {
        filepath = filepath.Trim();
        try
        {
            using (FileStream fileStream = new FileStream(filepath, 
                FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    FileInfo fileInfo = new FileInfo(filepath);

                    long bufferLength = fileInfo.Length;
                    byte[] buffer = new byte[bufferLength];
                    binaryReader.Read(buffer, 0, buffer.Length);

                    return buffer;   
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Unable to read the binary file!");
            return null;
        }
    }
    public byte[] ReadAllFromNetworkStream(NetworkStream stream)
    {
        List<byte> data = new List<byte>();

        byte[] buffer = new byte[1024];
        int bytesRead;

        try
        {
            do
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                data.AddRange(buffer.Take(bytesRead));
            } while (stream.DataAvailable);
        }
        catch(Exception ex)
        {
            Debug.WriteLine("Connection closed forcefully!");
            Console.WriteLine("Connection closed forcefully!");
            return null;
        }

        byte[] completeData = data.ToArray();

        return completeData;
    }



    public static List<byte[]> RidOutDelimiter(byte[] allByteFromStream, byte[] deliByte)
    {
        if (allByteFromStream == null || deliByte == null)
        {
            Debug.WriteLine("Null exception!");
            Console.WriteLine("Null exception!");
            return null;
        }

        List<byte[]> eachPacketInList = new List<byte[]>();
        byte[] singlePacket = null;
        int sInd = -1;
        int eInd = -1;
        bool waitForEInd = false;

        for (int i = 0; i < allByteFromStream.Length; i++)
        {
            if ((i + (deliByte.Length - 1)) < allByteFromStream.Length)
            {
                if (allByteFromStream[i] == deliByte[0] &&
                allByteFromStream[i + (deliByte.Length - 1)] == deliByte[deliByte.Length - 1])
                {
                    string mayBeDeili = Encoding.UTF8.GetString(allByteFromStream, i, deliByte.Length);

                    if (mayBeDeili == Encoding.UTF8.GetString(deliByte, 0, deliByte.Length))
                    {
                        int theStarting = -1;
                        if (i + deliByte.Length < allByteFromStream.Length)
                        {
                            theStarting = i + deliByte.Length; // inclusive
                        }
                        int theEnd = -1;
                        if (i > 0)
                        {
                            theEnd = i - 1; // inclusive
                        }

                        if (waitForEInd)
                        {
                            eInd = theEnd;

                            if (eInd != -1 && sInd != -1)
                            {
                                //cut the packet
                                singlePacket = new byte[(eInd - sInd) + 1];
                                for (int j = 0, k = sInd; k < eInd + 1; k++, j++)
                                {
                                    singlePacket[j] = allByteFromStream[k];
                                }
                                eachPacketInList.Add(singlePacket);
                            }
                        }
                        else
                        {
                            sInd = theStarting;
                        }
                        waitForEInd = !waitForEInd;
                    }
                }
            }
        }

        if(eachPacketInList.Count < 1)
        {
            return null;
        }
        return eachPacketInList;
    }
    public static byte[] ToBeSentDataToALibProtocolType(object mainData, string dType, string catag,
        string username, byte[] deliByte)
    {
        byte[] mainByte = null;

        // Differentiate the Datatype and change it to byte array.
        dType = dType.Trim();
        try
        {
            switch (dType)
            {
                case "double":
                case "Double":
                    dType = "double";
                    mainByte = BitConverter.GetBytes((double)mainData);
                    break;
                case "string":
                case "String":
                    dType = "string";
                    mainByte = Encoding.UTF8.GetBytes(mainData.ToString());
                    break;
                case "byte[]":
                case "Byte[]":
                    dType = "byte[]";
                    mainByte = (byte[])mainData;
                    break;
                case "bool":
                case "boolean":
                case "Bool":
                case "Boolean":
                    dType = "bool";
                    mainByte = BitConverter.GetBytes((bool)mainData);
                    break;
                default:
                    Debug.WriteLine("No Implementation for the datatype " + dType + " yet.");
                    return null;
            }
        }
        catch (FormatException ex)
        {
            Debug.WriteLine("Can not convert " + mainData + " to " + dType + ".");
            return null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Unknown error occurred while trying to convert " + mainData + " to " + dType + ".");
            return null;
        }

        // the size of the mainData after it is changed to byteArray
        long mainByteLength = mainByte.Length;

        //Build the metaData in string and next in byte[]
        string metaData = $"{catag.Trim()}\t{username.Trim()}\t{mainByteLength.ToString()}\t{dType}\n";
        byte[] metaDataByte = Encoding.UTF8.GetBytes(metaData);

        //build the byte[] that hold both the metadata and the main data
        byte[] allData = new byte[metaDataByte.Length + mainByte.Length];
        for (int i = 0; i < metaDataByte.Length; i++)
        {
            allData[i] = metaDataByte[i];
        }
        for (int j = 0, i = metaDataByte.Length; i < allData.Length; i++, j++)
        {
            allData[i] = mainByte[j];
        }

        //finally add to the head and the tail the delimiter
        byte[] finalByte = new byte[allData.Length + (deliByte.Length * 2)];

        for(int i = 0; i < deliByte.Length; i++)
        {
            finalByte[i] = deliByte[i];
        }
        for(int j = 0, i = deliByte.Length; j < allData.Length; i++, j++)
        {
            finalByte[i] = allData[j];
        }
        for (int j = 0, i = (deliByte.Length + allData.Length); j < deliByte.Length; i++, j++)
        {
            finalByte[i] = deliByte[j];
        }

        return finalByte;
    }
    public static object[] GetAllDataFromNonDelimitedPacket(byte[] allData)
    {
        //receiving
        // receive metaData
        if (allData == null)
        {
            Debug.WriteLine("The byte array you sent is null, unable to process!");
            return null;
        }


        string allTheDataStr = null;
        string category = null;
        string username = null;
        string mainDataLength = null;
        int lasIndex = 0;
        string mainDataDType = null;
        try
        {
            allTheDataStr = Encoding.UTF8.GetString(allData);
            category = allTheDataStr.Substring(0, allTheDataStr.IndexOf('\t'));

            allTheDataStr = allTheDataStr.Substring(allTheDataStr.IndexOf("\t") + 1);
            username = allTheDataStr.Substring(0, allTheDataStr.IndexOf('\t'));

            allTheDataStr = allTheDataStr.Substring(allTheDataStr.IndexOf("\t") + 1);
            mainDataLength = allTheDataStr.Substring(0, allTheDataStr.IndexOf('\t'));

            allTheDataStr = allTheDataStr.Substring(allTheDataStr.IndexOf("\t") + 1);
            lasIndex = allTheDataStr.IndexOf('\n');
            mainDataDType = allTheDataStr.Substring(0, lasIndex);
        }
        catch(Exception e)
        {
            Debug.WriteLine("The byte array you passed doesn't seem it is type of ALibNetDataProtocol.");
            return null;
        }
        

        // receive mainData
        int mainLen = 0;
        if (!int.TryParse(mainDataLength, out mainLen))
        {
            Debug.WriteLine("The byte array you passed doesn't seem it is type of ALibNetDataProtocol.");
            return null;
        }

        // the main Data to byte array
        byte[] mainDataToByteArray = new byte[mainLen];
        for (int j = allData.Length - 1, i = mainLen - 1; i > -1; i--, j--)
        {
            mainDataToByteArray[i] = allData[j];
        }

        // change the main Data that is in byte Array to the appropriate data type and put it to object
        object mainDataToObject = null;
        try
        {
            switch (mainDataDType)
            {
                case "double":
                    mainDataToObject = BitConverter.ToDouble(mainDataToByteArray);
                    break;
                case "string":
                    mainDataToObject = Encoding.UTF8.GetString(mainDataToByteArray);
                    break;
                case "byte[]":
                    mainDataToObject = mainDataToByteArray;
                    break;
                case "bool":
                    mainDataToObject = BitConverter.ToBoolean(mainDataToByteArray);
                    break;
                default:
                    Debug.WriteLine("The byte array you passed doesn't seem it is type of ALibNetDataProtocol.");
                    return null;
            }
        }
        catch (FormatException ex)
        {
            Debug.WriteLine("Unable to convert the byte array to it's original datatype!");
            return null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Unkown Error while trying to convert the byte array to an object type!");
            return null;
        }

        // Put the main Data that in object Type and the other meta data to object array.
        object[] allInfo = new object[4]
        {
            mainDataDType, // data type of the main Data
            mainDataToObject, // main Data
            category, // category
            username // username
        };

        return allInfo;
    }
}