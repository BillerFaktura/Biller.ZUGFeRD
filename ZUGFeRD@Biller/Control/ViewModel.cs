using OrderTypes_Biller.Order;
using s2industries.ZUGFeRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZUGFeRD_Biller.Control
{
    public class ViewModel : Biller.Core.Utils.PropertyChangedHelper
    {
        public ViewModel(Biller.UI.DocumentView.Contextual.DocumentEditViewModel documentEditViewModel)
        {
            ParentViewModel = documentEditViewModel;
        }

        public Biller.UI.DocumentView.Contextual.DocumentEditViewModel ParentViewModel { get { return GetValue(() => ParentViewModel); } set { SetValue(value); } }

        public string FilePath { get { return GetValue(() => FilePath); } set { SetValue(value); } }

        public void SaveZUGFeRD()
        {
            var document = ParentViewModel.Document;
            if (document is OrderTypes_Biller.Invoice.Invoice)
            {
                var invoice = document as OrderTypes_Biller.Invoice.Invoice;
                InvoiceDescriptor desc = InvoiceDescriptor.CreateInvoice(invoice.DocumentID, invoice.Date, CurrencyCodes.EUR, "");
                desc.Profile = Profile.Comfort;
                desc.AddNote(invoice.OrderOpeningText);
                desc.AddNote(ReplaceDocumentPlaceHolder(invoice.OrderClosingText, invoice));
                var displayedName = "";
                var output = new List<string>();
                if (!String.IsNullOrEmpty(invoice.Customer.MainAddress.CompanyName))
                {
                    displayedName = (invoice.Customer.MainAddress.CompanyName);

                    var names = (((invoice.Customer.MainAddress.Salutation + " " + invoice.Customer.MainAddress.Title).Trim() + " " + invoice.Customer.MainAddress.Forename).Trim() + " " + invoice.Customer.MainAddress.Surname).Trim();
                    if (!String.IsNullOrEmpty(names))
                        displayedName += ", " + names;
                }
                else
                {
                    if (!String.IsNullOrEmpty(invoice.Customer.MainAddress.Salutation))
                        displayedName = invoice.Customer.MainAddress.Salutation;

                    var names = ((invoice.Customer.MainAddress.Title + " " + invoice.Customer.MainAddress.Forename.Trim()).Trim() + " " + invoice.Customer.MainAddress.Surname).Trim();
                    if (!String.IsNullOrEmpty(names))
                        displayedName += ", " + names;
                }
                desc.SetBuyer(displayedName, invoice.Customer.MainAddress.Zip, invoice.Customer.MainAddress.City, invoice.Customer.MainAddress.Street, invoice.Customer.MainAddress.HouseNumber, invoice.Customer.MainAddress.Country);
                desc.SetTradePaymentTerms(ReplaceDocumentPlaceHolder(invoice.PaymentMethode.Text, invoice), invoice.Date);
            }
        }

        private string ReplaceDocumentPlaceHolder(string placeholder, Order order)
        {
            if (placeholder.Contains("{DocumentDate}"))
            {
                Regex.Replace(placeholder, "[{]DocumentDate[}][{][+]+[0-9]{1,}[}]", delegate(Match match)
                {
                    string v = match.ToString();
                    v = v.Replace("{Date}", "").Replace("{", "").Replace("}", "");
                    return order.Date.AddDays(int.Parse(v)).ToString("dd.MM.yyyy");
                });
                Regex.Replace(placeholder, "[{]DocumentDate[}][{][-]+[0-9]{1,}[}]", delegate(Match match)
                {
                    string v = match.ToString();
                    v = v.Replace("{DocumentDate}", "").Replace("{", "").Replace("}", "");
                    return order.Date.AddDays(-int.Parse(v)).ToString("dd.MM.yyyy");
                });
            }
            if (placeholder.Contains("{DateOfDerlivery}"))
            {
                Regex.Replace(placeholder, "[{]DateOfDerlivery[}][{][+]+[0-9]{1,}[}]", delegate(Match match)
                {
                    string v = match.ToString();
                    v = v.Replace("{Date}", "").Replace("{", "").Replace("}", "");
                    return order.DateOfDelivery.AddDays(int.Parse(v)).ToString("dd.MM.yyyy");
                });
                Regex.Replace(placeholder, "[{]DateOfDerlivery[}][{][-]+[0-9]{1,}[}]", delegate(Match match)
                {
                    string v = match.ToString();
                    v = v.Replace("{DateOfDerlivery}", "").Replace("{", "").Replace("}", "");
                    return order.DateOfDelivery.AddDays(-int.Parse(v)).ToString("dd.MM.yyyy");
                });
            }
            if (placeholder == "{OrderValueGross}")
                return order.OrderCalculation.OrderSummary.ToString();
            if (placeholder == "{OrderValueNet}")
                return order.OrderCalculation.NetOrderSummary.ToString();
            if (placeholder == "{CashBackValue}")
                return order.OrderCalculation.CashBack.ToString();
            return placeholder;
        }
        
    }
}
