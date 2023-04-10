using System;
using hdNes.Nes;

namespace hdNes
{
   
    class Program
    {
        //private static string _nesFilePath = "D:/Projects/Emulators/Nes 2021/hdNes/hdNes/Roms/Super_mario_brothers.nes";
        private static string _nesFilePath = "D:/Projects/Emulators/Nes 2021/hdNes/hdNes/Roms/nestest.nes";
        private static Board nesBoard;
        
        static void Main(string[] args)
        {
            Console.WriteLine("Emulator starting...");
            nesBoard = new Board();
            nesBoard.Cartridge.DecodeAndUploadNesFile(_nesFilePath);
            nesBoard.Cartridge.InjectRom(_nesFilePath);
            nesBoard.Reset();
            nesBoard.cpu.PC = 0xC000;
            Console.WriteLine("Program Counter at start: 0x" + nesBoard.cpu.PC.ToString("X4") + "h");
            //nesBoard.cpu.Debug(16384);
            nesBoard.cpu.FetchDebugAndReport(16384);
            Console.WriteLine(nesBoard.cpu.Read(0x02));
            Console.WriteLine(nesBoard.cpu.Read(0x03));

        }
        
    }
}