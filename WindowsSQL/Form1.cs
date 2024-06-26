﻿using Npgsql;
using System;
using System.Windows.Forms;

namespace WindowsSQL
{
    public partial class Form1 : Form
    {
        private string connString = "Host=localhost;Port=5432;Username=postgres;Password=gustavoroldao;Database=CadastroDB;";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnInserirClick(object sender, EventArgs e)
        {
            try
            {
                // Validar número existente na tabela
                /*
                if (NumeroJaExiste(int.Parse(txtNumero.Text)))
                {
                    MessageBox.Show("Não foi possível realizar o registro, pois o número já existe na tabela. Por favor, insira um número diferente.", "Número já Existente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                */

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("INSERT INTO cadastro (texto, numero) VALUES (@texto, @numero)", conn))
                    {
                        cmd.Parameters.AddWithValue("texto", txtTexto.Text);
                        cmd.Parameters.AddWithValue("numero", int.Parse(txtNumero.Text));
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Registro inserido com sucesso!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao tentar inserir o registro: {ex.Message}", "Erro de Inserção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAtualizarClick(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("UPDATE cadastro SET texto = @texto WHERE numero = @numero", conn))
                    {
                        cmd.Parameters.AddWithValue("@texto", txtTexto.Text);
                        cmd.Parameters.AddWithValue("@numero", int.Parse(txtNumero.Text));

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Registro atualizado com sucesso!");
                        }
                        else
                        {
                            MessageBox.Show("Nenhum registro encontrado com o número fornecido.");
                        }
                    }
                }
                LimparCampos();
            }
            catch (NpgsqlException npgsqlEx)
            {
                MessageBox.Show($"Erro ao atualizar registro no banco de dados: {npgsqlEx.Message}", "Erro de Banco de Dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FormatException formatEx)
            {
                MessageBox.Show($"Formato inválido: {formatEx.Message}", "Erro de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar registro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeletarClick(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("DELETE FROM cadastro WHERE numero = @numero", conn))
                    {
                        cmd.Parameters.AddWithValue("@numero", int.Parse(txtNumero.Text));

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Registro deletado com sucesso!");
                        }
                        else
                        {
                            MessageBox.Show("Nenhum registro encontrado com o número fornecido.");
                        }
                    }
                }
                LimparCampos();
            }
            catch (NpgsqlException npgsqlEx)
            {
                MessageBox.Show($"Erro ao deletar registro no banco de dados: {npgsqlEx.Message}", "Erro de Banco de Dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FormatException formatEx)
            {
                MessageBox.Show($"Formato inválido: {formatEx.Message}", "Erro de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao deletar registro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparCampos()
        {
            txtTexto.Text = "";
            txtNumero.Text = "";
        }

        // Método para verificar se o número já existe na tabela
        /*
        private bool NumeroJaExiste(int numero)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM cadastro WHERE numero = @numero", conn))
                {
                    cmd.Parameters.AddWithValue("numero", numero);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }
        */
    }
}
