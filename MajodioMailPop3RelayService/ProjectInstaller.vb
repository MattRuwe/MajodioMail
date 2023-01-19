Imports System.ComponentModel
Imports System.Configuration.Install

Public Class ProjectInstaller

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add initialization code after the call to InitializeComponent

    End Sub

    Public Overrides Sub Install(ByVal stateSaver As IDictionary)
        MyBase.Install(stateSaver)

        Dim sc As New System.ServiceProcess.ServiceController("Majodio Pop3 Relay")
        sc.Start()
    End Sub

End Class
