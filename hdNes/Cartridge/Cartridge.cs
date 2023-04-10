using System;
using System.IO;
using hdNes.Cartridge.Enums;
using hdNes.Cartridge.Interfaces;
using hdNes.Cartridge.MMC;

namespace hdNes.Cartridge
{
    public class Cartridge
    {
        //PRG ROM attributes:
        private int _prgRomBanks;
        private byte[] _prgRom;
        
        //PRG RAM attributes:
        private bool _hasPrgRam;
        private int _prgRamSize;
        private byte[] _prgRam;
        
        //CHR ROM attributes:
        private int _chrRomBanks;
        private byte[] _chrRom;
        private bool _usesChrRam;
        
        //Memory Mapper Chip attributes:
        private iMapper _memoryMapper;
        private byte _iNesMapperId;

        //Background layout attributes:
        private enumNametableMirroring _nametableMirroring;
        private bool _isFourScreenVRAM;
        
        //Tv system attributes:
        private enumTvSystem _tvSystem1;
        private enumTvSystem _tvSystem2;
        
        //Miscellaneous cartridge attributes:
        private bool _hasBattery;
        private bool _hasTrainer;
        private bool _busConflicts;

        /* .NES file format: https://wiki.nesdev.org/w/index.php/INES */
        public void DecodeAndUploadNesFile(string iNesFilePath)
        {
            //Check if file exist:
            if (!File.Exists(iNesFilePath))
                throw new Exception($"Unable to load iNes file: {iNesFilePath}");

            //Load iNes file into a bytes array:
            byte[] iNesRom = File.ReadAllBytes((iNesFilePath));
           
            //Check if header is valid: 
            if (BitConverter.ToInt32(iNesRom, 0) != 0x1A53454E)
                throw new Exception("Invalid iNes ROM header !");
            /* Note: The first 4 bytes (0 to 3) of the header must contain $4E $45 $53 $1A (NES<EOF>) */
            
            //Check if iNes 1 file format:
            var nesFileType = 1;
            if ((iNesRom[7] & 0x0C) == 0x08)
                throw new Exception("Emulator can only read iNes 1 file format !");
            Console.WriteLine($"NES file type: {(iNesRom[7] & 0x0C)}");
            
            //Setup PRG ROM:
            _prgRomBanks = iNesRom[4];
            var prgRomSize = _prgRomBanks * 16384;
            _prgRom = new byte[prgRomSize];
            Console.WriteLine($"PRG ROM Size: {prgRomSize}");
            
            //Setup CHR ROM:
            _chrRomBanks = iNesRom[5];
            var chrRomSize = 8192;
            if (_chrRomBanks != 0) chrRomSize = _chrRomBanks * 8192;
                else _usesChrRam = true;
            _chrRom = new byte[chrRomSize];
            Console.WriteLine($"CHR ROM Size: {chrRomSize}");
            
            //Get mirroring, battery, trainer, mapper lower nibble - all from 6th byte:
            _nametableMirroring = (Convert.ToBoolean(iNesRom[6] & 0x01)) ? 
                enumNametableMirroring.Vertical : enumNametableMirroring.Horizontal;
            _hasBattery = Convert.ToBoolean(iNesRom[6] & 0x02);
            _hasTrainer = Convert.ToBoolean(iNesRom[6] & 0x04);
            _isFourScreenVRAM = Convert.ToBoolean(iNesRom[6] & 0x08);
            _iNesMapperId = (byte)((iNesRom[6] & 0xF0) >> 4);
            
            //Get VS Unisystem, PlayChoice-10, mapper higher nibble:
            
            _iNesMapperId |= (byte) (iNesRom[7] & 0xF0);
            Console.WriteLine($"NES Mapper ID: {_iNesMapperId}");
            
            //Setup PRG RAM size (flags 8):
            _prgRamSize = iNesRom[8] * 8192;
            
            //Get TV system (flags 9):
            switch (iNesRom[9] & 0x01)
            {
                case 0: _tvSystem1 = enumTvSystem.NTSC; break;
                case 1: _tvSystem1 = enumTvSystem.PAL;  break;
            }
            
            //Get Tv system, PRG RAM presence and bus conflict (flags 10):
            switch (iNesRom[10] & 0x03)
            {
                case 0: _tvSystem2 = enumTvSystem.NTSC; break;
                case 1: _tvSystem2 = enumTvSystem.DualCompatible; break;
                case 2: _tvSystem2 = enumTvSystem.PAL;  break;
                case 3: _tvSystem2 = enumTvSystem.DualCompatible; break;
            }
            _hasPrgRam = Convert.ToBoolean(iNesRom[10] & 0x10);
            _busConflicts = Convert.ToBoolean(iNesRom[10] & 0x20);

            switch (_iNesMapperId)
            {
                case 0: _memoryMapper = new NROM(_prgRomBanks, _chrRomBanks); break;
            }
        }

        /* Pour test - A détruire. */
        public void InjectRom(string iNesFilePath)
        {
            //Check if file exist:
            if (!File.Exists(iNesFilePath))
                throw new Exception($"Unable to load iNes file: {iNesFilePath}");

            /* Structure du fichier pour mémoire :
             Les 16 premiers bytes consitituent les header,
             Comme c'est un ROM de type 0 alors 16384 suivt pour le PRG et 8192 pour le CHR
             soit un total de 24 592 bytes.*/
            
            byte[] iNesRom = File.ReadAllBytes((iNesFilePath));

            int prgRomLength = _prgRom.Length;
            for (ushort i = 0; i < prgRomLength - 1; i++)
            {
                _prgRom[i] = iNesRom[i + 16];
            }

            int chrRomLength = _chrRom.Length;
            for (ushort i = 0; i < chrRomLength - 1; i++)
            {
                _chrRom[i] = iNesRom[i + prgRomLength + 16];
            }
        }

        public void UnitTest_Configure(byte[] UnitTestRom)
        {
            //Setup PRG ROM:
            _prgRomBanks = 1;
            var prgRomSize = _prgRomBanks * 16384;
            _prgRom = new byte[prgRomSize];
            Console.WriteLine($"PRG ROM Size: {prgRomSize}");
            
            //Setup CHR ROM:
            _chrRomBanks = 1;
            var chrRomSize = 8192;
            if (_chrRomBanks != 0) chrRomSize = _chrRomBanks * 8192;
                else _usesChrRam = true;
            _chrRom = new byte[chrRomSize];
            Console.WriteLine($"CHR ROM Size: {chrRomSize}");

            _memoryMapper = new NROM(_prgRomBanks, _chrRomBanks);

            for (int i = 0; i < UnitTestRom.Length; i++)
            {
                _prgRom[i] = UnitTestRom[i];
            }
        }

        public bool cpuRead(ushort address, ref byte data)
        {
            uint mappedAdress = 0;

            if (_memoryMapper.cpuMapperRead(address, ref mappedAdress, ref data))
            {
                if (mappedAdress == 0xFFFFFFFF)
                {
                    return true;
                }
                else
                {
                    data = _prgRom[mappedAdress];
                }
                return true;
            }
            else
            {
                return false;    
            }
            
        }

        public bool cpuWrite(ushort address, byte data)
        {
            if (address >= 0x8000 && address <= 0xFFFF)
            {
                _prgRom[address & 0x3FFF] = data;   
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ppuRead(ushort address, out byte data)
        {
            data = 0;
            return false;
        }

        public bool ppuWrite(ushort address, byte data)
        {
            return false;
        }
    }
}