﻿using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;

namespace PreviousDevelopmentToRefactor.Environments
{
    public abstract class CommandBase
    {
        internal CommandBase()
        {
        }

        internal DocHelper CurDocHelper => CadHelper.CurDocHelper;
        internal EditorHelper CurEditorHelper => CurDocHelper.CurEditorHelper;

        internal void EndCommands(bool EndOrCancel)
        {
            if (!EndOrCancel)
                CurEditorHelper.WriteMessage($"\nCommand [{GetType().Name}] Cancelled.");
            else
                CurEditorHelper.WriteMessage($"\nCommand [{GetType().Name}] Succeeded.");
            CadHelper.Quit();
        }

        internal abstract CommandTransBase InitCommandTransBase(Transaction acTrans);
        
        public virtual void Run()
        {
            using (var acTrans = CurDocHelper.StartTransaction())
            {
                using (var cmd = InitCommandTransBase(acTrans))
                {
                    if (cmd.Run())
                    {
                        acTrans.Commit();
                        EndCommands(true);
                    }
                    else
                    {
                        acTrans.Abort();
                        EndCommands(false);
                    }
                }
            }
        }
    }
}