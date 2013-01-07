Imports System.Threading
Imports System.Text
Imports System.IO
Imports System.Net
Imports System.Text.RegularExpressions

Public Class MainForm
    Private _cancellationTokenSource As CancellationTokenSource
    Private _cancellationToken As CancellationToken

    Private Sub getHtmlButton_Click(sender As Object, e As EventArgs) Handles getHtmlButton.Click
        _cancellationTokenSource = New CancellationTokenSource
        _cancellationToken = _cancellationTokenSource.Token
        ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf StartRequest))

        getHtmlButton.Enabled = False
        progressBar.Enabled = True
        progressBar.Visible = True
        cancelButton.Enabled = True
        cancelButton.Visible = True
    End Sub

    Private Sub StartRequest()
        Dim inputBuffer(1024000) As Byte

        Dim request As HttpWebRequest =
                (DirectCast(WebRequest.Create("http://en.wikipedia.org/wiki/Line_of_succession_to_the_British_throne"), 
                            HttpWebRequest))

        request.
            BeginGetResponse(
                Sub(asyncResult As IAsyncResult)
                    Dim response As WebResponse = request.EndGetResponse(asyncResult)
                    Dim responseStream As Stream = response.GetResponseStream()
                    If responseStream Is Nothing Then Exit Sub

                    Dim bytesRead As Task(Of Integer) =
                            Task(Of Integer).Factory.FromAsync(AddressOf responseStream.BeginReadToEnd,
                                                                AddressOf responseStream.EndReadToEnd, inputBuffer, 0,
                                                                inputBuffer.Length, responseStream)
                    If (InvokeRequired) Then
                        BeginInvoke(Sub()
                                        progressBar.Value = 50
                                    End Sub
                                    )
                    End If
                    Dim ui As TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext()
                    bytesRead.
                                ContinueWith(
                                    Sub(ar As Task(Of Integer))
                                        progressBar.Value = 100
                                        If (Not _cancellationToken.IsCancellationRequested) Then
                                            Trace.WriteLine(String.Format("Read {0} bytes", ar.Result))
                                            Dim text As String = Encoding.ASCII.GetString(inputBuffer, 0, ar.Result)
                                            Me.SetData(text)
                                        End If
                                        getHtmlButton.Enabled = True
                                        progressBar.Enabled = True
                                        progressBar.Visible = True
                                        cancelButton.Enabled = False
                                        cancelButton.Visible = False
                                        DirectCast(response, IDisposable).Dispose()
                                    End Sub, ui)
                End Sub, request)
    End Sub

    Private Sub cancelButton_Click(sender As Object, e As EventArgs) Handles cancelButton.Click
        _cancellationTokenSource.Cancel()
    End Sub

    Private Sub SetData(ByVal html As String)
        If String.IsNullOrEmpty(html) Then Return

        listBox.Items.Clear()
        For Each x As String In GetScriptBodies(html)
            listBox.Items.Add(x)
        Next
    End Sub

    Private Shared Function GetScriptBodies(ByVal html As String) As IEnumerable(Of String)
        Const pattern As String = "<script.*?>\s*(?'scriptBody'.+?)\s*</script>"
        Dim matches As MatchCollection = Regex.Matches(html, pattern)
        Dim result As List(Of String) = New List(Of String)(matches.Count)
        result.AddRange(From match As Match In matches Select match.Groups("scriptBody").Value)
        Return result
    End Function
End Class
