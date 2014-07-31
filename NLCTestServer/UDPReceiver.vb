'***************************************************************************************
'* UDPReceiver Class
'* This class Recveives a UDP command from the Network Light Control Application
'*  and raises an event to update controls on the ReceiveForm when a command is received
'*
'* Created By Alex Behlen
'*
'***************************************************************************************

Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading

Public Class UDPReceiver

    Private _Port As Integer
    Private _Active As Boolean = True
    Private _udpServer As UdpClient
    Private _ipEndPoint As IPEndPoint
    Private _receiverThread As Thread

    Public Property Active() As Boolean
        Get
            Return _Active
        End Get
        Set(value As Boolean)
            _Active = value
        End Set
    End Property

    Public Sub New(ByVal port As Integer)
        _Port = port
        _ipEndPoint = New IPEndPoint(IPAddress.Any, _Port)
        Active = True

    End Sub

    Public Event UpdatePinStatus(ByVal sender As Object, ByVal e As UpdatePinStatusArgs)

    Public Sub Start()
        _udpServer = New UdpClient()
        _udpServer.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, True)
        _udpServer.Client.Bind(_ipEndPoint)

        ' Create new thread to receive udp packet
        _receiverThread = New System.Threading.Thread(AddressOf ReceiveMessage)
        _receiverThread.Start()
    End Sub

    Private Sub ReceiveMessage()
        Do While Active
            Dim incomingBytes As [Byte]() = _udpServer.Receive(_ipEndPoint)
            Dim data As String = Encoding.ASCII.GetChars(incomingBytes)
            Dim pin As Integer
            Dim status As Integer
            Integer.TryParse(data.Substring(0, 1), pin)
            Integer.TryParse(data.Substring(1, 1), status)

            'Raise event to update form controls
            OnUpdatePinStatus(New UpdatePinStatusArgs(pin, status))
        Loop
    End Sub

    Protected Overridable Sub OnUpdatePinStatus(ByVal e As UpdatePinStatusArgs)
        RaiseEvent UpdatePinStatus(Me, e)
    End Sub

    Public Sub Close()
        _receiverThread.Abort()
        _udpServer.Close()
        Active = False
    End Sub

End Class

