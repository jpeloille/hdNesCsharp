/* First implementation : 28-09-2022 */
/* Implemented by Julien PELOILLE */
/* Based on XamarinNES unit test source code */

using hdNes.Nes;
using NUnit.Framework;

namespace hdNes.Cpu.Test
{
    public class LSR_Tests
    {
        [Test]
        public void LSR_Accumulator_Clear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x4A });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0x80;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x80, board.cpu.A, "A should not be equal to 0x80.");
            Assert.AreEqual(0x40, board.cpu.A, "A should be equal to 0x40.");

            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
            Assert.AreEqual(false, board.cpu.C, "C not false.");
        }
        
        [Test]
        public void LSR_Accumulator_Carry()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x4A });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0x03;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x03, board.cpu.A, "A should not be equal to 0x03.");
            Assert.AreEqual(0x01, board.cpu.A, "A should be equal to 0x01.");

            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
            Assert.AreEqual(true, board.cpu.C, "C not true.");
        }       
        
        [Test]
        public void LSR_Accumulator_Zero_Carry()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x4A });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0x01;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x01, board.cpu.A, "A should not be equal to 0x01.");
            Assert.AreEqual(0x00, board.cpu.A, "A should be equal to 0x00.");

            //Check flags output:
            Assert.AreEqual(true, board.cpu.Z, "Z not true.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
            Assert.AreEqual(true, board.cpu.C, "C not true.");
        }   
        
        [Test]
        public void LSR_ZeroPage_Clear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x46, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.Write(0x00,0x80);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x80, board.cpu.Read(0x00), "M should not be equal to 0x80.");
            Assert.AreEqual(0x40, board.cpu.Read(0x00), "M should be equal to 0x40.");

            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
            Assert.AreEqual(false, board.cpu.C, "C not false.");
        }
        
        [Test]
        public void LSR_ZeroPage_Carry()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x46, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.Write(0x00,0x03);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x03, board.cpu.Read(0x00), "M should not be equal to 0x03.");
            Assert.AreEqual(0x01, board.cpu.Read(0x00), "M should be equal to 0x01.");

            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
            Assert.AreEqual(true, board.cpu.C, "C not true.");
        }  
        
        [Test]
        public void LSR_ZeroPage_Zero_Carry()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x46, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.Write(0x00,0x01);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x01, board.cpu.Read(0x00), "M should not be equal to 0x01.");
            Assert.AreEqual(0x00, board.cpu.Read(0x00), "M should be equal to 0x00.");

            //Check flags output:
            Assert.AreEqual(true, board.cpu.Z, "Z not true.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
            Assert.AreEqual(true, board.cpu.C, "C not true.");
        }  
        
        [Test]
        public void LSR_ZeroPageX_Clear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x56, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.X = 0x01;
            board.cpu.Write(0x01,0x80);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x80, board.cpu.Read(0x01), "M should not be equal to 0x80.");
            Assert.AreEqual(0x40, board.cpu.Read(0x01), "M should be equal to 0x40.");

            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
            Assert.AreEqual(false, board.cpu.C, "C not false.");
        }
        
        [Test]
        public void LSR_Absolute_Clear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x4E, 0x03, 0xC0, 0x80 });
            board.cpu.SetInUnitTestInitialState();

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x80, board.cpu.Read(0xC003), "M should not be equal to 0x80.");
            Assert.AreEqual(0x40, board.cpu.Read(0xC003), "M should be equal to 0x40.");

            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
            Assert.AreEqual(false, board.cpu.C, "C not false.");
        }
        
        [Test]
        public void LSR_AbsoluteX_Clear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x5E, 0x02, 0xC0, 0x80 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.X = 0x01;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x80, board.cpu.Read(0xC003), "M should not be equal to 0x80.");
            Assert.AreEqual(0x40, board.cpu.Read(0xC003), "M should be equal to 0x40.");

            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
            Assert.AreEqual(false, board.cpu.C, "C not false.");
        }
    }
}