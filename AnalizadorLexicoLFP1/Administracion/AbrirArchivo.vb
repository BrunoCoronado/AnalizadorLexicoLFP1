Public Class AbrirArchivo

    Public Function abrirArchivo() As String
        Dim openFileDialog As New Microsoft.Win32.OpenFileDialog
        openFileDialog.Filter = "Archivo lfp|*.lfp"
        openFileDialog.ShowDialog()

        Dim codigo As String = ""

        If openFileDialog.FileName IsNot "" Then
            MainWindow.ruta = openFileDialog.FileName
            Dim streamReader As New System.IO.StreamReader(MainWindow.ruta)
            Try
                Do While streamReader.Peek >= 0
                    codigo = codigo + streamReader.ReadLine() + vbCrLf
                Loop
                Console.WriteLine(codigo)
                Console.WriteLine("lectura terminada")
                streamReader.Close()
            Catch ex As Exception
                Console.WriteLine("lectura terminada en ex")
                streamReader.Close()
            End Try
        End If
        Return codigo
    End Function


End Class
