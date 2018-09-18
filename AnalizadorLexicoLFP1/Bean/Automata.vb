Public Class Automata
    Private _Terminales As ArrayList
    Private _NoTerminales As ArrayList
    Private _Producciones As ArrayList
    Private _EstadoInicial As String

    Public Sub New()
        Me._Terminales = New ArrayList
        Me._NoTerminales = New ArrayList
        Me._Producciones = New ArrayList
    End Sub

    Public Sub setTerminal(terminal As String)
        Me._Terminales.Add(terminal)
    End Sub

    Public Function getTerminales() As ArrayList
        Return Me._Terminales
    End Function

    Public Sub setNoTerminal(noTerminal As String)
        Me._NoTerminales.Add(noTerminal)
    End Sub

    Public Function getNoTerminales() As ArrayList
        Return Me._NoTerminales
    End Function

    Public Sub setProduccion(produccion As Produccion)
        Me._Producciones.Add(produccion)
    End Sub

    Public Function getProducciones() As ArrayList
        Return Me._Producciones
    End Function

    Public Property estadoInicial As String
        Get
            Return Me._EstadoInicial
        End Get
        Set(value As String)
            Me._EstadoInicial = value
        End Set
    End Property
End Class
