using NUnit.Framework;
using hdNes.Nes;

namespace hdNes.Cpu.Test
{
    public class DEC_Tests
    {
        [Test]
        public void DEC_ZeroPage()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xC6, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.Write(0x00,0x02);
            
            board.cpu.Tick(1);

            //Verify Memory Values
            Assert.AreNotEqual(0x02, board.cpu.Read(0x00));
            Assert.AreEqual(0x01, board.cpu.Read(0x00));

            //Verify Flags
            Assert.AreEqual(false, board.cpu.Z);
            Assert.AreEqual(false, board.cpu.C);
            Assert.AreEqual(false, board.cpu.N);
        }
        
        [Test]
        public void DEC_ZeroPage_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xC6, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.Write(0x00,0x01);
            
            board.cpu.Tick(1);

            //Verify Memory Values
            Assert.AreNotEqual(0x01, board.cpu.Read(0x00));
            Assert.AreEqual(0x00, board.cpu.Read(0x00));

            //Verify Flags
            Assert.AreEqual(true, board.cpu.Z);
            Assert.AreEqual(false, board.cpu.C);
            Assert.AreEqual(false, board.cpu.N);
        }
        
        [Test]
        public void DEC_ZeroPage_Negative()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xC6, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.Write(0x00,0x00);
            
            board.cpu.Tick(1);

            //Verify Memory Values
            Assert.AreNotEqual(0x00, board.cpu.Read(0x00));
            Assert.AreEqual(0xFF, board.cpu.Read(0x00));

            //Verify Flags
            Assert.AreEqual(false, board.cpu.Z);
            Assert.AreEqual(false, board.cpu.C);
            Assert.AreEqual(true, board.cpu.N);
        }
        
        [Test]
        public void DEC_ZeroPageX()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xD6, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.X = 0x01;
            board.cpu.Write(0x01,0x02);
            
            board.cpu.Tick(1);

            //Verify Memory Values
            Assert.AreNotEqual(0x02, board.cpu.Read(0x01));
            Assert.AreEqual(0x01, board.cpu.Read(0x01));

            //Verify Flags
            Assert.AreEqual(false, board.cpu.Z);
            Assert.AreEqual(false, board.cpu.C);
            Assert.AreEqual(false, board.cpu.N);
        }
    }
}