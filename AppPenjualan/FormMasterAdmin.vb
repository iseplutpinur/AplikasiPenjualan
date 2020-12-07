'perbaikan primary key tidakboleh diketik manual

Imports System.Data.Odbc
Public Class FormMasterAdmin
    Private temporary(10) As String

    'Tools
    Private Function cekDataInput() As Boolean
        Dim hasil As Boolean = False
        If txtKodeAdmin.Text = "" Then
            MsgBox("Kode admin harus di isi..!",
                vbOKOnly + MessageBoxIcon.Exclamation,
                "Peringatan")
            hasil = True
            txtKodeAdmin.Focus()
        ElseIf txtNamaAdmin.Text = "" Then
            MsgBox("Nama Admin harus di isi..!",
                vbOKOnly + MessageBoxIcon.Exclamation,
                "Peringatan")
            hasil = True
            txtNamaAdmin.Focus()
        ElseIf txtPassord.Text = "" Then
            MsgBox("Password harus di isi..!",
                vbOKOnly + MessageBoxIcon.Exclamation,
                "Peringatan")
            hasil = True
            txtPassord.Focus()
        End If
        cekDataInput = hasil
    End Function

    Private Sub bersihkan()
        txtKodeAdmin.Clear()
        txtNamaAdmin.Clear()
        txtPassord.Clear()
        cbLevel.Items.Clear()
        cbLevel.Items.Add("ADMIN")
        cbLevel.Items.Add("USER")
    End Sub

    Private Sub btnAwal()
        txtKodeAdmin.Enabled = False
        txtNamaAdmin.Enabled = False
        txtPassord.Enabled = False
        cbLevel.Enabled = False

        Button1.Text = "Input"
        Button2.Text = "Edit"
        Button3.Text = "Hapus"
        Button4.Text = "Tutup"

        Button1.Enabled = True
        Button2.Enabled = False
        Button3.Enabled = False
        Button4.Enabled = True
    End Sub

    Private Sub siapIsi()
        bersihkan()
        txtKodeAdmin.Enabled = True
        txtNamaAdmin.Enabled = True
        txtPassord.Enabled = True
        cbLevel.Enabled = True

        cbLevel.SelectedIndex = 0
        cbLevel.DropDownStyle = ComboBoxStyle.DropDownList
    End Sub

    Private Sub refreshTable(ByVal query As String)
        On Error Resume Next
        Call koneksi()
        If query = "d" Then
            Da = New OdbcDataAdapter("
                    SELECT
                        Kode_Admin AS `Kode Admin`,
                        Nama_Admin AS `Nama Admin`, 
                        Level_Admin AS `Level Admin`
                    FROM
                        tbl_admin",
                                 Conn)
        Else
            Da = New OdbcDataAdapter(query, Conn)
        End If
        Ds = New DataSet
        Da.Fill(Ds, "tbl_admin")
        DataGridView1.DataSource = Ds.Tables("tbl_admin")
        DataGridView1.ReadOnly = True

        bersihkan()
        btnAwal()
    End Sub

    Private Sub cari()
        If txtCariKey.Text = "" Then Exit Sub
        Dim query As String = "
                    SELECT
                        Kode_Admin AS `Kode Admin`,
                        Nama_Admin AS `Nama Admin`, 
                        Level_Admin AS `Level Admin`
                    FROM
                        tbl_admin
                    WHERE   
                        Nama_Admin
                    LIKE
                        '" & txtCariKey.Text & "%'
                       "
            refreshTable(query)
            temporary(10) = "y"
            Button4.Text = "Batal"
    End Sub
    '========================================================================================================
    Private Sub FormMasterAdmin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        refreshTable("d")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Button1.Text = "Input" Then
            Button1.Text = "Simpan"
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Text = "Batal"
            Call siapIsi()
        Else
            If cekDataInput() Then
                Exit Sub
            Else
                Try
                    Call koneksi()
                    Dim InputData As String = "INSERT INTO tbl_admin VALUES('" &
                        txtKodeAdmin.Text & "', '" &
                        txtNamaAdmin.Text & "', '" &
                        txtPassord.Text & "', '" &
                        cbLevel.Text & "')"

                    Cmd = New OdbcCommand(InputData, Conn)
                    Cmd.ExecuteNonQuery()
                    MsgBox("Data berhasil di simpan.",
                        vbOKOnly + MessageBoxIcon.Information,
                        "Peringatan")

                    refreshTable("d")
                Catch ex As Exception
                    MsgBox("Data gagal di simpan pastikan anda memasukan karakter yang sesuai atau data sudah ada.",
                        vbOKOnly + MessageBoxIcon.Error,
                        "Peringatan")
                End Try
            End If
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Button4.Text = "Tutup" Then
            End
        ElseIf Button4.Text = "Batal" Then
            bersihkan()
            btnAwal()
            If temporary(10) = "y" Then
                refreshTable("d")
                txtCariKey.Clear()
            End If
        End If
    End Sub

    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        On Error Resume Next
        Cmd = New OdbcCommand("
                            SELECT
                                Kode_Admin, Nama_Admin, Level_Admin, Password_Admin
                            FROM
                                tbl_admin
                            WHERE
                                Kode_Admin='" & DataGridView1.Rows(e.RowIndex).Cells(0).Value & "'",
                              Conn)
        Rd = Cmd.ExecuteReader
        Rd.Read()
        If Rd.HasRows Then
            siapIsi()
            txtKodeAdmin.Enabled = False
            txtKodeAdmin.Text = Rd.Item("Kode_Admin")
            txtNamaAdmin.Text = Rd.Item("Nama_Admin")

            temporary(0) = Rd.Item("Nama_Admin")
            temporary(1) = Rd.Item("Level_Admin")
            temporary(2) = Rd.Item("Password_Admin")

            txtPassord.Text = Rd.Item("Password_Admin")
            If Rd.Item("Level_Admin") = "ADMIN" Then
                cbLevel.SelectedIndex = 0
            Else
                cbLevel.SelectedIndex = 1
            End If
            Button4.Text = "Batal"
            Button1.Enabled = False
            Button2.Enabled = True
            Button3.Enabled = True
        End If

    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim result As DialogResult = MsgBox("Apakah anda yakin akan menghapus data " & temporary(0),
                MessageBoxButtons.YesNo + MessageBoxIcon.Question,
                "Peringatan")
        If result = DialogResult.Yes Then
            On Error Resume Next
            Call koneksi()
            Cmd = New OdbcCommand("
                            DELETE FROM
                                tbl_admin
                            WHERE
                                Kode_Admin='" & txtKodeAdmin.Text & "'",
                            Conn)
            Cmd.ExecuteNonQuery()

            MsgBox("Data berhasil di Dihapus.",
                vbOKOnly + MessageBoxIcon.Information,
                "Peringatan")
            refreshTable("d")
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If temporary(0) = txtNamaAdmin.Text And temporary(1) = cbLevel.Text And temporary(2) = txtPassord.Text Then Exit Sub


        Dim detail As String = ""
        If temporary(0) <> txtNamaAdmin.Text Then
            detail = "Nama Admin Dari " &
                temporary(0) &
                " Menjadi " &
                txtNamaAdmin.Text
        End If

        If temporary(1) <> cbLevel.Text Then
            If temporary(0) <> txtNamaAdmin.Text Then
                detail &= ", "
            End If

            detail &= "Level Admin Dari " &
                temporary(1) &
                " Menjadi " &
                cbLevel.Text
        End If

        If temporary(2) <> txtPassord.Text Then
            If detail <> "" Then
                detail &= " dan "
            End If

            detail &= "Password dirubah"
        End If

        Dim result As DialogResult = MsgBox("Apakah anda yakin akan Mengedit data ini..?  " & Chr(13) & detail,
                                            MessageBoxButtons.YesNo + MessageBoxIcon.Question,
                                            "Peringatan")
        If result = DialogResult.Yes Then
            On Error Resume Next
            Call koneksi()
            Cmd = New OdbcCommand("
                                    UPDATE 
                                        tbl_admin
                                    SET 
                                        Nama_Admin = '" & txtNamaAdmin.Text & "', 
                                        Password_Admin = '" & txtPassord.Text & "', 
                                        Level_Admin = '" & cbLevel.Text & "'
                                    WHERE 
                                        Kode_Admin = '" & txtKodeAdmin.Text & "'
                                    ",
                                      Conn)
            Cmd.ExecuteNonQuery()

            MsgBox("Data berhasil di Diedit.",
                vbOKOnly + MessageBoxIcon.Information,
                "Peringatan")
            refreshTable("d")
        End If
    End Sub

    Private Sub btnCari_Click(sender As Object, e As EventArgs) Handles btnCari.Click
        cari()
    End Sub

    Private Sub txtCariKey_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCariKey.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            Call cari()
        End If
    End Sub
End Class