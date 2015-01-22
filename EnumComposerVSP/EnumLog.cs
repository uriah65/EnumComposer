using System;
using EnumComposer;
using Microsoft.VisualStudio.Shell.Interop;

namespace Uriah65.EnumComposerVSP
{
    internal class EnumLog : IEnumLog
    {
        private IVsOutputWindowPane _outputPane;

        public EnumLog(IVsOutputWindowPane outputPane)
        {
            _outputPane = outputPane;
        }

        public void WriteLine(string format, params object[] arguments)
        {
            if (_outputPane != null)
            {
                string message = string.Format("EnumComposer: " + format, arguments);
                _outputPane.OutputString(message);
            }
        }

        //void AttemptPane()
        //{
        //    IVsOutputWindow outWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
        //    Guid generalPaneGuid = VSConstants.GUID_OutWindowGeneralPane; // P.S. There's also the GUID_OutWindowDebugPane available.
        //    IVsOutputWindowPane outputPane;
        //    outWindow.GetPane(ref generalPaneGuid, out outputPane);
        //}
    }
}