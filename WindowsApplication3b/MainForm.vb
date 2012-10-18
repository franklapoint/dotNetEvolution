Imports System.Text.RegularExpressions
Imports System.Net

Public Class MainForm
    Private webClient As WebClient

    Private Sub getHtmlButton_Click(sender As System.Object, e As System.EventArgs) Handles getHtmlButton.Click
        listBox.Items.Clear()
        webClient = New WebClient()
        AddHandler webClient.DownloadProgressChanged,
            Sub(o As Object, args As DownloadProgressChangedEventArgs) progressBar.Value = args.ProgressPercentage

        AddHandler webClient.DownloadStringCompleted,
            Sub(o As Object, args As DownloadStringCompletedEventArgs)
                getHtmlButton.Enabled = True
                cancelButton.Visible = False
                webClient.Dispose()
                webClient = Nothing
                If (args.Cancelled) Then
                    MessageBox.Show("Cancelled")
                    progressBar.Visible = False
                Else
                    For Each x As String In GetScriptBodies(args.Result)
                        listBox.Items.Add(x)
                    Next
                End If
            End Sub

        progressBar.Visible = True
        cancelButton.Visible = True
        progressBar.Value = 0
        webClient.DownloadStringAsync(New Uri("http://google.ca"))
        getHtmlButton.Enabled = False
    End Sub

    Private Sub cancelButton_Click(sender As Object, e As EventArgs) Handles cancelButton.Click
        If (Not webClient Is Nothing) Then
            webClient.CancelAsync()
        End If
    End Sub

    Private Shared Function GetScriptBodies(ByVal html As String) As IEnumerable
        Const pattern As String = "<script.*?>\s*(?'scriptBody'.+?)\s*</script>"
        Dim matches As MatchCollection = Regex.Matches(html, pattern)
        Dim result(matches.Count - 1) As String
        Dim i As Integer = 0
        For Each match As Match In matches
            Dim scriptBody As String = match.Groups("scriptBody").Value
            Dim num As Integer = i
            i = num + 1
            result(num) = scriptBody
        Next
        Return result
    End Function

End Class
