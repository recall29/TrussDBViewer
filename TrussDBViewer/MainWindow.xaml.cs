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
using System.Windows.Navigation;
using System.Windows.Shapes;
//using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Data.SQLite;
using System.Data;
using System.Configuration;
using System.Configuration.Assemblies;
using System.ComponentModel.DataAnnotations.Schema;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;

namespace TrussDBViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        public static DataAccess objDataAccess = new DataAccess();
        public string strConn = ConfigurationManager.ConnectionStrings["DBConnString"].ToString();
  
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;  
        }                 

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            BindTrussRecipes();
            //Controller robController;
            
            //RobotController controller = new RobotController();
            //List<ControllerInfo> foundControllers = controller.ControllerScanner();

            //robController = ControllerFactory.CreateFrom(foundControllers[0]);

            //ABB.Robotics.Controllers.RapidDomain.RobTarget rtRetrieved = controller.GetRobotTarget(robController);
            //List<ABB.Robotics.Controllers.RapidDomain.RobTarget> rtRobotTargets = controller.GetRobotTargets(robController);

        }       

        public void BindTrussRecipes()
        {            
            var con = new SQLiteConnection(strConn);
            DataTable dtTruss = new DataTable();
            dtTruss = objDataAccess.GetTrussRecipe();
            if(dtTruss.Rows.Count > 0)
            {
                dgTruss.ItemsSource = dtTruss.AsDataView();
            }
            con.Close();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            //Get the index of the currently selected row
            var index = dgTruss.SelectedIndex;
            var trussid = dgTruss.Columns[0].GetCellContent(dgTruss.Items[index]) as TextBlock;
            
            string txtID = trussid.Text;
            int delID = Convert.ToInt32(txtID);
            objDataAccess.DeleteTrussRecipe(delID);

            //Rebind to see changes
            BindTrussRecipes();
        }
       
        private void insertButton_Click(object sender, RoutedEventArgs e)
        {
            List<Object> lstValues = new List<object>();

            if(dgTruss.SelectedItems != null && dgTruss.SelectedItems.Count > 0)
            {
                DataRowView currentRow = (DataRowView)dgTruss.SelectedItems[0];
                //foreach(DataRowView row in dgTruss.SelectedItems[0])
                //{

                //}
                lstValues.Add(Convert.ToInt32(currentRow["TrussId"]));    
                lstValues.Add(Convert.ToInt32(currentRow["Placement"]));
                lstValues.Add(Convert.ToInt32(currentRow["DimX"]));
                lstValues.Add(Convert.ToInt32(currentRow["DimY"]));
                lstValues.Add(Convert.ToInt32(currentRow["DimZ"]));

                objDataAccess.InsertTrussWoodRecipe(lstValues);
            }
            BindTrussRecipes();
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            var con = new SQLiteConnection(strConn);

            try
            {
                con.Open();
                SQLiteDataAdapter dAD = new SQLiteDataAdapter();
                string updateString = $"Update NailPlate Set npx = 600 where NPID = 1";
                dAD.UpdateCommand = new SQLiteCommand(updateString, con);
                dAD.UpdateCommand.ExecuteNonQuery();

                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgNailPlate_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            //Refresh the dg rows
            dgNailPlate.Items.Refresh();
        }
    }
}
