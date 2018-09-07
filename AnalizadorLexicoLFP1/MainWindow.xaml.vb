Class MainWindow

    Private Sub analizar(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim manejoDeDatos As New ManejoDeDatos
        manejoDeDatos.analizarLexico(obtenerCodigo())
    End Sub


    Private Sub generarDiagrama(ByVal sender As System.Object, ByVal w As System.EventArgs)
        Dim manejoDeDatos As New ManejoDeDatos
        manejoDeDatos.diagramarCodigo(obtenerCodigo())
    End Sub

    Private Sub generarReporteTokens(ByVal sender As System.Object, ByVal w As System.EventArgs)
        Dim manejoDeDatos As New ManejoDeDatos
        manejoDeDatos.reporteDeTokens(obtenerCodigo())
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

    Private Sub MenuItem_Click(sender As Object, e As RoutedEventArgs)

    End Sub
End Class
