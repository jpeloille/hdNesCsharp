using hdNes.Nes.EventArgs;
using hdNes.Nes.Interfaces;

namespace hdNes.Nes
{
    public class RAM_GenericIC : I8BitDataBus
    {
        private uint _size;
        private byte[] _memory;

        public RAM_GenericIC(uint size)
        {
            _size = size;
            _memory = new byte[size];
        }

        public byte ReadByte(uint address)
        {
            return _memory[address];
        }

        public void WriteByte(uint address, byte data)
        {
            _memory[address] = data;
        }
    }
}