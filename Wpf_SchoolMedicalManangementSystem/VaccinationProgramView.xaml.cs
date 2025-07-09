using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Wpf_SchoolMedicalManangementSystem
{
    /// <summary>
    /// Interaction logic for VaccinationProgramView.xaml
    /// </summary>
    public partial class VaccinationProgramView : Window
    {
        public VaccinationProgramView()
        {
            InitializeComponent();
        }

        private void CreateProgram_Click(object sender, RoutedEventArgs e)
        {
            var form = new VaccinationProgramForm();
            form.ShowDialog();
        }
    }
}
