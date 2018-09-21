Public Class AnalizadorAutomata
    Private tokens As New ArrayList
    Private errores As New ArrayList
    Private existeError As Boolean
    Private esIdentificador As Boolean
    Private palabra As String = ""
    Private automata As Automata

    Public Sub analisisLexico(contenido As ArrayList)
        analizarContenido(contenido)
        verTablaTokensEnConsola()
        analizarEstructura()

        If existeError Then
            MessageBox.Show("Errores en el codigo!", "ERROR")
        Else
            MessageBox.Show("Sin Errores en el codigo!", "SIN ERRORES")
        End If

    End Sub

    Public Sub reporteDeSimbolos(contenido As ArrayList)
        analizarContenido(contenido)
        analizarEstructura()
        Dim graf As New GraficadorAutomata
        If existeError Then
            graf.dibujarReporteErrores(errores)
        Else
            graf.dibujarReporteDeSimbolos(automata)
        End If
    End Sub

    Public Sub diagramarAutomata(contenido As ArrayList)
        analizarContenido(contenido)
        analizarEstructura()
        Dim graf As New GraficadorAutomata
        If existeError Then
            graf.dibujarReporteErrores(errores)
        Else
            graf.dibujarAutomata(automata)
        End If
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
                        esIdentificador = True
                        palabra = palabra & linea(i)
                    Case "_"
                        esIdentificador = True
                        palabra = palabra & linea(i)
                    Case Else
                        validarEstadoPalabra(esIdentificador, (i - palabra.Length), noLinea)
                        agregarTokenATabla(linea(i), linea(i), i, noLinea)
                End Select
            Next
        Next
    End Sub

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
                                            If CType(tokens(i + contadorCaracteres), Token).lexema.Equals(",") Then
                                                contadorCaracteres += 1
                                                If CType(tokens(i + contadorCaracteres), Token).lexema.Equals("[") Then
                                                    contadorCaracteres += 1
                                                    'iniciamos la lectura del estado inicial
                                                    contadorCaracteres = contadorCaracteres + leerEstadoInicial(contadorCaracteres)
                                                    If CType(tokens(i + contadorCaracteres), Token).lexema.Equals("}") Then
                                                    End If
                                                End If
                                            End If
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
        For i = inicio To (tokens.Count - 1)
            If CType(tokens(i + 1), Token).lexema.Equals(",") Then
                contadorCaracteres += 1
                automata.setTerminal(CType(tokens(i), Token).lexema)
                contadorCaracteres += 1
                i = i + 1
            ElseIf CType(tokens(i + 1), Token).lexema.Equals("]") Then
                contadorCaracteres += 1
                automata.setTerminal(CType(tokens(i), Token).lexema)
                contadorCaracteres += 1
                Return contadorCaracteres
            End If
        Next
        Return contadorCaracteres
    End Function

    Private Function leerNoTerminales(ByVal inicio As Integer) As Integer
        Dim contadorCaracteres As Integer = 0
        For i = inicio To (tokens.Count - 1)
            If CType(tokens(i + 1), Token).lexema.Equals(",") Then
                contadorCaracteres += 1
                If validarNoTerminal(CType(tokens(i), Token).lexema) Then
                    existeError = True
                    errores.Add(tokens(i))
                Else
                    automata.setNoTerminal(CType(tokens(i), Token).lexema)
                End If
                contadorCaracteres += 1
                i = i + 1
            ElseIf CType(tokens(i + 1), Token).lexema.Equals("]") Then
                contadorCaracteres += 1
                If validarNoTerminal(CType(tokens(i), Token).lexema) Then
                    existeError = True
                    errores.Add(tokens(i))
                Else
                    automata.setNoTerminal(CType(tokens(i), Token).lexema)
                End If
                contadorCaracteres += 1
                Return contadorCaracteres
            End If
        Next
        Return contadorCaracteres
    End Function

    'revisar si es necesario validar la ER de los no terminales
    Private Function validarNoTerminal(ByVal palabra As String) As Boolean
        Dim errorEnNoTerminal As Boolean

        For i As Integer = 0 To (palabra.Length - 1)
            Select Case palabra(i)
                Case "a" To "z"
                Case "A" To "Z"
                Case "0" To "9"
                    If i = 0 Then
                        errorEnNoTerminal = True
                        Return errorEnNoTerminal
                    End If
                Case "_"
                    If i <> 1 Then
                        errorEnNoTerminal = True
                        Return errorEnNoTerminal
                    End If
                Case Else
                    errorEnNoTerminal = True
                    Return errorEnNoTerminal
            End Select
        Next
        Return errorEnNoTerminal
    End Function

    Private Function leerReglasDeProduccion(ByVal inicio As Integer) As Integer
        Dim contadorCaracteres As Integer = 0
        For i = inicio To (tokens.Count - 1)
            Dim produccion As New Produccion
            If CType(tokens(i), Token).lexema.Equals("{") Then
                contadorCaracteres += 1
                'tiene que venir un no terminal
                If buscarPalabra(CType(tokens(i + 1), Token).lexema, "No Terminal", i + 1) Then
                    contadorCaracteres += 1
                    produccion.estadoA = CType(tokens(i + 1), Token).lexema
                    If CType(tokens(i + 2), Token).lexema.Equals(",") Then
                        contadorCaracteres += 1
                        'tiene que venir un terminal o no
                        If buscarPalabra(CType(tokens(i + 3), Token).lexema, "Terminal", i + 3) Then
                            contadorCaracteres += 1
                            produccion.transicion = CType(tokens(i + 3), Token).lexema
                            If CType(tokens(i + 4), Token).lexema.Equals(",") Then
                                contadorCaracteres += 1
                                If buscarPalabra(CType(tokens(i + 5), Token).lexema, "No Terminal", i + 5) Then
                                    contadorCaracteres += 1
                                    produccion.estadoB = CType(tokens(i + 5), Token).lexema
                                    If CType(tokens(i + 6), Token).lexema.Equals("}") Then
                                        contadorCaracteres += 1
                                        If CType(tokens(i + 7), Token).lexema.Equals(",") Then
                                            contadorCaracteres += 1
                                            automata.setProduccion(produccion)
                                            i = i + 7
                                        ElseIf CType(tokens(i + 7), Token).lexema.Equals("]") Then
                                            contadorCaracteres += 1
                                            automata.setProduccion(produccion)
                                            Return contadorCaracteres
                                        End If
                                    End If
                                End If
                            End If
                        Else
                            If buscarPalabra(CType(tokens(i + 3), Token).lexema, "No Terminal", i + 3) Then
                                contadorCaracteres += 1
                                produccion.estadoB = CType(tokens(i + 3), Token).lexema
                                produccion.transicion = "Epsilon"
                                If CType(tokens(i + 4), Token).lexema.Equals("}") Then
                                    contadorCaracteres += 1
                                    If CType(tokens(i + 5), Token).lexema.Equals(",") Then
                                        contadorCaracteres += 1
                                        automata.setProduccion(produccion)
                                        i = i + 5
                                    ElseIf CType(tokens(i + 5), Token).lexema.Equals("]") Then
                                        contadorCaracteres += 1
                                        automata.setProduccion(produccion)
                                        Return contadorCaracteres
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            ElseIf CType(tokens(i), Token).lexema.Equals("]") Then
                contadorCaracteres += 1
                Return contadorCaracteres
            End If
        Next
        Return contadorCaracteres
    End Function

    Private Function leerEstadoInicial(ByVal inicio As Integer) As Integer
        Dim contadorCaracteres As Integer = 0
        If buscarPalabra(CType(tokens(inicio), Token).lexema, "No Terminal", inicio) Then
            automata.estadoInicial = CType(tokens(inicio), Token).lexema
            contadorCaracteres += 1
            If CType(tokens(inicio + 1), Token).lexema.Equals("]") Then
                contadorCaracteres += 1
                Return contadorCaracteres
            End If
        End If
        Return contadorCaracteres
    End Function

    Private Function buscarPalabra(ByVal palabra As String, ByVal tipo As String, ByVal posicionToken As Integer) As Boolean
        Dim lista As New ArrayList

        If tipo.Equals("Terminal") Then
            lista = automata.getTerminales
        Else
            lista = automata.getNoTerminales
        End If

        If lista.Contains(palabra) Then
            Return True
        End If

        If errores.Contains(tokens(posicionToken)) Then
        Else
            If tipo.Equals("No Terminal") Then
                errores.Add(tokens(posicionToken))
                existeError = True
            End If
        End If
        Return False
    End Function
End Class
