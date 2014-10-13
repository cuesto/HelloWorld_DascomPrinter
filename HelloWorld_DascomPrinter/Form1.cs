using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;
using HelloWorld_DascomPrinter.Properties;
using IFRBDASCOM;
using TfhkaNet.IF.DO;


namespace HelloWorld_DascomPrinter
{
    public partial class Form1 : Form
    {
        private IFRB _iFrb;
        private Tfhka _tfhka;
        private string result;
        private int ok;
        private float puerto, lbaudios;
        string[] precio = new string[2] { "00000000", "00" };
        string[] cantidad = new string[2] { "00000", "000" };
        private string[] desc = new string[2] { "00", "00" };
        // Dim precio As String() = New String() {ENTERO_PRECIO, DECIMAL_PRECIO}
        //Dim cantidad As String() = New String() {ENTERO_CANTIDAD, DECIMAL_CANTIDAD}
        //Dim desc As String() = New String() {ENTERO_DESCUENTO, DECIMAL_DESCUENTO}

        public Form1()
        {
            InitializeComponent();
            _iFrb = new IFRB();
            _tfhka = new Tfhka();
            puerto = 1;
            lbaudios = 9600;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            short result = _iFrb.abrirpuerto(puerto, lbaudios);
            if (true)
            {
                //MessageBox.Show("Se conectó correctamente");
                //_iFrb.cmd0e02(puerto, lbaudios, "Prueba texto", "800");
                //_iFrb.cmd0e06(puerto, lbaudios);
                short tipo = 0;
                string ncf = "1234567890123456789";
                string rs = "sustantivo";
                string rnc = "40220097683";
                string vacio = "";
                float v = 0;
                //_iFrb.cmd0a07(puerto, lbaudios);
                string _result = _iFrb.cmd0a01(ref tipo, ref vacio, ref vacio, ref vacio, ref vacio, ref ncf, rs, ref rnc, ref vacio,
                    ref v, ref v);
            }
            else
            {
                MessageBox.Show("No se pudo abrir el puerto");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            result = _iFrb.cmd0a02("0", "Cafe", "", "", "", "", "", "", "", "", "", 1, 60, 1, "111111", "unidad", "111111");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            result = _iFrb.cmd0A05(1, 2000);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            result = _iFrb.cmd0A06("descri 1", "descri 2", "descri 3");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            short _a = _iFrb.abrirpuerto(puerto, lbaudios);
            if (true)
            {
                MessageBox.Show(Resources.Form1_button5_Click_Se_conectó_correctamente);
                result = _iFrb.cmd0a07();
            }
            else
            {
                result = _iFrb.cmd0a07();
                MessageBox.Show(Resources.Form1_button5_Click_No_se_pudo_abrir_el_puerto);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                TfhkaNet.IF.DO.S1PrinterData S1;
                S2PrinterData S2;
                S3PrinterData S3;
                bool a;

                if (_tfhka.StatusPort)
                    _tfhka.CloseFpCtrl();

                if (_tfhka.OpenFpCtrl("COM1", 9600))
                {
                    //S1 = _tfhka.GetS1PrinterData();
                    S2 = _tfhka.GetS2PrinterData();
                    //S3 = _tfhka.GetS3PrinterData();

                    if (S2.TypeDocument != 0)
                        _tfhka.SendCmd("7");

                    string ncf = "1234512345123455432"; // - ("F" + NCF)
                    string ncfa; //NCFAfectado - ("iF0" + NCFA)
                    string rnc = "40220097683"; //RNC - ("iR0" + RNC)
                    string rs = "stark industries"; //RS - ("iS0" + RS)

                    _tfhka.SendCmd("F" + ncf);
                    _tfhka.SendCmd("iR0" + rnc);
                    _tfhka.SendCmd("iS0" + rs);
                    a = _tfhka.SendCmd("/0");
                }
            }
            catch (Exception es)
            {
                string err = es.Message;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            short result = _iFrb.abrirpuerto(puerto, lbaudios);
            string a = _iFrb.cmd0801();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (_tfhka.StatusPort)
                _tfhka.CloseFpCtrl();

            if (_tfhka.OpenFpCtrl("COM1", 9600))
            {
                double _precio = 300;
                int _cantidad = 1;
                double _descuento = 0;
                string descripcion = "Iron Suit Mark 3";
                string item;
                string code = "A0001";

                //format Item Line
                precio[0] = string.Format("{0:00000000.00}", _precio);
                precio = precio[0].Split('.');
                //00000010 + 00

                cantidad[0] = string.Format("{0:000000.00}", _cantidad);
                cantidad = cantidad[0].Split('.');
                // 000003 + 00

                desc[0] = string.Format("{0:00.00}", _descuento);
                desc = desc[0].Split('.');
                //!000000080000005000|codigo01|Pendrive
                //!000000605000000300|A0001|Iron Suit Mark 3
                //!000000080000005000
                //!000000100000000300Fe Suit
                item = "!";
                item += precio[0] + precio[1] + cantidad[0] + cantidad[1] + "|" + code + "||" + descripcion;
                bool a = _tfhka.SendCmd(item);
                string descItem = "p-" + desc[0] + desc[1];
                //a = _tfhka.SendCmd(descItem);

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (_tfhka.StatusPort)
                _tfhka.CloseFpCtrl();

            if (_tfhka.OpenFpCtrl("COM1", 9600))
            {
                double _pago = 20;
                string pagoParcial;
                bool isOk;
                //bool od = _tfhka.SendCmd("101");
                //tipo de pago parcial
                string[] pago = new string[2] { "0000000000", "00" };

                pago[0] = string.Format("{0:0000000000.00}", _pago);
                pago = pago[0].Split('.');

                pagoParcial = "2" + "01" + pago[0] + pago[1];
                isOk = _tfhka.SendCmd(pagoParcial);
                pagoParcial = "2" + "02" + pago[0] + pago[1];
                isOk = _tfhka.SendCmd(pagoParcial);
                pagoParcial = "2" + "03" + pago[0] + pago[1];
                isOk = _tfhka.SendCmd(pagoParcial);
                pagoParcial = "2" + "04" + pago[0] + pago[1];
                isOk = _tfhka.SendCmd(pagoParcial);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (_tfhka.StatusPort)
                _tfhka.CloseFpCtrl();

            if (_tfhka.OpenFpCtrl("COM1", 9600))
            {
                bool od = _tfhka.SendCmd("199");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (_tfhka.StatusPort)
                _tfhka.CloseFpCtrl();

            if (_tfhka.OpenFpCtrl("COM1", 9600))
            {
                _tfhka.SendCmd("7");
                _tfhka.CloseFpCtrl();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (_tfhka.StatusPort)
                _tfhka.CloseFpCtrl();

            if (_tfhka.OpenFpCtrl("COM1", 9600))
            {
                _tfhka.PrintZReport();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (_tfhka.StatusPort)
                _tfhka.CloseFpCtrl();

            if (_tfhka.OpenFpCtrl("COM1", 9600))
            {
                double _desc = 10.45;
                string descuento;

                string[] desc = new string[2] { "000000", "00" };

                desc[0] = string.Format("{0:000000.00}", _desc);
                desc = desc[0].Split('.');

                descuento = "q*" + desc[0] + desc[1];

                bool od = _tfhka.SendCmd(descuento);
            }
        }
    }
}
