using System.Text;
using System;

namespace LarinEncriprionApp
{
    class KeyGen
    {
        //генерація сессійного ключа
        public byte[] GenerateKey(string sKey)
        {
            int x = 0;
            byte[] preKey = ASCIIEncoding.ASCII.GetBytes(sKey);
            byte[] key = new Byte[128];
            Random rnd = new Random();
            rnd.NextBytes(key);
            for(int i = 0; i<128; i++, x++)
            {
                key[i] += preKey[x % preKey.Length];
            }
            return key;
        }
    }
}
