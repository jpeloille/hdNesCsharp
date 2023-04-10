/* First implementation : 29-09-2022 */
/* Implemented by Julien PELOILLE */
/* Based on XamarinNES unit test source code */

using hdNes.Nes;
using NUnit.Framework;

namespace hdNes.Cpu.Test
{
    public class ROL_Tests
    {
        [Test]
        public void ROL_Accumulator_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x2A });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0x00;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreEqual(0x00, board.cpu.A, "A should be equal to 0x00.");
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.C, "C not false.");
            Assert.AreEqual(true, board.cpu.Z, "Z not true.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
        }
        
        [Test]
        public void ROL_Accumulator_Negative()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x2A });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0x7F;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreEqual(0xFE, board.cpu.A, "A should be equal to 0xFE.");
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.C, "C not false.");
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(true, board.cpu.N, "N not true.");
        } 
        
        [Test]
        public void ROL_Accumulator_SetCarry()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x2A });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0xFF;
            board.cpu.C = false;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreEqual(0xFE, board.cpu.A, "A should be equal to 0xFE.");
            
            //Check flags output:
            Assert.AreEqual(true, board.cpu.C, "C not false.");
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(true, board.cpu.N, "N not true.");
        } 
        
                
        [Test]
        public void ROL_Accumulator_ApplyCarry()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x2A });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0xFF;
            board.cpu.C = true;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreEqual(0xFF, board.cpu.A, "A should be equal to 0xFF.");
            
            //Check flags output:
            Assert.AreEqual(true, board.cpu.C, "C not false.");
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(true, board.cpu.N, "N not true.");
        }
        
        [Test]
        public void ROL_ZeroPage_Clear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x26, 0x00 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.Write(0x00,0x3F);

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreEqual(0x7E, board.cpu.Read(0x00), "M should be equal to 0x7E.");
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.C, "C not false.");
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not true.");
        } 
    }
}