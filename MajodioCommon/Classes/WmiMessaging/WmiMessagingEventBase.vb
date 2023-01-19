Imports System.Management
Imports System.Management.Instrumentation

Namespace WmiMessaging
    Public MustInherit Class WmiMessagingEventBase
        Inherits BaseEvent

        Public MustOverride Function GetEventArgsInstace(ByVal properties As System.Management.PropertyDataCollection) As WmiMessagingEventArgsBase

    End Class
End Namespace