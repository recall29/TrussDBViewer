using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using System.Data.SQLite;
using System.Windows.Data;
using System.Windows;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;

namespace TrussDBViewer
{
    public class DataAccess
    {
        //TESTING THE GITHUB REPO COMMIT WITH A COMMENT
        string strSqliteConn = ConfigurationManager.ConnectionStrings["DBConnString"].ToString();
        //SQLiteConnection sqlConn = new SQLiteConnection();

        //How many sql functions would we need? Look into Entity Framework, might be a better way to do this. 

        public void InsertNailPlateRecipe(List<Object> lstVals)
        {
            string strSQL = $"Insert into NailPlate (npX, npY, npSize) values({lstVals[0]}, {lstVals[1]}, {lstVals[2]} )";
            var sqlConn = new SQLiteConnection(strSqliteConn);
            sqlConn.Open();

            SQLiteDataAdapter inAdap = new SQLiteDataAdapter();
            inAdap.InsertCommand = new SQLiteCommand(strSQL, sqlConn);
            inAdap.InsertCommand.ExecuteNonQuery();

            sqlConn.Close();
        }
       
        public void InsertTrussWoodRecipe(List<object> lstVals)
        {
            string strSQL = $"Insert into TrussWood_LU (TrussID, Placement, DimX, DimY, DimZ) Values({lstVals[0]}, {lstVals[1]}, {lstVals[2]}, {lstVals[3]}, {lstVals[4]})";
            var sqlConn = new SQLiteConnection(strSqliteConn);
            sqlConn.Open();

            SQLiteDataAdapter insertAdap = new SQLiteDataAdapter();
            insertAdap.InsertCommand = new SQLiteCommand(strSQL, sqlConn);
            insertAdap.InsertCommand.ExecuteNonQuery();

            sqlConn.Close();
        }

        public void UpdateTrussWoodRecipe(List<Object> lstUpdateVals, int intWoodID)
        {
            string strSQL = $"Update TrussWood_LU Set Placement, DimX = {lstUpdateVals[0]}, DimY = {lstUpdateVals[1]}, DimZ = {lstUpdateVals[2]} " +
                $"Where WoodID = {intWoodID} ";
            var sqlConn = new SQLiteConnection(strSqliteConn);
            sqlConn.Open();

            SQLiteDataAdapter updateAdap = new SQLiteDataAdapter();
            updateAdap.UpdateCommand = new SQLiteCommand(strSQL, sqlConn);
            updateAdap.InsertCommand.ExecuteNonQuery();

            sqlConn.Close();
        }

        public void DeleteTrussRecipe(int intDelID)
        {
            string strSQL = $"Delete from TrussWood_LU Where TrussID = {intDelID}";
            var sqlConn = new SQLiteConnection(strSqliteConn);
            try
            {
                sqlConn.Open();

                SQLiteDataAdapter delAdap = new SQLiteDataAdapter();
                delAdap.DeleteCommand = new SQLiteCommand(strSQL, sqlConn);
                delAdap.DeleteCommand.ExecuteNonQuery();

                sqlConn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public DataTable GetTrussRecipe()
        {
            string strSQL = "SELECT trs.trussid, trs.trusstype, twlu.Placement, tplu.PlacementType, twlu.Dimx, twlu.dimy, twlu.dimz " +
                    "from TrussRecipes as trs left join TrussWood_LU twlu on twlu.trussid = trs.trussid " +
                    "left join TrussPlacement_LU tplu on twlu.placement = tplu.placementid";

            var sqlConn = new SQLiteConnection(strSqliteConn);
            try
            {
                sqlConn.Open();
                //SQLiteCommand sqlCmd = new SQLiteCommand(strSQL, sqlConn);
                DataTable dtTruss = new DataTable();

                using (SQLiteDataAdapter dAdap = new SQLiteDataAdapter(strSQL, strSqliteConn))
                {
                    dAdap.Fill(dtTruss);
                }
                sqlConn.Close();
                return dtTruss;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
