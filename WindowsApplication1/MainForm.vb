Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Public Class MainForm
    Inherits Windows.Forms.Form

    Public Sub New()
        InitializeComponent()
    End Sub

    Private ReadOnly finalBuffer(1024000) As Byte

    Private Sub getHtmlButton_Click(sender As Object, e As EventArgs) Handles getHtmlButton.Click
        Dim webRequest As HttpWebRequest = DirectCast(
            Net.WebRequest.Create("http://www.microsoft.com/en/us/default.aspx"), 
            HttpWebRequest)
        webRequest.BeginGetResponse(New AsyncCallback(
                                    AddressOf GetResponseCallback), webRequest)
        getHtmlButton.Enabled = False
    End Sub

    Private Sub GetResponseCallback(ByVal asyncResult As IAsyncResult)
        Dim webRequest As HttpWebRequest = DirectCast(asyncResult.AsyncState, HttpWebRequest)
        Dim response As WebResponse = webRequest.EndGetResponse(asyncResult)
        Dim stream As Stream = response.GetResponseStream()
        If (Not stream Is Nothing) Then
            StreamHelper.BeginReadStreamToEnd(stream, finalBuffer, 0, CInt(finalBuffer.Length), New AsyncCallback(AddressOf ReadToEndCallback), stream)
        End If
    End Sub

    Private Sub ReadToEndCallback(ByVal asyncResult As IAsyncResult)
        Dim bytesRead As Integer = StreamHelper.EndReadStreamToEnd(asyncResult)
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
        For Each x As String In GetScriptBodies(html)
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

    Private Shared Function GetScriptBodies(ByVal html As String) As IEnumerable
        Dim enumerable As IEnumerable
        Const pattern As String = "<script.*?>\s*(?'scriptBody'.+?)\s*</script>"
        Dim matches As MatchCollection = Regex.Matches(html, pattern)
        Dim result(matches.Count - 1) As String
        If (matches.Count <> 0) Then
            Dim i As Integer = 0
            For Each match As Match In matches
                Dim scriptBody As String = match.Groups("scriptBody").Value
                Dim num As Integer = i
                i = num + 1
                result(num) = scriptBody
            Next
            enumerable = result
        Else
            enumerable = result
        End If
        Return enumerable
    End Function

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.listBox = New System.Windows.Forms.ListBox()
        Me.getHtmlButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'listBox
        '
        Me.listBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.listBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.listBox.FormattingEnabled = True
        Me.listBox.ItemHeight = 20
        Me.listBox.Location = New System.Drawing.Point(12, 50)
        Me.listBox.Name = "listBox"
        Me.listBox.Size = New System.Drawing.Size(258, 184)
        Me.listBox.TabIndex = 3
        '
        'getHtmlButton
        '
        Me.getHtmlButton.Location = New System.Drawing.Point(12, 20)
        Me.getHtmlButton.Name = "getHtmlButton"
        Me.getHtmlButton.Size = New System.Drawing.Size(75, 23)
        Me.getHtmlButton.TabIndex = 2
        Me.getHtmlButton.Text = "Get HTML..."
        Me.getHtmlButton.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(282, 255)
        Me.Controls.Add(Me.listBox)
        Me.Controls.Add(Me.getHtmlButton)
        Me.Name = "MainForm"
        Me.Text = "MainForm"
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents listBox As System.Windows.Forms.ListBox
    Private WithEvents getHtmlButton As System.Windows.Forms.Button
End Class
