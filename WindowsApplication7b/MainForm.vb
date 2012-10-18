Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.IO

Public Class MainForm
    Private _cancellationTokenSource As CancellationTokenSource
    Private _cancellationToken As CancellationToken

    Private Async Sub getHtmlButton_Click(sender As Object, e As EventArgs) Handles getHtmlButton.Click
        _cancellationTokenSource = New CancellationTokenSource
        _cancellationToken = _cancellationTokenSource.Token

        Dim request As HttpWebRequest = (DirectCast(WebRequest.Create("http://google.ca"), HttpWebRequest))

        getHtmlButton.Enabled = False
        progressBar.Value = 0
        progressBar.Visible = True
        cancelButton.Enabled = True
        cancelButton.Visible = True

        Dim index As Int32 = 0

        Dim inputBuffer As Byte()
        Using response As WebResponse = Await request.GetResponseAsync()
            inputBuffer = New Byte(response.ContentLength) {}
            Dim stream As Stream = response.GetResponseStream()
            Dim progress As New MyProgress
            AddHandler progress.ProgressChanged, AddressOf progress_ProgressChanged

            Dim bytesRead As Int32
            Do
                Try
                    Dim num1 As Integer = Await stream.ReadAsync(inputBuffer, index, inputBuffer.Length - index, _cancellationToken)
                    Dim num2 As Integer = num1
                    If (Not _cancellationToken.IsCancellationRequested) Then
                        index = index + num2
                        progress.Progress(index * 100 / inputBuffer.Length)
                    Else
                        Exit Do
                    End If
                Catch taskCanceledException As TaskCanceledException
                    Exit Do
                End Try
            Loop While bytesRead <> 0
        End Using

        listBox.Items.Clear()
        If Not _cancellationToken.IsCancellationRequested Then
            Trace.WriteLine(String.Format("Read {0} bytes", index))
            Dim html As String = Encoding.ASCII.GetString(inputBuffer, 0, index)
            If Not String.IsNullOrEmpty(html) Then
                For Each x As String In GetScriptBodies(html)
                    listBox.Items.Add(x)
                Next
            End If
        Else
            listBox.Items.Add("CANCELLED")
            progressBar.Visible = False
        End If

        getHtmlButton.Enabled = True
        cancelButton.Enabled = False
        cancelButton.Visible = False
    End Sub

    Private Sub progress_ProgressChanged(sender As Object, e As Int32)
        progressBar.Value = e
    End Sub

    Private Sub cancelButton_Click(sender As Object, e As EventArgs) Handles cancelButton.Click

    End Sub

    Private Shared Function GetScriptBodies(ByVal html As String) As IEnumerable(Of String)
        Const pattern As String = "<script.*?>\s*(?'scriptBody'.+?)\s*</script>"
        Dim matches As MatchCollection = Regex.Matches(html, pattern)
        Dim result As List(Of String) = New List(Of String)(matches.Count)
        result.AddRange(From match As Match In matches Select match.Groups("scriptBody").Value)
        Return result
    End Function
End Class