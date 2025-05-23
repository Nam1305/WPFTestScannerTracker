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
using System.Data.SQLite;
namespace ScannerTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string dbPath = @"D:\test.db";
        private SQLiteConnection connection;
        private int selectedBoxID = -1;
        public MainWindow()
        {
            InitializeComponent();
            InitializeDatabase();
            LoadBoxes();
        }

        private void InitializeDatabase()
        {
            connection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
            connection.Open();

            // Bật hỗ trợ khóa ngoại
            using (var cmd = new SQLiteCommand("PRAGMA foreign_keys = ON;", connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void LoadBoxes()
        {
            var boxes = new List<object>();
            using (var cmd = new SQLiteCommand("SELECT * FROM Boxes", connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        boxes.Add(new
                        {
                            BoxID = reader.GetInt32(0),
                            ProductCode = reader.GetString(1),
                            RequiredTrayCount = reader.GetInt32(2),
                            QuantityPerBox = reader.GetInt32(3),
                            QRCodeContent = reader.GetString(4),
                            Status = reader.GetString(5)
                        });
                    }
                }
            }
            dgBoxes.ItemsSource = boxes;
        }
    }
}