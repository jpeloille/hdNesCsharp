namespace hdNes.Nes.Interfaces
{
    public interface I8BitDataBus
    {
        byte ReadByte(uint address);

        void WriteByte(uint address, byte data);  
    }
}