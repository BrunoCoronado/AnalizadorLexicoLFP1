Public Class Caracteristica
    Private _visibilidad As Char
    Private _identificador As String
    Private _tipo As String

    Public Sub New()
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
            Me._tipo = value
        End Set
    End Property
End Class
