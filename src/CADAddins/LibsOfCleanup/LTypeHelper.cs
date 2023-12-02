﻿using System;
using System.Collections.Generic;
using CADAddins.Archive;
using CommonUtils;

namespace CADAddins.LibsOfCleanup
{
    public class LTypeHelper : DisposableClass
    {
        private readonly O_DocHelper _curDocHelper;
        private readonly O_EditorHelper _curEditorHelper;
        private readonly dynamic _lTypeTableId;
        public dynamic Ltypes { get; private set; }
        public Dictionary<string, List<dynamic>> Dirtytypes { get; private set; }
        public bool CleanTag { get; private set; }
        private Dictionary<string, dynamic> Cleantypes { get; set; }

        public LTypeHelper(dynamic lTypeTableId, O_DocHelper docHelper)
        {
            _lTypeTableId = lTypeTableId;
            _curDocHelper = docHelper;
            _curEditorHelper = _curDocHelper.CurEditorHelper;
            CleanTag = false;
            Cleantypes = new Dictionary<string, dynamic>();
            Dirtytypes = new Dictionary<string, List<dynamic>>();
            Ltypes = _lTypeTableId;
        }


        public void Cleanup()
        {
            if (Check()) return;

            var count = 0;
            using (var trans = _curDocHelper.StartTransaction())
            {
                var ge = Dirtytypes.GetEnumerator();
                while (ge.MoveNext())
                {
                    var cleanname = ge.Current.Key;
                    if (Cleantypes.ContainsKey(cleanname)) continue;
                    var ltype = ge.Current.Value[0];
                    string oldname = ltype.Name;
                    ltype.Name = cleanname;
                    _curEditorHelper.WriteMessage($"\nLinetype [{oldname}] renamed to [{cleanname}].");
                    Cleantypes[cleanname] = ltype;
                    count++;
                }

                ge.Dispose();
                CleanTag = true;
                trans.Commit();
            }

            _curEditorHelper.WriteMessage($"\n{count} Linetypes renamed in total.");
        }

        public bool Check()
        {
            foreach (var ltype in Ltypes)
            {
                string ltypeName = ltype.Name;
                if (!BoundPrefixUtils.HasBoundPrefix(ltypeName))
                {
                    Cleantypes[ltypeName] = ltype;
                    continue;
                }

                var cleanname = BoundPrefixUtils.RemoveBoundPrefix(ltypeName);
                if (!Dirtytypes.ContainsKey(cleanname)) Dirtytypes[cleanname] = new List<dynamic>();
                Dirtytypes[cleanname].Add(ltype);
            }

            if (Dirtytypes.Count == 0)
            {
                _curEditorHelper.WriteMessage("\nAll linetypes are fine without bound prefix.");
                CleanTag = true;
                return true;
            }

            return false;
        }

        public dynamic GetLTypeByCleanName(string name)
        {
            string cleanname;
            if (!BoundPrefixUtils.HasBoundPrefix(name))
                cleanname = name;
            else
                cleanname = BoundPrefixUtils.RemoveBoundPrefix(name);

            dynamic result;
            if (!Cleantypes.TryGetValue(cleanname, out result))
                throw new SystemException($"Can't find Linetype[{cleanname}]");
            return result;
        }

        protected override void DisposeUnManaged()
        {
            Cleantypes.Clear();
            Cleantypes = null;
        }

        protected override void DisposeManaged()
        {
            throw new NotImplementedException();
        }
    }
}