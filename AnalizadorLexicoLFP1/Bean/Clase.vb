Public Class Clase
    Private _nombre As String
    Private _atributos As New ArrayList
    Private _metodos As New ArrayList

    Public Property nombre As String
        Get
            Return Me._nombre
        End Get
        Set(value As String)
            Me._nombre = value
        End Set
    End Property

    Public Sub setMetodo(metodo As Caracteristica)
        Me._metodos.Add(metodo)
    End Sub

    Public Function getMetodos() As ArrayList
        Return Me._metodos
    End Function

    Public Sub setAtributo(atributo As Caracteristica)
        Me._atributos.Add(atributo)
    End Sub

    Public Function getAtributos() As ArrayList
        Return Me._atributos
    End Function
End Class
