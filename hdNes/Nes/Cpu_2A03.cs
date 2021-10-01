using System;
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
         private Instruction[] _instructionSet = new Instruction[256];
         
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
         
         
         //Cpu registers & flag structure:
         private byte A; //Accumulator
         private byte X; //Index register X
         private byte Y; //Index register Y
         private byte S; //Stack pointer
         private ushort PC; //Program counter
         private Cpu2A03FlagStructure P = new Cpu2A03FlagStructure(); //Processor status register
        
         //Cpu emulations state variables:
         private byte _opcode;
         private WordAddress _absoluteAddress;
         private WordAddress _relativeAddress;
         
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
            _instructionSet[0x69] = ADC; //Immediate
            _instructionSet[0x65] = ADC; //ZeroPage
            _instructionSet[0x75] = ADC; //ZeroPage, X
            _instructionSet[0x6D] = ADC; //Absolute
            _instructionSet[0x7D] = ADC; //Absolute, X
            _instructionSet[0x79] = ADC; //Absolute, Y
            _instructionSet[0x61] = ADC; //Indirect, X
            _instructionSet[0x71] = ADC; //Indirect, Y

            //AND memory with accumulator
            _instructionSet[0x29] = AND; //Immediate
            _instructionSet[0x25] = AND; //ZeroPage
            _instructionSet[0x35] = AND; //ZeroPage, X
            _instructionSet[0x2D] = AND; //Absolute
            _instructionSet[0x3D] = AND; //Absolute, X
            _instructionSet[0x39] = AND; //Absolute, Y
            _instructionSet[0x21] = AND; //Indirect, X
            _instructionSet[0x31] = AND; //Indirect, Y
        }
        
        #endregion

        #region Cpu emalution methods
        
        public void Reset()
        {
            PC = 0xFFFC;
            S = 0x00;
            P.Register = 0xFF;
            A = 0x00;
            _absoluteAddress.word = 0x0000;
            _relativeAddress.word = 0x0000;
        }

        public void Tick(int count)
        {
            while (count >= 1)
            {
                FetchOpcode();
                _instructionSet[_opcode]();
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
        }

        private void FetchAddress_ZeroPageX()
        {
            byte low = Read(PC); PC++;
            low += X;
            
            _absoluteAddress.low = low;
            _absoluteAddress.high = 0x00;
        }
        
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
        }
        
        private void FetchAddress_AbsoluteY()
        {
            _absoluteAddress.low = Read(PC);
            PC++;

            _absoluteAddress.high = Read(PC);
            PC++;

            _absoluteAddress.word += Y;
        }

        private void FetchAddress_IndirectX()
        {
            byte loa = Read(PC);
            PC++;

            ushort lsb = (ushort)((loa + X) & 0x00FF);
            _absoluteAddress.low = Read(lsb);
            
            ushort msb = (ushort)((loa + X + 1) & 0x00FF);
            _absoluteAddress.high = Read(msb);
        }
        
/*  [Obsolete]
        private void FetchAdress_IndirectY()
        {
            byte loa = Read(PC);
            PC++;

            ushort lsb = (ushort)((loa + Y) & 0x00FF);
            _absoluteAddress.low = Read(lsb);
            
            ushort msb = (ushort)((loa + 1) & 0x00FF);
            _absoluteAddress.high = Read(msb);
        }
*/

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
        
        #endregion

        #region Arythmetics methods
        
        private byte ADC(byte A, byte M)
        {
            P.C = false;
            
            ushort result16 = (ushort)(A + M);
            if (P.C) result16 += 1;
            
            byte result8 = (byte)result16;

            /* Flags Affected: N,V,Z,C */
            byte currentP = P.Register;
            currentP &= 0x3C;
            P.Register = currentP;

            P.N = ((result8 >> 7) & 1) == 1;
            P.V = ((result8 ^ A) & (result8 ^ M) & 0x80) == 0x80;
            P.Z = result8 == 0;
            P.C = result16 > 0xFF;
            
            return result8;
        }

        private byte AND(byte A, byte M)
        {
            ushort result16 = (ushort)(A & M);
            byte result8 = (byte)result16;

            /* Flags Affected: N,Z */
            byte currentP = P.Register;
            currentP &= 0x7D;
            P.Register = currentP;
            
            P.N = ((result8 >> 7) & 1) == 1;
            P.Z = result8 == 0;
            
            return result8;
        }
        
        #endregion
        
        #region Instructions implementation

        [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
        public class OpcodeDef : Attribute
        {
            public byte Opcode;
            public int Cycles;
            public AddressingModes AddressingMode;
            
        }
        
        private void ADC()
        {
            switch (_opcode)
            {
                case 0x69: FetchAddress_Immediate(); break;
                case 0x65: FetchAddress_ZeroPage();   break;
                case 0x75: FetchAddress_ZeroPageX();  break;
                case 0x6D: FetchAddress_Absolute();  break;
                case 0x7D: FetchAddress_AbsoluteX();  break;
                case 0x79: FetchAddress_AbsoluteY();  break;
                case 0x61: FetchAddress_IndirectX();  break;
                case 0x71: FetchAddress_IndirectY();  break; //Indirect Indexed.
            }
            
            byte M = 0x00; 
            M = Read(_absoluteAddress.word);
            byte result = ADC(A, M);
            A = result;
        }
        
        private void AND()
        {
            int cycles = 0;
            
            switch (_opcode)
            {
                case 0x29: FetchAddress_Immediate(); cycles -= 2; break;
                case 0x25: FetchAddress_ZeroPage();  cycles -= 3; break;
                case 0x35: FetchAddress_ZeroPageX(); cycles -= 4; break;
                case 0x2D: FetchAddress_Absolute();  cycles -= 4; break;
                case 0x3D: FetchAddress_AbsoluteX(); cycles -= 4; break;
                case 0x39: FetchAddress_AbsoluteY(); cycles -= 4; break;
                case 0x21: FetchAddress_IndirectX(); cycles -= 6; break;
                case 0x31: FetchAddress_IndirectY(); cycles -= 5; break;
            }
            
            byte M = 0x00;
            M = Read(_absoluteAddress.word);
            byte result = AND(A, M);
            A = result;
        }

        #endregion
        
        #region CPU unit testing methods
        
        public byte regA { get => A; set => A = value; }
        public byte regX { get => X; set => X = value; }
        public byte regY { get => Y; set => Y = value; }
        
        public Cpu2A03FlagStructure psr { get => P;  }
        
        public void UnitTest_Reset()
        {
            PC = 0xC000;
            P.Register = 0x24;
            Console.WriteLine(P.Register.ToString());
            S = 0xFD;
            A = 0x00;
            Y = 0x00;
            _absoluteAddress.word = 0x0000;
            _relativeAddress.word = 0x0000;
        }

        public void UnitTest_Write(ushort address, byte data)
        {
            Write(address, data);
        }
        
        
        #endregion
    }

    public class Cpu2A03FlagStructure
    {
        [Flags]
        public enum ProcessorStatusRegister : byte
        {
            Carry    = 1 << 0, //Carry flag,
            Zero     = 1 << 1, //Zero flag,
            Iro      = 1 << 2, //IR0 inhibit flag,
            Decimal  = 1 << 3, //Decimal mode flag,
            Brake    = 1 << 4, //Brake command flag,
            None     = 1 << 5, //Unused bit,
            Overflow = 1 << 6, //Overflow flag,
            Negative = 1 << 7  //Negative flag.
        }
        
        public bool C
        {
            get => _Carry;
            set => SetFlags(FlagsType.Carry, value);
        }
        public bool Z
        {
            get => _Zero;
            set => SetFlags(FlagsType.Zero, value);
        }
        public bool I
        {
            get => _Iro;
            set => SetFlags(FlagsType.Iro, value);
        }
        public bool D
        {
            get => _Decimal;
            set => SetFlags(FlagsType.Decimal, value);
        }
        public bool B
        {
            get => _Brake;
            set => SetFlags(FlagsType.Brake, value);
        }
        public bool X
        {
            get => _None;
            set => SetFlags(FlagsType.None, value);
        }
        public bool V
        {
            get => _Overflow;
            set => SetFlags(FlagsType.Overflow, value);
        }
        public bool N
        {
            get => _Negative;
            set => SetFlags(FlagsType.Negative, value);
        }

        public byte Register
        {
            get => GetRegister();
            set => SetRegister(value);
        }
        

        #region Private members

        private ProcessorStatusRegister _cpuStatusRegister;
        
        private bool _Carry = false;
        private bool _Zero = false;
        private bool _Iro = false;
        private bool _Decimal = false;
        private bool _Brake = false;
        private bool _None = false;
        private bool _Overflow = false;
        private bool _Negative = false;
        
        private enum FlagsType
        {
            Carry, 
            Zero,
            Iro,
            Decimal,
            Brake,
            None,
            Overflow,
            Negative
        }
        
        #endregion

        public Cpu2A03FlagStructure()
        {
            _cpuStatusRegister = new ProcessorStatusRegister();
            _cpuStatusRegister &= ~_cpuStatusRegister;
        }
        
        #region Methods

        private void SetFlags(FlagsType flagsType, bool state)
        {
            switch (flagsType)
            {
                case FlagsType.Carry:
                    _Carry = state;
                    _cpuStatusRegister &= ~(ProcessorStatusRegister.Carry);
                    _cpuStatusRegister |= (_Carry ? ProcessorStatusRegister.Carry : 0);
                    break;
                case FlagsType.Zero:
                    _Zero = state;
                    _cpuStatusRegister &= ~(ProcessorStatusRegister.Zero);
                    _cpuStatusRegister |= (_Zero ? ProcessorStatusRegister.Zero : 0);
                    break;
                case FlagsType.Iro:
                    _Iro = state;
                    _cpuStatusRegister &= ~(ProcessorStatusRegister.Iro);
                    _cpuStatusRegister |= (_Iro ? ProcessorStatusRegister.Iro : 0);
                    break;                    
                case FlagsType.Decimal:
                    _Decimal = state;
                    _cpuStatusRegister &= ~(ProcessorStatusRegister.Decimal);
                    _cpuStatusRegister |= (_Decimal ? ProcessorStatusRegister.Decimal : 0);
                    break; 
                case FlagsType.Brake:
                    _Brake = state;
                    _cpuStatusRegister &= ~(ProcessorStatusRegister.Brake);
                    _cpuStatusRegister |= (_Brake ? ProcessorStatusRegister.Brake : 0);
                    break; 
                case FlagsType.None:
                    _None = state;
                    _cpuStatusRegister &= ~(ProcessorStatusRegister.None);
                    _cpuStatusRegister |= (_None ? ProcessorStatusRegister.None : 0);
                    break; 
                case FlagsType.Overflow:
                    _Overflow = state;
                    _cpuStatusRegister &= ~(ProcessorStatusRegister.Overflow);
                    _cpuStatusRegister |= (_Overflow ? ProcessorStatusRegister.Overflow : 0);
                    break; 
                case FlagsType.Negative:
                    _Negative = state;
                    _cpuStatusRegister &= ~(ProcessorStatusRegister.Negative);
                    _cpuStatusRegister |= (_Negative ? ProcessorStatusRegister.Negative : 0);
                    break; 
            }
        }
        
        private byte GetRegister()
        {
            byte cpuStatusRegister = (byte)(_cpuStatusRegister);
            cpuStatusRegister &= 0xFF;
            return cpuStatusRegister;
        }

        private void SetRegister(byte value)
        {
            SetFlags(FlagsType.Carry, ((value) & 0x01) == 1);
            SetFlags(FlagsType.Zero, ((value >> 1) & 0x01) == 1);
            SetFlags(FlagsType.Iro, ((value >> 2) & 0x01) == 1);
            SetFlags(FlagsType.Decimal, ((value >> 3) & 0x01) == 1);
            SetFlags(FlagsType.Brake, ((value >> 4) & 0x01) == 1);
            SetFlags(FlagsType.None, ((value >> 5) & 0x01) == 1);
            SetFlags(FlagsType.Overflow, ((value >> 6) & 0x01) == 1);
            SetFlags(FlagsType.Negative, ((value >> 7) & 0x01) == 1);
        }
        #endregion

    }
}