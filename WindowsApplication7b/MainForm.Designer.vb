<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

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
        Me.cancelButton = New System.Windows.Forms.Button()
        Me.progressBar = New System.Windows.Forms.ProgressBar()
        Me.SuspendLayout()
        '
        'listBox
        '
        Me.listBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.listBox.FormattingEnabled = True
        Me.listBox.Location = New System.Drawing.Point(13, 46)
        Me.listBox.Name = "listBox"
        Me.listBox.Size = New System.Drawing.Size(259, 199)
        Me.listBox.TabIndex = 5
        '
        'getHtmlButton
        '
        Me.getHtmlButton.Location = New System.Drawing.Point(13, 16)
        Me.getHtmlButton.Name = "getHtmlButton"
        Me.getHtmlButton.Size = New System.Drawing.Size(75, 23)
        Me.getHtmlButton.TabIndex = 4
        Me.getHtmlButton.Text = "Get HTML..."
        Me.getHtmlButton.UseVisualStyleBackColor = True
        '
        'cancelButton
        '
        Me.cancelButton.Enabled = False
        Me.cancelButton.Location = New System.Drawing.Point(212, 16)
        Me.cancelButton.Name = "cancelButton"
        Me.cancelButton.Size = New System.Drawing.Size(60, 23)
        Me.cancelButton.TabIndex = 19
        Me.cancelButton.Text = "&Cancel"
        Me.cancelButton.UseVisualStyleBackColor = True
        Me.cancelButton.Visible = False
        '
        'progressBar
        '
        Me.progressBar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.progressBar.Enabled = False
        Me.progressBar.Location = New System.Drawing.Point(94, 16)
        Me.progressBar.Name = "progressBar"
        Me.progressBar.Size = New System.Drawing.Size(112, 23)
        Me.progressBar.TabIndex = 18
        Me.progressBar.Visible = False
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.Controls.Add(Me.cancelButton)
        Me.Controls.Add(Me.progressBar)
        Me.Controls.Add(Me.listBox)
        Me.Controls.Add(Me.getHtmlButton)
        Me.Name = "MainForm"
        Me.Text = "Main Form"
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents listBox As System.Windows.Forms.ListBox
    Private WithEvents getHtmlButton As System.Windows.Forms.Button
    Private WithEvents cancelButton As System.Windows.Forms.Button
    Private WithEvents progressBar As System.Windows.Forms.ProgressBar

End Class
