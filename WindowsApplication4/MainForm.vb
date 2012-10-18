Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Public Class MainForm

    Private ReadOnly finalBuffer(1024000) As Byte

    Private Sub getHtmlButton_Click(sender As System.Object, e As EventArgs) Handles getHtmlButton.Click
        Dim webRequest As HttpWebRequest = DirectCast(Net.WebRequest.Create("http://www.microsoft.com/en/us/default.aspx"), HttpWebRequest)
        webRequest.BeginGetResponse(New AsyncCallback(AddressOf GetResponseCallback), webRequest)
        getHtmlButton.Enabled = False
    End Sub

    Private Sub GetResponseCallback(ByVal asyncResult As IAsyncResult)
        Dim webRequest As HttpWebRequest = DirectCast(asyncResult.AsyncState, HttpWebRequest)
        Dim response As WebResponse = webRequest.EndGetResponse(asyncResult)
        Dim stream As Stream = response.GetResponseStream()
        If (Not stream Is Nothing) Then
            stream.BeginReadToEnd(finalBuffer, 0, CInt(finalBuffer.Length), New AsyncCallback(AddressOf ReadToEndCallback), stream)
        End If
    End Sub

    Private Sub ReadToEndCallback(ByVal asyncResult As IAsyncResult)
        Dim stream As Stream = asyncResult.AsyncState
        Dim bytesRead As Integer = stream.EndReadToEnd(asyncResult)
        Trace.WriteLine(String.Format("Read {0} bytes", bytesRead))
        Dim html As String = Encoding.ASCII.GetString(finalBuffer, 0, bytesRead)
        SetData(html)
        EnableButton()
    End Sub

    Delegate Sub EnableButtonDelegate()
    Delegate Sub SetDataDelegate(ByVal html As String)

    Private Sub SetData(ByVal html As String)
        If String.IsNullOrEmpty(html) Then Return

        If InvokeRequired Then
            Dim objArray(0) As Object
            objArray(0) = html
            BeginInvoke(New SetDataDelegate(AddressOf SetData), objArray)
            Return
        End If

        listBox.Items.Clear()
        For Each x In GetScriptBodies(html)
            listBox.Items.Add(x)
        Next
    End Sub

    Private Sub EnableButton()
        If (Not InvokeRequired) Then
            getHtmlButton.Enabled = True
        Else
            BeginInvoke(New EnableButtonDelegate(AddressOf EnableButton))
        End If
    End Sub

    Private Shared Function GetScriptBodies(ByVal html As String) As IEnumerable(Of String)
        Const pattern As String = "<script.*?>\s*(?'scriptBody'.+?)\s*</script>"
        Dim matches = Regex.Matches(html, pattern)
        Dim result As List(Of String) = New List(Of String)(matches.Count)
        For Each match As Match In matches
            Dim scriptBody As String = match.Groups("scriptBody").Value
            result.Add(scriptBody)
        Next
        'Dim someType = New With {.X = 10, .Y = 12}
        Return result
    End Function
End Class
