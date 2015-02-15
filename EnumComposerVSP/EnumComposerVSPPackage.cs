﻿using EnumComposer;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Uriah65.EnumComposerVSP
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidEnumComposerVSPPkgString)]
    public sealed class EnumComposerVSPPackage : Package
    {
        private IEnumLog log;

        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require
        /// any Visual Studio service because at this point the package object is created but
        /// not sited yet inside Visual Studio environment. The place to do all the other
        /// initialization is the Initialize method.
        /// </summary>
        public EnumComposerVSPPackage()
        {
            //Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }

        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            //Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                // Create the command for the menu item.
                CommandID menuCommandID = new CommandID(GuidList.guidEnumComposerVSPCmdSet, (int)PkgCmdIDList.cmdidRunEnumComposer);
                MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandID);

                mcs.AddCommand(menuItem);
            }
        }

        #endregion Package Members

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            ////// Show a Message Box to prove we were here
            ////IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
            ////Guid clsid = Guid.Empty;
            ////int result;
            ////Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(uiShell.ShowMessageBox(
            ////           0,
            ////           ref clsid,
            ////           "EnumComposer Visual Studio Package",
            ////           string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.ToString()),
            ////           string.Empty,
            ////           0,
            ////           OLEMSGBUTTON.OLEMSGBUTTON_OK,
            ////           OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
            ////           OLEMSGICON.OLEMSGICON_INFO,
            ////           0,        // false
            ////           out result));

            IVsOutputWindow outWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
            log = new EnumLog(outWindow);

            log.WriteLine("");
            log.WriteLine("Started.");
            RunComposerScan();
            log.WriteLine("Finished.");
        }

        private void RunComposerScan()
        {
            try
            {

                RunComposerScan_Inner();
            }
            catch (Exception ex)
            {
                string message = "Sorry, and exception has occurred." + Environment.NewLine + Environment.NewLine + ex.Message + Environment.NewLine + Environment.NewLine + "See the Output\\Debug window for details.";
                if (log != null)
                {
                    string logMessage = DedbugLog.ExceptionMessage(ex);
                    log.WriteLine(logMessage);
                }

                IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
                Guid clsid = Guid.Empty;
                int result;
                uiShell.ShowMessageBox(0,
                       ref clsid,
                       "EnumComposer Visual Studio Package",
                       message,
                       string.Empty,
                       0,
                       OLEMSGBUTTON.OLEMSGBUTTON_OK,
                       OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                       OLEMSGICON.OLEMSGICON_INFO,
                       0,        // false
                       out result);
            }
        }

        //private string SolutionPath()
        //{
        //    //todo: do better
        //    string solutionDirectory = ((EnvDTE.DTE)System.Runtime
        //                                                 .InteropServices
        //                                                 .Marshal
        //                                                 .GetActiveObject("VisualStudio.DTE.10.0"))
        //                                      .Solution.FullName;
        //    solutionDirectory = System.IO.Path.GetDirectoryName(solutionDirectory);

        //    return solutionDirectory;
        //}

        private void RunComposerScan_Inner()
        {
            DTE2 applicationObject = (DTE2)GetService(typeof(SDTE));

            ///applicationObject.Solution


            if (applicationObject.ActiveDocument == null)
            {
                return;
            }

            TextDocument document = (TextDocument)applicationObject.ActiveDocument.Object("TextDocument");
            if (document == null)
            {
                return;
            }

            DbReader dbReader = new DbReader();
            ConfigReader configReader = null;

            try
            {
                string docPath = applicationObject.ActiveDocument.Path;
                string solutionName = applicationObject.Solution.FullName;
                if (solutionName != "")
                {
                    string solutionPath = System.IO.Path.GetDirectoryName(applicationObject.Solution.FullName);
                    configReader = new ConfigReader(docPath, solutionPath, log);
                    dbReader._readConfigFunction = configReader.LocateConnectionInVSP; /* we provide config search function only if all is OK. */
                }
            }
            catch (Exception ex)
            {
                if (log != null)
                {
                    string logMessage = DedbugLog.ExceptionMessage(ex);
                    log.WriteLine(logMessage);
                }
            }


            ComposerStrings composer = new ComposerStrings(dbReader, log);
            ApplyComposer(document, composer);
        }

        //public void ApplyComposer_New(TextDocument document, ComposerStrings composer)
        //{
        //    /* get document bounds */
        //    EditPoint startEdit = document.CreateEditPoint(document.StartPoint);
        //    EditPoint endEdit = document.EndPoint.CreateEditPoint();

        //    /* run composer */
        //    string text = startEdit.GetText(document.EndPoint);
        //    composer.Compose(text);

        //    int ixLastInsert = text.Length;

        //    foreach (var model in composer.EnumModels.OrderByDescending(e => e.SpanEnd))
        //    {
        //        if (model.SpanEnd > ixLastInsert)
        //        {
        //            throw new ApplicationException("Invalid enumeration order for '" + model.Name + "'.");
        //        }

        //        EditPoint from = document.CreateEditPoint(document.StartPoint);
        //        EditPoint to = document.CreateEditPoint(document.StartPoint);
        //        int ix = endEdit.AbsoluteCharOffset;
        //        from.MoveToAbsoluteOffset(model.SpanStart);
        //        to.MoveToAbsoluteOffset(model.SpanEnd);

        //        from.Delete(to);
        //        from.Insert(model.ToCSharp());

        //    }
        //}

        public void ApplyComposer(TextDocument document, ComposerStrings composer)
        {
            /* get document bounds */
            EditPoint startEdit = document.CreateEditPoint(document.StartPoint);
            EditPoint endEdit = document.EndPoint.CreateEditPoint();

            /* run composer */
            string text = startEdit.GetText(document.EndPoint);
            composer.Compose(text);
            if (composer.EnumModels != null && composer.EnumModels.Count > 0)
            {
                /* get new file*/
                text = composer.GetResultFile();

                /* delete and re-insert full document */
                startEdit.Delete(endEdit);
                startEdit.Insert(text);
            }
        }

        public string Reverse(string text)
        {
            /* test method, not used */
            char[] cArray = text.ToCharArray();
            string reverse = "";
            for (int i = cArray.Length - 1; i > -1; i--)
            {
                reverse += cArray[i];
            }
            return reverse;
        }
    }
}