﻿using System.Threading;
using Autodesk.AutoCAD.ApplicationServices.Core;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Domain.Shared;
using NUnit.Framework;
using TestRunnerACAD;

namespace ACADTests.Cleanup
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class CleanupTests : TestBaseExecution
    {
        [Test]
        public void ReadLtypes()
        {

            void Action1(Database db, Transaction tr)
            {
                //get all line types in the database
                using (var ltypes = db.LinetypeTableId.GetObject(OpenMode.ForRead) as LinetypeTable)
                {
                    Assert.IsTrue(ltypes.Has(CleanupTestConsts.TestLType));
                }
            }
            // Run the tests
            TestBaseWDb.ExecuteTestActions(CleanupTestConsts.CleanupTestDwg, Action1);
        }
       
    }
}