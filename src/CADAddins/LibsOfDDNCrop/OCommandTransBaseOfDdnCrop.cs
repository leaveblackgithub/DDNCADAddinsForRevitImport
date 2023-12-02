﻿using System.Collections.Generic;
using Autodesk.AutoCAD.DatabaseServices;
using CADAddins.Archive;
using CADAddins.Cropper;
using CADAddins.Environments;

namespace CADAddins.LibsOfDDNCrop
{
    public class OCommandTransBaseOfDdnCrop : O_CommandTransBase
    {
        public OCommandTransBaseOfDdnCrop(Transaction acTrans) : base(acTrans)
        {
        }

        public override bool Run()
        {
            var sset = CurOEditorHelper2.GetSelection("\nSelect entities to be cropped:");
            if (sset == null) return false;
            Curve boundary;
            while (true)
            {
                var boundaryId = CurOEditorHelper2.GetEntity("\nSelect a closed Curve as crop boundary: ",
                    "Invalid selection: requires a closed Curve", typeof(Curve));
                if (boundaryId == ObjectId.Null) return false;

                boundary = AcTrans.GetObjectForRead(boundaryId) as Curve;
                ;
                if (boundary.Closed) break;
                CurOEditorHelper2.WriteMessage("\nPolyline should be closed.");
            }

            IEnumerable<ObjectId> objectIds = sset.GetObjectIds();
            var result = CropEntitiesWithBoundary(objectIds, boundary, WhichSideToKeep.Inside);
//            CurEditorHelper.WriteMessage(result.ToString());
            return true;
        }
    }
}