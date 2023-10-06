﻿using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using CADAddins.Archive;
using CADAddins.Environments;

[assembly: CommandClass(typeof(CADAddins.LibsOfDDNCrop.HatchUtils))]

namespace CADAddins.LibsOfDDNCrop
{
    public class OCommandTransBaseOfHatchUtils : O_CommandTransBase
    {
        public OCommandTransBaseOfHatchUtils(Transaction acTrans) : base(acTrans)
        {
        }

        public override bool Run()
        {
            var hatchId = CurOEditorHelper2.GetEntity("\nSelect Hatch: ", "\nNot a Hatch", typeof(Hatch));
            if (hatchId == ObjectId.Null) return false;
            var hatch = GetObjectForRead(hatchId) as Hatch;
            if (hatch == null) return false;
            CurOEditorHelper2.WriteMessage(hatch.GetHatchLoopTypes());
            hatch.GenerateHatchBoundaries(this);

            return true;
        }
    }
}