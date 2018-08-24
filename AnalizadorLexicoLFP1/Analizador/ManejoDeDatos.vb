Imports System.Text.RegularExpressions

Public Class ManejoDeDatos
    Private tokens As New ArrayList

    Public Sub New(textoAnalizar As ArrayList)
        leerCadena(textoAnalizar)
    End Sub

    Private Sub leerCadena(ByRef textoAnalizar As ArrayList)
        Dim noFila As Integer = 0
        For Each cadena As String In textoAnalizar
            analizarLinea(cadena, noFila)
            'For noCol As Integer = 0 To (cadena.Length - 1)
            '    automataFinito1(cadena.Chars(noCol), noFila, noCol)
            'Next
            noFila = noFila + 1
        Next
        verTabla()
        'For Each palabra As String In lexemas
        '    Console.WriteLine(palabra)
        'Next
    End Sub

    'en palabra vamos a concatenar los caracteres que sean validos en el lenguaje y no sean delimitadores
    Dim palabra As String = ""
    'bandera para validar si estamos leyendo un identificador
    Dim esIdentificador As Boolean
    Private Sub analizarLinea(ByRef lineaALeer As String, ByRef noLinea As Integer)

        'recorremos cada indice de la cadena
        For indiceCadena As Integer = 0 To (lineaALeer.Length - 1)
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
                Case Else
                    'si no es delimitador, verificamos que sea un identificador iniciando por letra
                    If Regex.Match(caracter, "[a-z]").Success Then
                        'activamos la bandera de que estamos leyendo un identificador
                        esIdentificador = True

                        'agregamos el caracter a palabra
                        palabra = palabra & caracter

                        'salimos del select
                        Exit Select
                    End If

                    'si estamos en un identificador debemos revisar si vienen numero o _
                    If Regex.Match(caracter, "[0-9]").Success Then
                        'viene un numero, es valido
                        'agregamos el caracter a palabra
                        palabra = palabra & caracter

                        'salimos del select
                        Exit Select
                    End If

                    If Regex.Match(caracter, "_").Success Then
                        'guion bajo es un simbolo valido para los identificadores
                        'agregamos el caracter a palabra
                        palabra = palabra & caracter

                        'salimos del select
                        Exit Select
                    End If
            End Select


            'entrarAlAutomata(lineaALeer(indiceCadena), indiceCadena)

            'If caracter Is vbCr Or caracter Is vbCrLf Or caracter Is vbTab Or caracter Is vbLf Then
            '    Console.WriteLine("no soy necesario")
            'End If
            'If lineaALeer(indiceCadena).ToString Is "[" Or lineaALeer(indiceCadena).ToString Is "]" Then

            'End If
        Next
    End Sub

    Private Sub agregarTokenATabla(ByRef lexema As String, ByRef tipo As String, ByRef columna As Integer, ByRef fila As Integer)
        tokens.Add(New Token(lexema, tipo, columna, fila))
    End Sub

    Private Sub validarEstadoPalabra(ByRef estado As Boolean, ByRef columna As Integer, ByRef fila As Integer)
        If estado Then
            validarPalabraReservada(palabra, columna, fila)
            palabra = ""
            esIdentificador = False
        End If
    End Sub

    Private Sub verTabla()
        Dim i As Integer = 0
        For Each token As Token In tokens
            Console.WriteLine(i & " Lexema: " & token.lexema.ToString & " Tipo: " & token.tipo & " Fila: " & token.fila.ToString & " Columna: " & token.columna.ToString)
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
