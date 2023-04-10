/* First implementation : 28-09-2022 */
/* Implemented by Julien PELOILLE */
/* Based on XamarinNES unit test source code */

using hdNes.Nes;
using NUnit.Framework;

namespace hdNes.Cpu.Test
{
    public class ORA_Tests
    {
        [Test]
        public void ORA_Immediate_Clear()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x09, 0x55 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0x2A;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreNotEqual(0x2A, board.cpu.A, "A should not be equal to 0x2A.");
            Assert.AreEqual(0x7F, board.cpu.A, "A should be equal to 0x7X.");

            //Check flags output:
            Assert.AreEqual(false, board.cpu.Z, "Z not false.");
            Assert.AreEqual(false, board.cpu.N, "N not false.");
        }
    }
}