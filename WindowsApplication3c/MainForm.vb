Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Text
Imports System.IO
Imports System.ComponentModel

Public Class MainForm
    Dim _webRequest As HttpWebRequest

    Private Sub getHtmlButton_Click(sender As Object, e As EventArgs) Handles getHtmlButton.Click
        listBox.Items.Clear()
        _webRequest = DirectCast(WebRequest.Create("http://www.gutenberg.org/files/1497/1497.txt"), HttpWebRequest)
        BackgroundWorker.RunWorkerAsync(_webRequest)
        getHtmlButton.Enabled = False
        progressBar.Value = 0
        progressBar.Visible = True
        cancelButton.Visible = True
    End Sub

    Private Sub cancelButton_Click(sender As Object, e As EventArgs) Handles cancelButton.Click
        Dim request As HttpWebRequest = _webRequest
        If (Not request Is Nothing) Then
            BackgroundWorker.CancelAsync()
            request.Abort()
        End If
    End Sub

    Private Shared Function GetScriptBodies(ByVal html As String) As IEnumerable(Of String)
        Const pattern As String = "<script.*?>\s*(?'scriptBody'.+?)\s*</script>"
        Dim matches As MatchCollection = Regex.Matches(html, pattern)
        Dim result As List(Of String) = New List(Of String)(matches.Count)
        For Each match As Match In matches
            Dim scriptBody As String = match.Groups("scriptBody").Value
            result.Add(scriptBody)
        Next
        Return result
    End Function

    Private Sub BackgroundWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) _
        Handles BackgroundWorker.DoWork
        Dim progress As Integer = 1
        Dim backgroundWokrer As BackgroundWorker = DirectCast(sender, BackgroundWorker)
        backgroundWokrer.ReportProgress(progress)
        Dim request As HttpWebRequest = DirectCast(e.Argument, HttpWebRequest)
        Try
            Dim response As WebResponse = request.GetResponse()
            Dim stream As Stream = response.GetResponseStream()
            Dim numArray(1024000) As Byte
            Dim asyncResult As IAsyncResult = StreamHelper.
                    BeginReadStreamToEnd(stream, numArray, 0,
                                         CInt(numArray.Length),
                                         Sub(ar As IAsyncResult)
                                         Dim bytesRead As Integer = StreamHelper.EndReadStreamToEnd(ar)
                                         Array.Resize(Of Byte)(numArray, bytesRead)
                                     End Sub, stream)

            While False = asyncResult.AsyncWaitHandle.WaitOne(200)
                BackgroundWorker.ReportProgress(progress)
                progress += (100 - progress) / 2
                If (Not BackgroundWorker.CancellationPending) Then Continue While
                e.Cancel = True
                Return
            End While
            backgroundWokrer.ReportProgress(100)
            e.Result = Encoding.ASCII.GetString(numArray, 0, CInt(numArray.Length))
        Catch ex As WebException
            If Not ex.Status = WebExceptionStatus.RequestCanceled Then Throw
            e.Cancel = True
        End Try
    End Sub

    Private Sub BackgroundWorker_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker.ProgressChanged
        progressBar.Value = e.ProgressPercentage
    End Sub

    Private Sub BackgroundWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker.RunWorkerCompleted
        getHtmlButton.Enabled = True
        cancelButton.Visible = False
        _webRequest = Nothing
        If (e.Cancelled) Then
            MessageBox.Show("Cancelled")
            progressBar.Visible = False
        Else
            For Each x As String In GetScriptBodies(e.Result)
                listBox.Items.Add(x)
            Next
        End If

    End Sub
End Class
