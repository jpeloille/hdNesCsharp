using System;
using System.Linq;
using System.Runtime.InteropServices;
using hdNes.Nes.Enums;
using System.Runtime.Loader;
using System.Threading;


namespace hdNes.Nes
{
    sealed partial class Cpu_2A03
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
        
    }
    
}