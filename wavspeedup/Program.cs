using System;
using System.Globalization;
using System.IO;

namespace wavspeedup
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {

                string fileName;
                do
                {
                    Console.Write("Enter filename: ");
                    fileName = Console.ReadLine();
                } while (!File.Exists(fileName));
                byte[] header = new byte[44];
                using (var stream = File.OpenRead(fileName))
                {
                    int count = stream.Read(header, 0, 44);
                }
                int mode = 0;
                do
                {
                    Console.Write("1 to speed up; 2 to slow down: ");
                    try
                    {
                        mode = Convert.ToInt32(Console.ReadLine());
                    }
                    catch { }
                } while (mode != 1 && mode != 2);
                string byteHex = Convert.ToByte(header[31]).ToString("x") + Convert.ToByte(header[30]).ToString("x") + Convert.ToByte(header[29]).ToString("x") + Convert.ToByte(header[28]).ToString("x");
                int bytes = int.Parse(byteHex, NumberStyles.HexNumber);
                if (mode == 1) bytes *= 2;
                else bytes /= 2;
                byteHex = bytes.ToString("x");
                byte[] byteFinal = new byte[4];
                char[] byteHexChar = new char[8];
                char[] temp = byteHex.ToCharArray();
                int g = temp.Length - 1;
                for (int i = 7; i >= 0; i--)
                {
                    if (g == 0)
                    {
                        byteHexChar[7 - i + 1] = temp[g];
                        byteHexChar[7 - i] = '0';
                        g--;
                        i -= 2;
                    }
                    if (g == -1)
                    {
                        byteHexChar[7 - i] = '0';
                    }
                    if (g - 1 >= 0)
                    {
                        byteHexChar[7 - i] = temp[g - 1];
                        byteHexChar[7 - i + 1] = temp[g];
                        i--;
                        g -= 2;
                    }
                }
                g = 0;
                for (int i = 0; i < 4; i++)
                {
                    byteFinal[i] = byte.Parse(byteHexChar[g].ToString() + byteHexChar[g + 1].ToString(), NumberStyles.HexNumber);
                    g += 2;
                }
                string freqHex = Convert.ToByte(header[27]).ToString("x") + Convert.ToByte(header[26]).ToString("x") + Convert.ToByte(header[25]).ToString("x") + Convert.ToByte(header[24]).ToString("x");
                int freqs = int.Parse(freqHex, NumberStyles.HexNumber);
                if (mode == 1) freqs *= 2;
                else freqs /= 2;
                freqHex = freqs.ToString("x");
                byte[] freqFinal = new byte[4];
                char[] freqHexChar = new char[8];
                char[] temp2 = freqHex.ToCharArray();
                int t = temp2.Length - 1;
                for (int i = 7; i >= 0; i--)
                {
                    if (t == 0)
                    {
                        freqHexChar[7 - i + 1] = temp2[t];
                        freqHexChar[7 - i] = '0';
                        t--;
                        i -= 2;
                    }
                    if (t == -1)
                    {
                        freqHexChar[7 - i] = '0';
                    }
                    if (t - 1 >= 0)
                    {
                        freqHexChar[7 - i] = temp2[t - 1];
                        freqHexChar[7 - i + 1] = temp2[t];
                        i--;
                        t -= 2;
                    }
                }
                t = 0;
                for (int i = 0; i < 4; i++)
                {
                    freqFinal[i] = byte.Parse(freqHexChar[t].ToString() + freqHexChar[t + 1].ToString(), NumberStyles.HexNumber);
                    t += 2;
                }
                using (var stream = File.Open(fileName, FileMode.Open))
                {
                    stream.Position = 28;
                    stream.Write(byteFinal, 0, 4);
                    stream.Position = 24;
                    stream.Write(freqFinal, 0, 4);
                }
                Console.WriteLine("Done! Start again? (y)");
            } while (Console.ReadKey(true).Key == ConsoleKey.Y);
        }
    }
}
