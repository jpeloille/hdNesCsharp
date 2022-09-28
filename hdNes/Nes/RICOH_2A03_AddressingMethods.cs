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
            _decodedAddress.word = PC;
            PC++;
        } //Confirmed 26-09-2021

        [AddressingModeAttribute(AddressingMode = AddressingMode.Absolute)]
        private void FetchAddress_Absolute()
        {
            _decodedAddress.low = Read(PC);
            PC++;
            
            _decodedAddress.high = Read(PC);
            PC++;
        } //Confirmed 26-09-2021

        [AddressingModeAttribute(AddressingMode = AddressingMode.ZeroPage)]
        private void FetchAddress_ZeroPage()
        {
            _decodedAddress.low = Read(PC);
            PC++;

            _decodedAddress.high = 0x00;
        } //Confirmed 01-10-2021

        [AddressingModeAttribute(AddressingMode = AddressingMode.ZeroPageX)]
        private void FetchAddress_ZeroPageX()
        {
            byte low = Read(PC); PC++;
            low += X;

            _decodedAddress.low = low;
            _decodedAddress.high = 0x00;
        } //Confirmed 01-10-2021

        [AddressingModeAttribute(AddressingMode = AddressingMode.ZeroPageY)]
        private void FetchAddress_ZeroPageY()
        {
            byte low = Read(PC); PC++;
            low += Y;

            _decodedAddress.low = low;
            _decodedAddress.high = 0x00;
        }

        [AddressingModeAttribute(AddressingMode = AddressingMode.AbsoluteX)]
        private void FetchAddress_AbsoluteX()
        {
            _decodedAddress.low = Read(PC);
            PC++;

            _decodedAddress.high = Read(PC);
            PC++;

            _decodedAddress.word += X;
        } //Confirmed 01-10-2021

        [AddressingModeAttribute(AddressingMode = AddressingMode.AbsoluteY)]
        private void FetchAddress_AbsoluteY()
        {
            _decodedAddress.low = Read(PC);
            PC++;

            _decodedAddress.high = Read(PC);
            PC++;

            _decodedAddress.word += Y;
        } //Confirmed 01-10-2021

        [AddressingModeAttribute(AddressingMode = AddressingMode.IndirectX)]
        private void FetchAddress_IndirectX()
        {
            byte loa = Read(PC);
            PC++;

            ushort lsb = (ushort)((loa + X) & 0x00FF);
            _decodedAddress.low = Read(lsb);

            ushort msb = (ushort)((loa + X + 1) & 0x00FF);
            _decodedAddress.high = Read(msb);
        } //Confirmed 01-10-2021

        [AddressingModeAttribute(AddressingMode = AddressingMode.IndirectY)]
        private void FetchAddress_IndirectY()
        {
            byte loa = Read(PC);
            PC++;

            ushort lsb = (ushort)((loa) & 0x00FF);
            _decodedAddress.low = Read(lsb);

            ushort msb = (ushort)((loa + 1) & 0x00FF);
            _decodedAddress.high = Read(msb);

            _decodedAddress.word += Y;
        }

        [AddressingModeAttribute(AddressingMode = AddressingMode.Indirect)]
        private void FetchAddress_Indirect()
        {
            byte IAL = Read(PC);
            PC++;
            
            byte IAH = Read(PC);
            PC++;

            ushort ADL_Adress = (ushort)(((IAH << 8) & 0XFF00) | IAL);
            _decodedAddress.low = Read(ADL_Adress);

            IAL = (byte)(IAL + 0X01);
            ushort ADH_Adress = (ushort)(((IAH << 8) & 0XFF00) | IAL);
            _decodedAddress.high = Read(ADH_Adress);
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
