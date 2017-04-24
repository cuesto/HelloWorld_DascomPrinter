using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;
using HelloWorld_DascomPrinter.Properties;
using TfhkaNet.IF.DO;


namespace HelloWorld_DascomPrinter
{
    public partial class Form1 : Form
    {
        private Tfhka _tfhka;
        private int lbaudios;
        private string puerto;
        string[] precio = new string[2] { "00000000", "00" };
        string[] cantidad = new string[2] { "00000", "000" };
        private string[] desc = new string[2] { "00", "00" };

        public Form1()
        {
            InitializeComponent();
            _tfhka = new Tfhka();
            puerto = "COM3";
            lbaudios = 9600;

            /* Prueba impresión automática */
            AbrirDocumentoTFHKA();
            AgregarArticuloRFHKA();
            AplicarPago();
            CerrarDocumentoTFHKA();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AbrirDocumentoTFHKA();
        }

        private void AbrirDocumentoTFHKA()
        {
            try
            {
                TfhkaNet.IF.DO.S1PrinterData S1;
                S2PrinterData S2;
                S3PrinterData S3;
                bool a;

                if (_tfhka.StatusPort)
                    _tfhka.CloseFpCtrl();

                if (_tfhka.OpenFpCtrl(puerto, lbaudios))
                {
                    S1 = _tfhka.GetS1PrinterData();
                    S2 = _tfhka.GetS2PrinterData();
                    S3 = _tfhka.GetS3PrinterData();

                    if (S2.TypeDocument != 0)
                        _tfhka.SendCmd("7");

                    string ncf = "1234512345123455432"; // - ("F" + NCF)
                    //string ncfa; //NCFAfectado - ("iF0" + NCFA)
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

        private void button8_Click(object sender, EventArgs e)
        {
            AgregarArticuloRFHKA();
        }

        private void AgregarArticuloRFHKA()
        {
            if (_tfhka.StatusPort)
                _tfhka.CloseFpCtrl();

            if (_tfhka.OpenFpCtrl(puerto, lbaudios))
            {
                double _precio = 100;
                int _cantidad = 1;
                double _descuento = 0;
                string descripcion = "Articulo de prueba";
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
                item = "\"";
                item += precio[0] + precio[1] + cantidad[0] + cantidad[1] + "|" + code + "||" + descripcion;
                bool a = _tfhka.SendCmd(item);
                string descItem = "p-" + desc[0] + desc[1];
                //a = _tfhka.SendCmd(descItem);

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AplicarPago();
        }

        private void AplicarPago()
        {
            if (_tfhka.StatusPort)
                _tfhka.CloseFpCtrl();

            if (_tfhka.OpenFpCtrl(puerto, lbaudios))
            {
                double _pago = 35;
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
            CerrarDocumentoTFHKA();
        }

        private void CerrarDocumentoTFHKA()
        {
            if (_tfhka.StatusPort)
                _tfhka.CloseFpCtrl();

            if (_tfhka.OpenFpCtrl(puerto, lbaudios))
            {
                bool od = _tfhka.SendCmd("199");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (_tfhka.StatusPort)
                _tfhka.CloseFpCtrl();

            if (_tfhka.OpenFpCtrl(puerto, lbaudios))
            {
                _tfhka.SendCmd("7");
                _tfhka.CloseFpCtrl();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (_tfhka.StatusPort)
                _tfhka.CloseFpCtrl();

            if (_tfhka.OpenFpCtrl(puerto, lbaudios))
            {
                _tfhka.PrintZReport();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (_tfhka.StatusPort)
                _tfhka.CloseFpCtrl();

            if (_tfhka.OpenFpCtrl(puerto, lbaudios))
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
