using hdNes.Nes;
using NUnit.Framework;

namespace hdNes.Cpu.Test
{
    public class DEY_Tests
    {
        [Test]
        public void DEY_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x88 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.Y = 1;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x01, board.cpu.Y);
            Assert.AreEqual(0x00, board.cpu.Y);

            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(true, board.cpu.Z);
        }

        [Test]
        public void DEY_Negative()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x88 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.Y = 0;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.Y);
            Assert.AreEqual(0xff, board.cpu.Y);

            //Check flags output:
            Assert.AreEqual(true, board.cpu.N);
            Assert.AreEqual(false, board.cpu.Z);
        }
    }
}
