﻿using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using CADAddins.Archive;
using CADAddins.Environments;

namespace CADAddins.LibsOfDDNCrop
{
    public class ReadHatchLoopType : CommandBase
    {
        [CommandMethod("RLT")]
        public override void Run()
        {
            base.Run();
        }

        internal override O_CommandTransBase InitCommandTransBase(Transaction acTrans)
        {
            return new OCommandTransBaseOfReadHatchLoopType(acTrans);
        }
    }
}