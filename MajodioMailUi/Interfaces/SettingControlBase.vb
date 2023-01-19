Public Delegate Sub MsgDeliveryEvent(ByVal Sender As Object, ByVal E As SettingsEventArgs)


Public Class SettingsControlBase
    Inherits System.Windows.Forms.UserControl

    Public Overridable Sub Initialize()
        Throw New NotImplementedException("This method cannot be invoked.  It is left available only because the designer cannot handle an abstract class.  You must override this method")
    End Sub

    Public Event Msg As MsgDeliveryEvent

    Protected Sub OnMsg(ByVal E As SettingsEventArgs)
        RaiseEvent Msg(Me, E)
    End Sub

    Private Sub InitializeComponent()
        '
        'SettingsControlBase
        '
        Me.Name = "SettingsControlBase"
        Me.Size = New System.Drawing.Size(304, 280)

    End Sub
End Class


Public Class SettingsEventArgs
    Inherits EventArgs

    Public Sub New()

    End Sub
End Class