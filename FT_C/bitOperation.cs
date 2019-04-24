using System;

namespace FT.C
{

    /// <summary>
    /// bit操作クラス
    /// </summary>
    public class bitOperation
    {

        /// <summary>
        /// バイト表記
        /// </summary>
        [Flags]
        public enum Bits : byte
        {
            /// <summary>0</summary>
            b00000000 = 0x00,

            /// <summary>1</summary>
            b00000001 = 0x01,

            /// <summary>2</summary>
            b00000010 = 0x02,

            /// <summary>4</summary>
            b00000100 = 0x04,

            /// <summary>8</summary>
            b00001000 = 0x08,

            /// <summary>16</summary>
            b00010000 = 0x10,

            /// <summary>32</summary>
            b00100000 = 0x20,

            /// <summary>64</summary>
            b01000000 = 0x40,

            /// <summary>128</summary>
            b10000000 = 0x80,

            /// <summary>256</summary>
            b11111111 = 0xFF,
        }

    }
}
