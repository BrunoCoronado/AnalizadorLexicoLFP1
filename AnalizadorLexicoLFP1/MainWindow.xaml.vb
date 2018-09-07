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