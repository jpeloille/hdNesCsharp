namespace hdNes.Nes
{
    public sealed partial class RP_2C02
    {
        /* PPU control register.    */
        /* Access: write.           */
        /* Address: $2000.          */
        private class PPUCTRL
        {
            public byte BaseNameTableAddress;   
                        // (0 = $2000; 1 = $2400; 2 = $2800; 3 = $2C00).
                        
            public bool VRamIncrement;          
                        // (0: add 1, going across; 1: add 32, going down).
                        
            public bool SpritePatternTableAddress;  
                        // (0: $0000; 1: $1000; for 8x8 sprites only).
                        
            public bool BackgroundPatternTableAddress;  
                        // (0: $0000; 1: $1000).
                        
            public bool  SpriteSize;  
                        // (0: 8x8 pixels; 1: 8x16 pixels – see PPU OAM#Byte 1).
                        
            public bool PpuMasterSlave; 
                        // (0: read backdrop from EXT pins; 1: output color on EXT pins)
                        
            public bool NmiEnabled;  
                        // Generate an NMI at the start of the vertical blanking interval.
        }
        
        /* PPU mask register.       */
        /* Access: write.           */
        /* Address: $2001.          */
        private class PPUMASK
        {
            public bool DisplayInGrayscale; 
            public bool ShowBackgroundLeftMost8PixelsOfScreen;
            public bool ShowSpriteLeftMost8PixelsOfScreen;
            public bool ShowBackground;
            public bool ShowSprite;
            public bool EmphasizeRed;   // (Green on PAL/Dendy).
            public bool EmphasizeGreen; // (Red on PAL/Dendy).
            public bool EmphasizeBlue;
        }

        /* PPU status register.     */
        /* Access: read.            */
        /* Address: $2002.          */
        private class PPUSTATUS
        {
            private byte _PpuOpenBus;
            public byte PpuOpenBus
            {
                get => _PpuOpenBus;
                set => _PpuOpenBus = (byte)(value & 0x1F);
            }
            public bool SpriteOverflow;
            public bool Sprite0Hit;
            public bool VerticalBlankHAsStarted;
        }
        
        /* OAM address.       .     */
        /* Access: write.           */
        /* Address: $2003.          */
        private class OAMADDR
        {
            private byte _oamAdress;

            public byte value
            {
                get => _oamAdress;
                set => _oamAdress = (byte)(value * 0xFF);
            }
        }
        
        public byte OAMDATA;
        public byte PPUSCROLL;
        public byte PPUADDR;
        public byte PPUDATA;
        public byte OAMDMA;
    }
}