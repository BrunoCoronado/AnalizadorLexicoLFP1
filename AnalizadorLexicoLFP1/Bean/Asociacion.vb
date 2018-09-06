Public Class Asociacion
    Private _padre As String
    Private _hijo As String
    Private _asociacion As String

    Public Property padre As String
        Get
            Return Me._padre
        End Get
        Set(value As String)
            Me._padre = value
        End Set
    End Property

    Public Property hijo As String
        Get
            Return Me._hijo
        End Get
        Set(value As String)
            Me._hijo = value
        End Set
    End Property

    Public Property asociacion As String
        Get
            Return Me._asociacion
        End Get
        Set(value As String)
            Select Case value
                Case "agregacion"
                    Me._asociacion = "odiamond"
                Case "composicion"
                    Me._asociacion = "diamond"
                Case "asociacionsimple"
                    Me._asociacion = "none"
                Case "herencia"
                    Me._asociacion = "onormal"
            End Select
        End Set
    End Property
End Class
