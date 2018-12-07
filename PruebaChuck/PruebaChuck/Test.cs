using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using System.Globalization;

namespace PruebaChuck
{
    public partial class Form1 : Form
    {
        CultureInfo myCIintl = new CultureInfo("en-GB", false);
        public static Uri ProxyString { get; private set; }

        //Entidad listas de frases
        public class ListFacts
        {
            public List<Facts>[] result { get; set; }
        }

        //Entidad con las frases
        public class Facts
        {
            public string [] Category { get; set; }
            public string Icon_url { get; set; }
            public string Url { get; set; }
            public string Value { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
        }

        //Conexión con la API
        public static string HttpGet(string URI)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            req.Proxy = new System.Net.WebProxy(ProxyString, true); //true means no proxy
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        //Función botón buscar
        private void buscar_Click(object sender, EventArgs e)
        {
            try
            {
                string URI = "";

                //Validaciones
                if (random.Checked)
                {
                    //Esto estaria mejor en un fichero config indicando las diferentes urls.

                    //Fact Aleatorio
                    URI = "https://api.chucknorris.io/jokes/random";
                    string textJson = HttpGet(URI);

                    Facts facts = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Facts>(textJson);
                    LoadGrid(facts);
                }
                else if (!String.IsNullOrEmpty(category.Text))
                {
                    //Fact por categoria, el combo categorias podria cargarse de la API.
                    URI = "https://api.chucknorris.io/jokes/random?category=" + category.Text;
                    string textJson = HttpGet(URI);

                    Facts facts = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Facts>(textJson);
                    LoadGrid(facts);
                }
                else if (!String.IsNullOrEmpty(freeText.Text))
                {
                    //Fact por texto libre
                    URI = "https://api.chucknorris.io/jokes/search?query=" + freeText.Text;
                    string textJson = HttpGet(URI);

                    //Nos devuelve mas de una linea
                    ListFacts facts = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<ListFacts>(textJson);

                    //ListFacts Facts = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<ListFacts>(HttpGet(URI));

                    //foreach (var item in ListFacts)
                    //{
                    //    Console.WriteLine("Category: {0}, Icon_url: {1}, Url: {2}, Value: {3}", item.Category, item.Icon_url, item.Url, item.Value);
                    //}
                }
                else
                {
                    MessageBox.Show("Debe seleccíonar un filtro.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //Carga el grid
        public void LoadGrid(Facts facts)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Facts");
                DataRow row = dt.NewRow();
                row["Facts"] = facts.Value;
                dt.Rows.Add(row);

                dataGridView1.DataSource = dt;
                dataGridView1.AutoResizeColumns();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void random_CheckedChanged(object sender, EventArgs e)
        {
            if (random.Checked)
            {
                category.Text = "";
                freeText.Text = "";
            }
            else
            {
                random.Checked = false;
            }
        }

        private void category_SelectedIndexChanged(object sender, EventArgs e)
        {
            // no vamos a permitir que se realicen busquedas random por categoria
            random.Checked = false;
            freeText.Text = "";
        }

        private void freeText_TextChanged(object sender, EventArgs e)
        {
            random.Checked = false;
            category.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            myCIintl = new CultureInfo("en-GB", false);
            language.SelectedIndex = 0;
        }

        //Cambiar idioma segun cultureinfo
        private void language_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (language.Text)
            {
                case "English":
                    myCIintl = new CultureInfo("en-GB", false);
                    label1.Text = "Category:";
                    label2.Text = "Free text:";
                    label3.Text = "Language:";
                    random.Text = "Random";
                    buscar.Text = "Search";
                    button1.Text = "Email";
                    break;
                case "Spanish":
                    myCIintl = new CultureInfo("es-ES", false);
                    label1.Text = "Categoría:";
                    label2.Text = "Texto Libre:";
                    label3.Text = "Idioma:";
                    random.Text = "Aleatorio";
                    buscar.Text = "Buscar";
                    button1.Text = "Correo";
                    break;
            }
        }

        private void email_Click(object sender, EventArgs e)
        {

        }
    }

}
