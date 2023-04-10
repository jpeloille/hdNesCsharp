/* First implementation : 29-09-2022 */
/* Implemented by Julien PELOILLE */
/* Based on XamarinNES unit test source code */

using hdNes.Nes;
using NUnit.Framework;

namespace hdNes.Cpu.Test
{
    public class PLA_Tests
    {
        [Test]
        public void PLA_Implied()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x48, 0xA9, 0x00, 0x68 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0x01;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreEqual(0x01, board.cpu.A, "A should be equal to 0x01.");
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
        }    
        
        [Test]
        public void PLA_Implied_Negative()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x48, 0xA9, 0x00, 0x68 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0x80;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreEqual(0x80, board.cpu.A, "A should be equal to 0x80.");
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
        }  
    }
}