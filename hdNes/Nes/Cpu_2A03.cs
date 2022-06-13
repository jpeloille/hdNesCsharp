using System;
using System.Linq;
using System.Runtime.InteropServices;
using hdNes.Nes.Enums;
using System.Runtime.Loader;
using System.Threading;


namespace hdNes.Nes
{
    public class Cpu_2A03
    {
        #region Parameters

         //Board where the component is tied to:
         private Board _board;
         
         //Instructions set container:
         private delegate void Instruction();
         private Instruction[] _instructionMethod = new Instruction[0xFF];
         
         private delegate void InstructionAddressMode();
         private InstructionAddressMode[] _instructionAddressMode = new InstructionAddressMode[0xFF];
         
         //16 bits address structure:
         [StructLayout(LayoutKind.Explicit, Size = 16)]
         private struct WordAddress
         {
             [FieldOffset(0)] public ushort word;
             [FieldOffset(0)] public byte low;
             [FieldOffset(1)] public byte high;

             public WordAddress(ushort address) : this()
             {
                 word = address;
             }
         }
         
         //Cpu registers:
         private byte A;    //Accumulator
         private byte X;    //Index register X
         private byte Y;    //Index register Y
         private byte S;    //Stack pointer
         private ushort PC; //Program counter
         
         //Cpu flags:
         public bool C;     //Carry flag,
         public bool Z;     //Zero flag,
         public bool I;     //IR0 inhibit flag, 
         public bool D;     //Decimal mode flag,
         public bool B;     //Brake command flag,
         public bool V;     //Overflow flag,
         public bool N;     //Negative flag.

         public byte P
         {
             get
             {
                 return (byte) (
                     (N ? 0x80 : 0) |
                     (V ? 0x40 : 0) |
                     (B ? 0x10 : 0) |
                     (D ? 0x08 : 0) |
                     (I ? 0x04 : 0) |
                     (Z ? 0x02 : 0) |
                     (C ? 0x01 : 0) | 0x20);
             }
             set
             {
                 N = (value & 0x80) != 0;
                 V = (value & 0x40) != 0;
                 B = (value & 0x10) != 0;
                 D = (value & 0x08) != 0;
                 I = (value & 0x04) != 0;
                 Z = (value & 0x02) != 0;
                 C = (value & 0x01) != 0;
             }
         }
        
         //Cpu emulations state variables:
         private byte _opcode;
         private WordAddress _absoluteAddress;
         private WordAddress _relativeAddress;
         
         private byte M;
         private ushort result16;
         private byte result8;
         private byte operand;
         
         private int cycles = 0;
         
        #endregion

        #region Constructor & class initialization

        public Cpu_2A03(Board board)
        {
            _board = board;
            GenerateInstructionTable();
        }

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
        }
        
        #endregion

        #region Cpu emalution methods
        
        public void Reset()
        {
            PC = 0xFFFC;
            S = 0x00;
            P = 0xFF;
            A = 0x00;
            _absoluteAddress.word = 0x0000;
            _relativeAddress.word = 0x0000;
        }

        public void Tick(int count)
        {
            while (count >= 1)
            {
                FetchOpcode();
                _instructionAddressMode[_opcode]();
                _instructionMethod[_opcode]();
                count--;
            }
        }
        
        public void FetchOpcode()
        {
            _opcode = Read(PC);
            PC++;
        }
        
        private byte Read(ushort address)
        {
            return _board.CpuRead(address);
        }

        private void Write(ushort address, byte data)
        {
            _board.CpuWrite(address, data);
        }

        #endregion

        #region Addressing modes methods

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
        
        private void NoAddressMode()
        {}
        
        #endregion
        
        #region Instructions implementation
        
        private void ADC()
        {
            //Get operand from memory:
            M = Read(_absoluteAddress.word);
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

        private void AND()
        {
            //Get operand from memory:
            M = Read(_absoluteAddress.word);
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

        private void ASL()
        {
            if (_opcode == 0x0A) operand = A;
                else operand = Read(_absoluteAddress.word);
            operand &= 0xFF;
            
            /* Faire le reset des flags impliqués */
            C = ((operand >> 7) & 1 ) == 1;
            operand = (byte)(((operand << 1) | 0) & 0xFF);
            
            N = ((operand >> 7) & 1) == 1;
            Z = operand == 0;

            if (_opcode == 0x0A) A = operand;
                else Write(_absoluteAddress.word, operand);
        }

        private void BCC()
        {
            if (!C)
            {
                _absoluteAddress.word = (ushort)(PC + _relativeAddress.word);
                PC = _absoluteAddress.word;
            }
        }

        private void BCS()
        {
            if (C)
            {
                _absoluteAddress.word = (ushort)(PC + _relativeAddress.word);
                PC = _absoluteAddress.word;
            }
        }

        private void BEQ()
        {
            if (Z)
            {
                _absoluteAddress.word =  (ushort)(PC + _relativeAddress.word);
                PC = _absoluteAddress.word;
            }
        }

        private void BIT()
        {
            //Get operand from memory:
            M = Read(_absoluteAddress.word);
            M &= 0xFF;

            Z = (A & M) == 0;
            N = (M & 0x80) != 0;
            V = (M & 0x60) != 0;
        }

        private void BMI()
        {
            if (N)
            {
                _absoluteAddress.word = (ushort)(PC + _relativeAddress.word);
                PC = _absoluteAddress.word;
            }
        }
        
        #endregion
        
        #region CPU unit testing methods
        
        public byte regA { get => A; set => A = value; }
        public byte regX { get => X; set => X = value; }
        public byte regY { get => Y; set => Y = value; }
        public ushort utPC { get => PC; set => PC = value; }
        
        public byte psr { get => P;  }
        
        public void UnitTest_Reset()
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
        
        
        #endregion
    }
    
}