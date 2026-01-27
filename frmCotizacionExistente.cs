using iTextSharp.text.pdf;
using iTextSharp.text;
using LogicaVentasAdmin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rectangle = iTextSharp.text.Rectangle;

namespace ModuloVentasAdmin
{
    public partial class frmCotizacionExistente : Form
    {

        private int pageSize = 100;
        private int currentPage = 1;
        private int totalPages = 0;
        public bool _TerminosExisten, _EnviaWSP = false;
        private DataTable dtCotizaciones;
        public DataTable dtTerminos = new DataTable();
        clsLogica logica = new clsLogica();

        public frmCotizacionExistente()
        {
            InitializeComponent();
            cargarTerminos();
        }

        private void btnCliente_Click(object sender, EventArgs e)
        {
            Form MensajeAdvertencia = new Form();
            using (frmBuscarClientes Mensaje = new frmBuscarClientes())
            {
                MensajeAdvertencia.StartPosition = FormStartPosition.CenterScreen;
                MensajeAdvertencia.FormBorderStyle = FormBorderStyle.None;
                MensajeAdvertencia.Opacity = .70d;
                MensajeAdvertencia.BackColor = Color.Black;
                MensajeAdvertencia.WindowState = FormWindowState.Maximized;
                MensajeAdvertencia.Location = this.Location;
                MensajeAdvertencia.ShowInTaskbar = false;
                Mensaje.Owner = MensajeAdvertencia;
                MensajeAdvertencia.Show();

                if (Mensaje.ShowDialog() == DialogResult.OK)
                {
                    if (Mensaje.ClienteId != 0)
                    {
                        txtClienteNombre.Text = Mensaje.ClienteNombre;
                        txtClienteID.Text = Mensaje.ClienteId.ToString();
                        cargarClientes();
                    }
                }

                MensajeAdvertencia.Dispose();
            }
        }

        private void txtClienteID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (String.IsNullOrWhiteSpace(txtClienteID.Text))
                    return;

                e.SuppressKeyPress = true;
                cargarClientes();
            }
        }
        private void cargarClientes()
        {
            string _Opcion = "BuscarPorCodigoLigero";

            ClienteENAC getClientes = new ClienteENAC
            {
                Opcion = _Opcion,
                Cliente = txtClienteID.Text,
                Nombre = txtClienteID.Text,
            };
            DataTable dtGetCliente = logica.SP_ClientesENAC(getClientes);
            if (dtGetCliente.Rows.Count > 0)
            {
                txtClienteID.Text = dtGetCliente.Rows[0]["ClienteID"].ToString();
                txtClienteNombre.Text = dtGetCliente.Rows[0]["NombreCompleto"].ToString();
                cargarCotizaciones();
            }
            else
            {
                Toast.Mostrar("No se encontro ese codigo de cliente.", TipoAlerta.Warning);
                txtClienteNombre.Text = String.Empty;
                txtClienteID.Text = String.Empty;
                txtClienteID.Focus();
            }
        }

        private void cargarCotizaciones()
        {
            string _Opcion = "getClientesNegociaciones";

            ClienteENAC getCotizacion = new ClienteENAC
            {
                Opcion = _Opcion,
                Cliente = txtClienteID.Text,
            };
            dtCotizaciones = logica.SP_ClientesENAC(getCotizacion);

            if (dtCotizaciones != null && dtCotizaciones.Rows.Count > 0)
            {
                totalPages = (int)Math.Ceiling((double)dtCotizaciones.Rows.Count / pageSize);
                mostrarPagina(currentPage);
                Toast.Mostrar("Cotización cargada exitosamente.", TipoAlerta.Success);
            }
            else
            {
                dgvCotizaciones.DataSource = null;
                lblPagina.Text = "Sin registros";
            }
        }
        private void cargarTerminos()
        {
            CotizacionTerminoDTO getTerminos = new CotizacionTerminoDTO
            {
                Opcion = "Listar"
            };
            dtTerminos = logica.SP_CotizacionTerminos(getTerminos);
            if (dtTerminos.Rows.Count > 0)
            {
                _TerminosExisten = true;
            }
        }
        private byte[] GenerarPDFCotizacionesAgrupado(DataTable dtCotizaciones, DataTable dtTerminos)
        {
           
            using (MemoryStream ms = new MemoryStream())
            {
                // 🔹 Detectar cantidad máxima de productos
                int maxProductos = dtCotizaciones.AsEnumerable()
                    .GroupBy(r => r["OrigenID"].ToString())
                    .Select(g => g.Select(r => r["Producto"].ToString()).Distinct().Count())
                    .DefaultIfEmpty(0)
                    .Max();

                // 🔹 Si hay más de 8 productos, usar orientación horizontal
                Document doc = maxProductos > 8
                    ? new Document(PageSize.LEGAL.Rotate(), 20, 20, 20, 20)
                    : new Document(PageSize.LETTER, 40, 40, 40, 40);

                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();
                doc.Open();

                // 🔹 Definir fuentes según cantidad de columnas
                iTextSharp.text.Font destinoHeader, fontHeader, fontCell;

                if (maxProductos > 8)
                {
                    destinoHeader = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);
                    fontHeader = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 6);
                    fontCell = FontFactory.GetFont(FontFactory.HELVETICA, 8);
                }
                else
                {
                    destinoHeader = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);
                    fontHeader = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);
                    fontCell = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                }


                // 🔹 Encabezado con logos
                PdfPTable headerTable = new PdfPTable(2);
                headerTable.WidthPercentage = 100;
                headerTable.SetWidths(new float[] { 150f, 150f });

                iTextSharp.text.Image logoIzq = iTextSharp.text.Image.GetInstance(@"\\192.168.1.179\Logos\Rapido cargo HONDURAS.png");
                logoIzq.ScaleAbsolute(140, 60);
                headerTable.AddCell(new PdfPCell(logoIzq) { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT });

                iTextSharp.text.Image logoDer = iTextSharp.text.Image.GetInstance(@"\\192.168.1.179\Logos\Logo Rapido Cargo Paqueteria.png");
                logoDer.ScaleAbsolute(150, 40);
                headerTable.AddCell(new PdfPCell(logoDer) { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT });

                doc.Add(headerTable);

                // 🔹 Título
                var fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                doc.Add(new Paragraph("NEGOCIACION ESPECIAL ACTIVA", fontTitulo)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingBefore = 10f,
                    SpacingAfter = 20f
                });

                // 🔹 Fuente bold tamaño 9
                var fontCellBold9 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);

                // 🔹 Cliente y Fecha
                PdfPTable clienteFechaTable = new PdfPTable(2);
                clienteFechaTable.WidthPercentage = 100;
                clienteFechaTable.SetWidths(new float[] { 250f, 180f });

                clienteFechaTable.AddCell(new PdfPCell(new Phrase("CLIENTE: " + txtClienteNombre.Text, fontCellBold9))
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT
                });

                clienteFechaTable.AddCell(new PdfPCell(new Phrase("FECHA: " + DateTime.Now.ToString("dd/MM/yyyy"), fontCellBold9))
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_RIGHT
                });

                doc.Add(clienteFechaTable);

                // 🔹 Agrupar por OrigenID
                var origenes = dtCotizaciones.AsEnumerable()
                    .GroupBy(r => new { OrigenID = r["OrigenID"].ToString(), OrigenNombre = r["Origen"].ToString() });

                foreach (var grupoOrigen in origenes)
                {
                    string origenNombre = grupoOrigen.Key.OrigenNombre;

                    // 🔹 Título por origen
                    doc.Add(new Paragraph($"ORIGEN: {origenNombre}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))
                    {
                        SpacingBefore = 5f,
                        SpacingAfter = 5f,
                        Alignment = Element.ALIGN_LEFT
                    });

                    var productos = grupoOrigen.Select(r => r["Producto"].ToString()).Distinct().ToList();
                    var destinos = grupoOrigen.Select(r => r["Destino"].ToString()).Distinct().ToList();

                    PdfPTable table = new PdfPTable(productos.Count + 1);
                    table.WidthPercentage = 100;

                    float[] widths = new float[productos.Count + 1];
                    widths[0] = 200f;
                    for (int i = 1; i < widths.Length; i++) widths[i] = 50f;
                    table.SetWidths(widths);

                    // 🔹 Encabezado
                    table.AddCell(new PdfPCell(new Phrase("DESTINO", destinoHeader))
                    {
                        BackgroundColor = new BaseColor(185, 203, 226),
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    });

                    foreach (var prod in productos)
                    {
                        table.AddCell(new PdfPCell(new Phrase(prod, fontHeader))
                        {
                            BackgroundColor = new BaseColor(185, 203, 226),
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        });
                    }

                    // 🔹 Filas por destino
                    var fontDestino = FontFactory.GetFont(FontFactory.HELVETICA, 7);

                    foreach (var destino in destinos)
                    {
                        table.AddCell(new PdfPCell(new Phrase(destino, fontDestino))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        });

                        // Resto de columnas: costos con fuente normal
                        foreach (var prod in productos)
                        {
                            var match = grupoOrigen.FirstOrDefault(r =>
                                r["Destino"].ToString() == destino &&
                                r["Producto"].ToString() == prod);

                            string costo = match != null ? Convert.ToDecimal(match["Costo"]).ToString("N2") : "-";

                            table.AddCell(new PdfPCell(new Phrase(costo, fontCell))
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                VerticalAlignment = Element.ALIGN_MIDDLE
                            });
                        }
                    }

                    doc.Add(table);
                    doc.Add(new Paragraph("\n"));
                }

                // 🔹 Términos y condiciones
                if (dtTerminos != null && dtTerminos.Rows.Count > 0)
                {
                    doc.Add(new Paragraph("\n"));
                    var fontTituloTerminos = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11);
                    doc.Add(new Paragraph("TERMINOS Y NEGOCIACIONES ESPECIALES", fontTituloTerminos));

                    doc.Add(new Paragraph("PRECIOS NO INCLUYEN IMPUESTO SOBRE VENTAS", FontFactory.GetFont(FontFactory.HELVETICA, 9)));
                    foreach (DataRow row in dtTerminos.Rows)
                    {
                        doc.Add(new Paragraph("- " + row["Descripcion"].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 9)));
                    }
                }
                doc.Add(new Paragraph("\n\n\n"));
                PdfPTable firmasTable = new PdfPTable(2);
                firmasTable.WidthPercentage = 100; firmasTable.SetWidths(new float[] { 250f, 250f });
                PdfPCell cellLineaIzq = new PdfPCell { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
                cellLineaIzq.AddElement(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(1f, 60f, BaseColor.BLACK, Element.ALIGN_CENTER, -2))));
                cellLineaIzq.AddElement(new Paragraph("Autorizado", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)) { Alignment = Element.ALIGN_CENTER }); 
                cellLineaIzq.AddElement(new Paragraph("Nancy D. Valle", FontFactory.GetFont(FontFactory.HELVETICA, 9)) { Alignment = Element.ALIGN_CENTER }); 
                cellLineaIzq.AddElement(new Paragraph("GERENTE ADMINISTRATIVO", FontFactory.GetFont(FontFactory.HELVETICA, 9)) { Alignment = Element.ALIGN_CENTER });
                firmasTable.AddCell(cellLineaIzq);

                PdfPCell cellLineaDer = new PdfPCell { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER }; 
                cellLineaDer.AddElement(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(1f, 60f, BaseColor.BLACK, Element.ALIGN_CENTER, -2)))); 
                cellLineaDer.AddElement(new Paragraph("Aprobado Por Cliente", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)) { Alignment = Element.ALIGN_CENTER }); 
                cellLineaDer.AddElement(new Paragraph("Firma y Sello", FontFactory.GetFont(FontFactory.HELVETICA, 9)) { Alignment = Element.ALIGN_CENTER }); 
                cellLineaDer.AddElement(new Paragraph(txtClienteNombre.Text, FontFactory.GetFont(FontFactory.HELVETICA, 9)) { Alignment = Element.ALIGN_CENTER }); 
                firmasTable.AddCell(cellLineaDer); doc.Add(firmasTable);

                doc.Close();
                return ms.ToArray();
            }
        }

        private void MostrarPDF(byte[] pdfBytes)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), "CotizacionTemp.pdf");
            File.WriteAllBytes(tempPath, pdfBytes);
            System.Diagnostics.Process.Start(tempPath); // abre con el visor de PDF instalado
        }
        private void EnviarPDF(byte[] pdfBytes)
        {
            string rutaDestino = @"\\192.168.1.179\CotizacionesEspecificas";
            Directory.CreateDirectory(rutaDestino);

            string nombreArchivo = "Cotizacion-Temporal.pdf";
            string path = Path.Combine(rutaDestino, nombreArchivo);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            // 🔹 Guardar temporalmente el PDF
            File.WriteAllBytes(path, pdfBytes);
            enviarCotizacion(path, nombreArchivo);
        }

        private void enviarCotizacion(String RutaArchivo, String NombreArchivo)
        {
            Form MensajeAdvertencia = new Form();
            using (frmMensajePersonalizado Mensaje = new frmMensajePersonalizado(RutaArchivo, NombreArchivo))
            {
                MensajeAdvertencia.StartPosition = FormStartPosition.CenterScreen;
                MensajeAdvertencia.FormBorderStyle = FormBorderStyle.None;
                MensajeAdvertencia.Opacity = .70d;
                MensajeAdvertencia.BackColor = Color.Black;
                MensajeAdvertencia.WindowState = FormWindowState.Maximized;
                MensajeAdvertencia.Location = this.Location;
                MensajeAdvertencia.ShowInTaskbar = false;
                Mensaje.Owner = MensajeAdvertencia;
                MensajeAdvertencia.Show();
                Mensaje.ShowDialog();
                MensajeAdvertencia.Dispose();
            }
        }


        private void mostrarPagina(int page)
        {
            if (dtCotizaciones == null || dtCotizaciones.Rows.Count == 0)
                return;

            int startIndex = (page - 1) * pageSize;
            int endIndex = Math.Min(startIndex + pageSize, dtCotizaciones.Rows.Count);

            DataTable dtPagina = dtCotizaciones.Clone(); // misma estructura

            for (int i = startIndex; i < endIndex; i++)
            {
                dtPagina.ImportRow(dtCotizaciones.Rows[i]);
            }

            dgvCotizaciones.DataSource = dtPagina;

            // Configuración de columnas
            dgvCotizaciones.Columns["OrigenID"].Visible = false;
            dgvCotizaciones.Columns["DestinoID"].Visible = false;
            dgvCotizaciones.Columns["Origen"].Width = 250;
            dgvCotizaciones.Columns["Destino"].Width = 250;
            dgvCotizaciones.Columns["Producto"].Width = 250;
            dgvCotizaciones.Columns["Costo"].Width = 120;

            // 🔹 actualizar label
            lblPagina.Text = $"Página {currentPage} de {totalPages}";

        }


        // 🔹 Botón siguiente
        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if ((currentPage * pageSize) < dtCotizaciones.Rows.Count)
            {
                currentPage++;
                mostrarPagina(currentPage);
            }
        }

        // 🔹 Botón anterior
        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                mostrarPagina(currentPage);
            }
        }

        private void txtClienteID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void frmCotizacionExistente_Load(object sender, EventArgs e)
        {

        }

        private void txtClienteID_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true; // 🔹 obliga a tratar Tab como tecla de entrada
            }
        }

        private async void btnPDF_Click(object sender, EventArgs e)
        {
            if (dgvCotizaciones.Rows.Count <= 0)
            {
                Toast.Mostrar("No se obtuvo informacion de este cliente.", TipoAlerta.Warning);
                return;
            }

            Toast.Mostrar("Generando Archivo PDF, por favor espere...", TipoAlerta.Info);
            Cursor.Current = Cursors.WaitCursor;
            btnPDF.Enabled = false;
            try
            {
                // 🔹 Generar PDF en segundo plano
                var pdfBytes = await Task.Run(() => GenerarPDFCotizacionesAgrupado(dtCotizaciones, dtTerminos));

                MostrarPDF(pdfBytes);

                Toast.Mostrar("Archivo PDF generado exitosamente.", TipoAlerta.Success);
                btnPDF.Enabled = true;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnProcesar_Click(object sender, EventArgs e)
        {
            _EnviaWSP = true;
            EnviarPDF(GenerarPDFCotizacionesAgrupado(dtCotizaciones, dtTerminos));
        }

        private void rbdNombre_CheckedChanged(object sender, EventArgs e)
        {
            if (rbdNombre.Checked)
            {
                txtClienteID.ReadOnly = true;

                Form MensajeAdvertencia = new Form();
                using (frmBuscarClientes Mensaje = new frmBuscarClientes())
                {
                    MensajeAdvertencia.StartPosition = FormStartPosition.CenterScreen;
                    MensajeAdvertencia.FormBorderStyle = FormBorderStyle.None;
                    MensajeAdvertencia.Opacity = .70d;
                    MensajeAdvertencia.BackColor = Color.Black;
                    MensajeAdvertencia.WindowState = FormWindowState.Maximized;
                    MensajeAdvertencia.Location = this.Location;
                    MensajeAdvertencia.ShowInTaskbar = false;
                    Mensaje.Owner = MensajeAdvertencia;
                    MensajeAdvertencia.Show();

                    if (Mensaje.ShowDialog() == DialogResult.OK)
                    {
                        if (Mensaje.ClienteId != 0)
                        {
                            txtClienteNombre.Text = Mensaje.ClienteNombre;
                            txtClienteID.Text = Mensaje.ClienteId.ToString();
                            cargarClientes();
                        }
                    }

                    MensajeAdvertencia.Dispose();
                }
            }
        }

        private void chkAplicar_CheckedChanged(object sender, EventArgs e)
        {
            // 🔹 Checked devuelve true/false
            txtDescuento.Enabled = chkAplicar.Checked;
            btnAplicar.Enabled = chkAplicar.Checked;
        }
        private void txtDescuento_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 🔹 Permitir control de teclas como Backspace
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            // 🔹 Permitir dígitos
            if (char.IsDigit(e.KeyChar))
            {
                return;
            }

            // 🔹 Permitir un solo punto decimal
            if (e.KeyChar == '.' && !txtDescuento.Text.Contains("."))
            {
                return;
            }

            // 🔹 Bloquear cualquier otro carácter
            e.Handled = true;
        }


        private void rdbCodigo_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCodigo.Checked)
            {
                txtClienteID.ReadOnly = false;
            }
        }

       
    }
}
