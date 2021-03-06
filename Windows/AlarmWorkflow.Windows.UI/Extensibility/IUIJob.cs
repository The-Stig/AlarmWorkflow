﻿using AlarmWorkflow.Shared.Core;

namespace AlarmWorkflow.Windows.UI.Extensibility
{
    /// <summary>
    /// Defines a means for a job that runs on UI-side after a new <see cref="Operation"/> has come in.
    /// This is the UI-side equivalent to the "IJob"-interface of the AlarmWorkflowEngine.
    /// </summary>
    public interface IUIJob
    {
        /// <summary>
        /// Initializes this job prior to its first use.
        /// </summary>
        /// <returns>The result of the initialization. Jobs that return false won't be called.</returns>
        bool Initialize();

        /// <summary>
        /// Runs the job-logic on the new operation.
        /// </summary>
        /// <param name="operationViewer">The instance of the <see cref="IOperationViewer"/> on which this job runs.</param>
        /// <param name="operation">The operation that has come in.</param>
        void OnNewOperation(IOperationViewer operationViewer, Operation operation);
    }
}
