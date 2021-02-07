using System;
using RJCP.IO.Ports;

namespace PMS5003 {
    public class Program {

        private static ushort GetShort(byte b1, byte b2)
        {
            return (ushort) (b2 | b1 << 8);
        }

        public static void Main(string[] args) {

            if (args.Length != 1)
            {
                Console.WriteLine("Usage: PMS5003.exe <port>");
                return;
            }

            var portName = args[0];

            using (var port = new SerialPortStream(portName))
            {
                port.BaudRate = 9600;
                port.Parity = Parity.None;
                port.Open();

                while (true)
                {
                    // search for header byte1
                    while (port.ReadByte() != 0x42) { }

                    if (port.ReadByte() != 0x4D)
                    {
                        Console.WriteLine("WARNING: Bad header");
                        continue;
                    }

                    // Frame length
                    var fmLength = GetShort((byte) port.ReadByte(), (byte) port.ReadByte());

                    // Datas
                    var buffer = new byte[fmLength];
                    port.Read(buffer, 0, buffer.Length);

                    // Read as ushort
                    var data = new ushort[fmLength / 2];
                    for (int i = 0; i < buffer.Length; i+=2)
                    {
                        data[i / 2] = GetShort(buffer[i], buffer[i + 1]);
                    }

                    // Checksum
                    ushort receiveSum = 0x42 + 0x4D + 28;
                    for (byte i = 0; i < (buffer.Length - 2); i++)
                    {
                        receiveSum += buffer[i];
                    }

                    if (receiveSum - data[13] != 0)
                    {
                        Console.WriteLine("WARNING: Bad Checksum");
                        continue;
                    }

                    Console.WriteLine($"PM1.0 ug / m3: {data[0]}");
                    Console.WriteLine($"PM2.5 ug / m3: {data[1]}");
                    Console.WriteLine($"PM10 ug/ m3: {data[2]}");

                    Console.WriteLine($"PM1.0 ug / m3(atmos env): {data[3]}");
                    Console.WriteLine($"PM2.5 ug / m3(atmos env): {data[4]}");
                    Console.WriteLine($"PM10 ug/ m3(atmos env): {data[5]}");

                    Console.WriteLine($"> 0.3um in 0.1L air: {data[6]}");
                    Console.WriteLine($"> 0.5um in 0.1L air: {data[7]}");
                    Console.WriteLine($"> 1.0um in 0.1L air: {data[8]}");
                    Console.WriteLine($"> 2.5um in 0.1L air: {data[9]}");
                    Console.WriteLine($"> 5.0um in 0.1L air: {data[10]}");
                    Console.WriteLine($"> 10um in 0.1L air: {data[11]}");

                }

                port.Close();
            }
        }
    }
}
