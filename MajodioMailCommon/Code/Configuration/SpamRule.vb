Namespace Configuration
    <Serializable()> _
    Public Class SpamRule
        Implements IComparable

        Private _name As String
        Private _regEx As String
        Private _action As SpamRuleAction
        Private _applyActionIfRegexMatches As Boolean
        Private _order As Integer

        Public Sub New()
            _name = String.Empty
            _regEx = String.Empty
            _action = SpamRuleAction.NoAction
            _order = -1
        End Sub

        Public Sub New(ByVal name As String, ByVal regEx As String, ByVal order As Integer, ByVal action As SpamRuleAction, ByVal applyActionIfRegexMatches As Boolean)
            _name = name
            _regEx = regEx
            If order < 0 Then
                Throw New ArgumentOutOfRangeException("order", "The value must be greater than 0")
            End If
            _order = order
            _action = action
            _applyActionIfRegexMatches = applyActionIfRegexMatches
        End Sub

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property RegEx() As String
            Get
                Return _regEx
            End Get
            Set(ByVal value As String)
                _regEx = value
            End Set
        End Property

        Public Property Action() As SpamRuleAction
            Get
                Return _action
            End Get
            Set(ByVal value As SpamRuleAction)
                _action = value
            End Set
        End Property

        Public Property ApplyActionIfRegexMatches() As Boolean
            Get
                Return _applyActionIfRegexMatches
            End Get
            Set(ByVal value As Boolean)
                _applyActionIfRegexMatches = value
            End Set
        End Property

        Public Property Order() As Integer
            Get
                Return _order
            End Get
            Set(ByVal value As Integer)
                _order = value
            End Set
        End Property

        Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
            Return _order.CompareTo(CType(obj, SpamRule).Order)
        End Function

        Public Overrides Function ToString() As String
            Return _name
        End Function
    End Class
End Namespace