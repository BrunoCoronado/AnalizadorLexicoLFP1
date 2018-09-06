Public Class Graficador
    Public Sub dibujarDiagrama(ByVal clases As ArrayList, ByVal asociaciones As ArrayList)
        Dim streamWriter As New System.IO.StreamWriter("diagrama.txt")

        streamWriter.WriteLine("digraph diagrama{")
        streamWriter.WriteLine("size=""5,5""")
        streamWriter.WriteLine("node[shape=record,style=filled,fillcolor=gray95]")
        streamWriter.WriteLine("edge[dir=back, arrowtail=empty]")

        Dim textoNodo As String

        For i As Integer = 0 To (clases.Count - 1)
            textoNodo = "" & CType(clases(i), Clase).nombre
            textoNodo = textoNodo & "[label = ""{" & CType(clases(i), Clase).nombre & "|"

            For Each atributo As Caracteristica In CType(clases(i), Clase).getAtributos
                textoNodo = textoNodo & "" + atributo.visibilidad + " " + atributo.identificador + ":" + atributo.tipo + "\n"
            Next

            textoNodo = textoNodo & "|"

            For Each metodo As Caracteristica In CType(clases(i), Clase).getMetodos
                textoNodo = textoNodo & "" + metodo.visibilidad + " " + metodo.identificador + "() :" + metodo.tipo + "\l"
            Next

            textoNodo = textoNodo & "}""]"
            streamWriter.WriteLine(textoNodo)
        Next

        For Each asociacion As Asociacion In asociaciones
            textoNodo = """" & asociacion.padre & """ -> """ & asociacion.hijo & """ [arrowhead=""" & asociacion.asociacion & """]"
            streamWriter.WriteLine(textoNodo)
        Next

        streamWriter.WriteLine("}")

        streamWriter.Close()
        Console.WriteLine("archivo creado")

        Dim prog As VariantType
        prog = Interaction.Shell("C:\Program Files (x86)\Graphviz 2.28\bin\dot.exe -Tjpg diagrama.txt -o diagrama.jpg", 1)
    End Sub
End Class
