﻿using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using General;

namespace PreviousDevelopmentToRefactor.Environments
{
    public class DocHelper : DisposableClassBase
    {
        private EditorHelper _curEditorHelper;

        public DocHelper(Document acDoc)
        {
            AcDoc = acDoc;
        }

        public Document AcDoc { get; }

        public Database AcCurDb => HostApplicationServices.WorkingDatabase;

        public ObjectId CurSpaceId => AcCurDb.CurrentSpaceId;
        public ObjectId BlockTableId => AcCurDb.BlockTableId;

        public EditorHelper CurEditorHelper =>
            _curEditorHelper ?? (_curEditorHelper =
                new EditorHelper(AcDoc.Editor));

        public string Name => AcDoc.Name;

        public Transaction StartTransaction()
        {
            return AcDoc.TransactionManager.StartTransaction();
        }


        public void Audit()
        {
            using (var trans = StartTransaction())
            {
                AcCurDb.Audit(true, false);
            }
        }

        protected override void DisposeUnManaged()
        {
        }

        protected override void DisposeManaged()
        {
        }
    }
}