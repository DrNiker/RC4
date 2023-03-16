using System;
using System.IO;
using System.Security.Cryptography;

namespace LarinEncriprionApp
{
    class Program
    {
        public static void Main()
        {
            Console.WriteLine("Enter filename with path(example:C:\\Users\\drnik\\Desktop\\1.txt): ");
            string path1 = Console.ReadLine();
            string path2 = System.Reflection.Assembly.GetExecutingAssembly().Location + ".enc";//шлях до зашифрованого файлу
            string path3 = path1;//шлях до розшифрованого файлу

            byte[] entryBytes = File.ReadAllBytes(path1);//зчитування файлу
            byte[] outBytes;
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();//створення криптопровайдеру та отримання ключів шифрування
            string pKey = "<RSAKeyValue><Modulus>pIHCVJAIa9Msi6TnGYSXdNpNdtoTs72ZNCapJ17JZxeBbGSBt2L+g69+h6lf7U81AKRrdVS0HJhtQyaPXDVAeHzYiBPuETyBI7wQwzXZH4JkR6V9H37XAmllnP+2X9biuBl87G0QuWbAZAfMp7d8+UnvpWkapWT/qKwM3GQDnL3AgukNJFb9GqQD3cmzX/KscJZNS7udbm33pbuSBcOV+bq/6cl1FHQbPtmO2sak00/ZYG0zziJN2nGW1KioTzcps2rPkiRwXu0Q68ZJpV6jVjhBkpBO61KlbjRqoQd26znrrJcGgCRkhRFq0yPdWe5ob6r9oO2+PFi81iCX+gWtrQ==</Modulus><Exponent>AQAB</Exponent><P>2h28nI61o1LFcFrRf/k0WmJfLzjFwtPoozJ2kV37QsnWaGZ6/sexiMLVgonqPEo0aZAjQj7MuUkFHOsz1hE/3MPqUVfCRecIHeVtjEQWy8+eg8Ct3kfaCCIiMJLDSPB2C9UguUOATTLeTOANwvOHPWVVkrexACm9T8BZ/SFqlBc=</P><Q>wRRaJyJ5m15x7gGGNKUafH3K5YBHZoT8vT1E78K1t+NYYCODMjC1aM8TsYzmzhu1Fun0B/Xl9ETP1Js2+m/pYxvpd+oLAIKaGjUREHZCqBjrpY7LuIZiBJITeKKgfY5Ta0At9ZFzS9lfnKXnzxa3gptat8bngw08NkmgXg3Hsts=</Q><DP>y2LFOJteRlBtaSpWhmWjatCl/0uKJPUizeLFayHepNW+x5UgBanCI0vCejoOIG/1VaYhKqmSjwfG68Nqj6eADGTnfzg9IJ+hEveX4h6vvxb3sdWei1ImvkTN6ss/rCkFel0faYHHBrVzUu3uBya8qgRu+iOtugAx1kb/4qkhT7s=</DP><DQ>DRz3vn6t6apxbGu4FK097kSQ2iNrNgBENgoKO4pq0xFiuCzRRUBfob2mpcFfD5J8KxE6nKvLsVpXDyZ3hmzBWhTUN2uy0ZcNJkiCXy7Y+hevMXBQT7j4wTy2i9PnuH9JFRinxmzZesZJoxb1dnAhiIJKl/6vTIxCkaFQffLLpE8=</DQ><InverseQ>aUSvNZN36b2p9k5qOyvpzM5m/hpIC53eiaIsHp1F26SDYn4XqZWMJuop57or7xPUnL+posqnu+69mXZtqdGTKMBTWViRKmIXn/PW4ZxR2NjnSo4XcBEVaTQPri9penBk02B5uSayij0nRWNFN2O4nOLQq0lUBGSgzxAPGBmk63s=</InverseQ><D>YfXrNNP4T80Wr6lo/KcaUblVe7rtFJGI06dZ/1nQcRVsOpG3NO88REqS/FH+D6ClHVbLrrjb8FNMbTpfEDedxqIBhzbnA1iWRiNNkBFW1A6fjJKdac3sV0FQAye7AGND6wEyhM+FsAHh+y8xzYXUY9aFYGiwN2FnZJffqLXd+M2JIxG2L16xYrIR9H1fiQv4tuHa5m4ALnqCIOP1K5Pi6rhGfMLcMiAceyE9y4dLiG3z7mWmj3oLaNO3FFp7UfazEQ7T8a+WCr2hUy5taB5UWencBMdYp+m21LtBjsmQ22Iz64pmukh6Qd8hFkbFDhS6Jiwf1AgQ92f1h6gVd2pfuQ==</D></RSAKeyValue>";
            
            string key = "Larin"; //генерація ключа симетричного шифрування
            KeyGen kg = new KeyGen();
            byte[] byteKey = new byte[128];
            byteKey = kg.GenerateKey(key);

            RC4 encoder = new RC4(byteKey);
            outBytes = encoder.Encode(entryBytes, entryBytes.Length);//шифрування симетричним шифруванням

            RSA.FromXmlString(pKey);
            byteKey = RSA.Encrypt(byteKey, false);//шифрування симетричного ключа

            byte[] temp = new byte[outBytes.Length + 256];
            for (int i = 0; i < outBytes.Length; i++)//запис ключа до повідомлення
            {
                temp[i] = outBytes[i];
            }
            for (int i = 0; i < 256; i++)
            {
                temp[outBytes.Length + i] = byteKey[i];
            }
            outBytes = temp;

            File.WriteAllBytes(path2, outBytes);//запис зашифрованого файлу

            entryBytes = File.ReadAllBytes(path2);//зчитування зашифрованого файлу

            byteKey = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                byteKey[i] = entryBytes[entryBytes.Length - (256 - i)];
            }

            RSA.FromXmlString(pKey);
            byteKey = RSA.Decrypt(byteKey, false);//розшифровка симетричного ключа

            temp = new byte[entryBytes.Length - 256];
            for (int i = 0; i < entryBytes.Length - 256; i++)
            {
                temp[i] = entryBytes[i];
            }
            entryBytes = temp;

            RC4 decoder = new RC4(byteKey);//розшифровка файлу
            outBytes = decoder.Decode(entryBytes, entryBytes.Length);

            File.WriteAllBytes(path3, outBytes); //запис розшифрованого фалйлу
        }
    }
}
