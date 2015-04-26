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

        private void DisableAllBoxes()
        {
            SpeedBox.Enabled = false;
            BitBox.Enabled = false;
            StopBitBox.Enabled = false;
            EvenBox.Enabled = false;
            PortBox.Enabled = false;
        }

        private void EnableAllBoxes()
        {
            SpeedBox.Enabled = true;
            BitBox.Enabled = true;
            StopBitBox.Enabled = true;
            EvenBox.Enabled = true;
            PortBox.Enabled = true;
        }

        private void ConnectionSettings_Load(object sender, EventArgs e)
        {
            // Выключаем до выбора ком-порта
            button1.Enabled = false;
            button2.Enabled = false;

            //Сканим порты
            foreach (string port in PhysLayer.scanPorts())
            {
                PortBox.Items.Add(port);
            }

          
            if(!PhysLayer.IsOpen())
            {
                 // Дефолтные значения параметров
                SpeedBox.SelectedIndex = 4;
                BitBox.SelectedIndex = 3;
                StopBitBox.SelectedIndex = 0;
                EvenBox.SelectedIndex = 0;
            } 

            else 
            {
                DisableAllBoxes();
                button2.Enabled = true;

                SpeedBox.Text = PhysLayer.GetSpeed();
                BitBox.Text = PhysLayer.GetDataBits();
                PortBox.Text = PhysLayer.GetPortName();
                StopBitBox.Text = PhysLayer.GetStopBits();
                EvenBox.Text = PhysLayer.GetParity();
            }
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

            PhysLayer.OpenPort(name, rate, dataBits, S, P);

            if(PhysLayer.IsOpen())
            {
                button1.Enabled = false;
                DisableAllBoxes();
                button2.Enabled = true;
            }

        }

        private void PortBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if(!PhysLayer.IsOpen())
            {
                if (PortBox.Text != "")
                    button1.Enabled = true;
                else
                    button1.Enabled = false;
            }
         }

        private void button2_Click(object sender, EventArgs e)
        {
            PhysLayer.DropConnection();
            if(!PhysLayer.IsOpen())
            {
                EnableAllBoxes();
                button1.Enabled = true;
                button2.Enabled = false;
            }
        }


    }
}
