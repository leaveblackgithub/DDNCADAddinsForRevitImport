﻿using System;
using Autodesk.AutoCAD.DatabaseServices;
using CommonUtils;
using CommonUtils.CustomExceptions;

namespace ACADBase
{
    public class DatabaseHelper:DisposableClass
    {
        public DatabaseHelper(Database cadDatabase)
        {
            CadDatabase = cadDatabase;
        }

        public Database CadDatabase { get; }

        protected override void DisposeUnManaged()
        {
        }

        protected override void DisposeManaged()
        {
            CadDatabase?.Dispose();
        }
        public CommandResult RunFuncInTransaction<T>(HandleValue handleValue,
            params Func<T, CommandResult>[] funcs) where T : DBObject
        {
            var result = new CommandResult();
            if (funcs.IsNullOrEmpty()) return result;
            using (var tr = NewTransactionHelper())
            {
                if (!TryGetObjectId(handleValue, out var objectId))
                {
                    return result.Cancel(ArgumentExceptionOfInvalidHandle._(handleValue.HandleAsLong));
                }
                result = tr.RunFuncsOnObject<T>(objectId, funcs);
                tr.Commit();
                return result;
            }
        }

        public CommandResult CreateInModelSpace<T>(out HandleValue handleValue,
            params Func<T, CommandResult>[] funcs)
            where T : Entity, new()
        {
            handleValue = null;
            using (var tr = NewTransactionHelper())
            {
                handleValue = tr.CreateInModelSpace<T>(GetBlockTableId<T>());
                tr.Commit();
            }
            return RunFuncInTransaction(handleValue, funcs);
        }

        private ObjectId GetBlockTableId<T>() where T : Entity, new()
        {
            return CadDatabase.BlockTableId;
        }

        public  TransactionHelper NewTransactionHelper()

        {
            return new TransactionHelper(CadDatabase.TransactionManager.StartTransaction());
        }

        public bool TryGetObjectId(HandleValue handleValue, out ObjectId objectId)
        {
            return CadDatabase.TryGetObjectId(handleValue.ToHandle(),out objectId);
        }
    }
}