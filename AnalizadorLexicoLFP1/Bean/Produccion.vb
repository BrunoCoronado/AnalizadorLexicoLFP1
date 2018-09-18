Public Class Produccion
    Private _EstadoA As String
    Private _EstadoB As String
    Private _Transicion As String

    Public Property estadoA As String
        Get
            Return Me._EstadoA
        End Get
        Set(value As String)
            Me._EstadoA = value
        End Set
    End Property

    Public Property estadoB As String
        Get
            Return Me._EstadoB
        End Get
        Set(value As String)
            Me._EstadoB = value
        End Set
    End Property

    Public Property transicion As String
        Get
            Return Me._Transicion
        End Get
        Set(value As String)
            Me._Transicion = value
        End Set
    End Property
End Class
