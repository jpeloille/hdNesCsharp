/* First implementation : 29-09-2022 */
/* Implemented by Julien PELOILLE */
/* Based on XamarinNES unit test source code */

using hdNes.Nes;
using NUnit.Framework;

namespace hdNes.Cpu.Test
{
    public class RTS_Tests
    {
        [Test]
        public void ROL_Accumulator_Zero()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x20, 0x05, 0xC0, 0xE8, 0x00, 0x60 });
            board.cpu.SetInUnitTestInitialState();

            board.cpu.Tick(20);

            //Check register values output:
            //Assert.AreEqual(0x01, board.cpu.X, "X");
            
            //Check flags output:
            Assert.AreEqual(false, board.cpu.V, "V");
            Assert.AreEqual(false, board.cpu.C, "C");
            Assert.AreEqual(false, board.cpu.Z, "Z");
            Assert.AreEqual(false, board.cpu.N, "N");
            
            Assert.AreEqual(0xFD, board.cpu.S, "S");
            
            Assert.AreEqual(0xC004, board.cpu.PC,"PC");
        }
    }
}