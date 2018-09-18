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
    Private automata As Automata
    Private Sub analizarEstructura()
        Dim contadorCaracteres As Integer = 0
        For i As Integer = 0 To (tokens.Count - 1)
            If CType(tokens(i), Token).lexema.Equals("G") Then
                automata = New Automata
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

                                    If CType(tokens(i + contadorCaracteres), Token).lexema.Equals(",") Then
                                        contadorCaracteres += 1
                                        If CType(tokens(i + contadorCaracteres), Token).lexema.Equals("[") Then
                                            contadorCaracteres += 1
                                            'iniciamos la lectura de las reglas de produccion
                                            contadorCaracteres = contadorCaracteres + leerReglasDeProduccion(contadorCaracteres)

                                            For Each produccion As Produccion In automata.getProducciones
                                                Console.WriteLine(produccion.estadoA & "->" & produccion.transicion & "->" & produccion.estadoB)
                                            Next

                                        End If
                                    End If
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
            If CType(tokens(i + 1), Token).lexema.Equals(",") Then
                contadorCaracteres += 1
                Console.WriteLine(CType(tokens(i), Token).lexema)
                CType(tokens(i), Token).tipo = "Terminal"
                automata.setTerminal(CType(tokens(i), Token).lexema)
                contadorCaracteres += 1
                i = i + 1
            ElseIf CType(tokens(i + 1), Token).lexema.Equals("]") Then
                contadorCaracteres += 1
                Console.WriteLine(CType(tokens(i), Token).lexema)
                CType(tokens(i), Token).tipo = "Terminal"
                automata.setTerminal(CType(tokens(i), Token).lexema)
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
                CType(tokens(i), Token).tipo = "No Terminal"
                automata.setNoTerminal(CType(tokens(i), Token).lexema)
                contadorCaracteres += 1
                i = i + 1
            ElseIf CType(tokens(i + 1), Token).lexema.Equals("]") Then
                contadorCaracteres += 1
                Console.WriteLine(CType(tokens(i), Token).lexema)
                CType(tokens(i), Token).tipo = "No Terminal"
                automata.setNoTerminal(CType(tokens(i), Token).lexema)
                contadorCaracteres += 1
                Console.WriteLine("-----------FIN------------")
                Return contadorCaracteres
            End If
        Next
        Return contadorCaracteres
    End Function

    Private Function leerReglasDeProduccion(ByVal inicio As Integer) As Integer
        Dim contadorCaracteres As Integer = 0
        Console.WriteLine("REGLAS DE PRODUCCION")
        For i = inicio To (tokens.Count - 1)
            Dim produccion As New Produccion
            If CType(tokens(i), Token).lexema.Equals("{") Then
                contadorCaracteres += 1
                'tiene que venir un no terminal
                If buscarPalabra(CType(tokens(i + 1), Token).lexema, "No Terminal") Then
                    contadorCaracteres += 1
                    Produccion.estadoA = CType(tokens(i + 1), Token).lexema
                    If CType(tokens(i + 2), Token).lexema.Equals(",") Then
                        contadorCaracteres += 1
                        'tiene que venir un terminal o no
                        If buscarPalabra(CType(tokens(i + 3), Token).lexema, "Terminal") Then
                            contadorCaracteres += 1
                            Produccion.transicion = CType(tokens(i + 3), Token).lexema
                            If CType(tokens(i + 4), Token).lexema.Equals(",") Then
                                contadorCaracteres += 1
                                If buscarPalabra(CType(tokens(i + 5), Token).lexema, "No Terminal") Then
                                    contadorCaracteres += 1
                                    Produccion.estadoB = CType(tokens(i + 5), Token).lexema
                                    If CType(tokens(i + 6), Token).lexema.Equals("}") Then
                                        contadorCaracteres += 1
                                        If CType(tokens(i + 7), Token).lexema.Equals(",") Then
                                            contadorCaracteres += 1
                                            automata.setProduccion(Produccion)
                                            i = i + 7
                                        ElseIf CType(tokens(i + 7), Token).lexema.Equals("]") Then
                                            contadorCaracteres += 1
                                            automata.setProduccion(Produccion)
                                            Return contadorCaracteres
                                        End If
                                    End If
                                End If
                            End If
                        Else
                            If buscarPalabra(CType(tokens(i + 3), Token).lexema, "No Terminal") Then
                                contadorCaracteres += 1
                                Produccion.estadoB = CType(tokens(i + 3), Token).lexema
                                Produccion.transicion = "Epsilon"
                                If CType(tokens(i + 4), Token).lexema.Equals("}") Then
                                    contadorCaracteres += 1
                                    If CType(tokens(i + 5), Token).lexema.Equals(",") Then
                                        contadorCaracteres += 1
                                        automata.setProduccion(Produccion)
                                        i = i + 5
                                    ElseIf CType(tokens(i + 5), Token).lexema.Equals("]") Then
                                        contadorCaracteres += 1
                                        automata.setProduccion(Produccion)
                                        Return contadorCaracteres
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            ElseIf CType(tokens(i), Token).lexema.Equals("]") Then
                contadorCaracteres += 1
                Console.WriteLine("-----------FIN------------")
                Return contadorCaracteres
            End If
        Next
    End Function

    'revisar si es necesario validar la ER de los no terminales
    Private Function validarNoTerminal(ByVal palabra As String) As Boolean

    End Function

    Private Function buscarPalabra(ByVal palabra As String, ByVal tipo As String) As Boolean
        Dim lista As New ArrayList

        If tipo.Equals("Terminal") Then
            lista = automata.getTerminales
        Else
            lista = automata.getNoTerminales
        End If

        For Each dato As String In lista
            If dato.Equals(palabra) Then
                Return True
            End If
        Next

        Return False
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
