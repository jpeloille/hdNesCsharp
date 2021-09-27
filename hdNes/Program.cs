using System;
using hdNes.Nes;

namespace hdNes
{
   
    class Program
    {
        private static string _nesFilePath = "D:/Projects/Emulators/Nes 2021/hdNes/hdNes/Roms/Super_mario_brothers.nes";
        private static Board nesBoard;
        
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            nesBoard = new Board();
            nesBoard.Cartridge.DecodeAndUploadNesFile(_nesFilePath);
            nesBoard.Reset();

        }
    }
}