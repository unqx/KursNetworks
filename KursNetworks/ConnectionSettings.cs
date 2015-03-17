using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace KursNetworks
{
    public partial class ConnectionSettings : Form
    {
        public ConnectionSettings()
        {
            InitializeComponent();
        }

        private void ConnectionSettings_Load(object sender, EventArgs e)
        {
            if(!PhysLayer.IsOpen())
            {
                //Получаем порты 
                foreach (string port in PhysLayer.scanPorts())
                {
                    PortBox.Items.Add(port);
                }

                // Дефолтные значения параметров
                SpeedBox.SelectedIndex = 4;
                BitBox.SelectedIndex = 4;
                StopBitBox.SelectedIndex = 1;
                EvenBox.SelectedIndex = 1;

                // Выключаем до выбора ком-порта
                
            } 

            else 
            {
                SpeedBox.Enabled = false;
                BitBox.Enabled = false;
                StopBitBox.Enabled = false;
                EvenBox.Enabled = false;
                PortBox.Enabled = false;

                SpeedBox.Text = PhysLayer.GetSpeed();
                BitBox.Text = PhysLayer.GetDataBits();
            }

            button1.Enabled = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Переменные под открытие соединения
            string name = PortBox.Text;
            int rate = Convert.ToInt32(SpeedBox.Text);
            int dataBits = Convert.ToInt32(BitBox.Text);

            // Выбор значения стоп-битов
            StopBits S = new StopBits();
            switch ( StopBitBox.SelectedIndex )
            {
                case 0: {
                    S = StopBits.One;
                    break;
                }
                case 1: {
                    S = StopBits.OnePointFive;
                    break;
                }
                case 2: {
                    S = StopBits.Two;
                    break;
                }
            }

            // Выбор значения четности
            Parity P = new Parity();
            switch (EvenBox.SelectedIndex)
            {
                case 0:
                    {
                        P = Parity.None;
                        break;
                    }
                case 1:
                    {
                        P = Parity.Even;
                        break;
                    }
                case 2:
                    {
                        P = Parity.Odd;
                        break;
                    }
            }

            PhysLayer.EstablishConnection(name, rate, dataBits, S, P);

            if(PhysLayer.IsOpen())
            {
                button1.Enabled = false;
            }

        }

        private void PortBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (PortBox.Text != "")
                button1.Enabled = true;
            else
                button1.Enabled = false;
        }
    }
}
