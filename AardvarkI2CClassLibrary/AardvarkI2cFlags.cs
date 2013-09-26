﻿namespace AardvarkI2CClassLibrary
{
    using System;

    public enum AardvarkI2cFlags
    {
        AA_I2C_10_BIT_ADDR = 1,
        AA_I2C_COMBINED_FMT = 2,
        AA_I2C_NO_FLAGS = 0,
        AA_I2C_NO_STOP = 4,
        AA_I2C_SIZED_READ = 0x10,
        AA_I2C_SIZED_READ_EXTRA1 = 0x20
    }
}

