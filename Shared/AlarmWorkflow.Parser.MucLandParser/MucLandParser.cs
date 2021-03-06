using System;
using AlarmWorkflow.Shared.Core;
using AlarmWorkflow.Shared.Diagnostics;
using AlarmWorkflow.Shared.Extensibility;

namespace AlarmWorkflow.Parser.MucLandParser
{
    /// <summary>
    /// Description of MucLandParser.
    /// </summary>
    [Export("MucLandParser", typeof(IParser))]
    sealed class MucLandParser : IParser
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MucLandParser class.
        /// </summary>
        public MucLandParser()
        {
        }

        #endregion

        #region IParser Members

        Operation IParser.Parse(string[] lines)
        {
            Operation operation = new Operation();

            try
            {
                foreach (string line in lines)
                {
                    string msg;
                    string prefix;
                    int x = line.IndexOf(':');
                    if (x != -1)
                    {
                        prefix = line.Substring(0, x);
                        msg = line.Substring(x + 1).Trim();

                        prefix = prefix.Trim().ToUpperInvariant();
                        switch (prefix)
                        {
                            case "EINSATZNR":
                                operation.OperationNumber = msg;
                                break;
                            case "MITTEILER":
                                operation.Messenger = msg;
                                break;
                            case "EINSATZORT":
                                operation.Location = msg;
                                break;
                            case "STRA�E":
                            case "STRABE":
                                operation.Street = msg;
                                break;
                            case "KREUZUNG":
                                operation.CustomData["Kreuzung"] = msg;
                                break;
                            case "ORTSTEIL/ORT":
                                operation.City = msg;
                                break;
                            case "OBJEKT":
                            case "9BJEKT":
                                operation.Property = msg;
                                break;
                            case "MELDEBILD":
                                operation.CustomData["Meldebild"] = msg;
                                break;
                            case "HINWEIS":
                                operation.Comment = msg;
                                break;
                            case "EINSATZPLAN":
                                operation.CustomData["Einsatzplan"] = msg;
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.LogException(this, ex);
            }

            return operation;
        }

        #endregion

    }
}
