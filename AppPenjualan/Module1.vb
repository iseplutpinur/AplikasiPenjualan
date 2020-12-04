Imports System.Data.Odbc
Module Module1
    Public Conn As New Odbc.OdbcConnection
    Public Da As OdbcDataAdapter
    Public Ds As DataSet
    Public Rd As OdbcDataReader
    Public Cmd As OdbcCommand
    Public MyDB As String

    Public Sub koneksi()
        MyDB = "Dsn=aplikasi_penjualan"
        Conn = New OdbcConnection(MyDB)
        If Conn.State = ConnectionState.Closed Then
            Conn.Open()
        End If
    End Sub
End Module
