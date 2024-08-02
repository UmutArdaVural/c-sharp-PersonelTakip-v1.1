using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace DersPersonel_v1._1
{
    public partial class Form1 : Form
    {   
        public void temizle()
        {
            txtid.Text = ""; txtad.Text = ""; txtsoyad.Text = ""; cmbsehirler.Text = ""; txtmaas.Text = ""; txtmeslek.Text = ""; rdbtbekar.Checked = false; rdbtevli.Checked = false;

        }

        public Form1()
        {
            InitializeComponent();
        }
       SqlConnection baglanti = new SqlConnection("Data Source=UMUTARDAVURAL\\SQLEXPRESS;Initial Catalog=PersonelVeriTabani;Integrated Security=True;TrustServerCertificate=True");

        private void Form1_Load(object sender, EventArgs e)
        {
           
            timer1.Start();
        }
        public int sayaç = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            Point yazınokta = new Point(); // Boş bir Point nesnesi oluşturuluyor.
            sayaç++;
            if (sayaç % 7 == 0)
            {
                yazınokta = new Point();
                yazınokta.Y = label8.Location.Y;
                yazınokta.Y = yazınokta.Y - 80;
                label8.Location = yazınokta; // label8'in konumu yeni Point nesnesine ayarlanıyor.
                sayaç = 0;
                yazınokta = new Point(510, 450); // Yeni bir Point nesnesi koordinatlarla oluşturuluyor.
                label8.Location = yazınokta; // label8'in konumu yeni Point nesnesine ayarlanıyor.
            }
            else
            {
                yazınokta = new Point();
                yazınokta.Y = label8.Location.Y;
                yazınokta.X = label8.Location.X;
                yazınokta.Y = yazınokta.Y - 100;
                label8.Location = yazınokta; // label8'in konumu yeni Point nesnesine ayarlanıyor.

            }
        }

        private void btnlistele_Click(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'personelVeriTabaniDataSet.Tbl_Per' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.tbl_PerTableAdapter.Fill(this.personelVeriTabaniDataSet.Tbl_Per);
        }

        private void btnkaydet_Click(object sender, EventArgs e)
        {
            bool evlimi = false;

            if (rdbtevli.Checked == true)
            {
                evlimi = true;
            }
            else
            {
                evlimi = false;
            }
            if (txtad.Text != "" && txtsoyad.Text != "" && cmbsehirler.Text != "" && txtmaas.Text != "" && txtmeslek.Text != "" && (evlimi ==true || evlimi== false) ) {
                try
                {
                    baglanti.Open();

                    SqlCommand sqlkomut = new SqlCommand("insert into Tbl_Per (PerAd, PerSoyad, PerSehir, PerMaas, PerMeslek,PerDurum) values (@p1, @p2, @p3, @p4, @p5,@p6)", baglanti);
                    sqlkomut.Parameters.AddWithValue("@p1", txtad.Text);
                    sqlkomut.Parameters.AddWithValue("@p2", txtsoyad.Text);
                    sqlkomut.Parameters.AddWithValue("@p3", cmbsehirler.Text);
                    sqlkomut.Parameters.AddWithValue("@p4", txtmaas.Text);
                    sqlkomut.Parameters.AddWithValue("@p5", txtmeslek.Text);
                    sqlkomut.Parameters.AddWithValue("@p6", evlimi);

                    sqlkomut.ExecuteNonQuery();

                    sqlkomut.Dispose(); // SqlCommand nesnesini serbest bırakma
                    MessageBox.Show("Başarı ile kaydedildi.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}");
                }
                finally
                {
                    if (baglanti.State == System.Data.ConnectionState.Open)
                    {
                        baglanti.Close();
                    }
                }
            }
            else {
                MessageBox.Show("hata her yeri doldurun");
            }

            temizle();
        }

        private void btntemizle_Click(object sender, EventArgs e)
        {
            temizle();

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            txtid.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            txtad.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            txtsoyad.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            cmbsehirler.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            txtmaas.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            txtmeslek.Text = dataGridView1.Rows[secilen].Cells[6].Value.ToString();

            string durum = dataGridView1.Rows[secilen].Cells[5].Value.ToString();
            if (durum == "True")
            {
                rdbtevli.Checked = true;

            }
            else { rdbtbekar.Checked = true;  }

        }

        private void btnsil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand sqlCommand = new SqlCommand("Delete from Tbl_Per Where Perid=@k1", baglanti );
            sqlCommand.Parameters.AddWithValue("@k1" ,txtid.Text);
            sqlCommand.ExecuteNonQuery();
            baglanti.Close();

            MessageBox.Show("Başarı ile silinmiştir ");
            temizle();
        }
    }
}
