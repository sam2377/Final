﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Final
{
    public partial class Form1 : Form
    {

        List<String> food = new List<string>();
        List<String> drink = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

		private void Form1_Load(object sender, EventArgs e) {

			checkedListBox1.Items.Clear();
			checkedListBox2.Items.Clear();
			comboBox1.Items.Clear();
			comboBox2.Items.Clear();
			comboBox1.Text = "";
			comboBox2.Text = "";
			String selectCommand1 = "SELECT * FROM Food";
			String selectCommand2 = "SELECT * FROM Drink";
			SqlConnection cn = new SqlConnection();
			cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
				"AttachDbFilename=|DataDirectory|Food.mdf;" +
				 "Integrated Security=True";
			cn.Open();
			DataSet ds1 = new DataSet();
			SqlDataAdapter daScore1 = new SqlDataAdapter(selectCommand1, cn);
			daScore1.Fill(ds1);
			DataSet ds2 = new DataSet();
			SqlDataAdapter daScore2 = new SqlDataAdapter(selectCommand2, cn);
			daScore2.Fill(ds2);

			dataGridView1.DataSource = ds1.Tables[0];
			dataGridView2.DataSource = ds2.Tables[0];

			DataTable dt1 = new DataTable();
			SqlDataAdapter da1 = new SqlDataAdapter("SELECT Times,id FROM Food WHERE Times > 0 ORDER BY Times DESC OFFSET 0 ROWS FETCH NEXT 5 ROWS ONLY", cn);
			da1.Fill(dt1);
			chart1.DataSource = dt1;
			chart1.Series["Frequency1"].XValueMember = "id";
			chart1.Series["Frequency1"].YValueMembers = "Times";

			DataTable dt2 = new DataTable();
			SqlDataAdapter da2 = new SqlDataAdapter("SELECT Times,id FROM Drink WHERE Times > 0 ORDER BY Times DESC OFFSET 0 ROWS FETCH NEXT 5 ROWS ONLY", cn);
			da2.Fill(dt2);
			chart2.DataSource = dt2;
			chart2.Series["Frequency2"].XValueMember = "id";
			chart2.Series["Frequency2"].YValueMembers = "Times";

			DataTable dt3 = new DataTable();
			SqlDataAdapter da3 = new SqlDataAdapter("SELECT DISTINCT toki,price FROM Record", cn);
			da3.Fill(dt3);
			chart3.DataSource = dt3;
			chart3.Series["kane"].XValueMember = "toki";
			chart3.Series["kane"].YValueMembers = "price";
			chart3.Series.ResumeUpdates();

			for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                checkedListBox1.Items.Add(dataGridView1[0, i].Value);
                comboBox1.Items.Add(dataGridView1[0, i].Value);
            }
            for (int i = 0; i < dataGridView2.RowCount - 1; i++)
            {
                checkedListBox2.Items.Add(dataGridView2[0, i].Value);
                comboBox2.Items.Add(dataGridView2[0, i].Value);
            }
            for(int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
            for(int i = 0; i < checkedListBox2.Items.Count; i++)
            {
                checkedListBox2.SetItemChecked(i, true);
            }

            cn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Equals("") && !textBox2.Text.Equals("") && textBox3.Text.Equals("F") || textBox3.Text.Equals("D"))
            {
                try
                {
                    int price = int.Parse(textBox2.Text);
                    MessageBox.Show("添加成功");

                    switch (textBox3.Text)
                    {
                        case "F":
                            try
                            {
                                SqlConnection cn = new SqlConnection();
                                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                                    "AttachDbFilename=|DataDirectory|Food.mdf;" +
                                    "Integrated Security=True";
                                cn.Open();
                                String sqlstr = "INSERT INTO Food(Id,Price) VALUES(@p1,@p2)";

                                SqlCommand cmd = new SqlCommand(sqlstr, cn);

                                cmd.Parameters.Add("@p1", SqlDbType.NVarChar).Value = textBox1.Text;
                                cmd.Parameters.Add("@p2", SqlDbType.Int).Value = price;

                                cmd.ExecuteNonQuery();
                                cn.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            Form1_Load(sender, e);
                            break;
                        case "D":
                            try
                            {
                                SqlConnection cn = new SqlConnection();
                                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                                    "AttachDbFilename=|DataDirectory|Food.mdf;" +
                                    "Integrated Security=True";
                                cn.Open();
                                String sqlstr = "INSERT INTO Drink(Id,Price) VALUES(@p1,@p2)";

                                SqlCommand cmd = new SqlCommand(sqlstr, cn);

                                cmd.Parameters.Add("@p1", SqlDbType.NVarChar).Value = textBox1.Text;
                                cmd.Parameters.Add("@p2", SqlDbType.Int).Value = price;
                                cmd.ExecuteNonQuery();
                                cn.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            Form1_Load(sender, e);
                            break;
                    }
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                }
                catch
                {
                    MessageBox.Show("請重新輸入");
                }
            }
            else
            {
                MessageBox.Show("請重新輸入");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            try{
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                "AttachDbFilename=|DataDirectory|Food.mdf;" +
                "Integrated Security=True";
                cn.Open();
                String sqlstr = "DELETE FROM Food WHERE Id = @p1";

                SqlCommand cmd = new SqlCommand(sqlstr, cn);

                cmd.Parameters.Add("@p1", SqlDbType.NVarChar).Value = comboBox1.Text;

                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            Form1_Load(sender, e);
			refreshFoodChart();
            MessageBox.Show("刪除成功");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                "AttachDbFilename=|DataDirectory|Food.mdf;" +
                "Integrated Security=True";
                cn.Open();
                String sqlstr = "DELETE FROM Drink WHERE Id = @p1";

                SqlCommand cmd = new SqlCommand(sqlstr, cn);

                cmd.Parameters.Add("@p1", SqlDbType.NVarChar).Value = comboBox2.Text;

                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Form1_Load(sender, e);
			refreshDrinkChart();
            MessageBox.Show("刪除成功");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            food.Clear();
            for(int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    food.Add(checkedListBox1.Items[i].ToString());
                }
            }
            if (food.Count > 0)
            {
                Random rnd = new Random(Guid.NewGuid().GetHashCode());
                int index = rnd.Next(0, food.Count);
                DialogResult dr = MessageBox.Show("你選到:" + food[index],"提示",MessageBoxButtons.OKCancel);
                if(dr == DialogResult.OK) {
                    int times = 0;
                    for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                    {
                        if (dataGridView1[0, i].Value.Equals(food[index]))
                        {
                            times = int.Parse(dataGridView1[2, i].Value.ToString());
                            times++;
                            break;
                        }
                    }
                    try
                    {
                        SqlConnection cn = new SqlConnection();
                        cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                        "AttachDbFilename=|DataDirectory|Food.mdf;" +
                        "Integrated Security=True";
                        cn.Open();
                        String sqlstr = "UPDATE Food SET times = @p1 WHERE Id = @p2";

                        SqlCommand cmd = new SqlCommand(sqlstr, cn);
                        cmd.Parameters.Add("@p1", SqlDbType.Int).Value = times;
                        cmd.Parameters.Add("@p2", SqlDbType.NVarChar).Value = food[index];

                        cmd.ExecuteNonQuery();

						record(food[index], 0);
                        cn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    Form1_Load(sender, e);
					refreshFoodChart();
                }
                else if(dr == DialogResult.Cancel){

                }
            }
            else
            {
                MessageBox.Show("請選多一點");
            }
            /*
            String str = "";
            for(int i = 0; i < food.Count; i++)
            {
                str += food.ElementAt(i);
                str += ",";
            }
            MessageBox.Show(str);
            */
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            drink.Clear();
            for (int i = 0; i < checkedListBox2.Items.Count; i++)
            {
                if (checkedListBox2.GetItemChecked(i))
                {
                    drink.Add(checkedListBox2.Items[i].ToString());
                }
            }
            if (drink.Count > 0)
            {
                Random rnd = new Random(Guid.NewGuid().GetHashCode());
                int index = rnd.Next(0, drink.Count);
                DialogResult dr = MessageBox.Show("你選到:" + drink[index], "提示", MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK)
                {
                    int times = 0;
                    for (int i = 0; i < dataGridView2.RowCount - 1; i++)
                    {
                        if (dataGridView2[0, i].Value.Equals(drink[index]))
                        {
                            times = int.Parse(dataGridView2[2, i].Value.ToString());
                            times++;
                            break;
                        }
                    }
                    try
                    {
                        SqlConnection cn = new SqlConnection();
                        cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                        "AttachDbFilename=|DataDirectory|Food.mdf;" +
                        "Integrated Security=True";
                        cn.Open();
                        String sqlstr = "UPDATE Drink SET times = @p1 WHERE Id = @p2";

                        SqlCommand cmd = new SqlCommand(sqlstr, cn);
                        cmd.Parameters.Add("@p1", SqlDbType.Int).Value = times;
                        cmd.Parameters.Add("@p2", SqlDbType.NVarChar).Value = drink[index];

                        cmd.ExecuteNonQuery();

						record(drink[index], 1);
						cn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    Form1_Load(sender, e);
					refreshDrinkChart();
				}
				else if (dr == DialogResult.Cancel)
                {

                }
            }
            else
            {
                MessageBox.Show("請選多一點");
            }
        }

		private void refreshFoodChart() {
			chart1.Series.ResumeUpdates();
		}

		private void refreshDrinkChart() {
			chart2.Series.ResumeUpdates();
		}


		private void record(String something, int which) {
			SqlConnection cn = new SqlConnection();
			cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
			"AttachDbFilename=|DataDirectory|Food.mdf;" +
			"Integrated Security=True";
			cn.Open();

			String sqlstr = null;
			int price = 0;

			if (which == 0) {
				sqlstr = "SELECT Price FROM Food WHERE id = @p1";
			}
			else if (which == 1) {
				sqlstr = "SELECT Price FROM Drink WHERE id = @p1";
			}

			SqlCommand cmd = new SqlCommand(sqlstr, cn);
			cmd.Parameters.Add("@p1", SqlDbType.NVarChar).Value = something;

			price = (int)cmd.ExecuteScalar();

			sqlstr = "INSERT INTO Record(Id,Price,toki) VALUES(@p1,@p2,@p3)";
			cmd = new SqlCommand(sqlstr, cn);

			cmd.Parameters.Add("@p1", SqlDbType.NVarChar).Value = something;
			cmd.Parameters.Add("@p2", SqlDbType.Int).Value = price;
			cmd.Parameters.Add("@p3", SqlDbType.Date).Value = DateTime.Today;

			cmd.ExecuteNonQuery();
			cn.Close();
		}
	}
}
