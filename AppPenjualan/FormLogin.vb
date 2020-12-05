Public Class FormLogin
    Private Sub FormLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BackColor = ColorTranslator.FromHtml("#292b2c")

        btnCancel.FlatStyle = FlatStyle.Flat
        btnCancel.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#fff")
        btnCancel.BackColor = ColorTranslator.FromHtml("#292b2c")
        btnCancel.ForeColor = ColorTranslator.FromHtml("#fff")


        btnLogin.FlatStyle = FlatStyle.Flat
        btnLogin.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#fff")
        btnLogin.BackColor = ColorTranslator.FromHtml("#fff")
        btnLogin.ForeColor = ColorTranslator.FromHtml("#292b2c")

        txtKode.BackColor = ColorTranslator.FromHtml("#292b2c")
        txtKode.ForeColor = ColorTranslator.FromHtml("#fff")

        txtPass.BackColor = ColorTranslator.FromHtml("#292b2c")
        txtPass.ForeColor = ColorTranslator.FromHtml("#fff")
        Call kondisiAwal()
    End Sub

    Sub kondisiAwal()
        txtKode.Clear()
        txtPass.Clear()
        txtKode.Focus()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Hide()
    End Sub

    Sub terbuka()
        FormMenuUtama.LoginToolStripMenuItem.Enabled = False
        FormMenuUtama.LogoutToolStripMenuItem.Enabled = True
        FormMenuUtama.MasterToolStripMenuItem.Enabled = True
        FormMenuUtama.TransaksiToolStripMenuItem.Enabled = True
        FormMenuUtama.LaporanToolStripMenuItem.Enabled = True
        FormMenuUtama.UtilityToolStripMenuItem.Enabled = True
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If txtKode.Text = "" Or txtPass.Text = "" Then
            MsgBox("Kode admin dan Password harus di isi",
                   vbOKOnly + MessageBoxIcon.Exclamation,
                   "Peringatan")
        Else
            Try
                Call koneksi()
                Cmd = New Odbc.OdbcCommand("SELECT * FROM tbl_admin WHERE kodeadmin='" &
                                       txtKode.Text &
                                       "' AND passwordadmin='" &
                                       txtPass.Text & "'", Conn)
                Rd = Cmd.ExecuteReader
                Rd.Read()
                If Rd.HasRows Then
                    Me.Close()
                    Call terbuka()
                Else
                    MsgBox("Kode admin atau Password salah..",
                    vbOKOnly + MessageBoxIcon.Exclamation,
                    "Peringatan")
                End If
            Catch ex As Exception
                MsgBox("Kode admin atau Password salah..",
                    vbOKOnly + MessageBoxIcon.Exclamation,
                    "Peringatan")
            End Try
        End If
    End Sub

    Private Sub cbPassVisib_CheckedChanged(sender As Object, e As EventArgs) Handles cbPassVisib.CheckedChanged
        If cbPassVisib.Checked Then
            txtPass.UseSystemPasswordChar = False
        Else
            txtPass.UseSystemPasswordChar = True
        End If
    End Sub
End Class