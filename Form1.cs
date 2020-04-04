using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Microsoft.VisualBasic.FileIO;

namespace Lab6
{
    public partial class Form1 : Form
    {
        private int step;
        private List<int> freaks = new List<int>(); //это чтото !!!!!!!!!!!!!
        private List<int> Zero = new List<int>();
        private List<List<int>> data = new List<List<int>>();

        private int size;
        private int cnt = 0;
        private int currentX = 0;
        private int maxSize;

        int P = 0;
        int L = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (TextFieldParser parser = new TextFieldParser(@"D:\Users\talkan\Desktop\Шото\Lab6\Lab6\Сигналы\6-76254.CSV"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                while (!parser.EndOfData)
                {
                    List<int> row = new List<int>();
                    string[] fields = parser.ReadFields();
                    foreach (string field in fields)
                    {
                        switch (cnt)
                        {
                            case 0:
                                break;
                            case 1:
                                step = Convert.ToInt32(field);
                                break;
                            case 2:
                            case 3:

                                freaks.Add(Convert.ToInt32(Math.Pow(2, Convert.ToInt32(field))));
                                break;
                            case 4:
                            case 5:

                                Zero.Add(Convert.ToInt32(field));
                                break;
                            case 6:
                            case 7:

                                freaks.Add(Convert.ToInt32(Math.Pow(2, Convert.ToInt32(field))));
                                break;
                            case 8:
                            case 9:
                                Zero.Add(Convert.ToInt32(field));
                                break;
                            case 10:
                                size = Convert.ToInt32(field);
                                currentX = size;
                                break;
                            case 11:
                                maxSize = Convert.ToInt32(field);
                                break;
                            default:
                                row.Add(Convert.ToInt32(field));
                                break;
                        }
                        if (cnt < 12)
                        {
                            break;
                        }
                    }
                    if (cnt > 11)
                    {
                        data.Add(row);
                    }
                    cnt++;
                }
            }

            chart1.ChartAreas[0].AxisX.ScaleView.Zoom(0, size);
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

            chart1.ChartAreas[0].AxisY.ScaleView.Zoom(0 - Zero[0], freaks[0]);
            chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;

            cnt = 0;
            for (currentX = 0; currentX < size; currentX += step)
            {
                for (int j = 0; j < 4; j++)
                {
                    chart1.Series[j].Points.AddXY(currentX, data[cnt][j] - Zero[j]);
                }
                cnt++;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            P = Convert.ToInt32(textBox2.Text);
            L = Convert.ToInt32(textBox1.Text);


            int[] sred = new int[] { 0, 0 ,0 ,0 };
            for (int i = L; i < P; i++)
            {
                for (int j = 0; j < 4; j++)
                    sred[j] += data[i][j] - Zero[j];
            }

            sred[0] = sred[0] / (P - L);
            sred[1] = sred[1] / (P - L);
            sred[2] = sred[2] / (P - L);
            sred[3] = sred[3] / (P - L);

            double[] sko = new double[] { 0, 0 , 0, 0};
            for (int i = L; i < P; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    sko[j] += Math.Pow((data[i][j] - Zero[j] - sred[j]), 2);
                }
            }
            sko[0] = Math.Sqrt(sko[0] / (P - L));
            sko[1] = Math.Sqrt(sko[1] / (P - L));
            sko[2] = Math.Sqrt(sko[2] / (P - L));
            sko[3] = Math.Sqrt(sko[3] / (P - L));

            label1.Text = "Эффективное значение шума для А: " + Convert.ToString(sko[0] * 0.03125) + "В";
            label4.Text = "Эффективное значение шума для В: " + Convert.ToString(sko[1] * 0.03125) + "В";
            label11.Text = "Эффективное значение шума для C: " + Convert.ToString(sko[2] * 0.03125) + "В";
            label15.Text = "Эффективное значение шума для D: " + Convert.ToString(sko[3] * 0.03125) + "В";


            int[] maxImpuls = new int[] { 0, 0, 0 ,0 };
            int[] minImpuls = new int[] { 1000, 1000, 1000 ,1000 };

            for (int i = L; i < P; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if ((data[i][j] - Zero[j]) > maxImpuls[j])
                    {
                        maxImpuls[j] = data[i][j] - Zero[j];
                    }
                    if ((data[i][j] - Zero[j]) < minImpuls[j])
                    {
                        minImpuls[j] = data[i][j] - Zero[j];
                    }
                }
            }

            label7.Text = "Минимум для А: " + Convert.ToString(minImpuls[0] * 0.03125) + "В";
            label2.Text = "Максимум для A: " + Convert.ToString(maxImpuls[0] * 0.03125) + "В";
            label8.Text = "Минимум для B: " + Convert.ToString(minImpuls[1] * 0.03125) + "В";
            label3.Text = "Максимум для B: " + Convert.ToString(maxImpuls[1] * 0.03125) + "В";
            label9.Text = "Минимум для C: " + Convert.ToString(minImpuls[2] * 0.03125) + "В";
            label12.Text = "Максимум для C: " + Convert.ToString(maxImpuls[2] * 0.03125) + "В";
            label13.Text = "Минимум для D: " + Convert.ToString(minImpuls[3] * 0.03125) + "В";
            label16.Text = "Максимум для D: " + Convert.ToString(maxImpuls[3] * 0.03125) + "В";
            label5.Text = "Размах: " + Convert.ToString((maxImpuls[0] - minImpuls[0]) * 0.03125) + "B";
            label6.Text = "Размах: " + Convert.ToString((maxImpuls[1] - minImpuls[1]) * 0.03125) + "B";
            label10.Text = "Размах: " + Convert.ToString((maxImpuls[2] - minImpuls[2]) * 0.03125) + "B";
            label14.Text = "Размах: " + Convert.ToString((maxImpuls[3] - minImpuls[3]) * 0.03125) + "B";
        }   
    }
}
