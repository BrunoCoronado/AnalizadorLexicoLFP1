Public Class Token
    Private _lexema As String
    Private _tipo As String
    Private _columna As Integer
    Private _fila As Integer

    Public Sub New(lexema As String, tipo As String, columna As Integer, fila As Integer)
        Me._lexema = lexema
        Me._tipo = tipo
        Me._columna = columna
        Me._fila = fila
    End Sub

    Public Property lexema As String
        Get
            Return Me._lexema
        End Get
        Set(value As String)
            Me._lexema = value
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

    Public Property columna As Integer
        Get
            Return Me._columna
        End Get
        Set(value As Integer)
            Me._columna = value
        End Set
    End Property

    Public Property fila As Integer
        Get
            Return Me._fila
        End Get
        Set(value As Integer)
            Me._fila = value
        End Set
    End Property
End Class
