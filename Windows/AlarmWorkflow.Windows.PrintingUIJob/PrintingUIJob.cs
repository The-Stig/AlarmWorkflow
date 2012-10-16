﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using AlarmWorkflow.Shared.Core;
using AlarmWorkflow.Shared.Diagnostics;
using AlarmWorkflow.Windows.PrintingUIJob.Config;
using AlarmWorkflow.Windows.UI.Extensibility;

namespace AlarmWorkflow.Windows.PrintingUIJob
{
    /// <summary>
    /// A UI-Job that automatically prints 
    /// </summary>
    [Export("PrintingUIJob", typeof(IUIJob))]
    class PrintingUIJob : IUIJob
    {
        #region Fields

        private Configuration _configuration;
        private Lazy<PrintQueue> _printQueue;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintingUIJob"/> class.
        /// </summary>
        public PrintingUIJob()
        {
            _printQueue = new Lazy<PrintQueue>(() =>
            {
                if (_configuration == null)
                {
                    return null;
                }

                LocalPrintServer printServer = new LocalPrintServer();

                // Pick the desired printer (even if none is selected, for convenience)
                PrintQueue queue = printServer.GetPrintQueues().FirstOrDefault(pq => pq.FullName.Equals(_configuration.PrinterName));

                // If there was a printer found, return that one
                if (queue != null)
                {
                    return queue;
                }

                // Otherwise see if we are supposed to return a custom named printer ...
                if (!string.IsNullOrWhiteSpace(_configuration.PrinterName))
                {
                    Logger.Instance.LogFormat(LogType.Warning, this, "Did not find a printer with name '{0}'. Using the default printer.", _configuration.PrinterName);
                }

                // Return the (desired) default printer
                return printServer.DefaultPrintQueue;
            });
        }

        #endregion

        #region Methods

        private bool CheckIsOperationAlreadyPrinted(Operation operation, bool addIfNot)
        {
            // If we shall always print any operation (not recommended)
            if (!_configuration.RememberPrintedOperations)
            {
                return false;
            }

            // Load the file that stores the printed operations
            string fileName = Path.Combine(Utilities.GetWorkingDirectory(), "Config\\PrintingUIPrintedOperations.lst");

            List<string> alreadyPrintedOperations = new List<string>();

            if (File.Exists(fileName))
            {
                alreadyPrintedOperations = new List<string>(File.ReadAllLines(fileName));
                if (alreadyPrintedOperations.Contains(operation.OperationNumber))
                {
                    // Already printed --> do nothing.
                    return true;
                }
            }

            if (addIfNot)
            {
                alreadyPrintedOperations.Add(operation.OperationNumber);
                File.WriteAllLines(fileName, alreadyPrintedOperations.ToArray());
            }

            return false;
        }

        #endregion

        #region IUIJob Members

        bool IUIJob.Initialize()
        {
            _configuration = Configuration.Load();
            return true;
        }

        void IUIJob.OnNewOperation(IOperationViewer operationViewer, Operation operation)
        {
            // Only print if we don't have already (verrrrrrrrrrrry helpful during debugging, but also a sanity-check)
            if (CheckIsOperationAlreadyPrinted(operation, true))
            {
                return;
            }

            ThreadPool.QueueUserWorkItem(o =>
            {
                // We need to wait for a bit to let the UI "catch a breath".
                // Otherwise, if printing immediately, it may have side-effects that parts of the visual aren't visible (bindings not updated etc.).
                Thread.Sleep(_configuration.WaitInterval);

                // After waiting, invoke the action on the UI thread
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {

                    PrintDialog dialog = new PrintDialog();
                    dialog.PrintQueue = _printQueue.Value;
                    dialog.PrintTicket = dialog.PrintQueue.DefaultPrintTicket;
                    dialog.PrintTicket.PageOrientation = PageOrientation.Landscape;
                    dialog.PrintTicket.CopyCount = _configuration.CopyCount;

                    dialog.PrintVisual(operationViewer.Visual, "New alarm " + operation.OperationNumber);
                }));
            });
        }

        #endregion
    }
}