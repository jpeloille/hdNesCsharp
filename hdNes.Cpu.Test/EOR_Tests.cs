using hdNes.Nes;
using NUnit.Framework;

namespace hdNes.Cpu.Test
{
    public class EOR_Tests
    {
        [Test]
        public void EOR_Immediate_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x49, 0xFF });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0xFF;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0xFF, board.cpu.A);
            Assert.AreEqual(0x00, board.cpu.A);

            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(true, board.cpu.Z);
        }

        [Test]
        public void EOR_Immediate_Negative()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x49, 0x7F });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0xFF;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0xFF, board.cpu.A);
            Assert.AreEqual(0x80, board.cpu.A);

            //Check flags output:
            Assert.AreEqual(true, board.cpu.N);
            Assert.AreEqual(false, board.cpu.Z);
        }

        [Test]
        public void EOR_ZeroPage_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x45, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0xFF;
            board.cpu.Write(0x00, 0xFF);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0xFF, board.cpu.A);
            Assert.AreEqual(0x00, board.cpu.A);

            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(true, board.cpu.Z);
        }

        [Test]
        public void EOR_ZeroPageX_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x55, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0xFF;
            board.cpu.X = 0x01;
            board.cpu.Write(0x01, 0xFF);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0xFF, board.cpu.A);
            Assert.AreEqual(0x00, board.cpu.A);

            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(true, board.cpu.Z);
        }

        [Test]
        public void EOR_Absolute_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x4D, 0x03, 0xC0, 0xFF });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0xFF;
            board.cpu.Write(0x01, 0xFF);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0xFF, board.cpu.A);
            Assert.AreEqual(0x00, board.cpu.A);

            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(true, board.cpu.Z);
        }

        [Test]
        public void EOR_AbsoluteX_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x5D, 0x02, 0xC0, 0xFF });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0xFF;
            board.cpu.X = 0x01;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0xFF, board.cpu.A);
            Assert.AreEqual(0x00, board.cpu.A);

            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(true, board.cpu.Z);
        }

        [Test]
        public void EOR_AbsoluteX_PageBoundary_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x5D, 0x01, 0xC0 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0xFF;
            board.cpu.X = 0xFF;
            board.cpu.Write(0xC100,0xFF);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0xFF, board.cpu.A);
            Assert.AreEqual(0x00, board.cpu.A);

            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(true, board.cpu.Z);
        }

        [Test]
        public void EOR_AbsoluteY_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x59, 0x02, 0xC0, 0xFF });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0xFF;
            board.cpu.Y = 0x01;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0xFF, board.cpu.A);
            Assert.AreEqual(0x00, board.cpu.A);

            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(true, board.cpu.Z);
        }

        [Test]
        public void EOR_AbsoluteY_PageBoundary_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x59, 0x01, 0xC0 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0xFF;
            board.cpu.Y = 0xFF;
            board.cpu.Write(0xC100, 0xFF);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0xFF, board.cpu.A);
            Assert.AreEqual(0x00, board.cpu.A);

            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(true, board.cpu.Z);
        }

        [Test]
        public void EOR_IndexedIndirect_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x41, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0xFF;
            board.cpu.X = 0x01;
            board.cpu.Write(0x00, 0x00);
            board.cpu.Write(0x01, 0x03);
            board.cpu.Write(0x02, 0x00);
            board.cpu.Write(0x03, 0xFF);


            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0xFF, board.cpu.A);
            Assert.AreEqual(0x00, board.cpu.A);

            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(true, board.cpu.Z);
        }

        [Test]
        public void EOR_IndirectIndexed_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x51, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0xFF;
            board.cpu.Y = 0x01;
            board.cpu.Write(0x00, 0x01);
            board.cpu.Write(0x01, 0x00);
            board.cpu.Write(0x02, 0xFF);
            
            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0xFF, board.cpu.A);
            Assert.AreEqual(0x00, board.cpu.A);

            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(true, board.cpu.Z);
        }

        [Test]
        public void EOR_IndirectIndexed_PageBoundary_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x51, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0xFF;
            board.cpu.Y = 0x01;
            board.cpu.Write(0x0000, 0xFF);
            board.cpu.Write(0x0001, 0x00);
            board.cpu.Write(0x0100, 0xFF);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0xFF, board.cpu.A);
            Assert.AreEqual(0x00, board.cpu.A);

            //Check flags output:
            Assert.AreEqual(false, board.cpu.N);
            Assert.AreEqual(true, board.cpu.Z);
        }
    }
}
