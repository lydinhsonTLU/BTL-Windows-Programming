using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Windows.Forms.DataVisualization.Charting;

namespace BTL_N6_ver3
{
    public partial class UserControl_tongquan: UserControl
    {
        public UserControl_tongquan()
        {
            InitializeComponent();
            LoadData();
        }

        private void UserControl_tongquan_Load(object sender, EventArgs e)
        {
            
        
        }

        private void UserControl_tongquan_Click(object sender, EventArgs e)
        {

        }


        private string connectionString = connectDb.connectString();
        private void LoadData()
        {
            try
            {
                // Lấy dữ liệu từ SQL Server
                DataTable dataTable = GetBookingData();

                // Tạo biểu đồ
                CreateColumnChart(dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable GetBookingData()
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Chỉ đếm số lượng phòng được đặt theo ngày
                string query = @"
                    SELECT 
                        Ngay_Den,
                        COUNT(DISTINCT Ma_Phong) AS SoLuongPhong
                    FROM 
                        DatPhongDonLe
                    GROUP BY 
                        Ngay_Den
                    ORDER BY 
                        Ngay_Den";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }

        private void CreateColumnChart(DataTable dataTable)
        {

            // Xóa tất cả series và ChartAreas hiện có
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

            // Tạo ChartArea mới
            ChartArea chartArea = new ChartArea("ChartArea1");
            chart1.ChartAreas.Add(chartArea);

            // Tạo series cho số lượng phòng
            Series seriesPhong = new Series("Số lượng phòng");
            seriesPhong.ChartType = SeriesChartType.Column;
            seriesPhong.Color = Color.FromArgb(255, 192, 192);

            // Thêm dữ liệu vào series
            foreach (DataRow row in dataTable.Rows)
            {
                string ngayDen = row["Ngay_Den"].ToString();
                int soLuongPhong = Convert.ToInt32(row["SoLuongPhong"]);

                seriesPhong.Points.AddXY(ngayDen, soLuongPhong);
            }

            // Thêm series vào chart
            chart1.Series.Add(seriesPhong);

            // Thiết lập tiêu đề và các thuộc tính khác
            chart1.Titles.Clear();
            chart1.Titles.Add(new Title("Thống kê số lượng phòng đặt theo ngày", Docking.Top, new Font("Arial", 14, FontStyle.Bold), Color.Black));

            // Cấu hình trục X
            chartArea.AxisX.Title = "";
            chartArea.AxisX.TitleFont = new Font("Arial", 12, FontStyle.Regular);
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.LabelStyle.Angle = -45;
            chartArea.AxisX.MajorGrid.LineColor = Color.White;

            // Cấu hình trục Y - chỉ hiển thị số nguyên dương
            chartArea.AxisY.Title = "";
            chartArea.AxisY.TitleFont = new Font("Arial", 12, FontStyle.Regular);
            chartArea.AxisY.MajorGrid.LineColor = Color.White;
            chartArea.AxisY.Minimum = 0;  // Đặt giá trị tối thiểu là 0
            chartArea.AxisY.Maximum = 24;
            chartArea.AxisY.IsStartedFromZero = true;
            chartArea.AxisY.IntervalType = DateTimeIntervalType.Number;  // Kiểu khoảng giá trị là số
            chartArea.AxisY.Interval = 2;  // Khoảng cách giữa các giá trị là 1 (số nguyên)

            // Hiển thị nhãn giá trị trên cột
            seriesPhong.IsValueShownAsLabel = true;
            seriesPhong.LabelFormat = "#,##0";  // Định dạng hiển thị số nguyên

            // Đặt chiều rộng cột
            seriesPhong["PointWidth"] = "0.3";
                
            // Cấu hình chú thích
            chart1.Legends.Clear();
            Legend legend = new Legend("Legend1");
            legend.Docking = Docking.Bottom;
            chart1.Legends.Add(legend);
        }
    }
}

