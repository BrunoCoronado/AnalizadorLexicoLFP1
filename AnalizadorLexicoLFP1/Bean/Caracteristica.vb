Public Class Caracteristica
    Private _visibilidad As Char
    Private _identificador As String
    Private _tipo As String

    Public Sub New()
    End Sub

    Public Sub New(visibilidad As Char, identificador As String, tipo As String)
        _visibilidad = visibilidad
        _identificador = identificador
        _tipo = tipo
    End Sub

    Public Sub New(visibilidad As Char, identificador As String)
        _visibilidad = visibilidad
        _identificador = identificador
    End Sub

    Public Property visibilidad As Char
        Get
            Return Me._visibilidad
        End Get
        Set(value As Char)
            Me._visibilidad = value
        End Set
    End Property

    Public Property identificador As String
        Get
            Return Me._identificador
        End Get
        Set(value As String)
            Me._identificador = value
        End Set
    End Property

    Public Property tipo As String
        Get
            Return Me._tipo
        End Get
        Set(value As String)
            Me._tipo = tipo
        End Set
    End Property
End Class
