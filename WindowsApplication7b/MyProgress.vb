Public Class MyProgress
    Inherits Progress(Of Int32)

    Public Sub Progress(ByVal value As Integer)
        OnReport(value)
    End Sub
End Class