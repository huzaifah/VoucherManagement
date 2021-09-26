using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using WPF.Sample.DataLayer.EntityClasses;
using WPF.Sample.ViewModelLayer;

namespace WPF.Sample.UserControls
{
    /// <summary>
    /// Interaction logic for VoucherListControl.xaml
    /// </summary>
    public partial class VoucherListControl : UserControl
    {
        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;

        public VoucherListControl()
        {
            InitializeComponent();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Set selected item
            _viewModel.Entity = (VoucherMaster)((Button)sender).Tag;

            DeleteVoucher();
        }

        public void DeleteVoucher()
        {
            // Ask if the user wants to delete this user  
            if (MessageBox.Show($"Delete Voucher {_viewModel.Entity.VoucherNo}?", "Delete?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _viewModel.Delete();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Set selected item
            _viewModel.Entity = (VoucherMaster)((Button)sender).Tag;

            // Go into Edit mode  
            _viewModel.BeginEdit(false);
        }

        private VoucherMaintenanceViewModel _viewModel;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = (VoucherMaintenanceViewModel)this.DataContext;
        }

        private void GenerateVoucherButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Entity = (VoucherMaster)((Button)sender).Tag;
            _viewModel.GenerateInvoice();
        }

        private void lvVoucherColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lvVouchers.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lvVouchers.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }
    }

    public class SortAdorner : Adorner
    {
        private static Geometry ascGeometry =
            Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");

        private static Geometry descGeometry =
            Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

        public ListSortDirection Direction { get; private set; }

        public SortAdorner(UIElement element, ListSortDirection dir)
            : base(element)
        {
            this.Direction = dir;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (AdornedElement.RenderSize.Width < 20)
                return;

            TranslateTransform transform = new TranslateTransform
            (
                AdornedElement.RenderSize.Width - 15,
                (AdornedElement.RenderSize.Height - 5) / 2
            );
            drawingContext.PushTransform(transform);

            Geometry geometry = ascGeometry;
            if (this.Direction == ListSortDirection.Descending)
                geometry = descGeometry;
            drawingContext.DrawGeometry(Brushes.Black, null, geometry);

            drawingContext.Pop();
        }
    }
}
