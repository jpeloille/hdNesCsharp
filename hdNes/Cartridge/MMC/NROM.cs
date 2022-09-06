using  hdNes.Cartridge.Interfaces;

namespace hdNes.Cartridge.MMC
{
    public class NROM : iMapper
    {
        
        private int _prgBanks;
        private int _chrBanks;
        public NROM(int prgBanks, int chrBanks)
        {
            _prgBanks = prgBanks;
            _chrBanks = chrBanks;
        }
        
        public bool cpuMapperRead(ushort address, ref uint mappedAdress, ref byte data)
        {
            // Two cases:
            // 1: PRG ROM = 16 kb then 
            //    CPU ADD 0x8000 -> 0xBFFF: map    PRG ROM ADD 0x0000 -> 0x3FF
            //    CPU ADD 0xC000 -> 0xFFFF: mirror PRG ROM ADD 0x0000 -> 0x3FF
            // 2: PRG ROM = 32 kb then 
            //    CPU ADD 0x8000 -> 0xFFFF: map    PRG ROM ADD 0x0000 -> 0x7FF
            
            if (address >= 0x8000 && address <= 0xFFFF)
            {
                mappedAdress = (ushort)(address & (_prgBanks > 1 ? 0x7FFF : 0x3FFF));
                return true;
            }

            return false;
        }

        public bool cpuMapperWrite(ushort address, ref uint mappedAdress, ref byte data)
        {
            throw new System.NotImplementedException();
        }

        public bool ppuMapperRead(ushort address, ref uint mappedAdress)
        {
            throw new System.NotImplementedException();
        }

        public bool ppuMapperWrite(ushort address, ref uint mappedAdress)
        {
            throw new System.NotImplementedException();
        }

        public void reset()
        {
            throw new System.NotImplementedException();
        }
    }
}