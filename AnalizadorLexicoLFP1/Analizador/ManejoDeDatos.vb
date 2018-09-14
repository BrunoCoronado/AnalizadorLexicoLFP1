
Public Class ManejoDeDatos
    Private tokens As New ArrayList
    Private errores As New ArrayList
    Private indicesDeClases As New ArrayList
    Private clases As New ArrayList
    Private clase As Clase
    Private esAtributo As Boolean
    Private asociaciones As New ArrayList
    Private indicesDeAsociaciones As New ArrayList

    'en palabra vamos a concatenar los caracteres que sean validos en el lenguaje y no sean delimitadores
    Dim palabra As String = ""
    'bandera para validar si estamos leyendo un identificador
    Dim esIdentificador As Boolean
    'bandera para ver si existe errores
    Dim errorEncontrado As Boolean
    'banderas para validar el contenido de la clase
    Dim contieneNombre As Boolean
    Dim contieneMetodos As Boolean

    Public Sub analizarLexico(textoAnalizar As ArrayList)
        analizarDatos(textoAnalizar)

        If errorEncontrado Then
            verTablaErroresEnConsola()
            MessageBox.Show("Errores en el codigo! *revisar Consola", "ERROR")
        Else
            verTablaTokensEnConsola() ' genera el reporte de tokens
            MessageBox.Show("Sin Errores en el codigo! *revisar Consola", "SIN ERRORES")
        End If
    End Sub

    Public Sub reporteDeTokens(textoAnalizar As ArrayList)
        analizarDatos(textoAnalizar)

        If errorEncontrado Then
            verTablaErroresEnConsola()
            Dim graficador As New Graficador
            graficador.dibujarReporteErrores(errores)
        Else
            verTablaTokensEnConsola() ' genera el reporte de tokens
            Dim graficador As New Graficador
            graficador.dibujarReporteTokens(tokens)
        End If
    End Sub

    Public Sub diagramarCodigo(textoAnalizar As ArrayList)
        analizarDatos(textoAnalizar)

        If errorEncontrado Then
            verTablaErroresEnConsola()
            MessageBox.Show("Errores en el codigo! *revisar Consola", "ERROR")
        Else
            verificarSintaxis() ' inicia el proces de diagramar con las funciones de abajo
            Dim graficador As New Graficador
            graficador.dibujarDiagrama(clases, asociaciones)
        End If
    End Sub

    Private Sub analizarDatos(ByVal textoAnalizar As ArrayList)
        Dim noFila As Integer = 0
        For Each cadena As String In textoAnalizar
            analizarLinea(cadena, noFila)
            noFila = noFila + 1
        Next
    End Sub

    Private Sub analizarLinea(ByVal lineaALeer As String, ByVal noLinea As Integer)
        Dim indiceCadena As Integer
        'recorremos cada indice de la cadena
        For indiceCadena = 0 To (lineaALeer.Length - 1)
            'lee caracter por caracter segun la linea que se lee en minusculas
            Dim caracter As String = lineaALeer(indiceCadena).ToString.ToLower

            'verificamos que el caracter pertenezca al lenguaje segun el automata del lenguaje aceptado
            Select Case caracter
                Case vbCrLf
                    validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
                Case vbCr
                    validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
                Case vbTab
                    'validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
                Case vbLf
                    validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
                Case " "
                    'validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
                Case "["
                    validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
                    agregarTokenATabla(caracter, "Delimitador", indiceCadena, noLinea)
                Case "]"
                    validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
                    agregarTokenATabla(caracter, "Delimitador", indiceCadena, noLinea)
                Case "{"
                    validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
                    agregarTokenATabla(caracter, "Delimitador", indiceCadena, noLinea)
                Case "}"
                    validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
                    agregarTokenATabla(caracter, "Delimitador", indiceCadena, noLinea)
                Case "("
                    validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
                    agregarTokenATabla(caracter, "Delimitador", indiceCadena, noLinea)
                Case ")"
                    validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
                    agregarTokenATabla(caracter, "Delimitador", indiceCadena, noLinea)
                Case "="
                    validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
                    agregarTokenATabla(caracter, "Delimitador", indiceCadena, noLinea)
                Case "+"
                    validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
                    agregarTokenATabla(caracter, "Palabra Reservada Visibilidad", indiceCadena, noLinea)
                Case "-"
                    validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
                    agregarTokenATabla(caracter, "Palabra Reservada Visibilidad", indiceCadena, noLinea)
                Case "#"
                    validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
                    agregarTokenATabla(caracter, "Palabra Reservada Visibilidad", indiceCadena, noLinea)
                Case ":"
                    validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
                    agregarTokenATabla(caracter, "Delimitador", indiceCadena, noLinea)
                Case ";"
                    validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
                    agregarTokenATabla(caracter, "Delimitador", indiceCadena, noLinea)
                Case "a" To "z"
                    'activamos la bandera de que estamos leyendo un identificador
                    esIdentificador = True

                    'agregamos el caracter a palabra
                    palabra = palabra & caracter
                Case "0" To "9"
                    If esIdentificador Then
                        palabra = palabra & caracter
                    Else
                        'no puede iniciar con numero una palabra
                        agregarErrorATabla(caracter, "Error", indiceCadena, noLinea)
                    End If
                Case "_"
                    If esIdentificador Then
                        palabra = palabra & caracter
                    Else
                        'no puede iniciar con _ una palabra
                        agregarErrorATabla(caracter, "Error", indiceCadena, noLinea)
                    End If
                Case Else
                    'manejar los errores que no pertenecen al lenguaje
                    agregarErrorATabla(caracter, "Error", indiceCadena, noLinea)
            End Select
        Next
        validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
    End Sub

    Private Sub agregarTokenATabla(ByVal lexema As String, ByVal tipo As String, ByVal columna As Integer, ByVal fila As Integer)
        tokens.Add(New Token(lexema, tipo, columna, fila))
    End Sub

    Private Sub agregarErrorATabla(ByVal lexema As String, ByVal tipo As String, ByVal columna As Integer, ByVal fila As Integer)
        errores.Add(New Token(lexema, tipo, columna, fila))
        errorEncontrado = True
    End Sub

    Private Sub validarEstadoPalabra(ByVal estado As Boolean, ByVal columna As Integer, ByVal fila As Integer)
        If estado Then
            validarPalabraReservada(palabra, columna, fila)
            palabra = ""
            esIdentificador = False
        End If
    End Sub

    Private Sub verTablaTokensEnConsola()
        Dim i As Integer = 0
        For Each token As Token In tokens
            Console.WriteLine(i & " Lexema: " & token.lexema.ToString & " Tipo: " & token.tipo & " Fila: " & token.fila.ToString & " Columna: " & token.columna.ToString)
            i = i + 1
        Next
    End Sub

    Private Sub verTablaErroresEnConsola()
        Dim i As Integer = 1
        For Each e As Token In errores
            Console.WriteLine(i & " error: " & e.lexema.ToString & " Fila: " & e.fila.ToString & " Columna: " & e.columna.ToString)
            i = i + 1
        Next
    End Sub

    Private Sub validarPalabraReservada(ByVal palabra As String, ByVal columna As Integer, ByVal fila As Integer)
        Dim tipo As String
        Select Case palabra
            Case "clase"
                tipo = "Palabra Reservada"
                'guardar indice donde se encuentra en listado aparte
                indicesDeClases.Add(tokens.Count)
            Case "nombre"
                tipo = "Palabra Reservada"
            Case "atributos"
                tipo = "Palabra Reservada"
            Case "metodos"
                tipo = "Palabra Reservada"
            Case "asociacion"
                tipo = "Palabra Reservada"
                'guardar indice donde se encuentra la asociacion
                indicesDeAsociaciones.Add(tokens.Count)
            Case "agregacion"
                tipo = "Palabra Reservada Asociacion"
            Case "composicion"
                tipo = "Palabra Reservada Asociacion"
            Case "asociacionsimple"
                tipo = "Palabra Reservada Asociacion"
            Case "herencia"
                tipo = "Palabra Reservada Asociacion"
            Case Else
                tipo = "Identificador"
        End Select
        agregarTokenATabla(palabra, tipo, columna, fila)
    End Sub

    Private Sub verificarSintaxis()
        Dim indice As Integer
        'analizamos los bloques clase
        For i As Integer = 0 To (indicesDeClases.Count - 1)
            indice = indicesDeClases(i)
            If CType(tokens(indice - 1), Token).lexema.Equals("[") Then
                If CType(tokens(indice), Token).lexema.Equals("clase") Then
                    If CType(tokens(indice + 1), Token).lexema.Equals("]") Then
                        If CType(tokens(indice + 2), Token).lexema.Equals("{") Then
                            clase = New Clase
                            leerContenidoClase(indice + 3)
                            If contieneNombre = True And contieneMetodos = True Then
                                contieneMetodos = False
                                contieneNombre = False
                                clases.Add(clase)
                                Console.WriteLine("clase agregada")
                            Else
                                Console.WriteLine("clase no agragada-falta bloque obligatorio")
                            End If
                        End If
                    End If
                End If
            End If
        Next

        'analizamos los bloques asociacion
        For i As Integer = 0 To (indicesDeAsociaciones.Count - 1)
            indice = indicesDeAsociaciones(i)
            If CType(tokens(indice - 1), Token).lexema.Equals("[") Then
                If CType(tokens(indice), Token).lexema.Equals("asociacion") Then
                    If CType(tokens(indice + 1), Token).lexema.Equals("]") Then
                        If CType(tokens(indice + 2), Token).lexema.Equals("{") Then
                            leerContenidoAsociacion(indice + 3)
                        End If
                    End If
                End If
            End If
        Next
    End Sub

    Private Sub leerContenidoAsociacion(ByVal inicio As Integer)
        Dim contadorTokens As Integer = 0

        For i As Integer = inicio To tokens.Count
            Dim asociacion As New Asociacion
            Select Case CType(tokens(i), Token).tipo
                Case "Identificador"
                    contadorTokens += 1

                    'reconocemos al padre de la asociacion
                    asociacion.padre = CType(tokens(i), Token).lexema
                    If CType(tokens(i + 1), Token).lexema.Equals(":") Then
                        contadorTokens += 1
                        If CType(tokens(i + 2), Token).tipo.Equals("Palabra Reservada Asociacion") Then
                            contadorTokens += 1
                            'reconocemos la asociacion
                            asociacion.asociacion = CType(tokens(i + 2), Token).lexema
                            If CType(tokens(i + 3), Token).lexema.Equals(":") Then
                                contadorTokens += 1
                                If CType(tokens(i + 4), Token).tipo.Equals("Identificador") Then
                                    contadorTokens += 1
                                    'reconocemos al hijo de la asociacion
                                    asociacion.hijo = CType(tokens(i + 4), Token).lexema
                                    If CType(tokens(i + 5), Token).lexema.Equals(";") Then

                                        asociaciones.Add(asociacion)
                                        Console.WriteLine("asociacion correcta")
                                        i = i + contadorTokens
                                        contadorTokens = 0
                                    End If
                                End If
                            End If
                        End If
                    End If



                Case "Delimitador"
                    Exit For
                    'terminan los atributos
            End Select
        Next


        'If CType(tokens(inicio), Token).tipo.Equals("Identificador") Then
        '    'reconocemos al padre de la asociacion
        '    asociacion.padre = CType(tokens(inicio), Token).lexema
        '    If CType(tokens(inicio + 1), Token).lexema.Equals(":") Then
        '        If CType(tokens(inicio + 2), Token).tipo.Equals("Palabra Reservada Asociacion") Then
        '            'reconocemos la asociacion
        '            asociacion.asociacion = CType(tokens(inicio + 2), Token).lexema
        '            If CType(tokens(inicio + 3), Token).lexema.Equals(":") Then
        '                If CType(tokens(inicio + 4), Token).tipo.Equals("Identificador") Then
        '                    'reconocemos al hijo de la asociacion
        '                    asociacion.hijo = CType(tokens(inicio + 4), Token).lexema
        '                    If CType(tokens(inicio + 5), Token).lexema.Equals(";") Then
        '                        asociaciones.Add(asociacion)
        '                        Console.WriteLine("asociacion correcta")
        '                    End If
        '                End If
        '            End If
        '        End If
        '    End If
        'End If
    End Sub

    Private Sub leerContenidoClase(ByVal inicio As Integer)
        For i As Integer = inicio To tokens.Count
            Select Case CType(tokens(i), Token).lexema
                Case "["
                    Select Case CType(tokens(i + 1), Token).lexema
                        Case "nombre"
                            'leer nombre
                            If CType(tokens(i + 2), Token).lexema.Equals("]") Then
                                i = i + leerNombreClase(i + 3)
                            End If
                        Case "atributos"
                            esAtributo = True
                            'leer atributos
                            If CType(tokens(i + 2), Token).lexema.Equals("]") Then
                                i = i + leerAtributosOMetodos(i + 3)
                            End If
                        Case "metodos"
                            esAtributo = False
                            contieneMetodos = True
                            'leer metodos
                            If CType(tokens(i + 2), Token).lexema.Equals("]") Then
                                i = i + leerAtributosOMetodos(i + 3)
                            End If
                    End Select
                Case "}"
                    'final de la clase
                    Console.WriteLine("Clase Correcta")
                    Exit For
            End Select
        Next
    End Sub

    Private Function leerAtributosOMetodos(ByRef inicio As Integer) As Integer
        Dim contadorTokens As Integer = 2
        If CType(tokens(inicio), Token).lexema.Equals("{") Then
            contadorTokens += 1
            'leer cada atributo existente
            For i As Integer = inicio + 1 To tokens.Count
                Select Case CType(tokens(i), Token).lexema
                    Case "("
                        contadorTokens += 1
                        'leer atributo
                        Dim tokensContados As Integer = leerAtributoOMetodo(i + 1)
                        i = i + tokensContados
                        contadorTokens = contadorTokens + tokensContados
                    Case "}"
                        contadorTokens += 1
                        Exit For
                        'terminan los atributos
                End Select
            Next
        End If
        Return contadorTokens
    End Function

    Private Function leerAtributoOMetodo(ByRef inicio As Integer) As Integer
        Dim caracteristica As New Caracteristica

        Dim contadorTokens As Integer = 0
        If CType(tokens(inicio), Token).tipo.Equals("Palabra Reservada Visibilidad") Then
            caracteristica.visibilidad = CType(tokens(inicio), Token).lexema
            contadorTokens += 1
            If CType(tokens(inicio + 1), Token).lexema.Equals(")") Then
                contadorTokens += 1
                If CType(tokens(inicio + 2), Token).tipo.Equals("Identificador") Then
                    caracteristica.identificador = CType(tokens(inicio + 2), Token).lexema
                    contadorTokens += 1
                    Select Case CType(tokens(inicio + 3), Token).lexema
                        Case ":"
                            contadorTokens += 1
                            If CType(tokens(inicio + 4), Token).tipo.Equals("Identificador") Then
                                caracteristica.tipo = CType(tokens(inicio + 4), Token).lexema
                                contadorTokens += 1
                                If CType(tokens(inicio + 5), Token).lexema.Equals(";") Then
                                    'fin

                                    If esAtributo Then
                                        clase.setAtributo(caracteristica)
                                    Else
                                        clase.setMetodo(caracteristica)
                                    End If

                                    contadorTokens += 1
                                    Return contadorTokens
                                End If
                            End If
                        Case ";"
                            'fin

                            If esAtributo Then
                                clase.setAtributo(caracteristica)
                            Else
                                clase.setMetodo(caracteristica)
                            End If

                            contadorTokens += 1
                            Return contadorTokens
                    End Select
                End If
            End If
        End If
        Return contadorTokens
    End Function

    Private Function leerNombreClase(ByRef inicio As Integer) As Integer
        Dim contadorTokens As Integer = 2
        If CType(tokens(inicio), Token).lexema.Equals("=") Then
            contadorTokens += 1
            If CType(tokens(inicio + 1), Token).tipo.Equals("Identificador") Then
                contadorTokens += 1
                'guardamos el nombre de la clase
                Me.clase.nombre = CType(tokens(inicio + 1), Token).lexema
                contieneNombre = True
                If CType(tokens(inicio + 2), Token).lexema.Equals(";") Then
                    'lectura de nombre de clase finalizada  
                    contadorTokens += 1
                    Return contadorTokens
                End If
            End If
        End If
        Return contadorTokens
    End Function

End Class
