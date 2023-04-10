using System;
using hdNes.Cartridge;

namespace hdNes.Nes
{
     public sealed partial class Board
    {
        //public readonly Cpu_2A03 cpu2A03;
        public readonly Ricoh2A03 cpu;

        /* Declared as public only for unit testing. */
        
        private byte[] CpuRam = new byte[2048];
        private byte[] PpuRam = new byte[16384];
        public Cartridge.Cartridge Cartridge;

        public Board()
        {
            //cpu2A03 = new Cpu_2A03(this);
            cpu = new Ricoh2A03(this);
            Cartridge = new Cartridge.Cartridge();
        }

        public void Reset()
        {
            //cpu2A03.Reset();
            cpu.SetInResetState();
        }

        public void UnitTest_Reset()
        {
            //cpu2A03.UnitTest_Reset();
            
        }

    }
}