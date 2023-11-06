using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Messenger
{
    public partial class SaveData
    {
        
        // Data class to be saved
        [Serializable]
        public class MyData
        {
            public MyData() { }
            public int Language { get; set; }
        }
        public SaveData.MyData data;
        // Save data to a file
        public void Save(MyData data)
        {
            using (FileStream stream = new FileStream("data.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MyData));
                serializer.Serialize(stream, data);
            }
        }

        // Load data from a file
        public MyData LoadData()
        {
            MyData data;
            if (File.Exists("data.xml"))
            {
                using (FileStream stream = new FileStream("data.xml", FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(MyData));
                    data = (MyData)serializer.Deserialize(stream);
                }
            }
            else
            {
                data = new MyData(); // Initialize with default values
            }
            return data;
        }
}
}
