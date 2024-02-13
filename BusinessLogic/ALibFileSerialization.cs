namespace ALib.BusinessLogic;



using System.IO;
using Newtonsoft.Json;



public class ALibFileSerialization<T>
{
    private string filePath;
    private T theObj;



    public ALibFileSerialization(string filePath) // to only read
    {
        this.filePath = filePath.Trim();
    }
    public ALibFileSerialization(string filePath, T theObj) // both write and read are permitted.
    {
        this.theObj = theObj;
        this.filePath = filePath;
    }



    public string FilePath
    {
        get { return this.filePath; }
    }
    public T TheObject
    {
        get { return this.theObj; }
    }



    public bool DelAllContent()
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(this.filePath))
            {
                // Truncate the file by not writing anything
            }


            //File.WriteAllText(this.filePath, null);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }



    public bool StoreObjToFile()
    {
        try
        {
            string serializedObj = JsonConvert.SerializeObject(this.theObj);
            using (StreamWriter sw = File.AppendText(this.filePath))
            {
                sw.WriteLine(serializedObj); // Write the serialized object to a new line
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public List<T> ReadObj()
    {
        try
        {
            List<T> objects = new List<T>();

            if (File.Exists(this.filePath))
            {
                using (StreamReader sr = File.OpenText(this.filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        T obj = JsonConvert.DeserializeObject<T>(line);
                        if (obj != null)
                        {
                            objects.Add(obj);
                        }
                    }
                }
            }

            return objects;
        }
        catch (Exception)
        {
            return null;
        }
    }
}