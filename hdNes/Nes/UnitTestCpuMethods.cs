namespace hdNes.Nes
{
    sealed partial class Cpu_2A03
    {
         
        public byte regA { get => A; set => A = value; }
        public byte regX { get => X; set => X = value; }
        public byte regY { get => Y; set => Y = value; }
        public ushort utPC { get => PC; set => PC = value; }
        
        public byte psr { get => P;  }
        
        public void SetInit()
        {
            PC = 0xC000;
            P = 0x24;
            S = 0xFD;
            A = 0x00;
            X = 0x00;
            Y = 0x00;
            _absoluteAddress.word = 0x0000;
            _relativeAddress.word = 0x0000;

            for (ushort i = 0; i < 0x2000; i++)
            {
                Write(i, 0x00);
            }
        }

        public void UnitTest_Write(ushort address, byte data)
        {
            Write(address, data);
        }
        
        public byte UnitTest_Read(ushort address)
        {
            return Read(address);
        }      
    }
}