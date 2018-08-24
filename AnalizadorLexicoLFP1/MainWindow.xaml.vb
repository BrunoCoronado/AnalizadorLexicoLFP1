Class MainWindow

    Private Sub analizar(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim noLineas = txtContenido.LineCount
        Dim lineasContenido As New ArrayList

        For i As Integer = 0 To (noLineas - 1)
            lineasContenido.Add(txtContenido.GetLineText(i))
        Next
        Dim ManejoDeDatos As New ManejoDeDatos(lineasContenido)
    End Sub

End Class
