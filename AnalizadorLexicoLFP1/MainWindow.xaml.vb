Class MainWindow
    Public Shared ruta As String = ""

    Private Sub analizar(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim manejoDeDatos As New ManejoDeDatos
        manejoDeDatos.analizarLexico(obtenerCodigo())
    End Sub


    Private Sub generarDiagrama(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim manejoDeDatos As New ManejoDeDatos
        manejoDeDatos.diagramarCodigo(obtenerCodigo())
    End Sub

    Private Sub generarReporteTokens(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim manejoDeDatos As New ManejoDeDatos
        manejoDeDatos.reporteDeTokens(obtenerCodigo())
    End Sub

    Private Sub abrirArchivo(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim abrirArchivo As New AbrirArchivo()
        txtContenido.Text = abrirArchivo.abrirArchivo()
    End Sub

    Private Sub guardarArchivo(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim guardarCodigo As New GuardarCodigo
        guardarCodigo.guardar(obtenerCodigo(), ruta)
    End Sub

    Private Sub guardarArchivoComo(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim guardarCodigo As New GuardarCodigo
        guardarCodigo.guardarComo(obtenerCodigo())
    End Sub

    Private Sub salirDelPrograma(ByVal sender As System.Object, ByVal e As System.EventArgs)
        System.Environment.Exit(0)
    End Sub

    Private Sub verManuales(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            'abrir los manuales, siempre y cuando estos se encuentren en el escritorio
            Dim direccion As String = My.Computer.FileSystem.SpecialDirectories.Desktop
            Process.Start(direccion + "\Manual Tecnico.pdf")
            Process.Start(direccion + "\Manual de usuario.pdf")
        Catch ex As Exception
            MessageBox.Show("Manuales no encotrados en desktop!", "ERROR")
        End Try
    End Sub

    Private Sub verAcercaDe(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MessageBox.Show("Analizador Lexico V1.0" & vbCrLf & "Bruno Marco José Coronado Morales" & vbCrLf & "201709362", "Acerca de")
    End Sub

    Private Function obtenerCodigo() As ArrayList
        Dim lineasContenido As New ArrayList

        Try
            For i As Integer = 0 To (txtContenido.LineCount - 1)
                lineasContenido.Add(txtContenido.GetLineText(i))
            Next
        Catch ex As Exception
            Console.WriteLine("Error al leer el codigo.")
        End Try

        Return lineasContenido
    End Function
End Class