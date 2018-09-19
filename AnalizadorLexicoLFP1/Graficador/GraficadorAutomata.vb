Public Class GraficadorAutomata
    Public Sub dibujarAutomata(ByVal automata As Automata)
        Dim direccion As String = My.Computer.FileSystem.SpecialDirectories.Desktop + "\automata"
        Try
            My.Computer.FileSystem.DeleteFile(direccion + ".txt")
            My.Computer.FileSystem.DeleteFile(direccion + ".svg")
        Catch ex As Exception

        End Try
        Dim streamWriter As New System.IO.StreamWriter(direccion + ".txt")

        streamWriter.WriteLine("digraph Automata_Finito{")
        streamWriter.WriteLine("rankdir=LR;")
        streamWriter.WriteLine("size=""8,5""")
        streamWriter.WriteLine("node [shape = doublecircle, color = blue]; U;")
        streamWriter.WriteLine("node [shape = circle];")

        Dim textoNodo As String

        For Each produccion As Produccion In automata.getProducciones
            textoNodo = "" & produccion.estadoA & "->" & produccion.estadoB & "[label = """ & produccion.transicion & """ ];"
            streamWriter.WriteLine(textoNodo)
        Next

        streamWriter.WriteLine("}")

        streamWriter.Close()
        Dim prog As VariantType
        prog = Interaction.Shell("dot.exe -Tsvg " + direccion + ".txt -o " + direccion + ".svg", 0)

        Console.WriteLine("archivo creado")

        Try
            Process.Start(direccion + ".svg")
        Catch ex As Exception
            Console.WriteLine("error al abrir")
            abrirArchivo(direccion + ".svg")
        End Try
    End Sub

    Private Sub abrirArchivo(ByVal archivo As String)
        Try
            Process.Start(archivo)
        Catch ex As Exception
            Console.WriteLine("tratando de abrir archivo")
            abrirArchivo(archivo)
        End Try
    End Sub


    Public Sub dibujarReporteTokens(ByVal listaTokens As ArrayList)
        Dim direccion As String = My.Computer.FileSystem.SpecialDirectories.Desktop + "\reporteTokens"
        Try
            My.Computer.FileSystem.DeleteFile("reporteTokens.html")
        Catch ex As Exception

        End Try

        Dim streamWriter As New System.IO.StreamWriter(direccion + ".html")

        'definimos el encabezado del archivo html
        streamWriter.WriteLine("<!DOCTYPE html>" & vbCrLf & "<html>" & vbCrLf & "<head>" & vbCrLf & "<title>Reporte de Tokens</title>" & vbCrLf & "</head>" & vbCrLf & "<body>" & vbCrLf & "<h1 align=""center"">Reporte de Tokens</h1>" & vbCrLf & "<style type=""text/css"">" & vbCrLf & ".tg  {border-collapse:collapse;border-spacing:0;margin:0px auto;}" & vbCrLf & ".tg td{font-family:Arial, sans-serif;font-size:14px;padding:10px 5px;border-style:solid;border-width:1px;overflow:hidden;word-break:normal;border-color:black;}" & vbCrLf & ".tg th{font-family:Arial, sans-serif;font-size:14px;font-weight:normal;padding:10px 5px;border-style:solid;border-width:1px;overflow:hidden;word-break:normal;border-color:black;}" & vbCrLf & ".tg .tg-4tse{background-color:#32cb00;color:#000000;text-align:left;vertical-align:top}" & vbCrLf & ".tg .tg-0lax{text-align:left;vertical-align:top}" & vbCrLf & "h1{font-family:Arial, sans-serif}" & vbCrLf & "</style>")

        'definimos la tabla y su encabezado
        streamWriter.WriteLine("<table class=""tg"">")
        streamWriter.WriteLine("<tr>" & vbCrLf & "<th class=""tg-4tse"">No</th>" & vbCrLf & "<th class=""tg-4tse"">Lexema</th>" & vbCrLf & "<th class=""tg-4tse"">Tipo</th>" & vbCrLf & "<th class=""tg-4tse"">Columna</th>" & vbCrLf & "<th class=""tg-4tse"">Linea</th>" & vbCrLf & "</tr>")

        'llenamos la tabla con el contenido
        For i As Integer = 0 To (listaTokens.Count - 1)
            streamWriter.WriteLine("<tr>")
            streamWriter.WriteLine("<td class=""tg-0Lax"">" + i.ToString + "<br></td>")
            streamWriter.WriteLine("<td Class=""tg-0Lax"">" + CType(listaTokens(i), Token).lexema + "</td>")
            streamWriter.WriteLine("<td class=""tg-0Lax"">" + CType(listaTokens(i), Token).tipo + "</td>")
            streamWriter.WriteLine("<td Class=""tg-0Lax"">" + CType(listaTokens(i), Token).columna.ToString + "</td>")
            streamWriter.WriteLine("<td class=""tg-0Lax"">" + CType(listaTokens(i), Token).fila.ToString + "</td>")
            streamWriter.WriteLine("</tr>")
        Next
        'definimos la parte final del archivo html
        streamWriter.WriteLine("</body>" & vbCrLf & "</html>")

        streamWriter.Close()
        Console.WriteLine("archivo creado")
        Try
            Process.Start(direccion + ".html")
        Catch ex As Exception
            Console.WriteLine("error al abrir")
            abrirArchivo(direccion + ".html")
        End Try

    End Sub

    Public Sub dibujarReporteErrores(ByVal listaErrores As ArrayList)
        Dim direccion As String = My.Computer.FileSystem.SpecialDirectories.Desktop + "\reporteErrores"
        Try
            My.Computer.FileSystem.DeleteFile(direccion + ".html")
        Catch ex As Exception

        End Try
        Dim streamWriter As New System.IO.StreamWriter(direccion + ".html")

        'definimos el encabezado del archivo html
        streamWriter.WriteLine("<!DOCTYPE html>" & vbCrLf & "<html>" & vbCrLf & "<head>" & vbCrLf & "<title>Reporte de Errores</title>" & vbCrLf & "</head>" & vbCrLf & "<body>" & vbCrLf & "<h1 align=""center"">Reporte de Tokens</h1>" & vbCrLf & "<style type=""text/css"">" & vbCrLf & ".tg  {border-collapse:collapse;border-spacing:0;margin:0px auto;}" & vbCrLf & ".tg td{font-family:Arial, sans-serif;font-size:14px;padding:10px 5px;border-style:solid;border-width:1px;overflow:hidden;word-break:normal;border-color:black;}" & vbCrLf & ".tg th{font-family:Arial, sans-serif;font-size:14px;font-weight:normal;padding:10px 5px;border-style:solid;border-width:1px;overflow:hidden;word-break:normal;border-color:black;}" & vbCrLf & ".tg .tg-4tse{background-color:#cb0d00;color:#000000;text-align:left;vertical-align:top}" & vbCrLf & ".tg .tg-0lax{text-align:left;vertical-align:top}" & vbCrLf & "h1{font-family:Arial, sans-serif}" & vbCrLf & "</style>")

        'definimos la tabla y su encabezado
        streamWriter.WriteLine("<table class=""tg"">")
        streamWriter.WriteLine("<tr>" & vbCrLf & "<th class=""tg-4tse"">No</th>" & vbCrLf & "<th class=""tg-4tse"">Lexema</th>" & vbCrLf & "<th class=""tg-4tse"">Tipo</th>" & vbCrLf & "<th class=""tg-4tse"">Columna</th>" & vbCrLf & "<th class=""tg-4tse"">Linea</th>" & vbCrLf & "</tr>")

        'llenamos la tabla con el contenido
        For i As Integer = 0 To (listaErrores.Count - 1)
            streamWriter.WriteLine("<tr>")
            streamWriter.WriteLine("<td class=""tg-0Lax"">" + i.ToString + "<br></td>")
            streamWriter.WriteLine("<td Class=""tg-0Lax"">" + CType(listaErrores(i), Token).lexema + "</td>")
            streamWriter.WriteLine("<td class=""tg-0Lax"">" + CType(listaErrores(i), Token).tipo + "</td>")
            streamWriter.WriteLine("<td Class=""tg-0Lax"">" + CType(listaErrores(i), Token).columna.ToString + "</td>")
            streamWriter.WriteLine("<td class=""tg-0Lax"">" + CType(listaErrores(i), Token).fila.ToString + "</td>")
            streamWriter.WriteLine("</tr>")
        Next
        'definimos la parte final del archivo html
        streamWriter.WriteLine("</body>" & vbCrLf & "</html>")

        streamWriter.Close()
        Console.WriteLine("archivo creado")
        Try
            Process.Start(direccion + ".html")
        Catch ex As Exception
            Console.WriteLine("error al abrir")
            abrirArchivo(direccion + ".html")
        End Try
    End Sub
End Class
