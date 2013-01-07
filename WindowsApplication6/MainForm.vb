Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks
Imports System.Threading

Public Class MainForm
    Private ReadOnly finalBuffer(1024000) As Byte

    Private Sub getHtmlButton_Click(sender As System.Object, e As EventArgs) Handles getHtmlButton.Click
        ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf StartRequest))
        getHtmlButton.Enabled = False
    End Sub

    Private Sub StartRequest()
        Dim inputBuffer(1024000) As Byte
        Dim webRequest As HttpWebRequest = DirectCast(Net.WebRequest.Create("http://google.ca"), HttpWebRequest)
        webRequest.BeginGetResponse(Sub(asyncResult)
                                        Dim response As WebResponse = webRequest.EndGetResponse(asyncResult)
                                        Dim stream As Stream = response.GetResponseStream()
                                        Dim bytesRead As Task(Of Integer) = Task(Of Integer).Factory.FromAsync(
                                            AddressOf stream.BeginReadToEnd, AddressOf stream.EndReadToEnd, inputBuffer, 0, inputBuffer.Length, stream)
                                        Dim ui = TaskScheduler.FromCurrentSynchronizationContext()
                                        bytesRead.ContinueWith(Sub(ar)
                                                                   Trace.WriteLine(String.Format("Read {0} bytes", ar.Result))
                                                                   Dim html = Encoding.ASCII.GetString(inputBuffer, 0, ar.Result)
                                                                   SetData(html)
                                                                   getHtmlButton.Enabled = True
                                                                   DirectCast(response, IDisposable).Dispose()
                                                               End Sub, ui)
                                    End Sub, webRequest)
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
