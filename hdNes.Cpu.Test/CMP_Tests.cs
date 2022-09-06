using NUnit.Framework;
using hdNes.Nes;

namespace hdNes.Cpu.Test
{
    public class CMP_Tests
    {
        [Test]
        public void CMP_Immediate_Carry()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xC9, 0x01 });
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0x7F;

            board._cpu2A03.Tick(1);

            //Verify Register Values
            Assert.AreEqual(0x7F, board._cpu2A03.regA);

            //Verify Flags
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(true, board._cpu2A03.C);
            Assert.AreEqual(false, board._cpu2A03.N);
        }

        [Test]
        public void CMP_Immediate_Negative()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xC9, 0x80 });
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0x7E;

            board._cpu2A03.Tick(1);

            //Verify Register Values
            Assert.AreEqual(0x7E, board._cpu2A03.regA);

            //Verify Flags
            Assert.AreEqual(false, board._cpu2A03.Z);
            Assert.AreEqual(false, board._cpu2A03.C);
            Assert.AreEqual(true, board._cpu2A03.N);
        }

        [Test]
        public void CMP_Immediate_Carry_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xC9, 0x7E });
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0x7E;

            board._cpu2A03.Tick(1);

            //Verify Register Values
            Assert.AreEqual(0x7E, board._cpu2A03.regA);

            //Verify Flags
            Assert.AreEqual(true, board._cpu2A03.Z);
            Assert.AreEqual(true, board._cpu2A03.C);
            Assert.AreEqual(false, board._cpu2A03.N);
        } 
        
        [Test]
        public void CMP_ZeroPage_Carry_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xC5, 0x00 });
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0x7E;
            board._cpu2A03.UnitTest_Write(0x00, 0x7E);

            board._cpu2A03.Tick(1);

            //Verify Register Values
            Assert.AreEqual(0x7E, board._cpu2A03.regA);

            //Verify Flags
            Assert.AreEqual(true, board._cpu2A03.Z);
            Assert.AreEqual(true, board._cpu2A03.C);
            Assert.AreEqual(false, board._cpu2A03.N);
        } 
        
        [Test]
        public void CMP_ZeroPageX_Carry_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xD5, 0x00 });
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0x7E;
            board._cpu2A03.regX = 0x01;
            board._cpu2A03.UnitTest_Write(0x01, 0x7E);

            board._cpu2A03.Tick(1);

            //Verify Register Values
            Assert.AreEqual(0x7E, board._cpu2A03.regA);

            //Verify Flags
            Assert.AreEqual(true, board._cpu2A03.Z);
            Assert.AreEqual(true, board._cpu2A03.C);
            Assert.AreEqual(false, board._cpu2A03.N);
        } 
        
        [Test]
        public void CMP_Absolute_Carry_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] {0xCD, 0x03, 0xC0, 0x7E });
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0x7E;

            board._cpu2A03.Tick(1);

            //Verify Register Values
            Assert.AreEqual(0x7E, board._cpu2A03.regA);

            //Verify Flags
            Assert.AreEqual(true, board._cpu2A03.Z);
            Assert.AreEqual(true, board._cpu2A03.C);
            Assert.AreEqual(false, board._cpu2A03.N);
        } 
        
        [Test]
        public void CMP_AbsoluteX_Carry_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xDD, 0x03, 0xC0, 0x00, 0x7E });
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0x7E;
            board._cpu2A03.regX = 0x01;

            board._cpu2A03.Tick(1);

            //Verify Register Values
            Assert.AreEqual(0x7E, board._cpu2A03.regA);

            //Verify Flags
            Assert.AreEqual(true, board._cpu2A03.Z);
            Assert.AreEqual(true, board._cpu2A03.C);
            Assert.AreEqual(false, board._cpu2A03.N);
        } 
        
        [Test]
        public void CMP_AbsoluteX_PageBoundary_Carry_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xDD, 0xFF, 0xC0 });
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0x7E;
            board._cpu2A03.regX = 0x01;
            board._cpu2A03.UnitTest_Write(0xC100, 0x7E);

            board._cpu2A03.Tick(1);

            //Verify Register Values
            Assert.AreEqual(0x7E, board._cpu2A03.regA);
            Assert.AreEqual(0x7E, board._cpu2A03.UnitTest_Read(0xC100));
            //Verify Flags
            Assert.AreEqual(true, board._cpu2A03.Z);
            Assert.AreEqual(true, board._cpu2A03.C);
            Assert.AreEqual(false, board._cpu2A03.N);
        } 
        
        [Test]
        public void CMP_AbsoluteY_Carry_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xD9, 0x03, 0xC0, 0x00, 0x7E });
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0x7E;
            board._cpu2A03.regY = 0x01;

            board._cpu2A03.Tick(1);

            //Verify Register Values
            Assert.AreEqual(0x7E, board._cpu2A03.regA);

            //Verify Flags
            Assert.AreEqual(true, board._cpu2A03.Z);
            Assert.AreEqual(true, board._cpu2A03.C);
            Assert.AreEqual(false, board._cpu2A03.N);
        } 
     
        [Test]
        public void CMP_AbsoluteY_PageBoundary_Carry_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xD9, 0xFF, 0xC0 });
            board.UnitTest_Reset();
            board._cpu2A03.regA = 0x7E;
            board._cpu2A03.regY = 0x01;
            board._cpu2A03.UnitTest_Write(0xC100, 0x7E);
            
            board._cpu2A03.Tick(1);

            //Verify Register Values
            Assert.AreEqual(0x7E, board._cpu2A03.regA);

            //Verify Flags
            Assert.AreEqual(true, board._cpu2A03.Z);
            Assert.AreEqual(true, board._cpu2A03.C);
            Assert.AreEqual(false, board._cpu2A03.N);
        } 
    }
    
}


