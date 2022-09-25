using System;
using System.Collections.Generic;
using System.Text;

namespace hdNes.Nes
{
    sealed partial class Ricoh2A03
    {
        public enum AddressingMode
        {
            None,
            Implied,
            Accumulator,
            Immediate,
            ZeroPage,
            Absolute,
            ZeroPageX,
            ZeroPageY,
            AbsoluteX,
            AbsoluteY,
            Indirect,
            IndirectX,
            IndirectY,
            Relative
        }

        [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
        public class AddressingModeAttribute : Attribute
        {
            public AddressingMode AddressingMode = AddressingMode.None;
        }
        
        [AddressingModeAttribute(AddressingMode = AddressingMode.Accumulator)]
        [AddressingModeAttribute(AddressingMode = AddressingMode.Implied)]
        private void ImpliedOrAccumulator()
        {

        }

        [AddressingModeAttribute(AddressingMode = AddressingMode.Immediate)]
        private void FetchAddress_Immediate()
        {
            _absoluteAddress.word = PC;
            PC++;
        } //Confirmed 26-09-2021

        [AddressingModeAttribute(AddressingMode = AddressingMode.Absolute)]
        private void FetchAddress_Absolute()
        {
            _absoluteAddress.low = Read(PC);
            PC++;
            
            _absoluteAddress.high = Read(PC);
            PC++;
        } //Confirmed 26-09-2021

        [AddressingModeAttribute(AddressingMode = AddressingMode.ZeroPage)]
        private void FetchAddress_ZeroPage()
        {
            _absoluteAddress.low = Read(PC);
            PC++;

            _absoluteAddress.high = 0x00;
        } //Confirmed 01-10-2021

        [AddressingModeAttribute(AddressingMode = AddressingMode.ZeroPageX)]
        private void FetchAddress_ZeroPageX()
        {
            byte low = Read(PC); PC++;
            low += X;

            _absoluteAddress.low = low;
            _absoluteAddress.high = 0x00;
        } //Confirmed 01-10-2021

        [AddressingModeAttribute(AddressingMode = AddressingMode.ZeroPageY)]
        private void FetchAddress_ZeroPageY()
        {
            byte low = Read(PC); PC++;
            low += Y;

            _absoluteAddress.low = low;
            _absoluteAddress.high = 0x00;
        }

        [AddressingModeAttribute(AddressingMode = AddressingMode.AbsoluteX)]
        private void FetchAddress_AbsoluteX()
        {
            _absoluteAddress.low = Read(PC);
            PC++;

            _absoluteAddress.high = Read(PC);
            PC++;

            _absoluteAddress.word += X;
        } //Confirmed 01-10-2021

        [AddressingModeAttribute(AddressingMode = AddressingMode.AbsoluteY)]
        private void FetchAddress_AbsoluteY()
        {
            _absoluteAddress.low = Read(PC);
            PC++;

            _absoluteAddress.high = Read(PC);
            PC++;

            _absoluteAddress.word += Y;
        } //Confirmed 01-10-2021

        [AddressingModeAttribute(AddressingMode = AddressingMode.IndirectX)]
        private void FetchAddress_IndirectX()
        {
            byte loa = Read(PC);
            PC++;

            ushort lsb = (ushort)((loa + X) & 0x00FF);
            _absoluteAddress.low = Read(lsb);

            ushort msb = (ushort)((loa + X + 1) & 0x00FF);
            _absoluteAddress.high = Read(msb);
        } //Confirmed 01-10-2021

        [AddressingModeAttribute(AddressingMode = AddressingMode.IndirectY)]
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

        [AddressingModeAttribute(AddressingMode = AddressingMode.Indirect)]
        private void FetchAddress_Indirect()
        {
            /*byte loa = Read(PC);
            PC++;

            ushort lsb = (ushort)(loa & 0x00FF);
            _absoluteAddress.low = Read(lsb);

            ushort msb = (ushort)((loa + 1) & 0x00FF);
            _absoluteAddress.high = Read(msb);*/
            
            //todo : Correct IND memory access method.
        }

        [AddressingModeAttribute(AddressingMode = AddressingMode.Relative)]
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
