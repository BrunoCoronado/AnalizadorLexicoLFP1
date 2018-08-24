Class MainWindow

    'StringCollection GetLinesCollectionFromTextBox(TextBox textBox)

    'StringCollection lines = New StringCollection();

    '// lineCount may be -1 if TextBox layout info Is Not up-to-date.
    'int lineCount = TextBox.LineCount;

    'For (int line = 0; line < lineCount; line++)
    '    // GetLineText takes a zero-based line index.
    '    lines.Add(textBox.GetLineText(line));

    'Return lines;
    '}

    Private Sub analizar(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim noLineas = txtContenido.LineCount
        Dim lineasContenido As New ArrayList

        For i As Integer = 0 To (noLineas - 1)
            lineasContenido.Add(txtContenido.GetLineText(i))
        Next
        Dim ManejoDeDatos As New ManejoDeDatos(lineasContenido)
    End Sub

End Class
