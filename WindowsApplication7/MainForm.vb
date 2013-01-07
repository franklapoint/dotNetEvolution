Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Public Class MainForm

    Private Async Sub getHtmlButton_Click(sender As Object, e As EventArgs) Handles getHtmlButton.Click
        Dim inputBuffer(1024000) As Byte

        getHtmlButton.Enabled = False
        Dim webRequest As HttpWebRequest = Await StartRequestAsync()
        Dim index As Integer = 0
        Using response As WebResponse = Await webRequest.GetResponseAsync()
            Dim stream As Stream = response.GetResponseStream()
            Dim bytesRead As Integer
            Do
                bytesRead = Await stream.ReadAsync(inputBuffer, index, inputBuffer.Length - index)
                index += bytesRead
            Loop Until bytesRead = 0

            Trace.WriteLine(String.Format("Read {0} bytes", index))
        End Using
        Dim html As String = Encoding.ASCII.GetString(inputBuffer, 0, index)

        listBox.Items.Clear()
        If (Not String.IsNullOrEmpty(html)) Then
            For Each x As String In GetScriptBodies(html)
                listBox.Items.Add(x)
            Next
        End If
        getHtmlButton.Enabled = True

    End Sub

    Private Function StartRequestAsync() As Task(Of HttpWebRequest)
        Return Task.Factory.StartNew(Function() DirectCast(Net.WebRequest.Create("http://google.ca"), HttpWebRequest))
    End Function

    Private Shared Iterator Function GetScriptBodies(html As String) As IEnumerable(Of String)
        Const pattern As String = "<script.*?>\s*(?'scriptBody'.+?)\s*</script>"
        Dim matches As MatchCollection = Regex.Matches(html, pattern)
        For Each match As Match In matches
            Dim scriptBody As String = match.Groups("scriptBody").Value
            Yield scriptBody
        Next
    End Function

End Class
