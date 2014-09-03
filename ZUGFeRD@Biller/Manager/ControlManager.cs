using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZUGFeRD_Biller.Manager
{
    public class ControlManager : Biller.UI.Interface.IEditObserver
    {
        Biller.UI.ViewModel.MainWindowViewModel mainWindowViewModel;
        public ControlManager(Biller.UI.ViewModel.MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            mainWindowViewModel.DocumentTabViewModel.RegisterObserver(this);
        }

        public void ReceiveArticleEditViewModel(Biller.UI.ArticleView.Contextual.ArticleEditViewModel articleEditViewModel)
        {
            //
        }

        public void ReceiveCustomerEditViewModel(Biller.UI.CustomerView.Contextual.CustomerEditViewModel customerEditViewModel)
        {
            //
        }

        public void ReceiveDocumentEditViewModel(Biller.UI.DocumentView.Contextual.DocumentEditViewModel documentEditViewModel)
        {
            documentEditViewModel.EditContentTabs.Add(new Control.EditTab());
        }
    }
}
