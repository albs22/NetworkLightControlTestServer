Public Class UpdatePinStatusArgs : Inherits System.EventArgs

    Private _Pin As Integer
    Private _Status As Integer

    Public Sub New(ByVal pin As Integer, ByVal status As Integer)
        _Pin = pin
        _Status = status
    End Sub

    Public Property Pin() As Integer
        Get
            Return _Pin
        End Get
        Set(value As Integer)
            _Pin = value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            Return _Status
        End Get
        Set(value As Integer)
            _Status = value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return "Pin: " & Pin & "  Status: " & Status & vbCrLf
    End Function

End Class
