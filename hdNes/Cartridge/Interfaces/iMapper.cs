namespace hdNes.Cartridge.Interfaces
{
    public interface iMapper
    {
        bool cpuMapperRead(ushort address, ref uint mappedAdress, ref byte data);
        bool cpuMapperWrite(ushort address, ref uint mappedAdress, ref byte data);
        
        bool ppuMapperRead(ushort address, ref uint mappedAdress);
        bool ppuMapperWrite(ushort address, ref uint mappedAdress);

        void reset();
    }
}