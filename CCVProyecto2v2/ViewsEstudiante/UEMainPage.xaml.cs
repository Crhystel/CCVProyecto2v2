using CCVProyecto2v2.DataAccess;
using CCVProyecto2v2.ViewsModels;

namespace CCVProyecto2v2.ViewsAdmin
{
    public partial class UEMainPage : ContentPage
    {
        public UEMainPage()
        {
            InitializeComponent();
            BindingContext = new UEMainViewModel(new DbbContext());
        }
    }
}
