using s2industries.ZUGFeRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZUGFeRD_Biller
{
    public class EntryPoint : Biller.UI.Interface.IPlugIn
    {
        private Manager.ControlManager controlManager;

        public EntryPoint(Biller.UI.ViewModel.MainWindowViewModel parentViewModel)
        {
            this.ParentViewModel = parentViewModel;
        }

        public Biller.UI.ViewModel.MainWindowViewModel ParentViewModel { get; private set; }

        public string Name
        {
            get { return "ZUGFeRD @ Biller"; }
        }

        public string Description
        {
            get { return ""; }
        }

        public double Version
        {
            get { return 1.20140903; }
        }

        public void Activate()
        {
            if (controlManager == null)
            {
                controlManager = new Manager.ControlManager(ParentViewModel);
            }
        }

        public List<Biller.UI.Interface.IViewModel> ViewModels()
        {
            return new List<Biller.UI.Interface.IViewModel>();
        }
    }
}
