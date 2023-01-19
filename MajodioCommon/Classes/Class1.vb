'Imports Oracle.DataAccess.Client
'Imports System.Reflection

'Public Class OracleConnectionPool
'    Public Shared Sub KillAppDomain()
'        System.AppDomain.Unload(AppDomain.CurrentDomain)
'    End Sub

'    Private Shared Function GetConnectionContext(ByVal Connection As OracleConnection) As Object
'        Dim m_opoConCtx As FieldInfo
'        Dim RVal As New Object
'        m_opoConCtx = GetType(OracleConnection).GetField("m_opoConCtx", BindingFlags.NonPublic Or BindingFlags.Instance)
'        If Not IsNothing(m_opoConCtx) Then
'            RVal = m_opoConCtx.GetValue(Connection)
'        End If
'        Return RVal
'    End Function

'    Public Shared Function GetConnectionPool(ByVal Connection As OracleConnection) As Object
'        Dim OracleInternalConnection As Object
'        Dim OracleConnectionPoolFieldInfo As FieldInfo

'        OracleInternalConnection = GetConnectionContext(Connection)
'        If Not IsNothing(OracleInternalConnection) Then
'            OracleConnectionPoolFieldInfo = OracleInternalConnection.GetType.GetField("pool", BindingFlags.Public Or BindingFlags.Instance)
'            Return OracleConnectionPoolFieldInfo.GetValue(OracleInternalConnection)
'        End If
'        Return Nothing
'    End Function

'    Public Shared Function GetConnectionPoolConnections(ByVal ConnectionPool As Object) As Stack
'        Dim ConnectionsFieldInfo As FieldInfo
'        Dim RVal As Stack = Nothing
'        ConnectionsFieldInfo = ConnectionPool.GetType.GetField("m_connections", BindingFlags.Public Or BindingFlags.Instance)
'        If Not IsNothing(ConnectionsFieldInfo) Then
'            RVal = ConnectionsFieldInfo.GetValue(ConnectionPool)
'        End If
'        Return RVal
'    End Function

'    Public Shared Function GetConnectionPoolTimer(ByVal ConnectionPool As Object) As System.Threading.Timer
'        Dim TimerFieldInfo As FieldInfo
'        Dim RVal As System.Threading.Timer = Nothing
'        TimerFieldInfo = ConnectionPool.GetType.GetField("m_timer", BindingFlags.NonPublic Or BindingFlags.Instance)
'        If Not IsNothing(TimerFieldInfo) Then
'            RVal = TimerFieldInfo.GetValue(ConnectionPool)
'        End If
'        Return RVal
'    End Function

'    Public Shared Function GetConnectionTotal(ByVal ConnectionPool As Object) As Int32
'        Dim CounterFieldInfo As FieldInfo
'        Dim Counter As Object
'        Dim TotalFieldInfo As FieldInfo
'        Dim RVal As Int32 = -1
'        CounterFieldInfo = ConnectionPool.GetType.GetField("m_counter", BindingFlags.NonPublic Or BindingFlags.Instance)
'        If Not IsNothing(CounterFieldInfo) Then
'            Counter = CounterFieldInfo.GetValue(ConnectionPool)
'            If Not IsNothing(Counter) Then
'                TotalFieldInfo = Counter.GetType.GetField("total", BindingFlags.Public Or BindingFlags.Instance)
'                If Not IsNothing(TotalFieldInfo) Then
'                    RVal = TotalFieldInfo.GetValue(Counter)
'                End If
'            End If
'        End If
'        Return RVal
'    End Function

'    Public Shared Function GetSkipDecrement(ByVal ConnectionPool As Object) As Boolean
'        Dim RVal As Boolean
'        Dim SkipDecrementFieldInfo As FieldInfo
'        SkipDecrementFieldInfo = ConnectionPool.GetType.GetField("m_skipDecrement", BindingFlags.NonPublic Or BindingFlags.Instance)
'        If Not IsNothing(SkipDecrementFieldInfo) Then
'            RVal = SkipDecrementFieldInfo.GetValue(ConnectionPool)
'        End If
'        Return RVal
'    End Function

'    Public Shared Sub RegulateNumberOfCons(ByVal ConnectionPool As Object)
'        Dim RegNumConsMethodInfo As MethodInfo
'        RegNumConsMethodInfo = ConnectionPool.GetType.GetMethod("RegulateNumOfCons", BindingFlags.NonPublic Or BindingFlags.Instance)
'        If Not IsNothing(RegNumConsMethodInfo) Then
'            RegNumConsMethodInfo.Invoke(ConnectionPool, New Object() {Nothing})
'            RegNumConsMethodInfo.Invoke(ConnectionPool, New Object() {Nothing})
'        End If
'    End Sub

'    Public Shared Function GetConnectionTimeout(ByVal Connection As OracleConnection) As TimeSpan
'        Dim TimeoutFieldInfo As FieldInfo
'        Dim ConnectionContext As Object = GetConnectionContext(Connection)
'        Dim RVal As TimeSpan
'        If Not IsNothing(ConnectionContext) Then
'            TimeoutFieldInfo = ConnectionContext.GetType.GetField("timeOut", BindingFlags.Public Or BindingFlags.Instance)
'            If Not IsNothing(TimeoutFieldInfo) Then
'                RVal = TimeoutFieldInfo.GetValue(ConnectionContext)
'            End If
'        End If
'        Return RVal
'    End Function

'    Public Shared Function GetOracleObjectType(ByVal Fullname As String) As Object
'        Dim A As [Assembly] = Reflection.Assembly.LoadFile("c:\oracle\ora92\bin\Oracle.DataAccess.dll")
'        Dim T As Type() = A.GetTypes()
'        Dim i As Integer
'        Dim RVal As New Object
'        For i = 0 To T.GetUpperBound(0)
'            If T(i).ToString.ToLower = Fullname Then
'                RVal = T(i)
'                Exit For
'            End If
'        Next
'        Return RVal
'    End Function

'    Public Shared Sub InitializePool()
'        Dim Pooler As Object
'        Dim MaxPools As Integer
'        Dim MaxElemsInPool As Integer
'        Dim PoolsFieldInfo As FieldInfo
'        Dim MaxPoolsFieldInfo As FieldInfo
'        Dim MaxElemsInPoolFieldInfo As FieldInfo
'        Dim PoolerInitializeMethodInfo As MethodInfo

'        Pooler = GetOracleObjectType("oracle.dataaccess.client.pooler")
'        If Not IsNothing(Pooler) Then
'            PoolsFieldInfo = Pooler.GetField("Pools", BindingFlags.NonPublic Or BindingFlags.Static)
'            PoolerInitializeMethodInfo = Pooler.GetMethod("Initialize", BindingFlags.Public Or BindingFlags.Static)
'            If Not IsNothing(PoolerInitializeMethodInfo) Then
'                MaxPoolsFieldInfo = Pooler.GetField("MaxPools", BindingFlags.Public Or BindingFlags.Static)
'                MaxPools = MaxPoolsFieldInfo.GetValue(Nothing)

'                MaxElemsInPoolFieldInfo = Pooler.GetField("MaxElemsInPool", BindingFlags.Public Or BindingFlags.Static)
'                MaxElemsInPool = MaxElemsInPoolFieldInfo.GetValue(Nothing)

'                PoolsFieldInfo.SetValue(Nothing, Nothing)
'                PoolerInitializeMethodInfo.Invoke(Nothing, New Object() {MaxPools, MaxElemsInPool})
'            End If
'        End If
'    End Sub

'    Public Shared Sub FlushPool(ByVal Connection As OracleConnection)
'        Dim OpsCon As Object = GetOracleObjectType("oracle.dataaccess.client.opscon")
'        Dim Connections As Stack
'        Dim Con As Object
'        Dim OpsConnDisposeMethodInfo As MethodInfo
'        Dim i As Integer
'        Dim OpsConCtxFieldInfo As FieldInfo
'        Dim OpsErrCtxFieldInfo As FieldInfo
'        Dim OpoConValCtxFieldInfo As FieldInfo
'        Dim OpoConRefCtxFieldInfo As FieldInfo
'        Connections = GetConnectionPoolConnections(GetConnectionPool(Connection))
'        If Not IsNothing(OpsCon) Then
'            OpsConnDisposeMethodInfo = OpsCon.GetMethod("Dispose", BindingFlags.Public Or BindingFlags.Static)
'            If Not IsNothing(OpsConnDisposeMethodInfo) Then
'                For i = 0 To Connections.Count - 1
'                    Con = Connections.Pop
'                    OpsConCtxFieldInfo = Con.GetType.GetField("opsConCtx", BindingFlags.Public Or BindingFlags.Instance)
'                    OpsErrCtxFieldInfo = Con.GetType.GetField("opsErrCtx", BindingFlags.Public Or BindingFlags.Instance)
'                    OpoConValCtxFieldInfo = Con.GetType.GetField("pOpoConValCtx", BindingFlags.Public Or BindingFlags.Instance)
'                    OpoConRefCtxFieldInfo = Con.GetType.GetField("opoConRefCtx", BindingFlags.Public Or BindingFlags.Instance)
'                    If Not IsNothing(OpsConCtxFieldInfo) AndAlso Not IsNothing(OpsErrCtxFieldInfo) AndAlso Not IsNothing(OpoConValCtxFieldInfo) AndAlso Not IsNothing(OpoConRefCtxFieldInfo) Then
'                        OpsConnDisposeMethodInfo.Invoke(Nothing, New Object() {CType(OpsConCtxFieldInfo.GetValue(Con), System.IntPtr), CType(OpsErrCtxFieldInfo.GetValue(Con), System.IntPtr), OpoConValCtxFieldInfo.GetValue(Con), OpoConRefCtxFieldInfo.GetValue(Con)})
'                        'OpsConnDisposeMethodInfo.Invoke(Nothing, New Object() {CType(OpsConCtxFieldInfo.GetValue(Con), System.IntPtr), CType(OpsErrCtxFieldInfo.GetValue(Con), System.IntPtr), Nothing, Nothing})
'                    End If
'                Next
'            End If
'        End If
'    End Sub
'End Class