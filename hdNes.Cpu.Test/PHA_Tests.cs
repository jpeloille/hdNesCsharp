/* First implementation : 29-09-2022 */
/* Implemented by Julien PELOILLE */
/* Based on XamarinNES unit test source code */

using hdNes.Nes;
using NUnit.Framework;

namespace hdNes.Cpu.Test
{
    public class PHA_Tests
    {
        [Test]
        public void PHA_Implied()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x48 });
            board.cpu.SetInUnitTestInitialState();
            board.cpu.A = 0x01;

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreEqual(0x01, 
                board.cpu.Read((ushort)(0x0100 + board.cpu.S +1)), 
                "A should be equal to 0x7X.");
        }
    }
}