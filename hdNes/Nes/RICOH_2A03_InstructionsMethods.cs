using System;
using static hdNes.Nes.Ricoh2A03.AddressingMode;

namespace hdNes.Nes
{
    sealed partial class Ricoh2A03
    {
        [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
        public class InstructionAttribute : Attribute
        {
            public string Mnemonic;
            public AddressingMode AddressingMode = None;
            public int OpCode;
            public int NoBytes;
            public int NoCycles;
        }

        #region ADC
        //Action : Add memory to accumulator with carry.
        //Operation :A + M + C -> A, C.
        //Flags : N,Z,C,V.
        [InstructionAttribute(Mnemonic = "ADC", AddressingMode = Immediate, OpCode = 0x69, NoBytes = 2, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "ADC", AddressingMode = ZeroPage,  OpCode = 0x65, NoBytes = 2, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "ADC", AddressingMode = ZeroPageX, OpCode = 0x75, NoBytes = 2, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "ADC", AddressingMode = Absolute,  OpCode = 0x6D, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "ADC", AddressingMode = AbsoluteX, OpCode = 0x7D, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "ADC", AddressingMode = AbsoluteY, OpCode = 0x79, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "ADC", AddressingMode = IndirectX, OpCode = 0x61, NoBytes = 2, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "ADC", AddressingMode = IndirectY, OpCode = 0x71, NoBytes = 2, NoCycles = 5)]
        private void ADC()
        {
            //Get operand from memory:
            M = ReadByte(_physicalAddress.word);
            M &= 0xFF;

            //Instruction Computation:
            result16 = (ushort)(A + M);
            if (C) result16 += 1;
            result8 = (byte)result16;

            //Refresh flag status:
            P &= 0x3C;
            N = ((result8 >> 7) & 1) == 1;
            V = ((result8 ^ A) & (result8 ^ M) & 0x80) == 0x80;
            Z = result8 == 0;
            C = result16 > 0xFF;

            //Set result:
            A = result8;
        }
        #endregion

        #region AND
        //Action : AND memory with accumulator.
        //Operation :A ^ M -> A.
        //Flags : N,Z.
        [InstructionAttribute(Mnemonic = "AND", AddressingMode = Immediate, OpCode = 0x29, NoBytes = 2, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "AND", AddressingMode = ZeroPage,  OpCode = 0x25, NoBytes = 2, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "AND", AddressingMode = ZeroPageX, OpCode = 0x35, NoBytes = 2, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "AND", AddressingMode = Absolute,  OpCode = 0x2D, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "AND", AddressingMode = AbsoluteX, OpCode = 0x3D, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "AND", AddressingMode = AbsoluteY, OpCode = 0x39, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "AND", AddressingMode = IndirectX, OpCode = 0x21, NoBytes = 2, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "AND", AddressingMode = IndirectY, OpCode = 0x31, NoBytes = 2, NoCycles = 5)]
        private void AND()
        {
            //Get operand from memory:
            M = ReadByte(_physicalAddress.word);
            M &= 0xFF;

            //Instruction Computation:
            result16 = (ushort)(A & M);
            result8 = (byte)result16;

            //Refresh flag status:
            P &= 0x7D;
            N = ((result8 >> 7) & 1) == 1;
            Z = result8 == 0;

            //Set result:
            A = result8;
        }
        #endregion

        #region ASL
        //Action : ASL shift left one bit (memory or accumulator).
        //Operation : C <- 76543210 <- 0.
        //Flags : N,Z,C.
        [InstructionAttribute(Mnemonic = "ASL", AddressingMode = Accumulator, OpCode = 0x0A, NoBytes = 1, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "ASL", AddressingMode = ZeroPage,    OpCode = 0x06, NoBytes = 2, NoCycles = 5)]
        [InstructionAttribute(Mnemonic = "ASL", AddressingMode = ZeroPageX,   OpCode = 0x16, NoBytes = 2, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "ASL", AddressingMode = Absolute,    OpCode = 0x0E, NoBytes = 3, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "ASL", AddressingMode = AbsoluteX,   OpCode = 0x1E, NoBytes = 3, NoCycles = 7)]
        private void ASL()
        {
            if (_opcode == 0x0A) operand = A;
            else operand = ReadByte(_physicalAddress.word);
            operand &= 0xFF;

            /* Faire le reset des flags impliqués */
            C = ((operand >> 7) & 1) == 1;
            operand = (byte)(((operand << 1) | 0) & 0xFF);

            N = ((operand >> 7) & 1) == 1;
            Z = operand == 0;

            if (_opcode == 0x0A) A = operand;
            else WriteByte(_physicalAddress.word, operand);
        }
        #endregion

        #region BCC
        //Action : BCC branch on carry clear.
        //Operation : Branch on C = 0.
        //Flags : -.
        [InstructionAttribute(Mnemonic = "BCC", AddressingMode = Relative, OpCode = 0x90, NoBytes = 2, NoCycles = 2)]
        private void BCC()
        {
            if (!C)
            {
                PC = _physicalAddress.word;
            }
        }
        #endregion

        #region BCS
        //Action : BCS branch on carry set.
        //Operation : Branch on C = 1.
        //Flags : -.
        [InstructionAttribute(Mnemonic = "BCS", AddressingMode = Relative, OpCode = 0xB0, NoBytes = 2, NoCycles = 2)]
        private void BCS()
        {
            if (C)
            {
                PC = _physicalAddress.word;
            }
        }
        #endregion

        #region BEQ
        //Action : BEQ branch on result zero.
        //Operation : Branch on Z = 0.
        //Flags : -.
        [InstructionAttribute(Mnemonic = "BEQ", AddressingMode = Relative, OpCode = 0xF0, NoBytes = 2, NoCycles = 2)]
        private void BEQ()
        {
            if (Z)
            {
                PC = _physicalAddress.word;
            }
        }
        #endregion

        #region BIT
        //Action : test bits in memory accumulator.
        //  Bit 6 & 7 are transferred to the status register.
        //  If the result of A^M is zero then Z=1, otherwise Z=0.
        //Operation : A ^ M, M7 -> N, M6 -> V.
        //Flags : N, Z, V.
        [InstructionAttribute(Mnemonic = "BIT", AddressingMode = ZeroPage, OpCode = 0x24, NoBytes = 2, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "BIT", AddressingMode = Absolute, OpCode = 0x2C, NoBytes = 3, NoCycles = 4)]
        private void BIT()
        {
            //Get operand from memory:
            M = ReadByte(_physicalAddress.word);
            M &= 0xFF;

            Z = (A & M) == 0;
            N = (M & 0x80) != 0;
            V = (M & 0x60) != 0;
        }
        #endregion

        #region BMI
        //Action : branch on result minus.
        //Operation : branch on N = 1.
        //Flags : -.
        [InstructionAttribute(Mnemonic = "BMI", AddressingMode = Relative, OpCode = 0x30, NoBytes = 2, NoCycles = 2)]
        private void BMI()
        {
            if (N)
            {
                _physicalAddress.word = (ushort)(PC + _relativeAddress.word);
                PC = _physicalAddress.word;
            }
        }
        #endregion

        #region BNE
        //Action : branch on result not zero.
        //Operation : branch on Z = 0.
        //Flags : -.
        [InstructionAttribute(Mnemonic = "BNE", AddressingMode = Relative, OpCode = 0xD0, NoBytes = 2, NoCycles = 2)]
        private void BNE()
        {
            if (!Z)
            {
                _physicalAddress.word = (ushort)(PC + _relativeAddress.word);
                PC = _physicalAddress.word;
            }
        }
        #endregion

        #region BPL
        //Action : branch on result plus.
        //Operation : branch on N = 0.
        //Flags : -.
        [InstructionAttribute(Mnemonic = "BPL", AddressingMode = Relative, OpCode = 0x10, NoBytes = 2, NoCycles = 2)]
        private void BPL()
        {
            if (!N)
            {
                _physicalAddress.word = (ushort)(PC + _relativeAddress.word);
                PC = _physicalAddress.word;
            }
        }
        #endregion

        #region BRK
        //Action : force break.Store the PC of the byte next the instruction. Store the status register.
        //Operation : force interrupt PC. PCL = 0xFFFE & PCH = 0xFFFF;
        //Flags : I = 1.
        [InstructionAttribute(Mnemonic = "BRK", AddressingMode = Implied, OpCode = 0x00, NoBytes = 1, NoCycles = 7)]
        private void BRK()
        {
            PC++;
            
            WriteByte((ushort)(0x100 + S), (byte)(PC >> 8 & 0x00FF));
            S--;
            
            WriteByte((ushort)(0x100 + S), (byte)(PC & 0x00FF));
            S--;
            
            WriteByte((ushort)(0x100 + S), P);
            S--;

            var PCL = ReadByte(0xFFFE);
            var PCH = ReadByte(0XFFFF);
            PC = (ushort)(((PCH << 8) & 0xFF00) | PCL);

        }
        #endregion

        #region BVC
        //Action : branch on overflow clear.
        //Operation : branch on V = 0.
        //Flags : -.
        [InstructionAttribute(Mnemonic = "BVC", AddressingMode = Relative, OpCode = 0x50, NoBytes = 2, NoCycles = 2)]
        private void BVC()
        {
            if (!V)
            {
                _physicalAddress.word = (ushort)(PC + _relativeAddress.word);
                PC = _physicalAddress.word;
            }
        }
        #endregion

        #region BVS
        //Action : branch on overflow set.
        //Operation : branch on V = 1.
        //Flags : -.
        [InstructionAttribute(Mnemonic = "BVS", AddressingMode = Relative, OpCode = 0x70, NoBytes = 2, NoCycles = 2)]
        private void BVS()
        {
            if (V)
            {
                _physicalAddress.word = (ushort)(PC + _relativeAddress.word);
                PC = _physicalAddress.word;
            }
        }
        #endregion

        #region CLC
        //Action : clear carry flag.
        //Operation : C <- 0.
        //Flags : C.
        [InstructionAttribute(Mnemonic = "CLC", AddressingMode = Implied, OpCode = 0x18, NoBytes = 1, NoCycles = 2)]
        private void CLC()
        {
            C = false;
        }
        #endregion

        #region CLD
        //Action : clear decimal mode.
        //Operation : D <- 0.
        //Flags : D.
        [InstructionAttribute(Mnemonic = "CLD", AddressingMode = Implied, OpCode = 0xD8, NoBytes = 1, NoCycles = 2)]
        private void CLD()
        {
            D = false;
        }
        #endregion

        #region CLI
        //Action : clear interrupt disable bit.
        //Operation : I <- 0.
        //Flags : I.
        [InstructionAttribute(Mnemonic = "CLI", AddressingMode = Implied, OpCode = 0x58, NoBytes = 1, NoCycles = 2)]
        private void CLI()
        {
            I = false;
        }
        #endregion

        #region CLV
        //Action : clear overflow flag.
        //Operation : V <- 0.
        //Flags : V.
        [InstructionAttribute(Mnemonic = "CLV", AddressingMode = Implied, OpCode = 0xB8, NoBytes = 1, NoCycles = 2)]
        private void CLV()
        {
            V = false;
        }
        #endregion

        #region CMP
        //Action : compare memory and accumulator.
        //Operation : A - M.
        //Flags : N,Z,C.
        [InstructionAttribute(Mnemonic = "CMP", AddressingMode = Immediate, OpCode = 0xC9, NoBytes = 2, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "CMP", AddressingMode = ZeroPage,  OpCode = 0xC5, NoBytes = 2, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "CMP", AddressingMode = ZeroPageX, OpCode = 0xD5, NoBytes = 2, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "CMP", AddressingMode = Absolute,  OpCode = 0xCD, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "CMP", AddressingMode = AbsoluteX, OpCode = 0xDD, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "CMP", AddressingMode = AbsoluteY, OpCode = 0xD9, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "CMP", AddressingMode = IndirectX, OpCode = 0xC1, NoBytes = 2, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "CMP", AddressingMode = IndirectY, OpCode = 0xD1, NoBytes = 2, NoCycles = 5)]
        private void CMP()
        {
            M = ReadByte(_physicalAddress.word);
            Z = (A == M);
            C = (A >= M);
            N = (((A - M) >> 7) & 1) == 1;
        }
        #endregion

        #region CPX
        //Action : compare memory and index X.
        //Operation : X - M.
        //Flags : N,Z,C.
        [InstructionAttribute(Mnemonic = "CPX", AddressingMode = Immediate, OpCode = 0xE0, NoBytes = 2, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "CPX", AddressingMode = ZeroPage,  OpCode = 0xE4, NoBytes = 2, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "CPX", AddressingMode = Absolute,  OpCode = 0xEC, NoBytes = 3, NoCycles = 4)]
        private void CPX()
        {
            M = ReadByte(_physicalAddress.word);
            Z = (X == M);
            C = (X >= M);
            N = (((X - M) >> 7) & 1) == 1;
        }
        #endregion

        #region CPY
        //Action : compare memory and index Y.
        //Operation : Y - M.
        //Flags : N,Z,C.
        [InstructionAttribute(Mnemonic = "CPY", AddressingMode = Immediate, OpCode = 0xC0, NoBytes = 2, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "CPY", AddressingMode = ZeroPage,  OpCode = 0xC4, NoBytes = 2, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "CPY", AddressingMode = Absolute,  OpCode = 0xCC, NoBytes = 3, NoCycles = 4)]
        private void CPY()
        {
            M = ReadByte(_physicalAddress.word);
            Z = (Y == M);
            C = (Y >= M);
            N = (((Y - M) >> 7) & 1) == 1;
        }
        #endregion

        #region DEC
        //Action : decrement memory by one.
        //Operation : M - 1 -> M.
        //Flags : N,Z.
        [InstructionAttribute(Mnemonic = "DEC", AddressingMode = ZeroPage,  OpCode = 0xC6, NoBytes = 2, NoCycles = 5)]
        [InstructionAttribute(Mnemonic = "DEC", AddressingMode = ZeroPageX, OpCode = 0xD6, NoBytes = 2, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "DEC", AddressingMode = Absolute,  OpCode = 0xCE, NoBytes = 3, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "DEC", AddressingMode = AbsoluteX, OpCode = 0xDE, NoBytes = 3, NoCycles = 7)]
        private void DEC()
        {
            M = ReadByte(_physicalAddress.word);
            result8 = (byte)(M - 0x01);
            WriteByte(_physicalAddress.word, result8);
            N = ((result8 >> 7) & 1) == 1;
            Z = result8 == 0;
        }
        #endregion

        #region DEX
        //Action : decrement index X by one.
        //Operation : X - 1 -> X.
        //Flags : N,Z.
        [InstructionAttribute(Mnemonic = "DEX", AddressingMode = Implied, OpCode = 0xCA, NoBytes = 1, NoCycles = 2)]
        private void DEX()
        {
            result8 = (byte)(X - 0x01);
            X = result8;
            N = ((result8 >> 7) & 1) == 1;
            Z = result8 == 0;
        }
        #endregion

        #region DEY
        //Action : decrement index Y by one.
        //Operation : Y - 1 -> Y.
        //Flags : N,Z.
        [InstructionAttribute(Mnemonic = "DEY", AddressingMode = Implied, OpCode = 0x88, NoBytes = 1, NoCycles = 2)]
        private void DEY()
        {
            result8 = (byte)(Y - 0x01);
            Y = result8;
            N = ((result8 >> 7) & 1) == 1;
            Z = result8 == 0;
        }
        #endregion

        #region EOR
        //Action : Exclusive-Or memory with accumulator.
        //Operation : A ^ M -> A.
        //Flags : N,Z.
        [InstructionAttribute(Mnemonic = "EOR", AddressingMode = Immediate, OpCode = 0x49, NoBytes = 2, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "EOR", AddressingMode = ZeroPage,  OpCode = 0x45, NoBytes = 2, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "EOR", AddressingMode = ZeroPageX, OpCode = 0x55, NoBytes = 2, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "EOR", AddressingMode = Absolute,  OpCode = 0x4D, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "EOR", AddressingMode = AbsoluteX, OpCode = 0x5D, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "EOR", AddressingMode = AbsoluteY, OpCode = 0x59, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "EOR", AddressingMode = IndirectX, OpCode = 0x41, NoBytes = 2, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "EOR", AddressingMode = IndirectY, OpCode = 0x51, NoBytes = 2, NoCycles = 5)]

        private void EOR()
        {
            M = ReadByte(_physicalAddress.word);
            result8 = (byte)(A ^ M);
            A = result8;
            N = ((result8 >> 7) & 1) == 1;
            Z = result8 == 0;
        }
        #endregion

        #region INC
        //Action : increment memory by one.
        //Operation : M + 1 -> M.
        //Flags : N,Z.
        [InstructionAttribute(Mnemonic = "INC", AddressingMode = ZeroPage,  OpCode = 0xE6, NoBytes = 2, NoCycles = 5)]
        [InstructionAttribute(Mnemonic = "INC", AddressingMode = ZeroPageX, OpCode = 0xF6, NoBytes = 2, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "INC", AddressingMode = Absolute,  OpCode = 0xEE, NoBytes = 3, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "INC", AddressingMode = AbsoluteX, OpCode = 0xFE, NoBytes = 3, NoCycles = 7)]
        private void INC()
        {
            M = ReadByte(_physicalAddress.word);
            result8 = (byte)(M + 1);
            WriteByte(_physicalAddress.word, result8);
            N = ((result8 >> 7) & 1) == 1;
            Z = result8 == 0;         
        }
        #endregion

        #region INX
        //Action : increment index X by one.
        //Operation : X + 1 -> X.
        //Flags : N, Z.
        [InstructionAttribute(Mnemonic = "INX", AddressingMode = Implied, OpCode = 0xE8, NoBytes = 1, NoCycles = 2)]
        private void INX()
        {
            result8 = (byte)(X + 1);
            X = result8;
            N = ((result8 >> 7) & 1) == 1;
            Z = result8 == 0;  
        }
        #endregion

        #region INY
        //Action : increment index Y by one.
        //Operation : Y + 1 -> Y.
        //Flags : N, Z.
        [InstructionAttribute(Mnemonic = "INY", AddressingMode = Implied, OpCode = 0xC8, NoBytes = 1, NoCycles = 2)]
        private void INY()
        {
            result8 = (byte)(Y + 1);
            Y = result8;
            N = ((result8 >> 7) & 1) == 1;
            Z = result8 == 0;  
        }
        #endregion

        #region JMP
        //Action : jump to new location.
        //Operation : (PC + 1) -> PCL, (PC + 2) -> PCH.
        //Flags : 6.
        [InstructionAttribute(Mnemonic = "JMP", AddressingMode = Absolute, OpCode = 0x4C, NoBytes = 3, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "JMP", AddressingMode = Indirect, OpCode = 0x6C, NoBytes = 3, NoCycles = 3)]
        private void JMP()
        {
            byte PCL = _physicalAddress.low;
            byte PCH = _physicalAddress.high;
            
            PC = (ushort)(((PCH << 8) & 0xFF00) | PCL);
        }
        #endregion
        
        #region JSR
        //Action : jump to new location saving return address.
        //Operation : (PC + 1) -> PCL, (PC + 2) -> PCH.
        //Flags : none.
        [InstructionAttribute(Mnemonic = "JSR", AddressingMode = Absolute, OpCode = 0x20, NoBytes = 3, NoCycles = 6)]
        private void JSR()
        {
            byte PCL = _physicalAddress.low;
            byte PCH = _physicalAddress.high;

            
            byte PCL_ToStore = (byte)(PC - 1 & 0x00FF);
            byte PCH_ToStore = (byte)(PC -1 >> 8);
            
            WriteByte((ushort)(0x100+S), PCH_ToStore);
            S--;
            
            WriteByte((ushort)(0x100+S), PCL_ToStore);
            S--;   
            
            PC = (ushort)((PCH << 8) & 0xFF00);
            PC = (ushort)(PC | PCL);
        }        
        #endregion
        
        #region LDA
        //Action : load accumulator with memory.
        //Operation : M -> A.
        //Flags : N,Z.
        [InstructionAttribute(Mnemonic = "LDA", AddressingMode = Immediate, OpCode = 0xA9, NoBytes = 2, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "LDA", AddressingMode = ZeroPage,  OpCode = 0xA5, NoBytes = 2, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "LDA", AddressingMode = ZeroPageX, OpCode = 0xB5, NoBytes = 2, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "LDA", AddressingMode = Absolute,  OpCode = 0xAD, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "LDA", AddressingMode = AbsoluteX, OpCode = 0xBD, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "LDA", AddressingMode = AbsoluteY, OpCode = 0xB9, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "LDA", AddressingMode = IndirectX, OpCode = 0xA1, NoBytes = 2, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "LDA", AddressingMode = IndirectY, OpCode = 0xB1, NoBytes = 2, NoCycles = 5)]
        private void LDA()
        {
            M = ReadByte(_physicalAddress.word);
            A = M;
            
            //Refresh flag status:
            P &= 0x7D;
            N = ((A >> 7) & 1) == 1;
            Z = A == 0;
        }        
        #endregion

        #region LDX
        //Action : load index X with memory.
        //Operation : M -> X.
        //Flags : N,Z.
        [InstructionAttribute(Mnemonic = "LDX", AddressingMode = Immediate, OpCode = 0xA2, NoBytes = 2, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "LDX", AddressingMode = ZeroPage,  OpCode = 0xA6, NoBytes = 2, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "LDX", AddressingMode = ZeroPageY, OpCode = 0xB6, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "LDX", AddressingMode = Absolute,  OpCode = 0xAE, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "LDX", AddressingMode = AbsoluteY, OpCode = 0xBE, NoBytes = 3, NoCycles = 4)]
        private void LDx()
        {
            M = ReadByte(_physicalAddress.word);
            X = M;
            
            //Refresh flag status:
            P &= 0x7D;
            N = ((X >> 7) & 1) == 1;
            Z = X == 0;
        }
        #endregion
        
        #region LDY
        //Action : load index Y with memory.
        //Operation : M -> Y.
        //Flags : N,Z.
        [InstructionAttribute(Mnemonic = "LDY", AddressingMode = Immediate, OpCode = 0xA0, NoBytes = 2, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "LDY", AddressingMode = ZeroPage,  OpCode = 0xA4, NoBytes = 2, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "LDY", AddressingMode = ZeroPageX, OpCode = 0xB4, NoBytes = 2, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "LDY", AddressingMode = Absolute,  OpCode = 0xAC, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "LDY", AddressingMode = AbsoluteX, OpCode = 0xBC, NoBytes = 3, NoCycles = 4)]
        private void LDY()
        {
            M = ReadByte(_physicalAddress.word);
            Y = M;
            
            //Refresh flag status:
            P &= 0x7D;
            N = ((Y >> 7) & 1) == 1;
            Z = Y == 0;
        }
        #endregion
        
        #region LSR
        //Action : shift right one bit (memory or accumulator).
        //Operation : 0 -> 76534210 -> C.
        //Flags : N = 0,Z, C.
        [InstructionAttribute(Mnemonic = "LSR", AddressingMode = Accumulator, OpCode = 0x4A, NoBytes = 1, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "LSR", AddressingMode = ZeroPage,    OpCode = 0x46, NoBytes = 2, NoCycles = 5)]
        [InstructionAttribute(Mnemonic = "LSR", AddressingMode = ZeroPageX,   OpCode = 0x56, NoBytes = 2, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "LSR", AddressingMode = Absolute,    OpCode = 0x4E, NoBytes = 3, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "LSR", AddressingMode = AbsoluteX,   OpCode = 0x5E, NoBytes = 3, NoCycles = 7)]
        private void LSR()
        {
            P &= 0x7C;
            if (_instructionAttributes[_opcode].AddressingMode == Accumulator)
            {
                C = (A & 0x01) == 1;
                A = (byte)((A >> 1) & 0xFF);
                N = ((A >> 7) & 1) == 1;
                Z = A == 0;
            }
            else
            {
                M = ReadByte(_physicalAddress.word);
                C = (M & 0x01) == 1;
                M = (byte)((M >> 1) & 0xFF);
                WriteByte(_physicalAddress.word, M);
                N = ((M >> 7) & 1) == 1;
                Z = M == 0;
            }
        }
        #endregion
        
        #region NOP
        //Action : no operation.
        //Operation : no operation during 2 cycles.
        //Flags : none.
        [InstructionAttribute (Mnemonic = "NOP", AddressingMode = Implied, OpCode = 0xEA, NoBytes = 1, NoCycles = 2)]
        private void NOP()
        {
            
        }
        #endregion
        
        #region ORA
        //Action : "OR" memory with accumulator".
        //Operation : A | M -> A.
        //Flags : N,Z.
        [InstructionAttribute (Mnemonic = "ORA", AddressingMode = Immediate, OpCode = 0x09, NoBytes = 2, NoCycles = 2)]
        [InstructionAttribute (Mnemonic = "ORA", AddressingMode = ZeroPage,  OpCode = 0x05, NoBytes = 2, NoCycles = 3)]
        [InstructionAttribute (Mnemonic = "ORA", AddressingMode = ZeroPageX, OpCode = 0x15, NoBytes = 2, NoCycles = 4)]
        [InstructionAttribute (Mnemonic = "ORA", AddressingMode = Absolute,  OpCode = 0x0D, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute (Mnemonic = "ORA", AddressingMode = AbsoluteX, OpCode = 0x1D, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute (Mnemonic = "ORA", AddressingMode = AbsoluteY, OpCode = 0x19, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute (Mnemonic = "ORA", AddressingMode = IndirectX, OpCode = 0x01, NoBytes = 2, NoCycles = 6)]
        [InstructionAttribute (Mnemonic = "ORA", AddressingMode = IndirectY, OpCode = 0x11, NoBytes = 2, NoCycles = 5)]
        private void ORA()
        {
            M = ReadByte(_physicalAddress.word);
            A = (byte)(M | A);
            N = ((A >> 7) & 1) == 1;
            Z = A == 0; 
        }
        #endregion      
        
        #region PHA
        //Action : push accumulator on stack.
        //Operation : A.
        //Flags : none.
        [InstructionAttribute (Mnemonic = "PHA", AddressingMode = Implied, OpCode = 0x48, NoBytes = 1, NoCycles = 3)]
        private void PHA()
        {
            WriteByte((ushort)(0x0100 + S), A);
            S--;
        }
        #endregion
        
        #region PHP
        //Action : push processor status on stack.
        //Operation : P.
        //Flags : none.
        [InstructionAttribute (Mnemonic = "PHP", AddressingMode = Implied, OpCode = 0x08, NoBytes = 1, NoCycles = 3)]
        private void PHP()
        {
            WriteByte((ushort)(0x0100 + S), P);
            S--;
        }
        #endregion
        
        #region PLA
        //Action : pull accumulator from stack.
        //Operation : A.
        //Flags : N,Z.
        [InstructionAttribute (Mnemonic = "PLA", AddressingMode = Implied, OpCode = 0x68, NoBytes = 1, NoCycles = 4)]
        private void PLA()
        {
            S++;
            A = ReadByte((ushort)(0x0100 + S));
            
            N = ((A >> 7) & 1) == 1;
            Z = A == 0; 
        }
        #endregion
        
        #region PLP
        //Action : pull processor status from stack.
        //Operation : P.
        //Flags : from stack.
        [InstructionAttribute (Mnemonic = "PLP", AddressingMode = Implied, OpCode = 0x28, NoBytes = 1, NoCycles = 4)]
        private void PLP()
        {
            S++;
            P = ReadByte((ushort)(0x0100 + S));

        }
        #endregion    
        
        #region ROL
        //Action : rotate one bit left (memory or accumulator).
        //Operation : cf. page B-22.
        //Flags : N,Z,C.
        [InstructionAttribute(Mnemonic = "ROL", AddressingMode = Accumulator, OpCode = 0x2A, NoBytes = 1, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "ROL", AddressingMode = ZeroPage,    OpCode = 0x26, NoBytes = 2, NoCycles = 5)]
        [InstructionAttribute(Mnemonic = "ROL", AddressingMode = ZeroPageX,   OpCode = 0x36, NoBytes = 2, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "ROL", AddressingMode = Absolute,    OpCode = 0x2E, NoBytes = 3, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "ROL", AddressingMode = AbsoluteX,   OpCode = 0x3E, NoBytes = 3, NoCycles = 7)]
        private void ROL()
        {
            bool B7 = false; 
            if (_instructionAttributes[_opcode].AddressingMode == Accumulator)
            {
                B7 = A >> 7 == 1;
                if (C) A = (byte)(A << 1 | 0x01);
                else A = (byte)(A << 1 & 0xFF);
                C = B7;
                N = ((A >> 7) & 1) == 1;
                Z = A == 0;  
            }
            else
            {
                M = ReadByte(_physicalAddress.word);
                
                B7 = M >> 7 == 1;
                
                if (C) M = (byte)(M << 1 | 0x01);
                else M = (byte)(M << 1 & 0xFF);
                WriteByte(_physicalAddress.word, M);
                
                C = B7;
                N = ((M >> 7) & 1) == 1;
                Z = M == 0;  
            }
        }
        #endregion
        
        #region ROR
        //Action : rotate one bit right (memory or accumulator).
        //Operation : cf. page B-22.
        //Flags : N,Z,C.
        [InstructionAttribute(Mnemonic = "ROR", AddressingMode = Accumulator, OpCode = 0x6A, NoBytes = 1, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "ROR", AddressingMode = ZeroPage, OpCode = 0x66, NoBytes = 2, NoCycles = 5)]
        [InstructionAttribute(Mnemonic = "ROR", AddressingMode = ZeroPageX, OpCode = 0x76, NoBytes = 2, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "ROR", AddressingMode = Absolute, OpCode = 0x6E, NoBytes = 3, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "ROR", AddressingMode = AbsoluteX, OpCode = 0x7E, NoBytes = 3, NoCycles = 7)]
        private void ROR()
        {
            bool B0 = false; 
            if (_instructionAttributes[_opcode].AddressingMode == Accumulator)
            {
                B0 = A << 7 == 1;
                
                if (C) A = (byte)(A >> 1 & 0x80);
                else A = (byte)(A >> 1 & 0x7F);
                
                C = B0;
                N = ((A >> 7) & 1) == 1;
                Z = A == 0;  
            }
            else
            {
                M = ReadByte(_physicalAddress.word);
                
                B0 = M << 7 == 1;
                
                if (C) M = (byte)(M >> 1 & 0x80);
                else M = (byte)(M >> 1 & 0x7F);
                WriteByte(_physicalAddress.word, M);
                
                C = B0;
                N = ((M >> 7) & 1) == 1;
                Z = M == 0;  
            }
        }
        #endregion
        
        #region RTI
        //Action : return from interrupt.
        //Operation : P & PC pull out stack.
        //Flags : from stack.
        [InstructionAttribute (Mnemonic = "RTI", AddressingMode = Implied,  OpCode = 0x40, NoBytes = 1, NoCycles = 6)]
        private void RTI()
        {
            S++;
            P = ReadByte((ushort)(0x100 + S));

            S++;
            byte PCL = ReadByte((ushort)(0x100 + S));

            S++;
            byte PCH = ReadByte((ushort)(0x100 + S));

            PC = (ushort)(((PCH << 8) & 0xFF00) | PCL);
        }
        #endregion

        #region RTS
        //Action : return from subroutine.
        //Operation : P & PC pull out stack.
        //Flags : none.
        [InstructionAttribute (Mnemonic = "RTS", AddressingMode = Implied,  OpCode = 0x60, NoBytes = 1, NoCycles = 6)]
        private void RTS()
        {
            S++;    
            byte PCL = ReadByte((ushort)(0x100 + S));
            
            S++;
            byte PCH = ReadByte((ushort)(0x100 + S));
            
            PC = (ushort)((PCH << 8) & 0xFF00);
            PC = (ushort)(PC | PCL);
            PC++;
        }
        #endregion
        
        #region SBC
        //Action : subtract memory from accumulator with borrow.
        //Operation : A - M - (C) -> A.
        //Flags : N,Z,C,V.
        [InstructionAttribute (Mnemonic = "SBC", AddressingMode = Immediate, OpCode = 0xE9, NoBytes = 2, NoCycles = 2)]
        [InstructionAttribute (Mnemonic = "SBC", AddressingMode = ZeroPage,  OpCode = 0xE5, NoBytes = 2, NoCycles = 3)]
        [InstructionAttribute (Mnemonic = "SBC", AddressingMode = ZeroPageX, OpCode = 0xF5, NoBytes = 2, NoCycles = 4)]
        [InstructionAttribute (Mnemonic = "SBC", AddressingMode = Absolute,  OpCode = 0xED, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute (Mnemonic = "SBC", AddressingMode = AbsoluteX, OpCode = 0xFD, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute (Mnemonic = "SBC", AddressingMode = AbsoluteY, OpCode = 0xF9, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute (Mnemonic = "SBC", AddressingMode = IndirectX, OpCode = 0xE1, NoBytes = 2, NoCycles = 6)]
        [InstructionAttribute (Mnemonic = "SBC", AddressingMode = IndirectY, OpCode = 0xF1, NoBytes = 2, NoCycles = 5)]
        private void SBC()
        {
            //Get operand from memory:
            M = ReadByte(_physicalAddress.word);
            M &= 0xFF;

            byte carryFlagValue = (byte)(C ? 0 : 1);
            
            //Instruction Computation:
            result16 = (ushort)(A - M - carryFlagValue);
            result8 = (byte)result16;

            //Refresh flag status:
            P &= 0x3C;
            N = ((result8 >> 7) & 1) == 1;
            V = ((result8 ^ A) & (result8 ^ M) & 0x80) == 0x80;
            Z = result8 == 0;
            C = (result16 & 0xFF00) >= 0 ;

            //Set result:
            A = result8;
        }
        #endregion
        
        #region SEC
        //Action : set carry flag.
        //Operation : 1 -> C.
        //Flags : C.
        [InstructionAttribute (Mnemonic = "SEC", AddressingMode = Implied,  OpCode = 0x38, NoBytes = 1, NoCycles = 2)]
        private void SEC()
        {
            C = true;
        }
        #endregion
        
        #region SED
        //Action : set decimal mode.
        //Operation : 1 -> D.
        //Flags : D.
        [InstructionAttribute (Mnemonic = "SED", AddressingMode = Implied,  OpCode = 0xF8, NoBytes = 1, NoCycles = 2)]
        private void SED()
        {
            D = true;
        }
        #endregion
        
        #region SEI
        //Action : set interrupt disable status.
        //Operation : 1 -> I.
        //Flags : I.
        [InstructionAttribute (Mnemonic = "SEI", AddressingMode = Implied,  OpCode = 0x78, NoBytes = 1, NoCycles = 2)]
        private void SEI()
        {
            I = true;
        }
        #endregion
        
        #region STA
        //Action : store accumulator in memory.
        //Operation : A -> M.
        //Flags : none.
        [InstructionAttribute (Mnemonic = "STA", AddressingMode = ZeroPage,  OpCode = 0x85, NoBytes = 2, NoCycles = 3)]
        [InstructionAttribute (Mnemonic = "STA", AddressingMode = ZeroPageX, OpCode = 0x95, NoBytes = 2, NoCycles = 4)]
        [InstructionAttribute (Mnemonic = "STA", AddressingMode = Absolute,  OpCode = 0x8D, NoBytes = 3, NoCycles = 4)]
        [InstructionAttribute (Mnemonic = "STA", AddressingMode = AbsoluteX, OpCode = 0x9D, NoBytes = 3, NoCycles = 5)]
        [InstructionAttribute (Mnemonic = "STA", AddressingMode = AbsoluteY, OpCode = 0x99, NoBytes = 3, NoCycles = 5)]
        [InstructionAttribute (Mnemonic = "STA", AddressingMode = IndirectX, OpCode = 0x81, NoBytes = 2, NoCycles = 6)]
        [InstructionAttribute (Mnemonic = "STA", AddressingMode = IndirectY, OpCode = 0x91, NoBytes = 2, NoCycles = 6)]
        private void STA()
        {
            WriteByte(_physicalAddress.word, A);
        }
        #endregion
        
        #region STX
        //Action : store index X in memory.
        //Operation : X -> M.
        //Flags : none.
        [InstructionAttribute (Mnemonic = "STX", AddressingMode = ZeroPage,  OpCode = 0x86, NoBytes = 2, NoCycles = 3)]
        [InstructionAttribute (Mnemonic = "STX", AddressingMode = ZeroPageX, OpCode = 0x96, NoBytes = 2, NoCycles = 4)]
        [InstructionAttribute (Mnemonic = "STX", AddressingMode = Absolute,  OpCode = 0x8E, NoBytes = 3, NoCycles = 4)]
        private void STX()
        {
            WriteByte(_physicalAddress.word, X);
        }
        #endregion
        
        #region STY
        //Action : store index Y in memory.
        //Operation : Y -> M.
        //Flags : none.
        [InstructionAttribute (Mnemonic = "STY", AddressingMode = ZeroPage,  OpCode = 0x84, NoBytes = 2, NoCycles = 3)]
        [InstructionAttribute (Mnemonic = "STY", AddressingMode = ZeroPageX, OpCode = 0x94, NoBytes = 2, NoCycles = 4)]
        [InstructionAttribute (Mnemonic = "STY", AddressingMode = Absolute,  OpCode = 0x8C, NoBytes = 3, NoCycles = 4)]
        private void STY()
        {
            WriteByte(_physicalAddress.word, Y);
        }
        #endregion
        
        #region TAX
        //Action : transfer accumulator to index X.
        //Operation : A -> X.
        //Flags : N,Z.
        [InstructionAttribute (Mnemonic = "TAX", AddressingMode = Implied,  OpCode = 0xAA, NoBytes = 1, NoCycles = 2)]
        private void TAX()
        {
            X = A;
            N = ((X >> 7) & 1) == 1;
            Z = X == 0; 
        }
        #endregion
        
        #region TAY
        //Action : transfer accumulator to index Y.
        //Operation : A -> Y.
        //Flags : N,Z.
        [InstructionAttribute (Mnemonic = "TAY", AddressingMode = Implied,  OpCode = 0xA8, NoBytes = 1, NoCycles = 2)]
        private void TAY()
        {
            Y = A;
            N = ((Y >> 7) & 1) == 1;
            Z = Y == 0; 
        }
        #endregion
        
        #region TYA
        //Action : transfer index Y to accumulator.
        //Operation : Y -> A.
        //Flags : N,Z.
        [InstructionAttribute (Mnemonic = "TYA", AddressingMode = Implied,  OpCode = 0x98, NoBytes = 1, NoCycles = 2)]
        private void TYA()
        {
            A = Y;
            N = ((A >> 7) & 1) == 1;
            Z = A == 0; 
        }
        #endregion
        
        #region TSX
        //Action : transfer stack pointer to index X.
        //Operation : S -> X.
        //Flags : N,Z.
        [InstructionAttribute (Mnemonic = "TSX", AddressingMode = Implied,  OpCode = 0xBA, NoBytes = 1, NoCycles = 2)]
        private void TSX()
        {
            X = S;
            N = ((X >> 7) & 1) == 1;
            Z = X == 0; 
        }
        #endregion
        
        #region TXA
        //Action : transfer index X to accumulator.
        //Operation : X -> A.
        //Flags : N,Z.
        [InstructionAttribute (Mnemonic = "TXA", AddressingMode = Implied,  OpCode = 0x8A, NoBytes = 1, NoCycles = 2)]
        private void TXA()
        {
            A = X;
            N = ((A >> 7) & 1) == 1;
            Z = A == 0; 
        }
        #endregion
        
        #region TXS
        //Action : transfer index X to stack pointer.
        //Operation : X -> S.
        //Flags : none.
        [InstructionAttribute (Mnemonic = "TXS", AddressingMode = Implied,  OpCode = 0x9A, NoBytes = 1, NoCycles = 2)]
        private void TXS()
        {
            S = X;
        }
        #endregion
    }
}
