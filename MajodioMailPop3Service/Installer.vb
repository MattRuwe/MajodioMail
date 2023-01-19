Imports System.ComponentModel
Imports System.Configuration.Install
Imports System.ServiceProcess

<RunInstaller(True)> _
Public Class Installer
    Inherits System.Configuration.Install.Installer

#Region " Component Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Installer overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private ServiceInstaller1 As ServiceInstaller
    Private processInstaller As ServiceProcessInstaller

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container
        processInstaller = New ServiceProcessInstaller

        ServiceInstaller1 = New ServiceInstaller
        ' The services will run under the system account.
        processInstaller.Account = ServiceAccount.LocalSystem

        ' The services will be started manually.
        ServiceInstaller1.StartType = ServiceStartMode.Automatic

        ' ServiceName must equal those on ServiceBase derived classes.            
        ServiceInstaller1.ServiceName = "Majodio Pop3"
        'ServiceInstaller1.ServicesDependedOn = New String() {"Majodio Logger"}

        ' Add installers to collection. Order is not important.
        Installers.Add(ServiceInstaller1)
        Installers.Add(processInstaller)

    End Sub
#End Region

    Public Overrides Sub Install(ByVal stateSaver As IDictionary)
        MyBase.Install(stateSaver)

        Dim sc As New System.ServiceProcess.ServiceController("Majodio Pop3")
        sc.Start()
    End Sub

End Class


