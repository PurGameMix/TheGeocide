using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.DataPersistence
{
    public class FileJsonDataHandler
    {
        private string _dataDirPath = "";
        private string _dataFileName = "";
        private bool _isEncryptionUsed;
        private readonly string _message = "Well done!";

        public FileJsonDataHandler(string dataDirPath, bool isEncryptionUsed, string dataFileName = "")
        {
            _dataDirPath = dataDirPath;
            setFile(isEncryptionUsed, dataFileName);
            if (!Directory.Exists(dataDirPath))
            {
                Directory.CreateDirectory(dataDirPath);
            }
        }

        public void setFile(bool isEncryptionUsed, string dataFileName)
        {
            _isEncryptionUsed = isEncryptionUsed;
            _dataFileName = isEncryptionUsed ? "enc_" + dataFileName : dataFileName;
        }

        public T Load<T>() {
            string fullPath = Path.Combine(_dataDirPath, _dataFileName);

            T loadedData = default(T);
            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    if (_isEncryptionUsed)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }

                    loadedData = JsonConvert.DeserializeObject<T>(dataToLoad);

                }
                catch(Exception e)
                {
                    Debug.LogError($"Error occured when trying to load data from file: {fullPath} \n Exception : {e}");
                }
            }

            return loadedData;
        }

        public void Save<T>(T data)
        {
            string fullPath = Path.Combine(_dataDirPath, _dataFileName);
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                string dataToStore = JsonConvert.SerializeObject(data);

                if (_isEncryptionUsed)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using(StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }catch(Exception e)
            {
                Debug.LogError($"Error occured when trying to save data to file: {fullPath} \n Exception : {e}");
            }
        }

        /// <summary>
        /// Xor encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string EncryptDecrypt(string data)
        {
            string modifedData = "";
            for(int i = 0; i < data.Length; i++)
            {
                modifedData += (char)(data[i] ^ _message[i % _message.Length]);
            }

            return modifedData;
        }
    }
}
