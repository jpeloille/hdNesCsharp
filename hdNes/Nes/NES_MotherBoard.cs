using hdNes.Nes.EventArgs;

namespace hdNes.Nes
{
     public sealed partial class Board
    {
        //public readonly Cpu_2A03 cpu2A03;
        public readonly Ricoh2A03 cpu;
        private RAM_GenericIC cpuRAM;
        public Cartridge.Cartridge Cartridge;
        
        public Board()
        {
            //cpu2A03 = new Cpu_2A03(this);
            cpu = new Ricoh2A03();
            cpuRAM = new RAM_GenericIC(0xFFFF);
            Cartridge = new Cartridge.Cartridge();
            InitializeCpuMemoryBusArbitrationLogic();
        }
        
        private void InitializeCpuMemoryBusArbitrationLogic()
        {
            for (int i = 0x0000; i < 0x2000; i++)
            {
                cpu.MemoryReadMapper[i] = cpuRAM.ReadByte;
                cpu.MemoryWriteMapper[i] = cpuRAM.WriteByte;
            }

            for (int i = 0x8000; i < 0xFFFF; i++)
            {
                cpu.MemoryReadMapper[i] = Cartridge.ReadByte;
            }
        }

        public void Reset()
        {
            //cpu2A03.Reset();
            cpu.SetInResetState();
        }
        
    }
}