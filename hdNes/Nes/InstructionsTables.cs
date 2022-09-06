namespace hdNes.Nes
{
    sealed partial class Cpu_2A03
    {
        private void GenerateInstructionTable()
        {
            //Add to memory to accumulator with carry
            _instructionMethod[0x69] = ADC; _instructionAddressMode[0x69] = FetchAddress_Immediate; 
            _instructionMethod[0x65] = ADC; _instructionAddressMode[0x65] = FetchAddress_ZeroPage;
            _instructionMethod[0x75] = ADC; _instructionAddressMode[0x75] = FetchAddress_ZeroPageX;
            _instructionMethod[0x6D] = ADC; _instructionAddressMode[0x6D] = FetchAddress_Absolute;
            _instructionMethod[0x7D] = ADC; _instructionAddressMode[0x7D] = FetchAddress_AbsoluteX;
            _instructionMethod[0x79] = ADC; _instructionAddressMode[0x79] = FetchAddress_AbsoluteY;
            _instructionMethod[0x61] = ADC; _instructionAddressMode[0x61] = FetchAddress_IndirectX;
            _instructionMethod[0x71] = ADC; _instructionAddressMode[0x71] = FetchAddress_IndirectY;

            //AND memory with accumulator
            _instructionMethod[0x29] = AND; _instructionAddressMode[0x29] = FetchAddress_Immediate;
            _instructionMethod[0x25] = AND; _instructionAddressMode[0x25] = FetchAddress_ZeroPage;
            _instructionMethod[0x35] = AND; _instructionAddressMode[0x35] = FetchAddress_ZeroPageX;
            _instructionMethod[0x2D] = AND; _instructionAddressMode[0x2D] = FetchAddress_Absolute;
            _instructionMethod[0x3D] = AND; _instructionAddressMode[0x3D] = FetchAddress_AbsoluteX;
            _instructionMethod[0x39] = AND; _instructionAddressMode[0x39] = FetchAddress_AbsoluteY;
            _instructionMethod[0x21] = AND; _instructionAddressMode[0x21] = FetchAddress_IndirectX;
            _instructionMethod[0x31] = AND; _instructionAddressMode[0x31] = FetchAddress_IndirectY;
            
            //ASL Shit Left One Bit (Memory or Accumulator)
            _instructionMethod[0x0A] = ASL; _instructionAddressMode[0x0A] = FetchAddress_Immediate;
            _instructionMethod[0x06] = ASL; _instructionAddressMode[0x06] = FetchAddress_ZeroPage;
            _instructionMethod[0x16] = ASL; _instructionAddressMode[0x16] = FetchAddress_ZeroPageX;
            _instructionMethod[0x0E] = ASL; _instructionAddressMode[0x0E] = FetchAddress_Absolute;
            _instructionMethod[0x1E] = ASL; _instructionAddressMode[0x1E] = FetchAddress_AbsoluteX;
            
            //BCC Branch on Carry Clear
            _instructionMethod[0x90] = BCC; _instructionAddressMode[0x90] = FetchAddress_Relative;
            
            //BCC Branch on Carry Set
            _instructionMethod[0xB0] = BCS; _instructionAddressMode[0xB0] = FetchAddress_Relative;
            
            //BEQ Branch on result zero
            _instructionMethod[0xF0] = BEQ; _instructionAddressMode[0xF0] = FetchAddress_Relative;
            
            //BIT Test bits in memory with accumulator
            _instructionMethod[0x24] = BIT; _instructionAddressMode[0x24] = FetchAddress_ZeroPage;
            _instructionMethod[0x2C] = BIT; _instructionAddressMode[0x2C] = FetchAddress_Absolute;
            
            //BMI Branch on result minus
            _instructionMethod[0x30] = BMI; _instructionAddressMode[0x30] = FetchAddress_Relative;
            
            //BNE Branch on result not zero
            _instructionMethod[0xD0] = BNE; _instructionAddressMode[0xD0] = FetchAddress_Relative;
            
            //BPL Branch on result plus
            _instructionMethod[0x10] = BPL; _instructionAddressMode[0x10] = FetchAddress_Relative;
            
            //BRK Force break
            
            
            //BVC Branch on overflow clear
            _instructionMethod[0x50] = BVC; _instructionAddressMode[0x50] = FetchAddress_Relative;
            
            //BVS Branch on overflow set
            _instructionMethod[0x70] = BVS; _instructionAddressMode[0x70] = FetchAddress_Relative;
            
            //CLC Clear carry flag
            _instructionMethod[0x18] = CLC; _instructionAddressMode[0x18] = FetchAddress_Relative;
            
            //CLD Clear decimal mode
            _instructionMethod[0xD8] = CLD; _instructionAddressMode[0xD8] = FetchAddress_Relative;
            
            //CLI Clear interrupt disable bit
            _instructionMethod[0x58] = CLI; _instructionAddressMode[0x58] = FetchAddress_Relative;
            
            //CLV Clear overflow flag
            _instructionMethod[0xB8] = CLV; _instructionAddressMode[0xB8] = FetchAddress_Relative;
            
            //CMP Compare memory and accumulator
            _instructionMethod[0xC9] = CMP; _instructionAddressMode[0xC9] = FetchAddress_Immediate;
            _instructionMethod[0xC5] = CMP; _instructionAddressMode[0xC5] = FetchAddress_ZeroPage;
            _instructionMethod[0xD5] = CMP; _instructionAddressMode[0xD5] = FetchAddress_ZeroPageX;
            _instructionMethod[0xCD] = CMP; _instructionAddressMode[0xCD] = FetchAddress_Absolute;
            _instructionMethod[0xDD] = CMP; _instructionAddressMode[0xDD] = FetchAddress_AbsoluteX;
            _instructionMethod[0xD9] = CMP; _instructionAddressMode[0xD9] = FetchAddress_AbsoluteY;
            _instructionMethod[0xC1] = CMP; _instructionAddressMode[0xC1] = FetchAddress_IndirectX;
            _instructionMethod[0xD1] = CMP; _instructionAddressMode[0xD1] = FetchAddress_IndirectY;
            
            //CPX Compare memory and Index X
            _instructionMethod[0xE0] = CPX; _instructionAddressMode[0xE0] = FetchAddress_Immediate;
            _instructionMethod[0xE4] = CPX; _instructionAddressMode[0xE4] = FetchAddress_ZeroPage;
            _instructionMethod[0xEC] = CPX; _instructionAddressMode[0xEC] = FetchAddress_Absolute;
            
            //CPY Compare memory and Index Y
            _instructionMethod[0xC0] = CPY; _instructionAddressMode[0xC0] = FetchAddress_Immediate;
            _instructionMethod[0xC4] = CPY; _instructionAddressMode[0xC4] = FetchAddress_ZeroPage;
            _instructionMethod[0xCC] = CPY; _instructionAddressMode[0xCC] = FetchAddress_Absolute;
            
            //DEC Decrement memory by one
            _instructionMethod[0xC6] = DEC; _instructionAddressMode[0xC6] = FetchAddress_ZeroPage;
            _instructionMethod[0xD6] = DEC; _instructionAddressMode[0xD6] = FetchAddress_ZeroPageX;
            _instructionMethod[0xCE] = DEC; _instructionAddressMode[0xCE] = FetchAddress_Absolute;
            _instructionMethod[0xDE] = DEC; _instructionAddressMode[0xDE] = FetchAddress_AbsoluteX;
        }  
    }
}