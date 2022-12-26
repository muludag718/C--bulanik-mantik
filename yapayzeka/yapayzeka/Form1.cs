using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace yapayzeka
{
    public partial class Form1 : Form
    {
        class KeyValue
        {
            public string key;
            public double deger;

            public KeyValue(string key, double deger)
            {
                this.key = key;
                this.deger = deger;
            }
        }
        public Form1()
        {
            InitializeComponent();
            graphics1 = panel1.CreateGraphics();
            graphics2 = panel2.CreateGraphics();
            graphics3 = panel3.CreateGraphics();
            getgrafik();
        }
        Graphics graphics1;
        Graphics graphics2;
        Graphics graphics3;
        int h = 1;
        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="" && textBox2.Text == "")
            {
                MessageBox.Show("Lütfen değerleri giriniz");
                return ;
            }
            double deg = Convert.ToDouble(textBox1.Text.Replace(".", ","));
            double deg2 = Convert.ToDouble(textBox2.Text.Replace(".", ","));
            var hava = havanemkişlemmu(deg2);
            var sıcaklık = sıcaklıkişlemmu(deg);
            getgrafik();
            var list=new List<string>();
            var dict=new Dictionary<string,double>();
            richTextBox2.Text = richTextBox3.Text = "";
            for (int i = 0; i < sıcaklık.Count; i++)
            {
                for (int j = 0; j < hava.Count; j++)
                {
                    var rules = Rulesmu(sıcaklık[i], hava[j]);
                    richTextBox2.Text += $"{h}.kural " + sıcaklık[i].key + " ve " + hava[j].key + " ise ısıtma değeri "+rules.key+" olur \n";
                    if (list.Contains(rules.key))
                    {
                        double dat = 0;
                        dict.TryGetValue(rules.key,out dat);
                        var mx = Math.Max(rules.deger, dat);
                        dict[rules.key]= mx;
                    }
                    else
                    {
                        list.Add(rules.key);
                        dict.Add(rules.key, rules.deger);
                    }
                    h++;
                }
            }
            var tlist=new List<KeyValue>();
            foreach (var item in dict)
            {
                richTextBox3.Text +="Isıtma Üyelik Değerleri "+ item.Key + "=>" + item.Value.ToString() + "\n";
                tlist.Add(new KeyValue(item.Key, item.Value));
            }
            if (tlist.Count > 1)
            {
                işlem(getortadeger(tlist[0].key), getortadeger(tlist[1].key), tlist[0].deger, tlist[1].deger);
            }
            if (tlist.Count<=1)
            {
                richTextBox1.Text = tlist[0].key.ToString();
                richTextBox1.Text += "\nÇıktı DEĞER ===>  1";
            }
        }
        void işlem(double x,double y,double uye,double uye2)
        {
            //işlem agrılıklı ortalama yontemı formulu
            var iş = ((x * uye) + (y * uye2)) / (uye + uye2);
            richTextBox1.Text ="Çıktı DEĞER ===>  " +iş.ToString();
            h = 0;
        }
        double getortadeger(string a)
        {
            ///ısıtmanın orta değerleri
            if (a == cokdusuk)
            {
                return 0.5;
            }else if (a == dusuk)
            {
                return 1.5;
            }else if (a == orta)
            {
                return 3;
            }else if (a == yuksek)
            {
                return 4.25;
            }
            else
            {
                return 5.25;
            }
        }
        double sıcaklıkmu(double deg,double min,double max)
        {
            double orta = min+((max - min) / 2);
            if (deg > min && deg <orta )
                return (deg - min) / (orta - min);
            else if (deg==orta)
                return 1;
            else if (deg > orta && deg < max)
                return (max - deg) / (max - orta);
            else return 0;
        }
        //deg=deger max=max deger min=min deger,mx ise tekrarlanan deger
        double sıcaklıkmu(double deg, double min,double max,double mx)
        {
            double orta = min + ((max - min) / 2);
            if (min == mx)
            {
                if (deg >= min && deg < orta)
                    return 1;
                else if (deg > orta && deg < max)
                    return (max - deg) / (max - orta);
                else return 0;
            }
            else if (max == mx)
            {
                if (deg <= max && deg > orta)
                    return 1;
                else if (deg > min && deg < orta)
                    return (deg - min) / (orta - min);
                else return 0;

            } else return 0;
        }
        List<KeyValue> havanemkişlemmu(double deg)
        {
             List <KeyValue> list=new List<KeyValue>();
            if (sıcaklıkmu(deg, 0, 40, 0) != 0)
                list.Add(new KeyValue(cokdusuk, sıcaklıkmu(deg, 0, 40, 0)));
            if (sıcaklıkmu(deg, 20, 59) != 0)
                list.Add(new KeyValue(dusuk, sıcaklıkmu(deg, 20, 59)));
            if (sıcaklıkmu(deg, 50, 70) != 0)
                list.Add(new KeyValue(orta, sıcaklıkmu(deg, 50, 70)));
            if (sıcaklıkmu(deg, 60, 90) != 0)
                list.Add(new KeyValue(yuksek, sıcaklıkmu(deg, 60, 90)));
            if (sıcaklıkmu(deg, 80, 100, 100) != 0)
                list.Add(new KeyValue(cokyuksek, sıcaklıkmu(deg, 80, 100, 100)));
            return list;
        }
        List<KeyValue> sıcaklıkişlemmu(double deg)
        {
             List<KeyValue> str = new List<KeyValue>();
            if (sıcaklıkmu(deg, -10, 10, -10) != 0)
                str.Add(new KeyValue(cokdusuk, sıcaklıkmu(deg, -10, 10, -10)));
            if (sıcaklıkmu(deg, 0, 15) != 0)
                str.Add(new KeyValue(dusuk, sıcaklıkmu(deg, 0, 15)));
            if (sıcaklıkmu(deg, 14, 26) != 0)
                str.Add(new KeyValue(orta, sıcaklıkmu(deg, 14, 26)));
            if (sıcaklıkmu(deg, 20, 46) != 0)
                str.Add(new KeyValue(yuksek, sıcaklıkmu(deg, 20, 46)));
            if (sıcaklıkmu(deg, 30, 50, 50) != 0)
                str.Add(new KeyValue(cokyuksek, sıcaklıkmu(deg, 30, 50, 50)));
            return str;
        }
        #region
        const string cokdusuk = "Çok Düşük";
        const string dusuk = "Düşük";
        const string orta = "Orta";
        const string yuksek = "Yüksek";
        const string cokyuksek = "Çok Yüksek";
        Color mavi = Color.Blue;
        Color kırmızı = Color.Red;
        Color mor = Color.DarkViolet;
        Color yeşil = Color.ForestGreen;
        Color turuncu = Color.DarkOrange;
        #endregion
        KeyValue Rulesmu(KeyValue smu,KeyValue hmu)
        {
            string sıcaklık = smu.key;
            string havanemi = hmu.key;
            string ısıtma = "";
            if (sıcaklık == cokdusuk)
            {
                switch (havanemi)
                {
                    case cokdusuk: ısıtma = cokyuksek; break;
                    case dusuk: ısıtma = cokyuksek; break;
                    case orta: ısıtma = yuksek; break;
                    case yuksek: ısıtma = yuksek; break;
                    case cokyuksek: ısıtma = yuksek; break;
                    default:
                        break;
                }
            }
            if (sıcaklık == dusuk)
            {
                switch (havanemi)
                {
                    case cokdusuk: ısıtma = yuksek; break;
                    case dusuk: ısıtma = yuksek; break;
                    case orta: ısıtma = yuksek; break;
                    case yuksek: ısıtma = orta; break;
                    case cokyuksek: ısıtma = orta; break;
                    default:
                        break;
                }
            }
            if (sıcaklık == orta)
            {
                switch (havanemi)
                {
                    case cokdusuk: ısıtma = orta; break;
                    case dusuk: ısıtma = orta; break;
                    case orta: ısıtma = orta; break;
                    case yuksek: ısıtma = orta; break;
                    case cokyuksek: ısıtma = dusuk; break;
                    default:
                        break;
                }
            }
            if (sıcaklık == yuksek)
            {
                ısıtma = dusuk;
            }
            if (sıcaklık == cokyuksek)
            {
                ısıtma = cokdusuk;
            }
            return new KeyValue(ısıtma, Math.Min(smu.deger, hmu.deger));
        }
        #region
        void sıcaklık(Graphics graphics)
        {
            List<double> range = new List<double>() {-10, 50};
            cizim(graphics, new List<double>() {-10,-10,0,10},cokdusuk, mavi, range);
            cizim(graphics, new List<double>() {0,7.5,15},dusuk ,kırmızı, range);
            cizim(graphics, new List<double>() {14,20,26}, orta,mor, range,-5);
            cizim(graphics, new List<double>() {20,30,40},yuksek ,yeşil, range);
            cizim(graphics, new List<double>() {30,40,50,50}, cokyuksek,turuncu, range);
        }
        void havanemi(Graphics graphics)
        {
            List<double> range = new List<double>() { 0, 100 };
            cizim(graphics, new List<double>() { 0, 0, 20, 40 }, cokdusuk,mavi, range);
            cizim(graphics, new List<double>() { 20, 39.5, 59 }, dusuk, kırmızı, range, -10);
            cizim(graphics, new List<double>() { 50, 60, 70 }, orta, mor, range);
            cizim(graphics, new List<double>() { 60, 75, 90 }, yuksek, yeşil, range);
            cizim(graphics, new List<double>() { 80, 90, 100, 100 }, cokyuksek, turuncu, range);
        }
        void Isıtma(Graphics graphics)
        {
            List<double> range = new List<double>() { 0, 6 };
            cizim(graphics, new List<double>() { 0, 0, 0.5, 1 }, cokdusuk, mavi, range);
            cizim(graphics, new List<double>() { 0, 1.5, 3 }, dusuk, kırmızı, range);
            cizim(graphics, new List<double>() { 2, 3, 4 }, orta, mor, range);
            cizim(graphics, new List<double>() { 3.5, 4.25, 5 }, yuksek, yeşil, range);
            cizim(graphics, new List<double>() { 4.5, 5.25, 6, 6 }, cokyuksek, turuncu, range);
        }
        private void cizim(Graphics sıcaklık, List<double> Params,string name,Color pencolor,List<double> Range,int tdeger=0)
        {
            List<double> pts = Params;
            Graphics gfx = sıcaklık;
            Point[] points = new Point[pts.Count];
            for (int i = 0; i < pts.Count; i++)
            {
                double value = (pts[i] - Range[0]) / (Range[1] - Range[0]);
                int pos = Convert.ToInt32(value * (panel1.Bounds.Width));
                if (i == 0 || i == pts.Count - 1)
                {
                    points[i] = new Point(pos, panel1.Bounds.Height - 20);
                }
                else
                {
                    points[i] = new Point(pos, 40);
                }
            }
            gfx.DrawLines(new Pen(pencolor,2), points);
            int diff = Convert.ToInt32((points[pts.Count - 1].X - points[0].X) / 2);
            int strvalue = points[0].X + diff;
            Font font = new Font("SanSerif", 10, FontStyle.Italic);
            gfx.DrawString(name, font, new SolidBrush(pencolor), new PointF(strvalue - 25, 20));
            gfx.DrawString(Params[0].ToString(), font, new SolidBrush(pencolor), new PointF(points[0].X+tdeger , panel1.Height - 15));
            if(points.Length>3)
                gfx.DrawString(Params[3].ToString(), font, new SolidBrush(pencolor), new PointF(points[3].X+tdeger, panel1.Height - 15));
            else
                gfx.DrawString(Params[2].ToString(), font, new SolidBrush(pencolor), new PointF(points[2].X + tdeger, panel1.Height - 15));
            gfx.DrawLine(new Pen(new SolidBrush(Color.Black)), new Point(0, panel1.Height - 19), new Point(panel1.Width, panel1.Height - 19));
        }
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            label_1.ForeColor = label_11.ForeColor = mavi;
            label_2.ForeColor = label_21.ForeColor = kırmızı;
            label_3.ForeColor = label_31.ForeColor = mor;
            label_4.ForeColor = label_41.ForeColor = yeşil;
            label_5.ForeColor = label_51.ForeColor = turuncu;
            getgrafik();
        }
        void getgrafik()
        {
                sıcaklık(graphics1);
                havanemi(graphics2);
                Isıtma(graphics3); 
        }
        private void button2_Click(object sender, EventArgs e)
        {
            getgrafik();
        }
    }

}
