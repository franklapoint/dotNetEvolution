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
        Me.listBox.TabIndex = 7
        '
        'getHtmlButton
        '
        Me.getHtmlButton.Location = New System.Drawing.Point(12, 20)
        Me.getHtmlButton.Name = "getHtmlButton"
        Me.getHtmlButton.Size = New System.Drawing.Size(75, 23)
        Me.getHtmlButton.TabIndex = 6
        Me.getHtmlButton.Text = "Get HTML..."
        Me.getHtmlButton.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(282, 255)
        Me.Controls.Add(Me.listBox)
        Me.Controls.Add(Me.getHtmlButton)
        Me.Name = "Form1"
        Me.Text = "MainForm"
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents listBox As System.Windows.Forms.ListBox
    Private WithEvents getHtmlButton As System.Windows.Forms.Button

End Class
