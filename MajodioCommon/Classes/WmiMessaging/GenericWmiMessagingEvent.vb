Imports System.Management
Imports System.Management.Instrumentation

Namespace WmiMessaging

    ''' <summary>
    ''' This is a generic class that can used to raise WMI event.  To use WMI, you must do the following
    ''' 
    ''' 1)  Add an Instrumented attribute to the Assembly.vb file (example: &lt;Assembly: Instrumented("Root\SpiderWmiMessaging")&gt;)
    ''' 2)  Create a class that inherits from System.Management.Instrumentation.BaseEvent.  This class cannot contain custom types.
    ''' 3)  Add an installer class to your assembly that derives from System.Configuration.Install.DefaultManagementProjectInstaller
    ''' 4)  Add a listner class that contains the WQL to retrieve data about your class.  The WQL should contain a select statement
    '''     that looks like "SELECT * FROM &lt;classname>" where classname is the name of the class that derives from BaseEvent
    ''' 5)  Run InstallUtil.exe against the assembly
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class GenericWmiMessagingEvent
        Inherits WmiMessagingEventBase

        Private _application As String
        Private _category As String
        Private _message As String
        Private _system As String

        Public Property Application() As String
            Get
                Return _application
            End Get
            Set(ByVal value As String)
                _application = value
            End Set
        End Property

        Public Property Category() As String
            Get
                Return _category
            End Get
            Set(ByVal value As String)
                _category = value
            End Set
        End Property

        Public Property Message() As String
            Get
                Return _message
            End Get
            Set(ByVal value As String)
                _message = value
            End Set
        End Property

        Public Property System() As String
            Get
                Return _system
            End Get
            Set(ByVal value As String)
                _system = value
            End Set
        End Property

        Public Overrides Function GetEventArgsInstace(ByVal parameters As System.Management.PropertyDataCollection) As WmiMessagingEventArgsBase
            Dim rVal As GenericWmiMessagingEventArgs = Nothing

            For Each [property] As PropertyData In parameters
                Select Case [property].Name.ToLower
                    Case "application"
                        Application = [property].Value
                    Case "category"
                        Category = [property].Value
                    Case "message"
                        Message = [property].Value
                    Case "system"
                        System = [property].Value
                End Select
            Next

            rVal = New GenericWmiMessagingEventArgs(Application, Category, Message, System)

            Return rVal
        End Function
    End Class
End Namespace


