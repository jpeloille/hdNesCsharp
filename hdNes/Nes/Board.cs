using System;
using hdNes.Cartridge;

namespace hdNes.Nes
{
    public class Board
    {
        public Cpu_2A03 _cpu2A03;
        /* Declared as public only for unit testing. */
        
        private byte[] CpuRam = new byte[0x2048];
        public Cartridge.Cartridge Cartridge;

        public Board()
        {
            _cpu2A03 = new Cpu_2A03(this);
            Cartridge = new Cartridge.Cartridge();
        }

        public void Reset()
        {
            _cpu2A03.Reset();
        }

        public void UnitTest_Reset()
        {
            _cpu2A03.UnitTest_Reset();
        }
        
        public byte CpuRead(ushort address)
        {
            byte data = 0x00;

            if (Cartridge.cpuRead(address, ref data))
            {
                //Cartridge address range: gain control over CPU.
            }
            else  if ((address >= 0x0000) && (address <= 0x1FFF))
            {
                //On board ROM attached to the CPU BUS - (Mirrored every 2048 bytes).
                return CpuRam[address & 0x07FF];
            }
            else if ((address >= 0x2000) && (address <= 0x3FFF))
            {
                //PPU I/O address range - (Mirrored every 8 bytes).
            }
            else if (address == 0x4015)
            {
                //APU read status.
            }
            else if (address >= 0x4016 && address <=0x4017)
            {
                //Controller.
            }
                
            return data;
        }

        public void CpuWrite(ushort address, byte data)
        {
            if ((address >= 0x0000) && (address <= 0x1FFF))
            {
                CpuRam[address & 0x07FF] = data;
            }
        }
        
    }
}