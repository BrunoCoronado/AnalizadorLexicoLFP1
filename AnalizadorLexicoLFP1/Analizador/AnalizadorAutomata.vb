Public Class AnalizadorAutomata
    Private tokens As New ArrayList
    Private esIdentificador As Boolean
    Private palabra As String = ""

    Public Sub analisisLexico(contenido As ArrayList)
        analizarContenido(contenido)
        verTablaTokensEnConsola()
        analizarEstructura()
    End Sub

    Private Sub analizarContenido(contenido As ArrayList)
        For noLinea As Integer = 0 To (contenido.Count - 1)
            Dim linea As String = CType(contenido(noLinea), String)
            For i As Integer = 0 To (linea.Length - 1)
                Select Case linea(i)
                    Case vbCrLf
                        validarEstadoPalabra(esIdentificador, (i - palabra.Length), noLinea)
                    Case vbCr
                        validarEstadoPalabra(esIdentificador, (i - palabra.Length), noLinea)
                    Case vbTab
                    Case vbLf
                        validarEstadoPalabra(esIdentificador, (i - palabra.Length), noLinea)
                    Case " "
                    Case "a" To "z"
                        esIdentificador = True
                        palabra = palabra & linea(i)
                    Case "A" To "Z"
                        esIdentificador = True
                        palabra = palabra & linea(i)
                    Case "0" To "9"
                        If esIdentificador Then
                            palabra = palabra & linea(i)
                        Else
                            agregarTokenATabla(linea(i), linea(i), i, noLinea)
                        End If
                    Case "_"
                        If esIdentificador And i = 1 Then
                            palabra = palabra & linea(i)
                        Else
                            agregarTokenATabla(linea(i), linea(i), i, noLinea)
                        End If
                    Case Else
                        'agregar todo a tabla
                        validarEstadoPalabra(esIdentificador, (i - palabra.Length), noLinea)
                        agregarTokenATabla(linea(i), linea(i), i, noLinea)
                End Select
            Next
        Next
    End Sub

    Private Sub analizarEstructura()
        Dim contadorCaracteres As Integer = 0
        For i As Integer = 0 To (tokens.Count - 1)
            If CType(tokens(i), Token).lexema.Equals("G") Then
                contadorCaracteres += 1
                If CType(tokens(i + contadorCaracteres), Token).lexema.Equals("=") Then
                    contadorCaracteres += 1
                    If CType(tokens(i + contadorCaracteres), Token).lexema.Equals("{") Then
                        contadorCaracteres += 1
                        If CType(tokens(i + contadorCaracteres), Token).lexema.Equals("[") Then
                            contadorCaracteres += 1
                            'iniciamos lectura de terminales
                            contadorCaracteres = contadorCaracteres + leerTerminales(contadorCaracteres)

                            If CType(tokens(i + contadorCaracteres), Token).lexema.Equals(",") Then
                                contadorCaracteres += 1
                                If CType(tokens(i + contadorCaracteres), Token).lexema.Equals("[") Then
                                    contadorCaracteres += 1
                                    'iniciamos lectura de no terminales
                                    contadorCaracteres = contadorCaracteres + leerNoTerminales(contadorCaracteres)
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        Next
    End Sub

    Private Function leerTerminales(ByVal inicio As Integer) As Integer
        Dim contadorCaracteres As Integer = 0
        Console.WriteLine("TERMINALES")
        For i = inicio To (tokens.Count - 1)
            Dim str1 As String = CType(tokens(i), Token).lexema
            Dim str2 As String = CType(tokens(i + 1), Token).lexema
            If CType(tokens(i + 1), Token).lexema.Equals(",") Then
                contadorCaracteres += 1
                Console.WriteLine(CType(tokens(i), Token).lexema)
                contadorCaracteres += 1
                i = i + 1
            ElseIf CType(tokens(i + 1), Token).lexema.Equals("]") Then
                contadorCaracteres += 1
                Console.WriteLine(CType(tokens(i), Token).lexema)
                contadorCaracteres += 1
                Console.WriteLine("-----------FIN------------")
                Return contadorCaracteres
            End If
        Next
        Return contadorCaracteres
    End Function

    Private Function leerNoTerminales(ByVal inicio As Integer) As Integer
        Dim contadorCaracteres As Integer = 0
        Console.WriteLine("NO TERMINALES")
        For i = inicio To (tokens.Count - 1)
            If CType(tokens(i + 1), Token).lexema.Equals(",") Then
                contadorCaracteres += 1
                Console.WriteLine(CType(tokens(i), Token).lexema)
                contadorCaracteres += 1
                i = i + 1
            ElseIf CType(tokens(i + 1), Token).lexema.Equals("]") Then
                contadorCaracteres += 1
                Console.WriteLine(CType(tokens(i), Token).lexema)
                contadorCaracteres += 1
                Console.WriteLine("-----------FIN------------")
                Return contadorCaracteres
            End If
        Next
        Return contadorCaracteres
    End Function

    Private Sub validarEstadoPalabra(ByVal estado As Boolean, ByVal columna As Integer, ByVal fila As Integer)
        If estado Then
            agregarTokenATabla(palabra, palabra, columna, fila)
            palabra = ""
            esIdentificador = False
        End If
    End Sub

    Private Sub agregarTokenATabla(ByVal lexema As String, ByVal tipo As String, ByVal columna As Integer, ByVal fila As Integer)
        tokens.Add(New Token(lexema, tipo, columna, fila))
    End Sub

    Private Sub verTablaTokensEnConsola()
        Dim i As Integer = 0
        For Each token As Token In tokens
            Console.WriteLine(i & " Lexema: " & token.lexema.ToString & " Tipo: " & token.tipo & " Fila: " & token.fila.ToString & " Columna: " & token.columna.ToString)
            i = i + 1
        Next
    End Sub
End Class
