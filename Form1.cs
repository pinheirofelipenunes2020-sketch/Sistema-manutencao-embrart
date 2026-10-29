using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ManutencaoPapelao
{
    public partial class Form1 : Form
    {
        string conexao = @"Data Source=DESKTOP-L6F3F9R;Initial Catalog=ManutencaoPapelao;Integrated Security=True";
        string senhaAdmin = "embrart123";
        TextBox txtQuem, txtMaquina, txtSetor, txtDescricao;
        ComboBox cmbTipo;
        Button btnSalvar, btnAmarelo, btnVerde, btnExcluir;
        DataGridView grid;
        Label lblA, lblB, lblC;

        public Form1() { InitializeComponent(); MontarTelaCompleta(); CarregarDados(); }

        void MontarTelaCompleta()
        {
            this.Text = "Embrart - Sistema de Manutenção - CIC Curitiba";
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(235, 238, 242);

            Panel header = new Panel() { Dock = DockStyle.Top, Height = 80, BackColor = Color.FromArgb(12, 25, 60) };
            Label logo = new Label() { Text = "📦 EMBRART EMBALAGENS INTELIGENTES", ForeColor = Color.FromArgb(255, 140, 0), Font = new Font("Segoe UI", 13, FontStyle.Bold), Location = new Point(20, 15), AutoSize = true };
            Label sub = new Label() { Text = "CIC Curitiba | Desde 1986 | ISO 9001 | Embalagens Logisticamente Perfeitas", ForeColor = Color.FromArgb(180, 200, 230), Font = new Font("Segoe UI", 8), Location = new Point(20, 48), AutoSize = true };
            header.Controls.Add(logo); header.Controls.Add(sub);

            Panel footer = new Panel() { Dock = DockStyle.Bottom, Height = 32, BackColor = Color.FromArgb(12, 25, 60) };
            Label ft = new Label() { Text = "EMBRART | Av. Juscelino K. Oliveira, 4329 - CIC | A MELHOR DO MERCADO DE PAPELÃO 📦 | Dica: Segure CTRL para selecionar várias linhas e excluir", ForeColor = Color.White, Font = new Font("Segoe UI", 7, FontStyle.Bold), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
            footer.Controls.Add(ft);

            Panel cardForm = new Panel() { Dock = DockStyle.Top, Height = 210, BackColor = Color.White, Padding = new Padding(15) };
            txtQuem = new TextBox() { Location = new Point(15, 28), Width = 190, Text = "Felipe Pinheiro", Font = new Font("Segoe UI", 10) };
            txtMaquina = new TextBox() { Location = new Point(220, 28), Width = 190, Text = "Onduladeira 2", Font = new Font("Segoe UI", 10) };
            txtSetor = new TextBox() { Location = new Point(425, 28), Width = 190, Text = "Corrugadeira", Font = new Font("Segoe UI", 10) };
            cmbTipo = new ComboBox() { Location = new Point(630, 28), Width = 160 }; cmbTipo.Items.AddRange(new string[] { "Elétrica", "Mecânica", "Parada", "Outros" });
            txtDescricao = new TextBox() { Location = new Point(15, 80), Width = 780, Height = 40, Multiline = true, Font = new Font("Segoe UI", 10), Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top };
            btnSalvar = CriarBotao("💾 SALVAR", Color.FromArgb(0, 160, 70), 15, 135);
            btnAmarelo = CriarBotao("🟡 EM ANDAMENTO", Color.FromArgb(255, 190, 0), 200, 135);
            btnVerde = CriarBotao("✅ FINALIZAR", Color.FromArgb(0, 130, 60), 385, 135);
            btnExcluir = CriarBotao("🗑️ EXCLUIR C/ SENHA", Color.FromArgb(200, 0, 0), 570, 135);
            btnSalvar.Click += BtnSalvar_Click; btnAmarelo.Click += BtnAmarelo_Click; btnVerde.Click += BtnVerde_Click; btnExcluir.Click += BtnExcluir_Click;
            cardForm.Controls.AddRange(new Control[] { Lbl("QUEM PEDIU", 15, 8), txtQuem, Lbl("MÁQUINA", 220, 8), txtMaquina, Lbl("SETOR", 425, 8), txtSetor, Lbl("TIPO", 630, 8), cmbTipo, Lbl("DESCREVA O PROBLEMA", 15, 60), txtDescricao, btnSalvar, btnAmarelo, btnVerde, btnExcluir });

            Panel meio = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(235, 238, 242), Padding = new Padding(15, 5, 15, 5) };
            meio.Controls.Add(Box("🔴 ABERTAS", "0", Color.FromArgb(200, 0, 0), 0, out lblA));
            meio.Controls.Add(Box("🟡 EM ANDAMENTO", "0", Color.FromArgb(180, 130, 0), 230, out lblB));
            meio.Controls.Add(Box("🟢 FECHADAS", "0", Color.FromArgb(0, 130, 60), 460, out lblC));
            meio.Controls.Add(Box("📦 EMBRART CIC", "Curitiba", Color.FromArgb(12, 25, 60), 690, out _));

            Panel cardGrid = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(10) };
            Label titGrid = new Label() { Text = "📋 FILA DE MANUTENÇÃO - SEGURE CTRL E CLIQUE EM VÁRIAS LINHAS PARA EXCLUIR VÁRIAS DE UMA VEZ", Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = Color.FromArgb(12, 25, 60), Dock = DockStyle.Top, Height = 25 };
            grid = new DataGridView() { Dock = DockStyle.Top, Height = 280, BackgroundColor = Color.White, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = true, RowTemplate = { Height = 35 }, EnableHeadersVisualStyles = false, ColumnHeadersHeight = 32, BorderStyle = BorderStyle.FixedSingle };
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(12, 25, 60); grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            Panel painelDecorativo = new Panel() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(245, 247, 250) };
            Label icones = new Label() { Text = "📦  📦  📦          ⚙️  🔧  🛠️  🔩  ⚙️          📦  📦  📦", Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.FromArgb(200, 210, 220), Dock = DockStyle.Top, Height = 40, TextAlign = ContentAlignment.MiddleCenter };
            Label fundo = new Label() { Text = "EMBRART EMBALAGENS INTELIGENTES - CIC CURITIBA\nDesde 1986 | ISO 9001 | A MELHOR DO MERCADO DE PAPELÃO\n🔧 Segure CTRL e selecione várias linhas para excluir 🔧", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(150, 170, 190), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
            painelDecorativo.Controls.Add(fundo); painelDecorativo.Controls.Add(icones);
            cardGrid.Controls.Add(painelDecorativo); cardGrid.Controls.Add(grid); cardGrid.Controls.Add(titGrid);
            this.Controls.Add(cardGrid); this.Controls.Add(meio); this.Controls.Add(cardForm); this.Controls.Add(footer); this.Controls.Add(header);
        }

        Panel Box(string titulo, string valor, Color cor, int x, out Label lblValor)
        {
            Panel p = new Panel() { Location = new Point(x, 5), Size = new Size(210, 60), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            Label t = new Label() { Text = titulo, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = cor, Location = new Point(8, 6), AutoSize = true };
            Label v = new Label() { Text = valor, Font = new Font("Segoe UI", 15, FontStyle.Bold), ForeColor = cor, Location = new Point(8, 24), AutoSize = true };
            p.Controls.Add(t); p.Controls.Add(v); lblValor = v; return p;
        }
        Label Lbl(string txt, int x, int y) => new Label() { Text = txt, Location = new Point(x, y), AutoSize = true, Font = new Font("Segoe UI", 7, FontStyle.Bold), ForeColor = Color.Gray };
        Button CriarBotao(string txt, Color back, int x, int y) { var b = new Button() { Text = txt, Location = new Point(x, y), Size = new Size(175, 38), BackColor = back, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 8, FontStyle.Bold) }; b.FlatAppearance.BorderSize = 0; return b; }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtQuem.Text) || cmbTipo.SelectedItem == null || string.IsNullOrWhiteSpace(txtDescricao.Text)) { MessageBox.Show("Preencha QUEM, TIPO e DESCRIÇÃO!"); return; }
            using (SqlConnection con = new SqlConnection(conexao)) { con.Open(); var cmd = new SqlCommand("INSERT INTO Requisicoes (QuemPediu, Maquina, Setor, TipoProblema, DescricaoProblema, Status) VALUES (@q,@m,@s,@t,@d,'Aberta')", con); cmd.Parameters.AddWithValue("@q", txtQuem.Text); cmd.Parameters.AddWithValue("@m", txtMaquina.Text); cmd.Parameters.AddWithValue("@s", txtSetor.Text); cmd.Parameters.AddWithValue("@t", cmbTipo.SelectedItem.ToString()); cmd.Parameters.AddWithValue("@d", txtDescricao.Text); cmd.ExecuteNonQuery(); }
            txtDescricao.Clear(); CarregarDados(); MessageBox.Show("✅ Salvo!");
        }
        void BtnAmarelo_Click(object sender, EventArgs e) => MudarStatus("Em Andamento");
        void BtnVerde_Click(object sender, EventArgs e) => MudarStatus("Fechada");
        void MudarStatus(string st) { if (grid.SelectedRows.Count == 0) { MessageBox.Show("Clica numa linha da tabela primeiro!"); return; } int id = Convert.ToInt32(grid.SelectedRows[0].Cells["Id"].Value); using (SqlConnection con = new SqlConnection(conexao)) { con.Open(); var cmd = new SqlCommand("UPDATE Requisicoes SET Status=@st WHERE Id=@id", con); cmd.Parameters.AddWithValue("@st", st); cmd.Parameters.AddWithValue("@id", id); cmd.ExecuteNonQuery(); } CarregarDados(); }

        // ===== ATUALIZADO - AGORA APAGA VÁRIAS LINHAS =====
        void BtnExcluir_Click(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count == 0) { MessageBox.Show("Selecione uma ou mais linhas para excluir!\nDica: Segure CTRL e clique em várias linhas."); return; }

            string senhaDigitada = PerguntarSenha();
            if (senhaDigitada != senhaAdmin) { MessageBox.Show("❌ Senha incorreta!\nA senha é: " + senhaAdmin); return; }

            int qtd = grid.SelectedRows.Count;
            if (MessageBox.Show($"Tem certeza que quer APAGAR {qtd} registro(s)?\nNão tem volta!", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(conexao))
                {
                    con.Open();
                    foreach (DataGridViewRow linha in grid.SelectedRows)
                    {
                        int id = Convert.ToInt32(linha.Cells["Id"].Value);
                        var cmd = new SqlCommand("DELETE FROM Requisicoes WHERE Id=@id", con);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                CarregarDados();
                MessageBox.Show($"🗑️ {qtd} registro(s) excluído(s) com sucesso!");
            }
        }

        string PerguntarSenha()
        {
            Form prompt = new Form() { Width = 380, Height = 180, FormBorderStyle = FormBorderStyle.FixedDialog, Text = "🔒 Área Restrita - Embrart", StartPosition = FormStartPosition.CenterScreen, MaximizeBox = false, MinimizeBox = false };
            Label lbl = new Label() { Left = 20, Top = 20, Text = "Digite a senha de administrador:", AutoSize = true };
            TextBox txtSenha = new TextBox() { Left = 20, Top = 50, Width = 320, UseSystemPasswordChar = true };
            Button btnOk = new Button() { Text = "OK - Excluir", Left = 20, Top = 90, Width = 150, BackColor = Color.FromArgb(200, 0, 0), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            Button btnCancel = new Button() { Text = "Cancelar", Left = 190, Top = 90, Width = 150, DialogResult = DialogResult.Cancel };
            btnOk.Click += (sender, ev) => { prompt.Close(); };
            prompt.Controls.Add(lbl); prompt.Controls.Add(txtSenha); prompt.Controls.Add(btnOk); prompt.Controls.Add(btnCancel);
            prompt.AcceptButton = btnOk;
            prompt.ShowDialog();
            return txtSenha.Text;
        }

        void CarregarDados() { try { using (SqlConnection con = new SqlConnection(conexao)) { con.Open(); var da = new SqlDataAdapter("SELECT Id, DataHora, QuemPediu, Maquina, Setor, TipoProblema, DescricaoProblema, Status FROM Requisicoes ORDER BY DataHora DESC", con); var dt = new System.Data.DataTable(); da.Fill(dt); grid.DataSource = dt; int a = 0, b = 0, c = 0; foreach (DataGridViewRow r in grid.Rows) { var s = r.Cells["Status"].Value?.ToString(); if (s == "Aberta") { r.DefaultCellStyle.BackColor = Color.FromArgb(255, 205, 210); a++; } if (s == "Em Andamento") { r.DefaultCellStyle.BackColor = Color.FromArgb(255, 249, 196); b++; } if (s == "Fechada") { r.DefaultCellStyle.BackColor = Color.FromArgb(200, 230, 201); c++; } } if (lblA != null) lblA.Text = a.ToString(); if (lblB != null) lblB.Text = b.ToString(); if (lblC != null) lblC.Text = c.ToString(); } } catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); } }
    }
}