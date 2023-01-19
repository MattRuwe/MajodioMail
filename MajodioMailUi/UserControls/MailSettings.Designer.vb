<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MailSettings
    Inherits SettingsControlBase

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.pgMailSettings = New System.Windows.Forms.PropertyGrid
        Me.SuspendLayout()
        '
        'pgMailSettings
        '
        Me.pgMailSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pgMailSettings.Location = New System.Drawing.Point(0, 0)
        Me.pgMailSettings.Name = "pgMailSettings"
        Me.pgMailSettings.Size = New System.Drawing.Size(876, 622)
        Me.pgMailSettings.TabIndex = 0
        '
        'MailSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pgMailSettings)
        Me.Name = "MailSettings"
        Me.Size = New System.Drawing.Size(876, 622)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pgMailSettings As System.Windows.Forms.PropertyGrid

End Class
