<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main2
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.txtMessage = New System.Windows.Forms.TextBox
        Me.btnAddSpam = New System.Windows.Forms.Button
        Me.btnCheck = New System.Windows.Forms.Button
        Me.btnAddHam = New System.Windows.Forms.Button
        Me.txtPath = New System.Windows.Forms.TextBox
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.btnStart = New System.Windows.Forms.Button
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblSpamProbability = New System.Windows.Forms.Label
        Me.cbFunction = New System.Windows.Forms.ComboBox
        Me.btnOutput = New System.Windows.Forms.Button
        Me.chkMoveFiles = New System.Windows.Forms.CheckBox
        Me.btnSerialize = New System.Windows.Forms.Button
        Me.btnDeserialize = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'txtMessage
        '
        Me.txtMessage.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMessage.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMessage.Location = New System.Drawing.Point(12, 71)
        Me.txtMessage.Multiline = True
        Me.txtMessage.Name = "txtMessage"
        Me.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtMessage.Size = New System.Drawing.Size(590, 335)
        Me.txtMessage.TabIndex = 1
        Me.txtMessage.WordWrap = False
        '
        'btnAddSpam
        '
        Me.btnAddSpam.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddSpam.Location = New System.Drawing.Point(515, 412)
        Me.btnAddSpam.Name = "btnAddSpam"
        Me.btnAddSpam.Size = New System.Drawing.Size(86, 23)
        Me.btnAddSpam.TabIndex = 2
        Me.btnAddSpam.Text = "Add As Spam"
        Me.btnAddSpam.UseVisualStyleBackColor = True
        '
        'btnCheck
        '
        Me.btnCheck.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCheck.Location = New System.Drawing.Point(12, 412)
        Me.btnCheck.Name = "btnCheck"
        Me.btnCheck.Size = New System.Drawing.Size(75, 23)
        Me.btnCheck.TabIndex = 3
        Me.btnCheck.Text = "Check"
        Me.btnCheck.UseVisualStyleBackColor = True
        '
        'btnAddHam
        '
        Me.btnAddHam.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddHam.Location = New System.Drawing.Point(429, 412)
        Me.btnAddHam.Name = "btnAddHam"
        Me.btnAddHam.Size = New System.Drawing.Size(80, 23)
        Me.btnAddHam.TabIndex = 4
        Me.btnAddHam.Text = "Add As Ham"
        Me.btnAddHam.UseVisualStyleBackColor = True
        '
        'txtPath
        '
        Me.txtPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPath.Location = New System.Drawing.Point(12, 12)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(507, 20)
        Me.txtPath.TabIndex = 5
        '
        'btnBrowse
        '
        Me.btnBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowse.Location = New System.Drawing.Point(525, 12)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowse.TabIndex = 6
        Me.btnBrowse.Text = "Browse"
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(422, 42)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(75, 23)
        Me.btnStart.TabIndex = 7
        Me.btnStart.Text = "Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 42)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Spam Probability:"
        '
        'lblSpamProbability
        '
        Me.lblSpamProbability.AutoSize = True
        Me.lblSpamProbability.Location = New System.Drawing.Point(106, 42)
        Me.lblSpamProbability.Name = "lblSpamProbability"
        Me.lblSpamProbability.Size = New System.Drawing.Size(13, 13)
        Me.lblSpamProbability.TabIndex = 9
        Me.lblSpamProbability.Text = "0"
        '
        'cbFunction
        '
        Me.cbFunction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbFunction.FormattingEnabled = True
        Me.cbFunction.Items.AddRange(New Object() {"Check Each Message", "Mark All Messages as Ham", "Mark All Messages as Spam"})
        Me.cbFunction.Location = New System.Drawing.Point(240, 42)
        Me.cbFunction.Name = "cbFunction"
        Me.cbFunction.Size = New System.Drawing.Size(176, 21)
        Me.cbFunction.TabIndex = 13
        '
        'btnOutput
        '
        Me.btnOutput.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOutput.Location = New System.Drawing.Point(93, 412)
        Me.btnOutput.Name = "btnOutput"
        Me.btnOutput.Size = New System.Drawing.Size(75, 23)
        Me.btnOutput.TabIndex = 14
        Me.btnOutput.Text = "Output"
        Me.btnOutput.UseVisualStyleBackColor = True
        '
        'chkMoveFiles
        '
        Me.chkMoveFiles.AutoSize = True
        Me.chkMoveFiles.Location = New System.Drawing.Point(157, 42)
        Me.chkMoveFiles.Name = "chkMoveFiles"
        Me.chkMoveFiles.Size = New System.Drawing.Size(77, 17)
        Me.chkMoveFiles.TabIndex = 15
        Me.chkMoveFiles.Text = "Move Files"
        Me.chkMoveFiles.UseVisualStyleBackColor = True
        '
        'btnSerialize
        '
        Me.btnSerialize.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSerialize.Location = New System.Drawing.Point(174, 412)
        Me.btnSerialize.Name = "btnSerialize"
        Me.btnSerialize.Size = New System.Drawing.Size(75, 23)
        Me.btnSerialize.TabIndex = 16
        Me.btnSerialize.Text = "Serialize"
        Me.btnSerialize.UseVisualStyleBackColor = True
        '
        'btnDeserialize
        '
        Me.btnDeserialize.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDeserialize.Location = New System.Drawing.Point(255, 412)
        Me.btnDeserialize.Name = "btnDeserialize"
        Me.btnDeserialize.Size = New System.Drawing.Size(75, 23)
        Me.btnDeserialize.TabIndex = 17
        Me.btnDeserialize.Text = "Deserialize"
        Me.btnDeserialize.UseVisualStyleBackColor = True
        '
        'Main2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(614, 447)
        Me.Controls.Add(Me.btnDeserialize)
        Me.Controls.Add(Me.btnSerialize)
        Me.Controls.Add(Me.chkMoveFiles)
        Me.Controls.Add(Me.btnOutput)
        Me.Controls.Add(Me.cbFunction)
        Me.Controls.Add(Me.lblSpamProbability)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.txtPath)
        Me.Controls.Add(Me.btnAddHam)
        Me.Controls.Add(Me.btnCheck)
        Me.Controls.Add(Me.btnAddSpam)
        Me.Controls.Add(Me.txtMessage)
        Me.Name = "Main2"
        Me.Text = "Main"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtMessage As System.Windows.Forms.TextBox
    Friend WithEvents btnAddSpam As System.Windows.Forms.Button
    Friend WithEvents btnCheck As System.Windows.Forms.Button
    Friend WithEvents btnAddHam As System.Windows.Forms.Button
    Friend WithEvents txtPath As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents btnStart As System.Windows.Forms.Button
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblSpamProbability As System.Windows.Forms.Label
    Friend WithEvents cbFunction As System.Windows.Forms.ComboBox
    Friend WithEvents btnOutput As System.Windows.Forms.Button
    Friend WithEvents chkMoveFiles As System.Windows.Forms.CheckBox
    Friend WithEvents btnSerialize As System.Windows.Forms.Button
    Friend WithEvents btnDeserialize As System.Windows.Forms.Button
End Class
