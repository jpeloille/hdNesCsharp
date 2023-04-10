/* First implementation : 29-09-2022 */
/* Implemented by Julien PELOILLE */
/* Based on XamarinNES unit test source code */

using hdNes.Nes;
using NUnit.Framework;

namespace hdNes.Cpu.Test
{
    public class PHP_Tests
    {
        [Test]
        public void PHP_Implied()
        {
            //Initialize fictive board:
            Board board = new Board();
            board.Cartridge.UnitTest_Configure(new byte[] { 0x08 });
            board.cpu.SetInUnitTestInitialState();

            board.cpu.Tick(2);

            //Check register values output:
            Assert.AreEqual(0x24, 
                board.cpu.Read((ushort)(0x0100 + board.cpu.S +1)), 
                "A should be equal to 0x7X.");
        }
    }
}