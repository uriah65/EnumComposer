using System;
using EnumComposer;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;

namespace Uriah65.EnumComposerVSP
{
    internal class EnumLog : IEnumLog
    {
        IVsOutputWindow _outputWindow;

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
                string message = string.Format(Environment.NewLine + "EnumComposer: " + format, arguments);
                outputPane.OutputString(message);
                outputPane.Activate();
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