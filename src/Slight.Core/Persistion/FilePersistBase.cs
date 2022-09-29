using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Slight.Core
{
    [PersistConvert(typeof(DefaultPersistConvert), typeof(DefaultPersistConvertBack))]
    public abstract class FilePersistBase : PersistBase
    {

        private readonly string fileName;

        protected FilePersistBase(string fileName)
        {
            this.fileName = fileName;
        }

        protected override IDictionary<string, byte[]> Read()
        {
            Dictionary<string, byte[]> mapper = new();
            if (File.Exists(fileName))
            {
                byte[] fileBuffer = File.ReadAllBytes(fileName);
                byte[] buffer = DecryptBufffer("Slight.Core.EncryptToken", fileBuffer);

                using MemoryStream bufferStream = new(buffer);
                using BinaryReader reader = new(bufferStream);
                while (bufferStream.Position < bufferStream.Length)
                {
                    byte[] headerBuffer = reader.ReadBytes(reader.ReadInt32());

                    string header = System.Text.Encoding.UTF8.GetString(headerBuffer);

                    byte[] data = reader.ReadBytes(reader.ReadInt32());

                    mapper[header] = data;
                }
                fileBuffer = null;
                buffer = null;
            }

            return mapper;
        }

        protected override void Wtire(IReadOnlyDictionary<string, byte[]> persistMapper)
        {
            using MemoryStream bufferStream = new();
            using BinaryWriter writer = new(bufferStream);
            foreach (KeyValuePair<string, byte[]> kv in persistMapper)
            {
                byte[] header = System.Text.Encoding.UTF8.GetBytes(kv.Key.Trim());
                writer.Write(header.Length);
                writer.Write(header);

                writer.Write(kv.Value.Length);
                writer.Write(kv.Value);
            }

            byte[] buffer = EncryptBufffer("Slight.Core.EncryptToken", bufferStream.ToArray());

            using FileStream stream = File.OpenWrite(fileName);
            using BinaryWriter writer2 = new(stream);
            writer2.Write(buffer);
        }

        protected virtual byte[] DecryptBufffer(string encryptKey, byte[] buffer)
        {
            return Cryptor.Decrypt(encryptKey, buffer);
        }
        protected virtual byte[] EncryptBufffer(string encryptKey, byte[] buffer)
        {
            return Cryptor.Encrypt(encryptKey, buffer);
        }


        public class DefaultPersistConvert : IPersistConvert
        {
            public byte[] Convert(object propertyValue)
            {
                using MemoryStream stream = new();
                BinaryFormatter formatter = new();

                formatter.Serialize(stream, propertyValue);

                return stream.ToArray();

            }
        }

        public class DefaultPersistConvertBack : IPersistConvertBack
        {
            public object ConvertBack(byte[] targetPropertyValueBuffer, Type targetPropertyType)
            {
                using (MemoryStream stream = new())
                {
                    stream.Write(targetPropertyValueBuffer, 0, targetPropertyValueBuffer.Length);
                    stream.Seek(0, SeekOrigin.Begin);
                    BinaryFormatter formatter = new();
                    object target = formatter.Deserialize(stream);

                    if (targetPropertyType.IsAssignableFrom(target.GetType()))
                    {
                        return target;
                    }
                }

                return true;
            }
        }
    }
}