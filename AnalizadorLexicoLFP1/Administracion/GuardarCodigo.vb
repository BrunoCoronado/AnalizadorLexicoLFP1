Public Class GuardarCodigo

    Public Sub guardarComo(ByVal codigo As ArrayList)
        Dim saveFileDialog As New Microsoft.Win32.SaveFileDialog
        saveFileDialog.Filter = "Archivo lfp|*.lfp"
        saveFileDialog.ShowDialog()

        If saveFileDialog.FileName IsNot "" Then
            MainWindow.ruta = saveFileDialog.FileName
            escribirArchivo(codigo, MainWindow.ruta)
        End If
    End Sub

    Public Sub guardar(ByVal codigo As ArrayList, ByVal ruta As String)
        If ruta.Equals("") Then
            guardarComo(codigo)
        Else
            escribirArchivo(codigo, ruta)
        End If
    End Sub

    Private Sub escribirArchivo(ByVal codigo As ArrayList, ByVal ruta As String)
        Dim streamWriter As New System.IO.StreamWriter(ruta)

        For Each linea As String In codigo
            Dim l As String = Replace(linea, vbCrLf, "")
            streamWriter.WriteLine(l)
        Next

        streamWriter.Close()
        Console.WriteLine("archivo creado")
    End Sub
End Class
