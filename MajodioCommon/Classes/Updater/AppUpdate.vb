'Imports System.Reflection
'Imports System.IO
'Imports System.Web
'Imports System.Text
'Imports System.Net
'Imports System.Xml
'Imports System.Globalization
'Imports System.Diagnostics

'Namespace Majodio.Common.Updater
'    Public Class AppUpdate
'        Private _ParentAssembly As System.reflection.Assembly

'        Public Sub New()
'            _ParentAssembly = System.Reflection.Assembly.GetCallingAssembly()
'        End Sub

'        Public Function IsUpdateAvailable() As Boolean
'            Dim RVal As Boolean = False
'            Return CompareVersion(LocalVersion, LocalVersion) < 0
'        End Function

'        Public ReadOnly Property Product() As String
'            Get
'                Return CType(_ParentAssembly.GetCustomAttributes(GetType(AssemblyProductAttribute), False)(0), AssemblyProductAttribute).Product
'            End Get
'        End Property

'        Public ReadOnly Property LocalVersion() As String
'            Get
'                Return _ParentAssembly.GetName.Version.ToString(4)
'            End Get
'        End Property

'        Public Shared Function GetDirectoryContents(ByVal url As String, ByVal deep As Boolean) As SortedList
'            Dim reader1 As StreamReader
'            Dim request1 As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
'            request1.Headers.Add("Translate: f")
'            request1.Credentials = CredentialCache.DefaultCredentials
'            Dim text1 As String = "<?xml version=""1.0"" encoding=""utf-8"" ?><a:propfind xmlns:a=""DAV:""><a:prop><a:displayname/><a:iscollection/><a:getlastmodified/></a:prop></a:propfind>"
'            request1.Method = "PROPFIND"
'            If deep Then
'                request1.Headers.Add("Depth: infinity")
'            Else
'                request1.Headers.Add("Depth: 1")
'            End If
'            request1.ContentLength = text1.Length
'            request1.ContentType = "text/xml"
'            Dim stream1 As Stream = request1.GetRequestStream
'            stream1.Write(Encoding.ASCII.GetBytes(text1), 0, Encoding.ASCII.GetBytes(text1).Length)
'            stream1.Close()
'            Try
'                Dim response1 As HttpWebResponse = CType(request1.GetResponse, HttpWebResponse)
'                reader1 = New StreamReader(response1.GetResponseStream)
'            Catch exception1 As WebException
'                Debug.WriteLine(("APPMANAGER:  Error accessing Url " & url))
'                Throw exception1
'            End Try
'            Dim builder1 As New StringBuilder
'            Dim chArray1 As Char() = New Char(1024 - 1) {}
'            Dim num1 As Integer = 0
'            num1 = reader1.Read(chArray1, 0, 1024)
'            Do While (num1 > 0)
'                builder1.Append(chArray1, 0, num1)
'                num1 = reader1.Read(chArray1, 0, 1024)
'            Loop
'            reader1.Close()
'            Dim document1 As New XmlDocument
'            document1.LoadXml(builder1.ToString)
'            Dim manager1 As New XmlNamespaceManager(document1.NameTable)
'            manager1.AddNamespace("a", "DAV:")
'            Dim list1 As XmlNodeList = document1.SelectNodes("//a:prop/a:displayname", manager1)
'            Dim list2 As XmlNodeList = document1.SelectNodes("//a:prop/a:iscollection", manager1)
'            Dim list3 As XmlNodeList = document1.SelectNodes("//a:prop/a:getlastmodified", manager1)
'            Dim list4 As XmlNodeList = document1.SelectNodes("//a:href", manager1)
'            Dim list5 As New SortedList
'            Dim num2 As Integer
'            For num2 = 0 To list1.Count - 1
'                Dim chArray2 As Char() = New Char() {"/"c}
'                Dim chArray3 As Char() = New Char() {"/"c}
'                If (Not list4.ItemOf(num2).InnerText.ToLower(New CultureInfo("en-US")).TrimEnd(chArray2) Is url.ToLower(New CultureInfo("en-US")).TrimEnd(chArray3)) Then
'                    Dim resource1 As New Resource
'                    resource1.Name = list1.ItemOf(num2).InnerText
'                    resource1.IsFolder = Convert.ToBoolean(Convert.ToInt32(list2.ItemOf(num2).InnerText))
'                    resource1.Url = list4.ItemOf(num2).InnerText
'                    resource1.LastModified = Convert.ToDateTime(list3.ItemOf(num2).InnerText)
'                    list5.Add(resource1.Url, resource1)
'                End If
'            Next num2
'            Return list5
'        End Function


'    End Class
'End Namespace

