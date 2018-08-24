Imports System.Text.RegularExpressions

Public Class ManejoDeDatos
    Private tokens As New ArrayList
    Private errores As New ArrayList

    Public Sub New(textoAnalizar As ArrayList)
        analizarDatos(textoAnalizar)
    End Sub

    Private Sub analizarDatos(ByRef textoAnalizar As ArrayList)
        Dim noFila As Integer = 0
        For Each cadena As String In textoAnalizar
            analizarLinea(cadena, noFila)
            noFila = noFila + 1
        Next
        If errorEncontrado Then
            verTablaErrores()
        Else
            'antes de mostrar la tabla de tokens hay que analizar si se cumple la "sintaxis"
            verTablaTokens()
        End If
    End Sub

    'en palabra vamos a concatenar los caracteres que sean validos en el lenguaje y no sean delimitadores
    Dim palabra As String = ""
    'bandera para validar si estamos leyendo un identificador
    Dim esIdentificador As Boolean
    'bandera para ver si existe errores
    Dim errorEncontrado As Boolean

    Private Sub analizarLinea(ByRef lineaALeer As String, ByRef noLinea As Integer)
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
                    validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
                Case vbLf
                    validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
                Case " "
                    validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
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
                    agregarTokenATabla(caracter, "Palabra Reservada", indiceCadena, noLinea)
                Case "-"
                    validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
                    agregarTokenATabla(caracter, "Palabra Reservada", indiceCadena, noLinea)
                Case "#"
                    validarEstadoPalabra(esIdentificador, (indiceCadena - palabra.Length), noLinea)
                    agregarTokenATabla(caracter, "Palabra Reservada", indiceCadena, noLinea)
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

    Private Sub agregarTokenATabla(ByRef lexema As String, ByRef tipo As String, ByRef columna As Integer, ByRef fila As Integer)
        tokens.Add(New Token(lexema, tipo, columna, fila))
    End Sub

    Private Sub agregarErrorATabla(ByRef lexema As String, ByRef tipo As String, ByRef columna As Integer, ByRef fila As Integer)
        errores.Add(New Token(lexema, tipo, columna, fila))
        errorEncontrado = True
    End Sub

    Private Sub validarEstadoPalabra(ByRef estado As Boolean, ByRef columna As Integer, ByRef fila As Integer)
        If estado Then
            validarPalabraReservada(palabra, columna, fila)
            palabra = ""
            esIdentificador = False
        End If
    End Sub

    Private Sub verTablaTokens()
        Dim i As Integer = 0
        For Each token As Token In tokens
            Console.WriteLine(i & " Lexema: " & token.lexema.ToString & " Tipo: " & token.tipo & " Fila: " & token.fila.ToString & " Columna: " & token.columna.ToString)
            i = i + 1
        Next
    End Sub

    Private Sub verTablaErrores()
        Dim i As Integer = 1
        For Each e As Token In errores
            Console.WriteLine(i & " error: " & e.lexema.ToString & " Fila: " & e.fila.ToString & " Columna: " & e.columna.ToString)
            i = i + 1
        Next
    End Sub

    Private Sub validarPalabraReservada(ByRef palabra As String, ByRef columna As Integer, ByRef fila As Integer)
        Dim tipo As String
        Select Case palabra
            Case "clase"
                tipo = "Palabra Reservada"
            Case "nombre"
                tipo = "Palabra Reservada"
            Case "atributos"
                tipo = "Palabra Reservada"
            Case "metodos"
                tipo = "Palabra Reservada"
            Case "asociacion"
                tipo = "Palabra Reservada"
            Case "agregacion"
                tipo = "Palabra Reservada"
            Case "composicion"
                tipo = "Palabra Reservada"
            Case "asociacionsimple"
                tipo = "Palabra Reservada"
            Case "herencia"
                tipo = "Palabra Reservada"
            Case Else
                tipo = "Identificador"
        End Select
        agregarTokenATabla(palabra, tipo, columna, fila)
    End Sub

End Class
