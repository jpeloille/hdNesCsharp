using hdNes.Nes;
using NUnit.Framework;

namespace hdNes.Cpu.Test
{
    public class INC_Tests
    {
        [Test]
        public void INC_ZeroPage_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xE6, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.Write(0x00, 0xFF);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0xFF, board.cpu.Read(0x00));
            Assert.AreEqual(0x00, board.cpu.Read(0x00));

            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(true, board.cpu.Z);
        }   
        
        [Test]
        public void INC_ZeroPage_Negative()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xE6, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.Write(0x00, 0x7F);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x7F, board.cpu.Read(0x00));
            Assert.AreEqual(0x80, board.cpu.Read(0x00));

            //Check flags output:
            Assert.AreEqual(true, board.cpu.N);
            Assert.AreEqual(false, board.cpu.Z);
        }
        
        [Test]
        public void INC_ZeroPageX_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xF6, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.X = 0x01;
            board.cpu.Write(0x01, 0xFF);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0xFF, board.cpu.Read(0x01));
            Assert.AreEqual(0x00, board.cpu.Read(0x01));

            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(true, board.cpu.Z);
        } 
        
        [Test]
        public void INC_Absolute_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xEE, 0x03, 0xC0, 0xFF });
            board.cpu.SetInUnitTestInitialState();

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0xFF, board.cpu.Read(0xC003));
            Assert.AreEqual(0x00, board.cpu.Read(0x0C003));

            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(true, board.cpu.Z);
        }
        
        [Test]
        public void INC_AbsoluteX_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xFE, 0x03, 0xC0, 0x00, 0xFF });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.X = 0x01;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0xFF, board.cpu.Read(0xC004));
            Assert.AreEqual(0x00, board.cpu.Read(0x0C004));

            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(true, board.cpu.Z);
        }
    }
}