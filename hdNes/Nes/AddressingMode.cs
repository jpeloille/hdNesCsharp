namespace hdNes.Nes
{
    sealed partial class Cpu_2A03 
    {
        private void FetchAddress_Immediate()
        {
            _absoluteAddress.word = PC;
            PC++;
        } //Confirmed 26-09-2021

        private void FetchAddress_Absolute()
        {
            _absoluteAddress.low = Read(PC);
            PC++;

            _absoluteAddress.high = Read(PC);
            PC++;
        } //Confirmed 26-09-2021

        private void FetchAddress_ZeroPage()
        {
            _absoluteAddress.low = Read(PC);
            PC++;

            _absoluteAddress.high = 0x00;
        } //Confirmed 01-10-2021

        private void FetchAddress_ZeroPageX()
        {
            byte low = Read(PC); PC++;
            low += X;
            
            _absoluteAddress.low = low;
            _absoluteAddress.high = 0x00;
        } //Confirmed 01-10-2021
        
        private void FetchAddress_ZeroPageY()
        {
            byte low = Read(PC); PC++;
            low += Y;
            
            _absoluteAddress.low = low;
            _absoluteAddress.high = 0x00;
        }

        private void FetchAddress_AbsoluteX()
        {
            _absoluteAddress.low = Read(PC);
            PC++;

            _absoluteAddress.high = Read(PC);
            PC++;

            _absoluteAddress.word += X;
        } //Confirmed 01-10-2021
        
        private void FetchAddress_AbsoluteY()
        {
            _absoluteAddress.low = Read(PC);
            PC++;

            _absoluteAddress.high = Read(PC);
            PC++;

            _absoluteAddress.word += Y;
        } //Confirmed 01-10-2021

        private void FetchAddress_IndirectX()
        {
            byte loa = Read(PC);
            PC++;

            ushort lsb = (ushort)((loa + X) & 0x00FF);
            _absoluteAddress.low = Read(lsb);
            
            ushort msb = (ushort)((loa + X + 1) & 0x00FF);
            _absoluteAddress.high = Read(msb);
        } //Confirmed 01-10-2021
        
        private void FetchAddress_IndirectY()
        {
            byte loa = Read(PC);
            PC++;

            ushort lsb = (ushort)((loa) & 0x00FF);
            _absoluteAddress.low = Read(lsb);
            
            ushort msb = (ushort)((loa + 1) & 0x00FF);
            _absoluteAddress.high = Read(msb);

            _absoluteAddress.word += Y;
        }
        
        private void FetchAddress_Indirect()
        {
            byte loa = Read(PC);
            PC++;

            ushort lsb = (ushort)(loa & 0x00FF);
            _absoluteAddress.low = Read(lsb);
            
            ushort msb = (ushort)((loa + 1) & 0x00FF);
            _absoluteAddress.high = Read(msb);
        }
        
        private void FetchAddress_Relative()
        {
            // This address mode is exclusive to branch instructions. The address
            // The address must reside within -128 to +127 of the branch instruction.

            _relativeAddress.low = Read(PC);
            PC++;

            if ((_relativeAddress.low & 0x80) == 1)
            {
                _relativeAddress.low = 0x00;
                _relativeAddress.high = 0xFF;
            }
        }  
    }
}