using hdNes.Nes;
using NUnit.Framework;

namespace hdNes.Cpu.Test
{
    public class LDA_Tests
    {
        [Test]
        public void LDA_Immediate_Clear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xA9, 0x01 });
            board.cpu.SetInUnitTestInitialState();

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A);
            Assert.AreEqual(0x01, board.cpu.A);
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
        }
        
        [Test]
        public void LDA_Immediate_Negative()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xA9, 0x80 });
            board.cpu.SetInUnitTestInitialState();

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A, "A should not equal to 0x00.");
            Assert.AreEqual(0x80, board.cpu.A, "A should be equal to 0x80.");
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(true, board.cpu.N, "N not true.");
        }
        
        [Test]
        public void LDA_ZeroPage_Clear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xA5, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.Write(0x00, 0x01);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A, "A should not equal to 0x00.");
            Assert.AreEqual(0x01, board.cpu.A, "A should be equal to 0x01.");
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
        }
        
        [Test]
        public void LDA_ZeroPageX_Clear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xB5, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.X = 0x01;
            board.cpu.Write(0x01, 0x01);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A, "A should not equal to 0x00.");
            Assert.AreEqual(0x01, board.cpu.A, "A should be equal to 0x01.");
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
        }
        
        [Test]
        public void LDA_Absolute_Clear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xAD, 0x03, 0xC0, 0x01 });
            board.cpu.SetInUnitTestInitialState();

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A, "A should not equal to 0x00.");
            Assert.AreEqual(0x01, board.cpu.A, "A should be equal to 0x01.");
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
        }
        
        [Test]
        public void LDA_AbsoluteX_Clear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xBD, 0x02, 0xC0, 0x01 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.X = 0x01;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A, "A should not equal to 0x00.");
            Assert.AreEqual(0x01, board.cpu.A, "A should be equal to 0x01.");
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
        }
        
        [Test]
        public void LDA_AbsoluteX_PageBoundary_Clear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xBD, 0xFF, 0xC0 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.X = 0x01;
            board.cpu.Write(0xC100, 0x01);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A, "A should not equal to 0x00.");
            Assert.AreEqual(0x01, board.cpu.A, "A should be equal to 0x01.");
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
        }
        
        [Test]
        public void LDA_AbsoluteY_Clear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xB9, 0x02, 0xC0, 0x01 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.Y = 0x01;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A, "A should not equal to 0x00.");
            Assert.AreEqual(0x01, board.cpu.A, "A should be equal to 0x01.");
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
        }
        
        [Test]
        public void LDA_AbsoluteY_PageBoundary_Clear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xB9, 0xFF, 0xC0 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.Y = 0x01;
            board.cpu.Write(0xC100,0x01);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A, "A should not equal to 0x00.");
            Assert.AreEqual(0x01, board.cpu.A, "A should be equal to 0x01.");
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
        }
        
        [Test]
        public void LDA_IndexedIndirect_Clear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xA1, 0x01, 0xC0, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.X = 0x01;
            board.cpu.Write(0x00,0x01);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A, "A should not equal to 0x00.");
            Assert.AreEqual(0x01, board.cpu.A, "A should be equal to 0x01.");
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
        }       
        
        [Test]
        public void LDA_IndirectIndexed_Clear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xB1, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.Y = 0x01;
            board.cpu.Write(0x00,0x01);
            board.cpu.Write(0x01,0x00);
            board.cpu.Write(0x02,0x01);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A, "A should not equal to 0x00.");
            Assert.AreEqual(0x01, board.cpu.A, "A should be equal to 0x01.");
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
        }       
        
        [Test]
        public void LDA_IndirectIndexed_PageBoundary_Clear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xB1, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.Y = 0x01;
            board.cpu.Write(0x00,0xFF);
            board.cpu.Write(0x01,0x00);
            board.cpu.Write(0x100,0x01);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A, "A should not equal to 0x00.");
            Assert.AreEqual(0x01, board.cpu.A, "A should be equal to 0x01.");
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
        }   
   
        [Test]
        public void LDA_IndirectIndexed_BigY_PageBoundary_Clear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0xB1, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.Y = 0x03;
            board.cpu.Write(0x00,0xFE);
            board.cpu.Write(0x01,0x00);
            board.cpu.Write(0x0101,0x01);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x00, board.cpu.A, "A should not equal to 0x00.");
            Assert.AreEqual(0x01, board.cpu.A, "A should be equal to 0x01.");
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
        }
        
    }
}