using ProviderLibrary.TemplateMethod;
using ProviderLibrary.Builder;
using ProviderLibrary.Facade;
using ProviderLibrary.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CustomControls;
using ZstdSharp.Unsafe;
using System.Drawing.Text;
using MySqlX.XDevAPI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static Provajder._Provider_dbDataSet1;
using ProviderLibrary.Loggers;

namespace Provajder
{
    public partial class Form1 : Form
    {
        private EventLogger logger;
        public Form1()
        {
            logger = new EventLogger();
            logger.AddObserver(new FileLogger("Logs", "user_logs.txt"));
            InitializeComponent();
            //skaliranje prozora aplikacije na sirinu ekrana
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.WindowState = FormWindowState.Maximized;
            linijaPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tabPage1.Dock = DockStyle.Fill;

            //autoscroll za Flow Layout ispise
            klijentiIspisFl.AutoScroll = true;
            ispisUgovoraFl.AutoScroll = true;
            ispisTVPaketifl.AutoScroll = true;
            ispisKombinovaniPaketifl.AutoScroll = true;
            ispisInternetPaketifl.AutoScroll = true;

            obojiElemente();


        }

        public void obojiElemente()
        {
            //boje elemenata
            ispisKombinovaniPaketifl.AutoScroll = true;
            tabPage1.BackColor = Color.FromArgb(255, 234, 219, 200);
            tabPage3.BackColor = Color.FromArgb(255, 234, 219, 200);
            tabPage4.BackColor = Color.FromArgb(255, 234, 219, 200);
            linijaPanel.BackColor = Color.FromArgb(255, 248, 240, 229);
            linija2Panel.BackColor = Color.FromArgb(255, 248, 240, 229);
            linija3Panel.BackColor = Color.FromArgb(255, 248, 240, 229);

            //----------------------------------------

            //inicijalizacija slika za tabove

            ImageList imageList1 = new ImageList();
            imageList1.ImageSize = new Size(68, 68);
            string pathApp = Application.StartupPath;
            pathApp = Path.GetFullPath(Path.Combine(pathApp, @"..\..\Images"));
            imageList1.Images.Add("key1", Image.FromFile(@pathApp + "\\showClient.png"));
            imageList1.Images.Add("key2", Image.FromFile(@pathApp + "\\packet.png"));
            imageList1.Images.Add("key3", Image.FromFile(@pathApp + "\\registration.png"));



            tabControl1.Dock = DockStyle.Fill;
            tabControl1.ImageList = imageList1;
            tabControl1.TabPages["tabPage1"].ImageKey = "key1";
            tabControl1.TabPages["tabPage3"].ImageIndex = 1;
            tabControl1.TabPages["tabPage4"].ImageKey = "key3";
            searchBarClientTb.Margin = new Padding(5, 5, 5, 5);
            searchBarClientTb.BorderStyle = BorderStyle.FixedSingle;
            brUgovoraDetaljiTab1lb.TextAlign = ContentAlignment.TopLeft;

            //-----------------------------------------------------------
        }


        private void urediTab1()
        {
            string nazivProvajdera = DatabaseCommands.dajNazivProvajdera();
            nazivProvajdera1Lb.Text = nazivProvajdera;
            nazivProvajdera2Lb.Text = nazivProvajdera;
            nazivProvajdera3Lb.Text = nazivProvajdera;
            klijentiIspisFl.Controls.Clear();
            opisDetaljiTab1lb.Text = "";
            ispisUgovoraFl.Controls.Clear();




            List<Klijent> klijenti = new List<Klijent>();
            klijenti = DatabaseCommands.dajSveKlijente();


            //Ispis svi klijenti

            foreach (Klijent k in klijenti)
            {
                RoundedButton btn = new RoundedButton();
                btn.BackColor = Color.FromArgb(255, 248, 240, 229);
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 16, 44, 87);
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = Color.FromArgb(255, 118, 192, 163);
                btn.Width = 170;
                btn.Height = 45;
                klijentiIspisFl.Controls.Add(btn);

                btn.Click += PrikaziPaketeKlijneta;

                btn.Text = k.Username.ToString();
            }
            specificneDetaljiTab1lb.TextAlign = ContentAlignment.TopLeft;
        }


        //od klijenta pokupim sve ugovore i ispisem pakete sa tih ugovora
        private void PrikaziPaketeKlijneta(object sender, EventArgs e)
        {

            nazivDetaljiTab1lb.Text = "";
            cenaDetaljiTab1lb.Text = "";
            specificneDetaljiTab1lb.Text = "";
            brUgovoraDetaljiTab1lb.Text = "";
            ispisInternetPaketifl.Controls.Clear();
            ispisTVPaketifl.Controls.Clear();
            ispisKombinovaniPaketifl.Controls.Clear();
            ispisUgovoraFl.Controls.Clear();
            opisDetaljiTab1lb.Text = "";



            RoundedButton currentSelectedButton = (RoundedButton)sender;
            currentSelectedButton.BackColor = Color.FromArgb(255, 16, 44, 87);
            Klijent klijent = DatabaseCommands.dajKlijentaPoUsernameu(currentSelectedButton.Text);
            List<Ugovor> ugovori = new List<Ugovor>();
            ugovori = DatabaseCommands.dajUgovore(klijent.Id.ToString());

            currentSelectedButton.ForeColor = Color.White;


            foreach (Control control in klijentiIspisFl.Controls)
            {
                if (control != currentSelectedButton)
                {
                    control.BackColor = Color.FromArgb(255, 248, 240, 229);
                    currentSelectedButton.BackColor = Color.FromArgb(255, 16, 44, 87);
                    control.ForeColor = Color.Black;
                }
                else
                {
                    List<Packet> TVpaketi = new List<Packet>();
                    TVpaketi = DatabaseCommands.dajTvPakete(ugovori);
                    foreach (Packet tv in TVpaketi)
                    {
                        RoundedButton btn = new RoundedButton();
                        btn.BackColor = Color.FromArgb(255, 248, 240, 229);
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 16, 44, 87);
                        btn.FlatAppearance.BorderSize = 1;
                        btn.FlatAppearance.BorderColor = Color.FromArgb(255, 118, 192, 163);
                        btn.Width = 520;
                        btn.Height = 45;
                        ispisTVPaketifl.Controls.Add(btn);
                        //btn.Click += PrikaziDetaljePaketaEvent;
                        btn.Text = tv.Name.ToString() + "  |  " + tv.Price.ToString();
                        btn.Click += prikaziDetaljeTab;
                    }

                    List<Packet> Internetpaketi = new List<Packet>();
                    Internetpaketi = DatabaseCommands.dajInternetPakete(ugovori);
                    foreach (Packet i in Internetpaketi)
                    {
                        RoundedButton btn = new RoundedButton();
                        btn.BackColor = Color.FromArgb(255, 248, 240, 229);
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 16, 44, 87);
                        btn.FlatAppearance.BorderSize = 1;
                        btn.FlatAppearance.BorderColor = Color.FromArgb(255, 118, 192, 163);
                        btn.Width = 520;
                        btn.Height = 45;
                        ispisInternetPaketifl.Controls.Add(btn);
                        //btn.Click += PrikaziDetaljePaketaEvent;
                        btn.Text = i.Name.ToString() + "  |  " + i.Price.ToString();
                        btn.Click += prikaziDetaljeTab;
                    }

                    List<Packet> Kombinovanipaketi = new List<Packet>();
                    Kombinovanipaketi = DatabaseCommands.dajKombinovanePakete(ugovori);
                    foreach (Packet k in Kombinovanipaketi)
                    {
                        RoundedButton btn = new RoundedButton();
                        btn.BackColor = Color.FromArgb(255, 248, 240, 229);
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 16, 44, 87);
                        btn.FlatAppearance.BorderSize = 1;
                        btn.FlatAppearance.BorderColor = Color.FromArgb(255, 118, 192, 163);
                        btn.Width = 520;
                        btn.Height = 45;
                        ispisKombinovaniPaketifl.Controls.Add(btn);
                        //btn.Click += PrikaziDetaljePaketaEvent;
                        btn.Text = k.Name.ToString() + "  |  " + k.Price.ToString();
                        btn.Click += prikaziDetaljeTab;
                    }

                    foreach (Ugovor u in ugovori)
                    {
                        RoundedButton btn = new RoundedButton();
                        btn.BackColor = Color.FromArgb(255, 248, 240, 229);
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 16, 44, 87);
                        btn.FlatAppearance.BorderSize = 1;
                        btn.FlatAppearance.BorderColor = Color.FromArgb(255, 118, 192, 163);
                        btn.Width = 170;
                        btn.Height = 45;
                        ispisUgovoraFl.Controls.Add(btn);

                        btn.Text = u.Id.ToString();
                        btn.Click += prikaziDetaljeUgovora;
                    }

                    ImePodacilb.Text = klijent.Ime.ToString();
                    PrezimePodacilb.Text = klijent.Prezime.ToString();

                    JmbgPodacilb.Text = klijent.JMBG.ToString();

                }

            }
        }

        private void prikaziDetaljeUgovora(object sender, EventArgs e)
        {
            Klijent klijent = new Klijent();

            foreach (RoundedButton btn in klijentiIspisFl.Controls)
            {
                if (btn.BackColor != Color.FromArgb(255, 248, 240, 229))
                {
                    klijent = DatabaseCommands.dajKlijentaPoUsernameu(btn.Text);
                }
            }
            RoundedButton currentSelectedButton = (RoundedButton)sender;
            string naziv = currentSelectedButton.Text.Trim();

            Packet paket = new Packet();
            paket = DatabaseCommands.dajPaketPoUgovoru(Convert.ToInt32(naziv));
            Ugovor ug = DatabaseCommands.dajIdUgovoraPoPaketuiKlijetu(paket.ID, klijent.Id);
            paket = DatabaseCommands.dajSpecificniPaket(paket);


            string[] vrednosti = new string[5];
            if (paket.Type == Packet.PacketType.TV)
            {

                PaketDetaljiTemplate p = new TvPaketDetalji();
                vrednosti = p.ispisDetalji(Convert.ToInt32(naziv), paket);


            }
            else if (paket.Type == Packet.PacketType.INTERNET)
            {
                PaketDetaljiTemplate p = new InternetPaketDetalji();
                vrednosti = p.ispisDetalji(Convert.ToInt32(naziv), paket);


            }
            else if (paket.Type == Packet.PacketType.COMBINED)
            {
                PaketDetaljiTemplate p = new KombinovaniPaketDetalji();
                vrednosti = p.ispisDetalji(Convert.ToInt32(naziv), paket);
            }

            nazivDetaljiTab1lb.Text = vrednosti[0];
            cenaDetaljiTab1lb.Text = vrednosti[1];
            specificneDetaljiTab1lb.Text = vrednosti[3];
            opisDetaljiTab1lb.Text = vrednosti[2];
            brUgovoraDetaljiTab1lb.Text = vrednosti[4] + "\nDatum pocetka ugovora: " + ug.Datum_od.ToString() + "\nDatum do: " + ug.Datum_do.ToString();

        }



        //search bar-ovi
        private void searchBarClientTb_Enter(object sender, EventArgs e)
        {
            if (searchBarClientTb.Text == "Unesite username...")
            {
                searchBarClientTb.Text = "";
                searchBarClientTb.ForeColor = SystemColors.WindowText; // Postavi boju teksta na podrazumevanu
            }
        }

        private void searchBarClientTb_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchBarClientTb.Text))
            {
                searchBarClientTb.Text = "Unesite username...";
                searchBarClientTb.ForeColor = SystemColors.GrayText; // Postavi boju teksta na sivu
            }
        }
        /* za izbaciti - obrisani search barovi na tabu3
        private void searchIntPaketTb_Enter(object sender, EventArgs e)
        {
            if (searchIntPaketTb.Text == "Pretrazite Internet paket...")
            {
                searchIntPaketTb.Text = "";
                searchIntPaketTb.ForeColor = SystemColors.WindowText; // Postavi boju teksta na podrazumevanu
            }

        }

        private void searchIntPaketTb_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchIntPaketTb.Text))
            {
                searchIntPaketTb.Text = "Pretrazite Internet paket...";
                searchIntPaketTb.ForeColor = SystemColors.GrayText; // Postavi boju teksta na sivu
            }
        }

        private void searchTVpaketTb_Enter(object sender, EventArgs e)
        {
            if (searchTVpaketTb.Text == "Pretrazite TV paket...")
            {
                searchTVpaketTb.Text = "";
                searchTVpaketTb.ForeColor = SystemColors.WindowText; // Postavi boju teksta na podrazumevanu
            }
        }

        private void searchTVpaketTb_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchTVpaketTb.Text))
            {
                searchTVpaketTb.Text = "Pretrazite TV paket...";
                searchTVpaketTb.ForeColor = SystemColors.GrayText; // Postavi boju teksta na sivu
            }
        }*/
        //--------------------------------------------------------------

        /// 
        /// /////////////////////////////////////////////////////////////////////////
        ///

        private RoundedButton postaviPodesavanjaButtona(RoundedButton btn)
        {
            btn.BackColor = Color.FromArgb(255, 248, 240, 229);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 16, 44, 87);
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Color.FromArgb(255, 118, 192, 163);
            btn.Width = 350;
            btn.Height = 50;
            btn.Click += prikaziDetaljePaketaEvent;

            return btn;
        }

        List<RoundedButton> rbtnList;
        private void urediTab2()
        {
            rbtnList = new List<RoundedButton>();
            InternetPaketiFl.AutoScroll = true;
            TvPaketiFl.AutoScroll = true;
            KombinovaniPaketiFl.AutoScroll = true;

            InternetPaketiFl.Controls.Clear();
            TvPaketiFl.Controls.Clear();
            KombinovaniPaketiFl.Controls.Clear();

            List<Packet> InternetPaketi = new List<Packet>();
            InternetPaketi = DatabaseCommands.dajInternetPakete();

            foreach (var internetPaket in InternetPaketi)
            {
                RoundedButton btn = new RoundedButton();
                btn.Text = internetPaket.Name.ToString() + "   |   " + internetPaket.Price;
                btn = postaviPodesavanjaButtona(btn);
                InternetPaketiFl.Controls.Add(btn);
                rbtnList.Add(btn);
                btn.Click += prikaziDetaljeTab2;
            }

            List<Packet> TvPaketi = new List<Packet>();
            TvPaketi = DatabaseCommands.dajTvPakete();

            foreach (var tvPaket in TvPaketi)
            {
                RoundedButton btn = new RoundedButton();
                btn.Text = tvPaket.Name.ToString() + "   |   " + tvPaket.Price;
                btn = postaviPodesavanjaButtona(btn);
                TvPaketiFl.Controls.Add(btn);
                rbtnList.Add(btn);
                btn.Click += prikaziDetaljeTab2;
            }

            List<Packet> KombinovaniPaketi = new List<Packet>();
            KombinovaniPaketi = DatabaseCommands.dajKombinovanePakete();

            foreach (var kombinovaniPaket in KombinovaniPaketi)
            {
                RoundedButton btn = new RoundedButton();
                btn.Text = kombinovaniPaket.Name.ToString() + "   |   " + kombinovaniPaket.Price;
                btn = postaviPodesavanjaButtona(btn);
                KombinovaniPaketiFl.Controls.Add(btn);
                rbtnList.Add(btn);
                btn.Click += prikaziDetaljeTab2;
            }
        }

        private void prikaziDetaljeTab(object sender, EventArgs e)
        {

            Klijent klijent = new Klijent();

            foreach (RoundedButton btn in klijentiIspisFl.Controls)
            {
                if (btn.BackColor != Color.FromArgb(255, 248, 240, 229))
                {
                    klijent = DatabaseCommands.dajKlijentaPoUsernameu(btn.Text);
                }
            }

            RoundedButton currentSelectedButton = (RoundedButton)sender;
            string naziv = currentSelectedButton.Text;
            naziv = naziv.Split('|')[0].Trim();
            Packet paket = new Packet();
            paket = DatabaseCommands.dajPaketPoNazivu(naziv);

            Ugovor ug = DatabaseCommands.dajIdUgovoraPoPaketuiKlijetu(paket.ID, klijent.Id);

            paket = DatabaseCommands.dajSpecificniPaket(paket);
            string[] vrednosti = new string[5];
            if (paket.Type == Packet.PacketType.TV)
            {

                PaketDetaljiTemplate p = new TvPaketDetalji();
                vrednosti = p.ispisDetalji(ug.Id, paket);


            }
            else if (paket.Type == Packet.PacketType.INTERNET)
            {
                PaketDetaljiTemplate p = new InternetPaketDetalji();
                vrednosti = p.ispisDetalji(ug.Id, paket);


            }
            else if (paket.Type == Packet.PacketType.COMBINED)
            {
                PaketDetaljiTemplate p = new KombinovaniPaketDetalji();
                vrednosti = p.ispisDetalji(ug.Id, paket);
            }

            nazivDetaljiTab1lb.Text = vrednosti[0];
            cenaDetaljiTab1lb.Text = vrednosti[1];
            specificneDetaljiTab1lb.Text = vrednosti[3];
            brUgovoraDetaljiTab1lb.Text = vrednosti[4] + "\nDatum pocetka ugovora: " + ug.Datum_od.ToString() + "\nDatum do: " + ug.Datum_do.ToString();
            opisDetaljiTab1lb.Text = vrednosti[2];

        }

        private void prikaziDetaljeTab2(object sender, EventArgs e)
        {
            RoundedButton currentSelectedButton = (RoundedButton)sender;
            currentSelectedButton.ForeColor = Color.White;
            string naziv = currentSelectedButton.Text;
            naziv = naziv.Split('|')[0].Trim();
            Packet paket = new Packet();
            paket = DatabaseCommands.dajPaketPoNazivu(naziv);

            paket = DatabaseCommands.dajSpecificniPaket(paket);
            string[] vrednosti = new string[5];
            if (paket.Type == Packet.PacketType.TV)
            {

                PaketDetaljiTemplate p = new TvPaketDetalji();
                vrednosti = p.ispisDetalji(0, paket);


            }
            else if (paket.Type == Packet.PacketType.INTERNET)
            {
                PaketDetaljiTemplate p = new InternetPaketDetalji();
                vrednosti = p.ispisDetalji(0, paket);


            }
            else if (paket.Type == Packet.PacketType.COMBINED)
            {
                PaketDetaljiTemplate p = new KombinovaniPaketDetalji();
                vrednosti = p.ispisDetalji(0, paket);
            }

            nazivDetaljiTab2lb.Text = vrednosti[0];
            cenaDetaljiTab2lb.Text = vrednosti[1];
            opisDetaljiTab2lb.Text = vrednosti[2];
            specificneDetaljiTab2lb.Text = vrednosti[3];

        }

        public void prikaziDetaljePaketaEvent(object sender, EventArgs e)
        {
            RoundedButton currentSelectedButton = (RoundedButton)sender;

            foreach (Control control in rbtnList)
            {
                if (control != currentSelectedButton)
                {
                    control.BackColor = Color.FromArgb(255, 248, 240, 229);
                    control.ForeColor = Color.Black;
                    currentSelectedButton.BackColor = Color.FromArgb(255, 16, 44, 87);
                }
            }
        }




        /// 
        /// /////////////////////////////////////////////////////////////////////////
        /// 


        private void Form1_Load(object sender, EventArgs e)
        {
            urediTab1();
            urediTab2();
        }

        private void dodajKlijentaBtn_Click(object sender, EventArgs e)
        {
            string ime = imeKorisnikaTb.Text;
            string prezime = prezimeKorisnikaTb.Text;
            string jmbg = jmbgKorisnikaTb.Text;
            string username = usernameKorisnikaTb.Text;

            Klijent k = DatabaseCommands.dajKlijentaPoUsernameu(username);

            if (k.Username != username)
            {

                if (ime == "" || prezime == "" || username == "" || jmbg == "")
                {
                    MessageBox.Show("Sva polja su obavezna!");
                }
                else
                {
                    DatabaseCommands.registrujNovogKorisnika(ime, prezime, jmbg, username);
                    MessageBox.Show("Uspesno ste registovali korisnika!");
                    logger.LogEvent($"Novi klijent {ime} {prezime} sa JMBG:{jmbg} registrovan pod korisnickim imenom {username}");
                    imeKorisnikaTb.Clear();
                    prezimeKorisnikaTb.Clear();
                    jmbgKorisnikaTb.Clear();
                    usernameKorisnikaTb.Clear();
                    urediTab1();
                }
            }
            else
            {
                MessageBox.Show("Postoji klijent sa tim username-om!");
                usernameKorisnikaTb.Focus();
                usernameKorisnikaTb.SelectAll();
            }
        }
        
        private void dodajPaketBtn_Click(object sender, EventArgs e)
        {
            dodajPaketBtn.Visible = false;
            
            nazivUnosPaketaTB.Text = "";
            cenaUnosPaketaTB.Text = "";
            opisUnosPaketaTB.Text = "";
            brzinainternetaunostb.Text = "";
            brojkanlaunostb.Text = "";
            nazivDetaljiTab2lb.Text = "";
            cenaDetaljiTab2lb.Text = "";
            opisDetaljiTab2lb.Text = "";
            specificneDetaljiTab2lb.Text = "";
            obrisiPaketBt.Visible = false;


            TVkombinovaniCLB.Items.Clear();
            internetKombinovaniCLB.Items.Clear();
            //izlistavanje TV i Internet paketa za formiranje Kombinovanog
            string naziv;
            List<Packet> TvPaketi = new List<Packet>();
            TvPaketi = DatabaseCommands.dajTvPakete();

            List<Packet> Internetpaketi = new List<Packet>();
            Internetpaketi = DatabaseCommands.dajInternetPakete();

            foreach (var tvPaket in TvPaketi)
            {
                naziv = tvPaket.Name + " | BrojKanala: " + tvPaket.NumOfChannels;
                TVkombinovaniCLB.Items.Add(naziv);
            }

            foreach (var intPaket in Internetpaketi)
            {
                naziv = intPaket.Name + " | Brzina: " + intPaket.InternetSpeed;
                internetKombinovaniCLB.Items.Add(naziv);
            }
            //omogucavanje da samo jedan bude cekiran
            TVkombinovaniCLB.ItemCheck += CheckedTVCLB_ItemCheck;
            internetKombinovaniCLB.ItemCheck += CheckedINTERNETCLB_ItemCheck;

            //Izbor radioButton-a
            tvPaketrb.CheckedChanged += RadioButtonProvera;
            internetpaketrb.CheckedChanged += RadioButtonProvera;
            kombinovanirb.CheckedChanged += RadioButtonProvera;


            unosNovogPaketaPn.Visible = true;
            ispisDetaljiTab2Pn.Visible = false;

        }

        private void RadioButtonProvera(object sender, EventArgs e)
        {
            RadioButton aktivni = (RadioButton)sender;
            if (aktivni.Checked)
            {
                foreach (Control kontrola in this.Controls)
                {
                    if (kontrola is RadioButton radioButton && radioButton != aktivni) radioButton.Checked = false;
                }
            }
        }

        private void CheckedTVCLB_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            for (int i = 0; i < TVkombinovaniCLB.Items.Count; i++)
            {
                if (i != e.Index)
                {
                    TVkombinovaniCLB.SetItemChecked(i, false);
                }
            }
        }

        private void CheckedINTERNETCLB_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            for (int i = 0; i < internetKombinovaniCLB.Items.Count; i++)
            {
                if (i != e.Index)
                {
                    internetKombinovaniCLB.SetItemChecked(i, false);
                }
            }
        }

        private void Paketrb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                if (string.Equals(rb.Name, "tvPaketrb"))
                {
                    TvPn.Visible = true;
                    TvPn.BringToFront();
                    kombinovaniPn.Visible = false;
                }
                else if (string.Equals(rb.Name, "internetpaketrb"))
                {
                    internetPn.Visible = true;
                    internetPn.BringToFront();
                    kombinovaniPn.Visible = false;
                }
                else
                {
                    kombinovaniPn.Visible = true;
                    kombinovaniPn.BringToFront();
                }
            }

        }

        private void searchBarClientTb_TextChanged(object sender, EventArgs e)
        {
            List<Klijent> klijenti = new List<Klijent>();
            klijentiIspisFl.Controls.Clear();
            TextBox tb = (TextBox)sender;
            if (tb.Text == "Unesite username...")
            {
                klijenti = DatabaseCommands.dajKlijentaPoUsernameuLike("");
            }
            else
                klijenti = DatabaseCommands.dajKlijentaPoUsernameuLike(tb.Text);


            foreach (Klijent k in klijenti)
            {
                RoundedButton btn = new RoundedButton();
                btn.BackColor = Color.FromArgb(255, 248, 240, 229);
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 16, 44, 87);
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = Color.FromArgb(255, 118, 192, 163);
                btn.Width = 170;
                btn.Height = 45;
                klijentiIspisFl.Controls.Add(btn);

                btn.Click += PrikaziPaketeKlijneta;

                btn.Text = k.Username.ToString();
            }
        }

        private RadioButton DajAktiviraniRB()
        {

            foreach (Control control in unosNovogPaketaPn.Controls)
            {
                if (control is RadioButton radioButton && radioButton.Checked)
                {
                    return radioButton;
                }
            }
            return null; // Nijedan RadioButton nije čekiran
        }

        private void snimiPaketBt_Click(object sender, EventArgs e)
        {
            string naziv = nazivUnosPaketaTB.Text;
            string cena = cenaUnosPaketaTB.Text;
            string opis = opisUnosPaketaTB.Text;

            RadioButton aktivirani = DajAktiviraniRB();
            if (aktivirani == null)
            {
                MessageBox.Show("Morate izabrati tip paketa koji unosite");
            }
            else
            {
                if (naziv == "" || cena == "" || opis == "")
                {
                    MessageBox.Show("Sva polja su obavezna");
                }
                else
                {
                    if (string.Equals(aktivirani.Name, "tvPaketrb"))
                    {
                        string brKanala = brojkanlaunostb.Text;
                        if (brKanala == "") MessageBox.Show("Sva polja su obavezna");
                        else
                        {

                            DatabaseCommands.dodajNoviTvPaket(naziv, Convert.ToInt32(cena), Convert.ToInt32(brKanala), opis);
                            MessageBox.Show("Uspesno ste dodali TV paket " + naziv);
                            logger.LogEvent($"TV paket {naziv} od {brKanala} kanala uvrsten u ponudu po ceni od {cena}");
                            //reset
                            unosNovogPaketaPn.Visible = false;
                            dodajPaketBtn.Visible = true;
                            ispisDetaljiTab2Pn.Visible = true;
                            obrisiPaketBt.Visible = true;
                            urediTab2();

                        }
                    }
                    else if (string.Equals(aktivirani.Name, "internetpaketrb"))
                    {
                        string brzinaInterneta = brzinainternetaunostb.Text;
                        if (brzinaInterneta == "") MessageBox.Show("Sva polja su obavezna");
                        else
                        {
                            DatabaseCommands.dodajNoviInternetPaket(naziv, Convert.ToInt32(cena), Convert.ToInt32(brzinaInterneta), opis);
                            MessageBox.Show("Uspesno ste dodali Internet paket " + naziv);
                            logger.LogEvent($"Internet paket {naziv} sa protokom od {brzinaInterneta} Mbit/s uvrsten u ponudu po ceni od {cena}");
                            //reset
                            unosNovogPaketaPn.Visible = false;
                            dodajPaketBtn.Visible = true;
                            ispisDetaljiTab2Pn.Visible = true;
                            obrisiPaketBt.Visible = true;
                            urediTab2();
                        }
                    }
                    else
                    {
                        string tv = dajCekiraniPaket(TVkombinovaniCLB);
                        string internet = dajCekiraniPaket(internetKombinovaniCLB);
                        if (tv == null || internet == null)
                        {
                            MessageBox.Show("Kombinovani paket mora da ima i TV paket i INTERNET paket");
                        }
                        else
                        {
                            tv = tv.Split('|')[0].Trim();
                            internet = internet.Split('|')[0].Trim();

                            Packet internetPaket = new Packet();
                            internetPaket = DatabaseCommands.dajPaketPoNazivu(internet);

                            Packet tvPaket = new Packet();
                            tvPaket = DatabaseCommands.dajPaketPoNazivu(tv);
                            DatabaseCommands.dodajNoviKombinovaniPaket(internetPaket.ID, tvPaket.ID, naziv, Convert.ToInt32(cena), opis);

                            MessageBox.Show("Uspesno ste dodali kombinovani paket " + naziv);
                            logger.LogEvent($"Kombinovani paket {naziv} sa internet paketom '{internetPaket.Name}' i TV paketom '{tvPaket.Name}' uvrsten u ponudu po ceni od {cena}");
                            //reset
                            unosNovogPaketaPn.Visible = false;
                            dodajPaketBtn.Visible = true;
                            ispisDetaljiTab2Pn.Visible = true;
                            obrisiPaketBt.Visible = true;
                            urediTab2();
                        }

                    }

                }

            }

        }

        private string dajCekiraniPaket(CheckedListBox cListBox)
        {
            foreach (object i in cListBox.CheckedItems)
            {
                return i.ToString();

            }
            return null;
        }

        private void dodajUgovorBt_Click(object sender, EventArgs e)
        {
            tvPaketiUgovorCBL.Items.Clear();
            internetPaketiUgovorCBL.Items.Clear();
            kombinovaniPaketiUgovorCBL.Items.Clear();

            //Packet pck = new Packet();
            //pronalazi klijenta

            Klijent klijent = new Klijent();
            klijent = null;
            foreach (RoundedButton btn in klijentiIspisFl.Controls)
            {
                if (btn.BackColor != Color.FromArgb(255, 248, 240, 229))
                {
                    klijent = DatabaseCommands.dajKlijentaPoUsernameu(btn.Text);
                }
            }

            if (klijent == null)
            {
                MessageBox.Show("Izaberite klijenta");
            }
            else
            {
                string naziv;
                int klijent_id = klijent.Id;
                unosUgovoraPn.Visible = true;
                unosUgovoraPn.BringToFront();

                List<Packet> TvPaketi = new List<Packet>();
                TvPaketi = DatabaseCommands.dajTvPakete();

                List<Packet> Internetpaketi = new List<Packet>();
                Internetpaketi = DatabaseCommands.dajInternetPakete();

                List<Packet> KombinovaniPaketi = new List<Packet>();
                KombinovaniPaketi = DatabaseCommands.dajKombinovanePakete();

                foreach (var tvPaket in TvPaketi)
                {
                    naziv = tvPaket.Name + " | BrojKanala: " + tvPaket.NumOfChannels + " | Cena: " + tvPaket.Price; ;
                    tvPaketiUgovorCBL.Items.Add(naziv);
                }

                foreach (var intPaket in Internetpaketi)
                {
                    naziv = intPaket.Name + " | Brzina: " + intPaket.InternetSpeed + " | Cena: " + intPaket.Price; ;
                    internetPaketiUgovorCBL.Items.Add(naziv);
                }

                foreach (var kPaket in KombinovaniPaketi)
                {
                    naziv = kPaket.Name + " | Brzina: " + kPaket.InternetSpeed + " | Cena: " + kPaket.Price;
                    kombinovaniPaketiUgovorCBL.Items.Add(naziv);
                }

                tvPaketiUgovorCBL.ItemCheck += CheckedTVUgovorCLB_ItemCheck;
                internetPaketiUgovorCBL.ItemCheck += CheckedInternetUgovorCLB_ItemCheck;
                kombinovaniPaketiUgovorCBL.ItemCheck += CheckedKombinovaniUgovorCLB_ItemCheck;



            }



        }

        private void CheckedTVUgovorCLB_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int j;
            for (int i = 0; i < tvPaketiUgovorCBL.Items.Count; i++)
            {
                if (i != e.Index)
                {
                    tvPaketiUgovorCBL.SetItemChecked(i, false);

                }
                for (j = 0; j < internetPaketiUgovorCBL.Items.Count; j++)
                {
                    internetPaketiUgovorCBL.SetItemChecked(j, false);
                }
                for (j = 0; j < kombinovaniPaketiUgovorCBL.Items.Count; j++)
                {
                    kombinovaniPaketiUgovorCBL.SetItemChecked(j, false);
                }
            }
        }

        private void CheckedInternetUgovorCLB_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int j;
            for (int i = 0; i < internetPaketiUgovorCBL.Items.Count; i++)
            {
                if (i != e.Index)
                {
                    internetPaketiUgovorCBL.SetItemChecked(i, false);
                }
                for (j = 0; j < tvPaketiUgovorCBL.Items.Count; j++)
                {
                    tvPaketiUgovorCBL.SetItemChecked(j, false);
                }
                for (j = 0; j < kombinovaniPaketiUgovorCBL.Items.Count; j++)
                {
                    kombinovaniPaketiUgovorCBL.SetItemChecked(j, false);
                }
            }
        }

        private void CheckedKombinovaniUgovorCLB_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int j;
            for (int i = 0; i < kombinovaniPaketiUgovorCBL.Items.Count; i++)
            {
                if (i != e.Index)
                {
                    kombinovaniPaketiUgovorCBL.SetItemChecked(i, false);
                }
                for (j = 0; j < internetPaketiUgovorCBL.Items.Count; j++)
                {
                    internetPaketiUgovorCBL.SetItemChecked(j, false);
                }
                for (j = 0; j < tvPaketiUgovorCBL.Items.Count; j++)
                {
                    tvPaketiUgovorCBL.SetItemChecked(j, false);
                }
            }
        }


        private void roundedButton3_Click(object sender, EventArgs e)
        {
            dodajPaketBtn.Visible = true;
            ispisDetaljiTab2Pn.Visible = true;
            unosNovogPaketaPn.Visible = false;
            obrisiPaketBt.Visible = true;

            nazivDetaljiTab2lb.Text = "";
            cenaDetaljiTab2lb.Text = "";
            opisDetaljiTab2lb.Text = "";
            specificneDetaljiTab2lb.Text = "";
            urediTab2();
        }

        private void snimiUgovorBt_Click(object sender, EventArgs e)
        {
            nazivDetaljiTab1lb.Text = "";
            cenaDetaljiTab1lb.Text = "";
            opisDetaljiTab1lb.Text = "";
            specificneDetaljiTab1lb.Text = "";
            brUgovoraDetaljiTab1lb.Text = "";
            dodajUgovorBt.Visible = false;
            string tv = dajCekiraniPaket(tvPaketiUgovorCBL);
            string internet = dajCekiraniPaket(internetPaketiUgovorCBL);
            string kombinovani = dajCekiraniPaket(kombinovaniPaketiUgovorCBL);

            Klijent klijent = new Klijent();
            foreach (RoundedButton btn in klijentiIspisFl.Controls)
            {
                if (btn.BackColor != Color.FromArgb(255, 248, 240, 229))
                {
                    klijent = DatabaseCommands.dajKlijentaPoUsernameu(btn.Text);
                }
            }

            int klijent_id = klijent.Id;
            int paket_id;
            Packet paket = new Packet();
            DateTime datumOD = DateTime.Now;
            TimeSpan time = new TimeSpan(730, 0, 0, 0);
            DateTime datumDO = datumOD.Add(time);
            Ugovor ug = new Ugovor();
            string txt = "";
            if (tv != null)
            {
                tv = tv.Split('|')[0].Trim();
                paket = DatabaseCommands.dajPaketPoNazivu(tv);
                paket_id = paket.ID;

                bool response = DatabaseCommands.kreirajUgovor(klijent_id, paket_id, datumOD.ToString(), datumDO.ToString());
                if (response)
                {
                    MessageBox.Show("Uspesno ste pretplatili " + klijent.Ime + " " + klijent.Prezime + " na Tv paket: " + paket.Name);
                    //restrat
                    urediTab1();
                    unosUgovoraPn.Visible = false;
                    dodajUgovorBt.Visible = true;
                    ispisTVPaketifl.Controls.Clear();
                    ispisInternetPaketifl.Controls.Clear();
                    ispisKombinovaniPaketifl.Controls.Clear();
                    ispisUgovoraFl.Controls.Clear();
                    txt = $"Korisnik {klijent.Username} se pretplatio na uslugu Televizije {paket.Name}";
                }
                else {
                    MessageBox.Show("Korisnik " + klijent.Ime + " " + klijent.Prezime + " vec ima ugovoroen Tv paket: " + paket.Name);
                }
                
               
            }
            else if (internet != null)
            {
                internet = internet.Split('|')[0].Trim();
                paket = DatabaseCommands.dajPaketPoNazivu(internet);
                paket_id = paket.ID;
                bool response = DatabaseCommands.kreirajUgovor(klijent_id, paket_id, datumOD.ToString(), datumDO.ToString());
                if (response)
                {
                    MessageBox.Show("Uspesno ste pretplatili " + klijent.Ime + " " + klijent.Prezime + " na Internet paket: " + paket.Name);
                    //restrat
                    urediTab1();
                    unosUgovoraPn.Visible = false;
                    dodajUgovorBt.Visible = true;
                    ispisTVPaketifl.Controls.Clear();
                    ispisInternetPaketifl.Controls.Clear();
                    ispisKombinovaniPaketifl.Controls.Clear();
                    ispisUgovoraFl.Controls.Clear();
                    txt = $"Korisnik {klijent.Username} se pretplatio na uslugu Interneta {paket.Name}";
                }

                else {
                    MessageBox.Show("Korisnik " + klijent.Ime + " " + klijent.Prezime + " vec ima ugovoren Internet paket: " + paket.Name);

                }
                
               
            }
            else if (kombinovani != null)
            {
                kombinovani = kombinovani.Split('|')[0].Trim();
                paket = DatabaseCommands.dajPaketPoNazivu(kombinovani);
                paket_id = paket.ID;
                bool response = DatabaseCommands.kreirajUgovor(klijent_id, paket_id, datumOD.ToString(), datumDO.ToString());
                if (response)
                {
                    MessageBox.Show("Uspesno ste pretplatili " + klijent.Ime + " " + klijent.Prezime + " na Kombinovani paket: " + paket.Name);
                    //restrat
                    urediTab1();
                    unosUgovoraPn.Visible = false;
                    dodajUgovorBt.Visible = true;
                    ispisTVPaketifl.Controls.Clear();
                    ispisInternetPaketifl.Controls.Clear();
                    ispisKombinovaniPaketifl.Controls.Clear();
                    ispisUgovoraFl.Controls.Clear();
                    txt = $"Korisnik {klijent.Username} se pretplatio na kombinovanu uslugu {paket.Name}";

                }
                else {
                    MessageBox.Show("Korisnik " + klijent.Ime + " " + klijent.Prezime + " vec ima ugovoren Kombinovni paket: " + paket.Name);

                }
            }
            else
            {
                MessageBox.Show("Morate izabrati neki paket!!!!!");
                return;
            }
            ug = DatabaseCommands.dajIdUgovoraPoPaketuiKlijetu(paket_id, klijent_id);
            txt += $" pod ugovorom {ug.Id} koji vazi do {ug.Datum_do}";
            logger.LogEvent(txt);
        }

private void odustaniUgovor_Click(object sender, EventArgs e)
        {
            unosUgovoraPn.Visible = false;
            dodajUgovorBt.Visible=true;
        }

       
        public void postavljanjeTeksta(string tekst)
        {
            nazivDetaljiTab1lb.Text = tekst;
        }

        private void roundedButton2_Click(object sender, EventArgs e)
        {
            if (brUgovoraDetaljiTab1lb.Text == "")
            {
                MessageBox.Show("Morate izabrati ugovor");
            }
            else {
               
                int idUgovora = Convert.ToInt32(brUgovoraDetaljiTab1lb.Text.Split(' ')[2].Split('\n')[0]);
                DatabaseCommands.izbrisiUgovor(idUgovora);
                MessageBox.Show("Uspesno ste obrisali ugovor");
                urediTab1();
                nazivDetaljiTab1lb.Text = "";
                cenaDetaljiTab1lb.Text = "";
                specificneDetaljiTab1lb.Text = "";
                brUgovoraDetaljiTab1lb.Text = "";
                opisDetaljiTab1lb.Text = "";
                ispisTVPaketifl.Controls.Clear();
                ispisInternetPaketifl.Controls.Clear();
                ispisKombinovaniPaketifl.Controls.Clear();
                logger.LogEvent($"Ugovor sa ID:{idUgovora} obrisan");
            }
            
        }

        private void obrisiKlijentaBt_Click(object sender, EventArgs e)
        {
            Klijent klijent = new Klijent();
            klijent = null;
            foreach (RoundedButton btn in klijentiIspisFl.Controls)
            {
                if (btn.BackColor != Color.FromArgb(255, 248, 240, 229))
                {
                    klijent = DatabaseCommands.dajKlijentaPoUsernameu(btn.Text);
                }
            }
            if (klijent == null)
            {
                MessageBox.Show("Morate izabrati korisnika");
            }
            else {
                bool uspesan = DatabaseCommands.obrisiKorisnika(klijent.Id);
                if (uspesan)
                {
                    MessageBox.Show("Uspesno ste obrisali korisnika");
                    ImePodacilb.Text = "";
                    PrezimePodacilb.Text = "";
                    JmbgPodacilb.Text = "";
                    logger.LogEvent($"Korisnik {klijent.Ime} {klijent.Prezime} pod korisnickim imenom {klijent.Username} obrisan");
                    urediTab1();
                }
                else
                {
                    MessageBox.Show("Nije moguce obrisati klijenta koji ima aktivan ugovor");
                }
                
            }
        }

        private void obrisiPaketBt_Click(object sender, EventArgs e)
        {
            if (nazivDetaljiTab2lb.Text != "") {
                string nazivPaketa = nazivDetaljiTab2lb.Text.Split(':')[1].Trim();
                int idPaketa = DatabaseCommands.dajPaketPoNazivu(nazivPaketa).ID;

                bool uspesno = DatabaseCommands.obrisiPaket(idPaketa);
                if (uspesno)
                {
                    MessageBox.Show("Uspesno ste obrisali paket");
                    nazivDetaljiTab2lb.Text = "";
                    cenaDetaljiTab2lb.Text = "";
                    opisDetaljiTab2lb.Text = "";
                    specificneDetaljiTab2lb.Text = "";
                    urediTab2();
                    logger.LogEvent($"Paket pod nazivom {nazivPaketa} obrisan");
                }
                else
                {
                    MessageBox.Show("Nije moguce obrisati paket koji ima aktivan ugovor\nili\nNe mozete da izbrisete paket koji je deo Kombinovanog paketa");
                }
            }
            else
            {
                MessageBox.Show("Morate izabrati paket");
            }
           

        }

        private void obrisiPaketBt_ChangeUICues(object sender, UICuesEventArgs e)
        {

        }
    }
}
