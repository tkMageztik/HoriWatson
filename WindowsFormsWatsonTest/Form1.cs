using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Net.Http;
using System.Threading;

namespace WindowsFormsWatsonTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //Verificar la correcta manera de hacer el primer request.
            enviarMensaje("");
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                /*TestWatson1.Program p = new TestWatson1.Program();
                
                TestWatson1.Program.enviarMensaje(textBox1.Text);
                textBox2.Text = p.mensaje;*/

                enviarMensaje(textBox1.Text);
                textBox1.Text = "";
            }
        }

        public static async Task enviarMensaje2() {

        }

        private void textoAVoz()
        {

            var baseurl = "https://stream.watsonplatform.net/text-to-speech/api";

        }

        public async Task enviarMensaje(string mensaje)
        {
            var baseurl = "https://gateway.watsonplatform.net/conversation/api";
            //var workspace = "4e612f8f-588f-4ba3-8b77-d7c3db6ec47b";

            //Demo Luis Alvarado
            //var workspace = "f8a11222-610b-4974-9ee7-db35ff9c10ee";
            //Ejemplo carro IBM
            //var workspace = "7e51f107-62df-4b23-8d55-4add0684c844";
            //Demo Juan RDC
            //var workspace = "57f1b180-ddc8-497d-a599-ed2c2adf308c";
            var workspace = "6dcec688-83d2-4cd0-99c5-66e90278f0e7";

            //var username = "62b81250-24b9-430f-8a41-dd73bd2f519a";
            //var username = "c3b01d3d-50fb-4f80-b889-d5b53b49bffd";
            var username = "6dae1a60-2bfa-4f62-890b-7be7048104ef";

            //var password = "A2NVe73JNdKL";
            //var password = "rOEOXpytY50p";
            var password = "uuiT4XrIZ5Wa";
            var context = null as object;
            var entities = null as object;
            var intents = null as object;
            var output = null as object;

            var var_context = new
            {
                usuario = default(string),
                usuario_temp = default(string),
                action = default(string),
                option = default(string)
            };

            if (this.contexto != null)
            {
                var_context = JsonConvert.DeserializeAnonymousType(this.contexto.ToString(),var_context);

                if (var_context.usuario == "off")
                {
                    string usuario = null;

                    if (var_context.usuario_temp != "" && var_context.usuario_temp != null)
                    {
                        if (textBox1.Text.ToUpper() == "SI")
                        {
                            listaUsuarios().TryGetValue(var_context.usuario_temp.ToUpper(), out usuario);
                        }
                    }
                    else
                    {
                        listaUsuarios().TryGetValue(textBox1.Text.ToUpper(), out usuario);
                    }

                    if (usuario != null)
                    {
                        if (var_context.usuario_temp != "" && var_context.usuario_temp != null)
                        {
                            this.contexto = JsonConvert.DeserializeObject(this.contexto.ToString().Replace("\"usuario\": \"off\"", "\"usuario\": \"" + usuario + "\""));
                            this.contexto = JsonConvert.DeserializeObject(this.contexto.ToString().Replace("\"usuario_temp\": \"" + var_context.usuario_temp + "\"", "\"usuario_temp\": \"\""));
                            this.contexto = JsonConvert.DeserializeObject(this.contexto.ToString().Replace("\"action\": \"\"", "\"action\": \"inicio\""));
                        }
                        else
                        {
                            this.contexto = JsonConvert.DeserializeObject(this.contexto.ToString().Replace("\"usuario\": \"off\"", "\"usuario\": \"" + usuario + "\""));
                            this.contexto = JsonConvert.DeserializeObject(this.contexto.ToString().Replace("\"action\": \"\"", "\"action\": \"inicio\""));
                        }

                        //this.contexto = JsonConvert.DeserializeObject(this.contexto.ToString().Replace("\"validando_ticket\": \"\"", "\"validando_ticket\": \"" + "on" + "\""));
                    }
                    else
                    {
                        //!#despedida
                        //this.contexto = JsonConvert.DeserializeObject(this.contexto.ToString().Replace("\"usuario\": \"\"", "\"usuario\": \"\""));
                        if (var_context.usuario_temp != "" && var_context.usuario_temp != null)
                        {
                            //this.contexto = JsonConvert.DeserializeObject(this.contexto.ToString().Replace("\"usuario\": \"off\"", "\"usuario\": \"" + usuario + "\""));
                            this.contexto = JsonConvert.DeserializeObject(this.contexto.ToString().Replace("\"usuario_temp\": \"" + var_context.usuario_temp + "\"", "\"usuario_temp\": \"\""));
                            this.contexto = JsonConvert.DeserializeObject(this.contexto.ToString().Replace("\"action\": \"\"", "\"action\": \"inicio_error\""));
                        }
                        else
                        {
                            //this.contexto = JsonConvert.DeserializeObject(this.contexto.ToString().Replace("\"usuario\": \"off\"", "\"usuario\": \"" + usuario + "\""));
                            this.contexto = JsonConvert.DeserializeObject(this.contexto.ToString().Replace("\"action\": \"\"", "\"action\": \"inicio_error\""));
                        }

                    }
                }


                if (var_context.option == "" || var_context.option == null)
                {
                    /*if (this.contexto.ToString().IndexOf("Reset Farewells Or Negative Decision Replies") == -1)
                    {*/
                        this.contexto = JsonConvert.DeserializeObject(this.contexto.ToString().Replace("\"option\": \"\"", "\"option\": \"opciones\""));
                    /*}*/
                }
                else
                {

                }
            }



            if (this.contexto != null)
            {
                context = this.contexto;
            }


            var message = new
            {
                input = new { text = mensaje },
                context/*,
                entities,
                intents,
                output*/
            };


            var resp = await baseurl
                .AppendPathSegments("v1", "workspaces", workspace, "message")

                //revisar en que difiere la fecha, supuestamente es fecha (versión) de la api a la que se consulta
                .SetQueryParam("version", "2017-05-26")
                .WithBasicAuth(username, password)
                .AllowAnyHttpStatus()
                .PostJsonAsync(message);

            //JsonConvert.SerializeObject(product, Formatting.Indented)
            var json = await resp.Content.ReadAsStringAsync();

            var answer = new
            {
                intents = default(object),
                entities = default(object),
                input = default(object),
                output = new
                {
                    text = default(string[]),
                    nodes_visited = default(string[])
                },
                context = default(object)

            };

            var answer2 = new
            {
                intents = default(object),
                entities = default(object),
                input = default(object),
                output = new
                {
                    text = default(string[]),
                    nodes_visited = default(string[])
                },
                context = default(object)
            };



            var var_input = new
            {
                text = default(string)
            };

            //string t = "{\"intents\":[],\"entities\":[],\"input\":{\"text\":\"\"},\"output\":{\"text\":[\"Hi. It looks like a nice drive today. What would you like me to do?  \"],\"nodes_visited\":[\"Start And Initialize Context\"],\"log_messages\":[]},\"context\":{\"timezone\":\"America/Lima\",\"conversation_id\":\"64f1b515-2352-4f14-9f17-b8daf0c46640\",\"system\":{\"dialog_stack\":[{\"dialog_node\":\"root\"}],\"dialog_turn_counter\":1,\"dialog_request_counter\":1,\"_node_output_map\":{\"Start And Initialize Context\":[0,0]},\"branch_exited\":true,\"branch_exited_reason\":\"completed\"},\"AConoff\":\"off\",\"lightonoff\":\"off\",\"musiconoff\":\"off\",\"appl_action\":\"\",\"heateronoff\":\"off\",\"volumeonoff\":\"off\",\"wipersonoff\":\"off\",\"default_counter\":0}}";

            answer = JsonConvert.DeserializeAnonymousType(json, answer);
            //var newJson = JsonConvert.SerializeObject(t);
            //answer2 = JsonConvert.DeserializeAnonymousType(newJson, answer2);

            txtJsonConsole.Text = answer.ToString();

            this.contexto = answer.context;
            this.intents = answer.intents;
            this.entities = answer.entities;
            this.output = answer.output;


            var_input = JsonConvert.DeserializeAnonymousType(answer.input.ToString(), var_input);


            var output_salida = answer?.output?.text?.Aggregate(
                new StringBuilder(),
                (sb, l) => sb.AppendLine(l),
                sb => sb.ToString());

            this.output_pantalla = output_salida;

            nodosFlujo = answer.output.nodes_visited;

            /*if (nodosFlujo.Contains("Valida_Usuario"))
            {
                string usuario;
                listaUsuarios().TryGetValue(var_input.text.ToUpper(), out usuario);

                if (usuario != null)
                {
                    this.contexto = JsonConvert.DeserializeObject(context.ToString().Replace("\"usuario\": \"\"", "\"usuario\": \"" + usuario + "\""));
                }
                else
                {
                    this.contexto = JsonConvert.DeserializeObject(context.ToString().Replace("\"usuario\": \"\"", "\"usuario\": \"\""));
                }
            }*/


            evaluaConsultas(mensaje);

            textBox2.AppendText((this.output_pantalla == "" ? output : this.output_pantalla) + "\r\n");

            //textBox2.AppendText();

            //+ " desde la bd devuelve>> " + mensaje2;
        }

        private void evaluaConsultas(string mensaje)
        {

            if (nodosFlujo.Contains("Bienvenido"))
            {


            }

            if (nodosFlujo.Contains("Response Wait Response from Backend - Ticket"))
            {

                ticket = Regex.Match(mensaje, @"\d+").ToString();
                DATA t = new WindowsFormsWatsonTest.DATA();
                //DataTable dt = t.D_ListarTickets(Regex.Match(ticket, @"\d+").Value);
                DataTable dt = t.D_ListarTickets(ticket);

                if (dt != null)
                {
                    /* output = output.Replace("ref_num", dt.Rows[0]["ref_num"].ToString())
                         .Replace("status", dt.Rows[0]["status"].ToString())
                         .Replace("Assigned to", dt.Rows[0]["Asignado Actual"].ToString());*/
                    if (dt.Rows.Count > 0)
                    {
                        output_pantalla = dt.Rows[0]["ref_num"].ToString() + " " +
                                dt.Rows[0]["status"].ToString() + " " +
                                dt.Rows[0]["Asignado Actual"].ToString();

                        //this.contexto = JsonConvert.DeserializeObject(context.ToString().Replace("\"usuario\": \"\"", "\"usuario\": \"" + usuario + "\""));

                    }
                    else
                    {
                        output_pantalla += "No existe el número de ticket ingresado \r\n";

                        //TODO: MEJORAR... 

                        this.contexto = JsonConvert.DeserializeObject(this.contexto.ToString().Replace("\"action\": \"obtener_ticket\"", "\"action\": \"evalua\""));

                        /*int indinicial = this.contexto.ToString().IndexOf("action");
                        int indfinal = this.contexto.ToString().IndexOf(",", indinicial);
                        
                        this.*/
                    }
                }
                else
                {
                    output_pantalla = "No existe el número de ticket ingresado \r\n";
                }
            }

            if (nodosFlujo.Contains("Response Wait Response from Backend - Tickets"))
            {
                output_pantalla += "Ticket 1 \r\n";
                output_pantalla += "Ticket 2 \r\n";
                output_pantalla += "Ticket 3 \r\n";
                output_pantalla += "Ticket 4 \r\n";
                output_pantalla += "Ticket 5 \r\n";

                this.contexto = JsonConvert.DeserializeObject(this.contexto.ToString().Replace("\"action\": \"listar_tickets\"", "\"action\": \"evalua\""));
            }

            if (nodosFlujo.Contains("Response Wait Response from Backend - Tickets by Status"))
            {
                output_pantalla += "Ticket 1 cerrado \r\n";

                this.contexto = JsonConvert.DeserializeObject(this.contexto.ToString().Replace("\"action\": \"listar_tickets_por_estado\"", "\"action\": \"evalua\""));
            }
        }

        public string[] nodosFlujo { set; get; }

        public string mensaje2 { set; get; }
        public object contexto { set; get; }
        public object entities { set; get; }
        public object intents { set; get; }
        public object output { set; get; }
        public string ticket { set; get; }
        public string output_pantalla { set; get; }
        public string usuario { set; get; }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private Dictionary<string, string> listaUsuarios()
        {
            Dictionary<string, string> usuarios = new Dictionary<string, string>();
            usuarios.Add("JUARUI", "Juan Ruiz de Castilla");
            usuarios.Add("HORPUG", "Horacio Puga");
            usuarios.Add("GROGUI", "Grover Gutierrez");
            return usuarios;
        }

    }
}
