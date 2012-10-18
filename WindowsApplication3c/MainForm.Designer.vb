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
        Me.progressBar = New System.Windows.Forms.ProgressBar()
        Me.cancelButton = New System.Windows.Forms.Button()
        Me.listBox = New System.Windows.Forms.ListBox()
        Me.getHtmlButton = New System.Windows.Forms.Button()
        Me.BackgroundWorker = New System.ComponentModel.BackgroundWorker()
        Me.SuspendLayout()
        '
        'progressBar
        '
        Me.progressBar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.progressBar.Location = New System.Drawing.Point(70, 19)
        Me.progressBar.Name = "progressBar"
        Me.progressBar.Size = New System.Drawing.Size(70, 23)
        Me.progressBar.TabIndex = 12
        Me.progressBar.Visible = False
        '
        'cancelButton
        '
        Me.cancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cancelButton.Location = New System.Drawing.Point(145, 22)
        Me.cancelButton.Margin = New System.Windows.Forms.Padding(2)
        Me.cancelButton.Name = "cancelButton"
        Me.cancelButton.Size = New System.Drawing.Size(56, 19)
        Me.cancelButton.TabIndex = 13
        Me.cancelButton.Text = "&Cancel"
        Me.cancelButton.UseVisualStyleBackColor = True
        Me.cancelButton.Visible = False
        '
        'listBox
        '
        Me.listBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.listBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.listBox.FormattingEnabled = True
        Me.listBox.ItemHeight = 17
        Me.listBox.Location = New System.Drawing.Point(9, 47)
        Me.listBox.Margin = New System.Windows.Forms.Padding(2)
        Me.listBox.Name = "listBox"
        Me.listBox.Size = New System.Drawing.Size(194, 140)
        Me.listBox.TabIndex = 11
        '
        'getHtmlButton
        '
        Me.getHtmlButton.Location = New System.Drawing.Point(9, 22)
        Me.getHtmlButton.Margin = New System.Windows.Forms.Padding(2)
        Me.getHtmlButton.Name = "getHtmlButton"
        Me.getHtmlButton.Size = New System.Drawing.Size(56, 19)
        Me.getHtmlButton.TabIndex = 10
        Me.getHtmlButton.Text = "Get HTML..."
        Me.getHtmlButton.UseVisualStyleBackColor = True
        '
        'BackgroundWorker
        '
        Me.BackgroundWorker.WorkerReportsProgress = True
        Me.BackgroundWorker.WorkerSupportsCancellation = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(212, 207)
        Me.Controls.Add(Me.progressBar)
        Me.Controls.Add(Me.cancelButton)
        Me.Controls.Add(Me.listBox)
        Me.Controls.Add(Me.getHtmlButton)
        Me.Name = "MainForm"
        Me.Text = "Main Form"
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents progressBar As System.Windows.Forms.ProgressBar
    Private WithEvents cancelButton As System.Windows.Forms.Button
    Private WithEvents listBox As System.Windows.Forms.ListBox
    Private WithEvents getHtmlButton As System.Windows.Forms.Button
    Friend WithEvents BackgroundWorker As System.ComponentModel.BackgroundWorker

End Class
