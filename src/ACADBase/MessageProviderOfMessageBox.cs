﻿using System;
using System.Windows.Forms;
using NLog;

namespace ACADBase
{
    internal class MessageProviderOfMessageBox : IMessageProvider
    {
        public void Show(string message)
        {
            MessageBox.Show(message);
        }

        public void Error(Exception exception)
        {
            var exString = exception.ToString();
            LogManager.GetCurrentClassLogger().Error(exString);
            Show(exString);
        }
    }
}