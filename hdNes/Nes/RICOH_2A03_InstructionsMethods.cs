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
            public int NoCycles;
        }

        #region ADC
        //Action : Add memory to accumulator with carry.
        //Operation :A + M + C -> A, C.
        //Flags : N,Z,C,V.
        [InstructionAttribute(Mnemonic = "ADC", AddressingMode = Immediate, OpCode = 0x69, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "ADC", AddressingMode = ZeroPage,  OpCode = 0x65, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "ADC", AddressingMode = ZeroPageX, OpCode = 0x75, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "ADC", AddressingMode = Absolute,  OpCode = 0x6D, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "ADC", AddressingMode = AbsoluteX, OpCode = 0x7D, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "ADC", AddressingMode = AbsoluteY, OpCode = 0x79, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "ADC", AddressingMode = IndirectX, OpCode = 0x61, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "ADC", AddressingMode = IndirectY, OpCode = 0x71, NoCycles = 5)]
        private void ADC()
        {
            //Get operand from memory:
            M = Read(_decodedAddress.word);
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
        [InstructionAttribute(Mnemonic = "AND", AddressingMode = Immediate, OpCode = 0x29, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "AND", AddressingMode = ZeroPage,  OpCode = 0x25, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "AND", AddressingMode = ZeroPageX, OpCode = 0x35, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "AND", AddressingMode = Absolute,  OpCode = 0x2D, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "AND", AddressingMode = AbsoluteX, OpCode = 0x3D, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "AND", AddressingMode = AbsoluteY, OpCode = 0x39, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "AND", AddressingMode = IndirectX, OpCode = 0x21, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "AND", AddressingMode = IndirectY, OpCode = 0x31, NoCycles = 5)]
        private void AND()
        {
            //Get operand from memory:
            M = Read(_decodedAddress.word);
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
        [InstructionAttribute(Mnemonic = "ASL", AddressingMode = Accumulator, OpCode = 0x0A, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "ASL", AddressingMode = ZeroPage,    OpCode = 0x06, NoCycles = 5)]
        [InstructionAttribute(Mnemonic = "ASL", AddressingMode = ZeroPageX,   OpCode = 0x16, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "ASL", AddressingMode = Absolute,    OpCode = 0x0E, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "ASL", AddressingMode = AbsoluteX,   OpCode = 0x1E, NoCycles = 7)]
        private void ASL()
        {
            if (_opcode == 0x0A) operand = A;
            else operand = Read(_decodedAddress.word);
            operand &= 0xFF;

            /* Faire le reset des flags impliqués */
            C = ((operand >> 7) & 1) == 1;
            operand = (byte)(((operand << 1) | 0) & 0xFF);

            N = ((operand >> 7) & 1) == 1;
            Z = operand == 0;

            if (_opcode == 0x0A) A = operand;
            else Write(_decodedAddress.word, operand);
        }
        #endregion

        #region BCC
        //Action : BCC branch on carry clear.
        //Operation : Branch on C = 0.
        //Flags : -.
        [InstructionAttribute(Mnemonic = "BCC", AddressingMode = Relative, OpCode = 0x90, NoCycles = 2)]
        private void BCC()
        {
            if (!C)
            {
                _decodedAddress.word = (ushort)(PC + _relativeAddress.word);
                PC = _decodedAddress.word;
            }
        }
        #endregion

        #region BCS
        //Action : BCS branch on carry set.
        //Operation : Branch on C = 1.
        //Flags : -.
        [InstructionAttribute(Mnemonic = "BCS", AddressingMode = Relative, OpCode = 0xB0, NoCycles = 2)]
        private void BCS()
        {
            if (C)
            {
                _decodedAddress.word = (ushort)(PC + _relativeAddress.word);
                PC = _decodedAddress.word;
            }
        }
        #endregion

        #region BEQ
        //Action : BEQ branch on result zero.
        //Operation : Branch on Z = 0.
        //Flags : -.
        [InstructionAttribute(Mnemonic = "BEQ", AddressingMode = Relative, OpCode = 0xF0, NoCycles = 2)]
        private void BEQ()
        {
            if (Z)
            {
                _decodedAddress.word = (ushort)(PC + _relativeAddress.word);
                PC = _decodedAddress.word;
            }
        }
        #endregion

        #region BIT
        //Action : test bits in memory accumulator.
        //  Bit 6 & 7 are transferred to the status register.
        //  If the result of A^M is zero then Z=1, otherwise Z=0.
        //Operation : A ^ M, M7 -> N, M6 -> V.
        //Flags : N, Z, V.
        [InstructionAttribute(Mnemonic = "BIT", AddressingMode = ZeroPage, OpCode = 0x24, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "BIT", AddressingMode = Absolute, OpCode = 0x2C, NoCycles = 4)]
        private void BIT()
        {
            //Get operand from memory:
            M = Read(_decodedAddress.word);
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
        [InstructionAttribute(Mnemonic = "BMI", AddressingMode = Relative, OpCode = 0x30, NoCycles = 2)]
        private void BMI()
        {
            if (N)
            {
                _decodedAddress.word = (ushort)(PC + _relativeAddress.word);
                PC = _decodedAddress.word;
            }
        }
        #endregion

        #region BNE
        //Action : branch on result not zero.
        //Operation : branch on Z = 0.
        //Flags : -.
        [InstructionAttribute(Mnemonic = "BNE", AddressingMode = Relative, OpCode = 0xD0, NoCycles = 2)]
        private void BNE()
        {
            if (!Z)
            {
                _decodedAddress.word = (ushort)(PC + _relativeAddress.word);
                PC = _decodedAddress.word;
            }
        }
        #endregion

        #region BPL
        //Action : branch on result plus.
        //Operation : branch on N = 0.
        //Flags : -.
        [InstructionAttribute(Mnemonic = "BPL", AddressingMode = Relative, OpCode = 0x10, NoCycles = 2)]
        private void BPL()
        {
            if (!N)
            {
                _decodedAddress.word = (ushort)(PC + _relativeAddress.word);
                PC = _decodedAddress.word;
            }
        }
        #endregion

        #region BRK
        //Action : force break.Store the PC of the byte next the instruction. Store the status register.
        //Operation : force interrupt PC. PCL = 0xFFFE & PCH = 0xFFFF;
        //Flags : I = 1.
        [InstructionAttribute(Mnemonic = "BRK", AddressingMode = Implied, OpCode = 0x00, NoCycles = 7)]
        private void BRK()
        {
            PC++;
            
            Write((ushort)(0x100 + S), (byte)(PC >> 8 & 0x00FF));
            S--;
            
            Write((ushort)(0x100 + S), (byte)(PC & 0x00FF));
            S--;
            
            Write((ushort)(0x100 + S), P);
            S--;

            var PCL = Read(0xFFFE);
            var PCH = Read(0XFFFF);
            PC = (ushort)(((PCH << 8) & 0xFF00) | PCL);

        }
        #endregion

        #region BVC
        //Action : branch on overflow clear.
        //Operation : branch on V = 0.
        //Flags : -.
        [InstructionAttribute(Mnemonic = "BVC", AddressingMode = Relative, OpCode = 0x50, NoCycles = 2)]
        private void BVC()
        {
            if (!V)
            {
                _decodedAddress.word = (ushort)(PC + _relativeAddress.word);
                PC = _decodedAddress.word;
            }
        }
        #endregion

        #region BVS
        //Action : branch on overflow set.
        //Operation : branch on V = 1.
        //Flags : -.
        [InstructionAttribute(Mnemonic = "BVS", AddressingMode = Relative, OpCode = 0x70, NoCycles = 2)]
        private void BVS()
        {
            if (V)
            {
                _decodedAddress.word = (ushort)(PC + _relativeAddress.word);
                PC = _decodedAddress.word;
            }
        }
        #endregion

        #region CLC
        //Action : clear carry flag.
        //Operation : C <- 0.
        //Flags : C.
        [InstructionAttribute(Mnemonic = "CLC", AddressingMode = Implied, OpCode = 0x18, NoCycles = 2)]
        private void CLC()
        {
            C = false;
        }
        #endregion

        #region CLD
        //Action : clear decimal mode.
        //Operation : D <- 0.
        //Flags : D.
        [InstructionAttribute(Mnemonic = "CLD", AddressingMode = Implied, OpCode = 0xD8, NoCycles = 2)]
        private void CLD()
        {
            D = false;
        }
        #endregion

        #region CLI
        //Action : clear interrupt disable bit.
        //Operation : I <- 0.
        //Flags : I.
        [InstructionAttribute(Mnemonic = "CLI", AddressingMode = Implied, OpCode = 0x58, NoCycles = 2)]
        private void CLI()
        {
            I = false;
        }
        #endregion

        #region CLV
        //Action : clear overflow flag.
        //Operation : V <- 0.
        //Flags : V.
        [InstructionAttribute(Mnemonic = "CLV", AddressingMode = Implied, OpCode = 0xB8, NoCycles = 2)]
        private void CLV()
        {
            V = false;
        }
        #endregion

        #region CMP
        //Action : compare memory and accumulator.
        //Operation : A - M.
        //Flags : N,Z,C.
        [InstructionAttribute(Mnemonic = "CMP", AddressingMode = Immediate, OpCode = 0xC8, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "CMP", AddressingMode = ZeroPage,  OpCode = 0xC5, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "CMP", AddressingMode = ZeroPageX, OpCode = 0xD5, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "CMP", AddressingMode = Absolute,  OpCode = 0xCD, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "CMP", AddressingMode = AbsoluteX, OpCode = 0xDD, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "CMP", AddressingMode = AbsoluteY, OpCode = 0xD9, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "CMP", AddressingMode = IndirectX, OpCode = 0xC1, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "CMP", AddressingMode = IndirectY, OpCode = 0xD1, NoCycles = 5)]
        private void CMP()
        {
            M = Read(_decodedAddress.word);
            Z = (A == M);
            C = (A >= M);
            N = (((A - M) >> 7) & 1) == 1;
        }
        #endregion

        #region CPX
        //Action : compare memory and index X.
        //Operation : X - M.
        //Flags : N,Z,C.
        [InstructionAttribute(Mnemonic = "CPX", AddressingMode = Immediate, OpCode = 0xE0, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "CPX", AddressingMode = ZeroPage,  OpCode = 0xE4, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "CPX", AddressingMode = Absolute,  OpCode = 0xEC, NoCycles = 4)]
        private void CPX()
        {
            M = Read(_decodedAddress.word);
            Z = (X == M);
            C = (X >= M);
            N = (((X - M) >> 7) & 1) == 1;
        }
        #endregion

        #region CPY
        //Action : compare memory and index Y.
        //Operation : Y - M.
        //Flags : N,Z,C.
        [InstructionAttribute(Mnemonic = "CPY", AddressingMode = Immediate, OpCode = 0xC0, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "CPY", AddressingMode = ZeroPage,  OpCode = 0xC4, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "CPY", AddressingMode = Absolute,  OpCode = 0xCC, NoCycles = 4)]
        private void CPY()
        {
            M = Read(_decodedAddress.word);
            Z = (Y == M);
            C = (Y >= M);
            N = (((Y - M) >> 7) & 1) == 1;
        }
        #endregion

        #region DEC
        //Action : decrement memory by one.
        //Operation : M - 1 -> M.
        //Flags : N,Z.
        [InstructionAttribute(Mnemonic = "DEC", AddressingMode = ZeroPage,  OpCode = 0xC6, NoCycles = 5)]
        [InstructionAttribute(Mnemonic = "DEC", AddressingMode = ZeroPageX, OpCode = 0xD6, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "DEC", AddressingMode = Absolute,  OpCode = 0xCE, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "DEC", AddressingMode = AbsoluteX, OpCode = 0xDE, NoCycles = 7)]
        private void DEC()
        {
            M = Read(_decodedAddress.word);
            result8 = (byte)(M - 0x01);
            Write(_decodedAddress.word, result8);
            N = ((result8 >> 7) & 1) == 1;
            Z = result8 == 0;
        }
        #endregion

        #region DEX
        //Action : decrement index X by one.
        //Operation : X - 1 -> X.
        //Flags : N,Z.
        [InstructionAttribute(Mnemonic = "DEX", AddressingMode = Implied, OpCode = 0xCA, NoCycles = 2)]
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
        [InstructionAttribute(Mnemonic = "DEY", AddressingMode = Implied, OpCode = 0x88, NoCycles = 2)]
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
        [InstructionAttribute(Mnemonic = "EOR", AddressingMode = Immediate, OpCode = 0x49, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "EOR", AddressingMode = ZeroPage,  OpCode = 0x45, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "EOR", AddressingMode = ZeroPageX, OpCode = 0x55, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "EOR", AddressingMode = Absolute,  OpCode = 0x4D, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "EOR", AddressingMode = AbsoluteX, OpCode = 0x5D, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "EOR", AddressingMode = AbsoluteY, OpCode = 0x59, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "EOR", AddressingMode = IndirectX, OpCode = 0x41, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "EOR", AddressingMode = IndirectY, OpCode = 0x51, NoCycles = 5)]

        private void EOR()
        {
            M = Read(_decodedAddress.word);
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
        [InstructionAttribute(Mnemonic = "INC", AddressingMode = ZeroPage,  OpCode = 0xE6, NoCycles = 5)]
        [InstructionAttribute(Mnemonic = "INC", AddressingMode = ZeroPageX, OpCode = 0xF6, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "INC", AddressingMode = Absolute,  OpCode = 0xEE, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "INC", AddressingMode = AbsoluteX, OpCode = 0xFE, NoCycles = 7)]
        private void INC()
        {
            M = Read(_decodedAddress.word);
            result8 = (byte)(M + 1);
            Write(_decodedAddress.word, result8);
            N = ((result8 >> 7) & 1) == 1;
            Z = result8 == 0;         
        }
        #endregion

        #region INX
        //Action : increment index X by one.
        //Operation : X + 1 -> X.
        //Flags : N, Z.
        [InstructionAttribute(Mnemonic = "INX", AddressingMode = Implied, OpCode = 0xE8, NoCycles = 2)]
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
        [InstructionAttribute(Mnemonic = "INY", AddressingMode = Implied, OpCode = 0xC8, NoCycles = 2)]
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
        [InstructionAttribute(Mnemonic = "JMP", AddressingMode = Absolute, OpCode = 0x4C, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "JMP", AddressingMode = Indirect, OpCode = 0x6C, NoCycles = 3)]
        private void JMP()
        {
            byte PCL = _decodedAddress.low;
            byte PCH = _decodedAddress.high;
            
            PC = (ushort)(((PCH << 8) & 0xFF00) | PCL);
        }
        #endregion
        
        #region JSR
        //Action : jump to new location saving return address.
        //Operation : (PC + 1) -> PCL, (PC + 2) -> PCH.
        //Flags : none.
        [InstructionAttribute(Mnemonic = "JSR", AddressingMode = Absolute, OpCode = 0x20, NoCycles = 6)]
        private void JSR()
        {
            byte PCL = _decodedAddress.low;
            byte PCH = _decodedAddress.high;

            Write((ushort)(0x100+S), PCH);
            S--;
            
            Write((ushort)(0x100+S), PCL);
            S--;

            PC = (ushort)((PCH << 8) & 0xFF00);
            PC = (ushort)(PC | PCL);
        }        
        #endregion
        
        #region LDA
        //Action : load accumulator with memory.
        //Operation : M -> A.
        //Flags : N,Z.
        [InstructionAttribute(Mnemonic = "LDA", AddressingMode = Immediate, OpCode = 0xA9, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "LDA", AddressingMode = ZeroPage,  OpCode = 0xA5, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "LDA", AddressingMode = ZeroPageX, OpCode = 0xB5, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "LDA", AddressingMode = Absolute,  OpCode = 0xAD, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "LDA", AddressingMode = AbsoluteX, OpCode = 0xBD, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "LDA", AddressingMode = AbsoluteY, OpCode = 0xB9, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "LDA", AddressingMode = IndirectX, OpCode = 0xA1, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "LDA", AddressingMode = IndirectY, OpCode = 0xB1, NoCycles = 5)]
        private void LDA()
        {
            M = Read(_decodedAddress.word);
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
        [InstructionAttribute(Mnemonic = "LDX", AddressingMode = Immediate, OpCode = 0xA2, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "LDX", AddressingMode = ZeroPage,  OpCode = 0xA6, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "LDX", AddressingMode = ZeroPageY, OpCode = 0xB6, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "LDX", AddressingMode = Absolute,  OpCode = 0xAE, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "LDX", AddressingMode = AbsoluteY, OpCode = 0xBE, NoCycles = 4)]
        private void LDx()
        {
            M = Read(_decodedAddress.word);
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
        [InstructionAttribute(Mnemonic = "LDY", AddressingMode = Immediate, OpCode = 0xA0, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "LDY", AddressingMode = ZeroPage,  OpCode = 0xA4, NoCycles = 3)]
        [InstructionAttribute(Mnemonic = "LDY", AddressingMode = ZeroPageX, OpCode = 0xB4, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "LDY", AddressingMode = Absolute,  OpCode = 0xAC, NoCycles = 4)]
        [InstructionAttribute(Mnemonic = "LDY", AddressingMode = AbsoluteX, OpCode = 0xBC, NoCycles = 4)]
        private void LDY()
        {
            M = Read(_decodedAddress.word);
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
        [InstructionAttribute(Mnemonic = "LSR", AddressingMode = Accumulator, OpCode = 0x4A, NoCycles = 2)]
        [InstructionAttribute(Mnemonic = "LSR", AddressingMode = ZeroPage,    OpCode = 0x46, NoCycles = 5)]
        [InstructionAttribute(Mnemonic = "LSR", AddressingMode = ZeroPageX,   OpCode = 0x56, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "LSR", AddressingMode = Absolute,    OpCode = 0x4E, NoCycles = 6)]
        [InstructionAttribute(Mnemonic = "LSR", AddressingMode = AbsoluteX,   OpCode = 0x5E, NoCycles = 7)]
        private void LSR()
        {
            if (_instructionAttributes[_opcode].AddressingMode == AddressingMode.Immediate)
            {
                C = (A << 7) == 1;
                
            }
            else
            {
                
            }
        }
        #endregion
    }
}
