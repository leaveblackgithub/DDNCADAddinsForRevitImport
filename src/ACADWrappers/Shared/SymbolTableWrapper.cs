﻿using Autodesk.AutoCAD.DatabaseServices;
using Domain.Shared;
using System.Collections.Generic;
using System;
using CommonUtils;

namespace ACADWrappers.Shared
{
    public class SymbolTableWrapper : ISymbolTableWrapper
    {
        public DatabaseWrapper DbWrapper { get; }
        public ObjectId SymbolTableId { get; }
        public Dictionary<string, IntPtr> SymbolTableRecordNames { get; set; }

        public SymbolTableWrapper (DatabaseWrapper dbWrapper,ObjectId symbolTableId)
        {
            DbWrapper = dbWrapper;
            SymbolTableId = symbolTableId;
        }
        public void ReadSymbolTableRecordNames()
        {
            DbWrapper.RunInTransaction(ReadSymbolTableRecordNamesFrTr);
        }

        private void ReadSymbolTableRecordNamesFrTr(Transaction transaction)
        {
            SymbolTableRecordNames= new Dictionary<string, IntPtr>();
            transaction.ReadDbObject<SymbolTable>(SymbolTableId, table => table.CycleReadRecord(transaction,ReadRecordName));
        }
        private void ReadRecordName(SymbolTableRecord record)
        {
            SymbolTableRecordNames.Add(record.Name, record.Id.OldIdPtr);
        }
    }
}