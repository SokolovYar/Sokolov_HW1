using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows.Forms;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection conn = null;
        string cs = "";
        DataTable table = null;
        SqlDataReader reader = null;
        SqlCommand cmd = null;
        string text = null;


        public MainWindow()
        {
            InitializeComponent();

            conn = new SqlConnection();
            cs = @" Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = Warehouse; Integrated Security = SSPI;";
            conn.ConnectionString = cs;
            conn.Open();

            //Задание 2
            commandList.Items.Add("SELECT * FROM Products");
            commandList.Items.Add("SELECT* FROM ProductTypes");
            commandList.Items.Add("SELECT* FROM Suppliers");
            commandList.Items.Add("SELECT TOP 1 * FROM Products\r\nORDER BY Quantity DESC");
            commandList.Items.Add("SELECT TOP 1 * FROM Products\r\nORDER BY Quantity ASC");
            commandList.Items.Add("SELECT TOP 1 * FROM Products\r\nORDER BY Cost DESC");
            commandList.Items.Add("SELECT TOP 1 * FROM Products\r\nORDER BY Cost ASC");

            //Задание 3
            commandList.Items.Add("SELECT* FROM Products\r\nWHERE Products.ProductType = 1");
            commandList.Items.Add("SELECT* FROM Products\r\nWHERE Products.SupplierID = 1");
            commandList.Items.Add("SELECT * FROM Products\r\nORDER BY Products.DeliveryDate ASC");
            commandList.Items.Add("SELECT ProductTypes.TypeName, AVG(Products.Quantity) AS AVEREAGE_AMOUNT FROM Products\r\nJOIN ProductTypes ON ProductTypes.ProductTypeID = Products.ProductID\r\nGROUP BY ProductTypes.ProductTypeID, ProductTypes.TypeName");

            //Задание 4
            commandList.Items.Add("SELECT TOP 1 Suppliers.SupplierName, SUM(Products.Quantity) FROM Products\r\nJOIN Suppliers ON Suppliers.SupplierID = Products.SupplierID\r\nGROUP BY Suppliers.SupplierID, Suppliers.SupplierName");
            commandList.Items.Add("SELECT TOP 1 Suppliers.SupplierName, SUM(Products.Quantity) FROM Products\r\nJOIN Suppliers ON Suppliers.SupplierID = Products.SupplierID\r\nGROUP BY Suppliers.SupplierID, Suppliers.SupplierName\r\nORDER BY SUM(Products.Quantity) ASC");
            commandList.Items.Add("SELECT TOP 1 Products.ProductName, SUM(Products.Quantity) FROM Products\r\nJOIN ProductTypes ON ProductTypes.ProductTypeID = Products.ProductType\r\nGROUP BY ProductTypes.ProductTypeID,  Products.ProductName\r\nORDER BY SUM(Products.Quantity) DESC");
            commandList.Items.Add("SELECT TOP 1 Products.ProductName, SUM(Products.Quantity) FROM Products\r\nJOIN ProductTypes ON ProductTypes.ProductTypeID = Products.ProductType\r\nGROUP BY ProductTypes.ProductTypeID,  Products.ProductName\r\nORDER BY SUM(Products.Quantity) ASC");


            //Чтобы запустить следующие необходимо скопировать запрос в ТекстБокс. 
            /*
                INSERT INTO Products (ProductID, ProductName, SupplierID, ProductType, Quantity, Cost, DeliveryDate)
                VALUES (15, N'Новый товар', 1 , 1, 1, 9.99, GETDATE());

                INSERT INTO Suppliers (SupplierID, SupplierName)
                VALUES(4, N'ЗАО "Хлебокомбинат 1"');
                
                INSERT INTO ProductTypes(ProductTypeID,	TypeName)
                VALUES(4, N'Игрушки'); 
             * */

        }

        private void Ex_button_Click(object sender, RoutedEventArgs e)
        {

 
            try
            {
                if (reader != null) reader.Close();

                //выбор команды из списка или из текстБокса
                if (commandList.SelectedItem != null && user_Command.Text == "") text = commandList.SelectedItem.ToString();
                else text = user_Command.Text;

                cmd = new SqlCommand(text, conn);
                table = new DataTable();
                reader = cmd.ExecuteReader();
                int line = 0;
                do
                {
                    while (reader.Read())
                    {
                        if (line == 0)
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                table.Columns.Add(reader.GetName(i));
                            }
                            line++;
                        }
                        DataRow row = table.NewRow();
                        for (int i = 0; i <
                        reader.FieldCount; i++)
                        {
                            row[i] = reader[i];
                        }
                        table.Rows.Add(row);
                    }
                } while (reader.NextResult());

                DataView Source = new DataView(table);
                dataGrid.Items.Refresh();
                dataGrid.ItemsSource = Source;

            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message);
            }

        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void commandList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            user_Command.Text = "";
        }
    }
}