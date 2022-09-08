using hdNes.Nes;
using NUnit.Framework;

namespace hdNes.Cpu.Test
{
    public class DEX_Tests
    {
        [Test]
        public void DEX_Zero()
        {        
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xCA });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.X = 1;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x01, board.cpu.X);
            Assert.AreEqual(0x00, board.cpu.X);

            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(true, board.cpu.Z);
        }

        [Test]
        public void DEX_Negative()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xCA });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.X = 0;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.X);
            Assert.AreEqual(0xFF, board.cpu.X);

            //Check flags output:
            Assert.AreEqual(true, board.cpu.N);
            Assert.AreEqual(false, board.cpu.Z);
        }
    }
}
