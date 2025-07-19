using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;


namespace BTL_N6_ver3
{
    public partial class frm_hoadon: Form
    {
        public frm_hoadon()
        {
            InitializeComponent();
        }


        //hàm tạo lấy thông tin từ UC thanh toán
        public void SetHoadon (string ngayTT, string cccd, string ten, string maP, string gia, string soNgayO)
        {
            lbl_ngayTT.Text = DateTime.Today.ToString("dd-MM-yyyy");
            lbl_maKH.Text = cccd;
            lblTenKH.Text = ten;
            lblPhong.Text = maP;
            lbl_Gia.Text = gia;
            lblSoNgayO.Text = soNgayO;

            int giaPhong = Convert.ToInt32(lbl_Gia.Text);
            int soNgayO_total = Convert.ToInt32(lblSoNgayO.Text);
           
            int tongtien = giaPhong * soNgayO_total;
            

            lbl_tongtien2.Text = lbl_Tongtien1.Text = Convert.ToString(tongtien);
            
        }

        private void frm_hoadon_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult r = MessageBox.Show("Quay về trang chủ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (r == DialogResult.No)
            {
                e.Cancel = true;   
            }   
        }
    }
}
