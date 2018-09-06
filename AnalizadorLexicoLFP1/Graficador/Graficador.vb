Public Class Graficador
    Public Sub dibujarDiagrama(ByVal clases As ArrayList)
        Dim streamWriter As New System.IO.StreamWriter("diagrama.txt")

        streamWriter.WriteLine("digraph diagrama{")
        streamWriter.WriteLine("size=""5,5""")
        streamWriter.WriteLine("node[shape=record,style=filled,fillcolor=gray95]")
        streamWriter.WriteLine("edge[dir=back, arrowtail=empty]")

        For i As Integer = 0 To (clases.Count - 1)
            Dim nodoClase As String = "" + i.ToString
            nodoClase = nodoClase & "[label = ""{" & CType(clases(i), Clase).nombre & "|"

            For Each atributo As Caracteristica In CType(clases(i), Clase).getAtributos
                nodoClase = nodoClase & "" + atributo.visibilidad + " " + atributo.identificador + ":" + atributo.tipo + "\n"
            Next

            nodoClase = nodoClase & "|"

            For Each metodo As Caracteristica In CType(clases(i), Clase).getMetodos
                nodoClase = nodoClase & "" + metodo.visibilidad + " " + metodo.identificador + "() :" + metodo.tipo + "\l"
            Next

            nodoClase = nodoClase & "}""]"
            streamWriter.WriteLine(nodoClase)
        Next

        streamWriter.WriteLine("}")
        streamWriter.Close()
        Console.WriteLine("archivo creado")

        Dim prog As VariantType
        prog = Interaction.Shell("C:\Program Files (x86)\Graphviz 2.28\bin\dot.exe -Tjpg diagrama.txt -o diagrama.jpg", 1)
    End Sub
End Class
