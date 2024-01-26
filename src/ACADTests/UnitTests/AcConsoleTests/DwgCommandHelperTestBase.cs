﻿using System;
using System.Linq.Expressions;
using ACADBase;
using Autodesk.AutoCAD.DatabaseServices;
using Moq;
using NLog;
using NUnit.Framework;

namespace ACADTests.UnitTests.AcConsoleTests
{
    public class DwgCommandHelperTestBase
    {
        protected const string TestDrawingPath = @"D:\leaveblackgithub\DDNCADAddinsForRevitImport\src\ACADTests\TestDrawing.dwg";
        protected IDwgCommandHelper DwgCommandHelperOfTestDwg=>
            _dwgCommandHelperOfTestDwg??(_dwgCommandHelperOfTestDwg=new DwgCommandHelper(TestDrawingPath, GetMsgProviderMockObj()));
        protected IDwgCommandHelper DwgCommandHelperActive=>_dwgHelperActive??(_dwgHelperActive=new DwgCommandHelper("",GetMsgProviderMockObj()));
        protected Mock<IMessageProvider> MsgProviderMockInitInBase =>_msgProviderMockInitInSetup??(_msgProviderMockInitInSetup=new Mock<IMessageProvider>());
        // protected Action<Database> EmptyDbAction;
        private TestException _exInitInBase;
        private Mock<IMessageProvider> _msgProviderMockInitInSetup;
        private IDwgCommandHelper _dwgCommandHelperOfTestDwg;
        private IDwgCommandHelper _dwgHelperActive;
        private Mock<DwgCommandHelper> _dwgCommandBaseMockProtected;
        private DwgCommandHelper _dwgCommandHelperOfMsgBox;

        protected TestException ExInitInBase=>_exInitInBase??(_exInitInBase=new TestException(nameof(ExInitInBase)));

        protected DwgCommandHelper DwgCommandHelperOfMsgBox =>
            _dwgCommandHelperOfMsgBox ?? (_dwgCommandHelperOfMsgBox = new DwgCommandHelper(""));

        protected Mock<DwgCommandHelper> DwgCommandBaseMockProtected=>_dwgCommandBaseMockProtected??(_dwgCommandBaseMockProtected=new Mock<DwgCommandHelper>("",GetMsgProviderMockObj()));

        [SetUp]
        public virtual void SetUp()
        {
            // DwgCommandHelperTest = new DwgCommandHelper(
            //     TestDrawingPath, messageProvider);
            // DwgCommandHelperActive = new DwgCommandHelper("", messageProvider);
            // EmptyDbAction = (db => LogManager.GetCurrentClassLogger().Info("EmptyDbAction"));
            MsgProviderInvokeClear();
        }

        [TearDown]
        public virtual void TearDown()
        {
            // MsgProviderInvokeClear();
        }

        protected IMessageProvider GetMsgProviderMockObj()
        {
            MsgProviderInvokeClear();
            return MsgProviderMockInitInBase.Object;
        }
        private void MsgProviderInvokeClear()
        {
            MsgProviderMockInitInBase.Invocations.Clear();
        }

        protected void EmptyDbAction(Database db)
        {
            LogManager.GetCurrentClassLogger().Info("EmptyDbAction");
        }

        protected void MsgProviderShowExInitInBaseOnce()
        {
            MsgProviderVerifyExOnce(m=>m.Error(ExInitInBase));
        }

        protected void MsgProviderVerifyExTypeOnce<T>() where T : Exception
        {
            MsgProviderVerifyExOnce(m=>m.Error(It.IsAny<T>()));
        }
        protected void MsgProviderVerifyExOnce(Expression<Action<IMessageProvider>> checkExceptAction)
        {
            MsgProviderMockInitInBase.Verify(checkExceptAction, Times.Once);
            MsgProviderInvokeClear();
        }
    }
}