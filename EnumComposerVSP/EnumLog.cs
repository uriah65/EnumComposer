﻿using EnumComposer;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace Uriah65.EnumComposerVSP
{
    internal class EnumLog : IEnumLog
    {
        private IVsOutputWindow _outputWindow;

        public EnumLog(IVsOutputWindow outputWindow)
        {
            _outputWindow = outputWindow;
        }

        public void WriteLine(string format, params object[] arguments)
        {
            Guid generalPaneGuid = VSConstants.GUID_OutWindowDebugPane;//.GUID_OutWindowGeneralPane;
            IVsOutputWindowPane outputPane;
            _outputWindow.GetPane(ref generalPaneGuid, out outputPane);
            if (outputPane != null)
            {
                string message = string.Format("{0}{1} EnumComposer: ", Environment.NewLine, DateTime.Now.ToString("HH:mm:ss"));
                message += string.Format(format, arguments);
                outputPane.OutputString(message);
                outputPane.Activate();
            }
        }
    }
}