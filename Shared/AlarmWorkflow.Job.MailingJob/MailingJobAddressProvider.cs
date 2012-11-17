using System.Collections.Generic;
using System.Xml.Linq;
using AlarmWorkflow.Shared.Addressing;
using AlarmWorkflow.Shared.Core;

namespace AlarmWorkflow.Job.MailingJob
{
    [Export("MailingJobAddressProvider", typeof(IAddressProvider))]
    class MailingJobAddressProvider : IAddressProvider
    {
        #region IAddressProvider Members

        string IAddressProvider.AddressType
        {
            get { return "Mail"; }
        }

        object IAddressProvider.ParseXElement(XElement element)
        {
            string address = element.Value;
            string receiptType = element.TryGetAttributeValue("Type", MailingEntryObject.ReceiptType.To.ToString());

            return MailingEntryObject.FromAddress(address, receiptType);
        }

        XElement IAddressProvider.GetXmlData(object customData)
        {
            MailingEntryObject obj = (MailingEntryObject)customData;

            XElement element = new XElement("Mail");
            element.Add(new XAttribute("Type", obj.Type.ToString()));
            element.Add(obj.Address);
            return element;
        }

        #endregion

        #region IAddressProviderInfo Members

        IEnumerable<ProviderDataDetail> IAddressProviderInfo.GetDataDetails()
        {
            yield return new ProviderDataDetail() { Name = "Address" };
        }

        #endregion
    }
}
